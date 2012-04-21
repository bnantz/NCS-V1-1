//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Storage;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <devdoc>
    /// Represents a configuration file according to the schema for Fx 1.1
    /// </devdoc>
    internal class ConfigurationFile : IDisposable
    {
        public const string MetaConfigSectionName = "metaConfig";
        private const string rootNode = "configuration";
        private const string configSectionsXPath = "//configuration/configSections";
        private const string configSectionsElement = "configSections";
        private const string configSectionElement = "section";
        private const string nameAttribute = "name";
        private const string typeAttribute = "type";

        private readonly ConfigurationFile parent;

        private static object removedSectionObj = new object();
        private static object groupSectionObj = new object();

        private Hashtable results;
        private Hashtable sectionHandlers;
        private Hashtable unevaluatedSections;
        private string configurationFile;
        private bool shouldInheritSections;

        public ConfigurationFile() : this(null)
        {
        }

        public ConfigurationFile(ConfigurationFile parent)
        {
            this.results = new Hashtable();
            this.parent = parent;
            this.unevaluatedSections = new Hashtable();
        }

        ~ConfigurationFile()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        private enum SectionType
        {
            NotFound = 0,
            Group = 1,
            Section = 2
        }

        internal string FileName
        {
            get { return this.configurationFile; }
        }

        internal string SectionName
        {
            get { return ConfigurationSettings.SectionName; }
        }

        /// <devdoc>
        /// The sections in the config file that has been unevaluated.
        /// </devdoc>
        internal Hashtable UnevaluatedSections
        {
            get { return this.unevaluatedSections; }
        }

        private Hashtable SectionHandlers
        {
            get
            {
                if (sectionHandlers == null)
                {
                    sectionHandlers = new Hashtable();
                }
                return sectionHandlers;
            }
        }

        public bool Load(string filename)
        {
            SetConfigurationFileName(filename);

            XmlTextReader reader = null;
            try
            {
                reader = OpenXmlTextReader(this.configurationFile);
                if (reader != null)
                {
                    ReadConfiguration(reader);
                    if (reader.Depth == 1)
                    {
                        VerifySections(reader, null);
                    }
                    return true;
                }
            }
            catch (XmlException e)
            {
                throw new ConfigurationException(e.Message, e, this.configurationFile, e.LineNumber);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return false;
        }

        /// <devdoc>
        /// Append a new section to the configSections of the configuration file given the 
        /// sectionName and typeName.  The objectData is
        /// </devdoc>        
        public void AppendSection(string sectionName, string typeName, XmlNode objectData)
        {
            ArgumentValidation.CheckForNullReference(sectionName, "sectionName");
            ArgumentValidation.CheckForNullReference(typeName, "typeName");
            ArgumentValidation.CheckForNullReference(objectData, "objectData ");
            if (this.configurationFile == null)
            {
                throw new InvalidOperationException(SR.ExceptionConfigFileNotLoaded);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(this.configurationFile);

            XmlNode configSections = doc.DocumentElement.SelectSingleNode(configSectionsXPath);
            if (configSections == null)
            {
                if (doc.DocumentElement.ChildNodes.Count == 0)
                {
                    configSections = doc.DocumentElement.AppendChild(doc.CreateElement(configSectionsElement));
                }
                else
                {
                    configSections = doc.DocumentElement.InsertBefore(doc.CreateElement(configSectionsElement), doc.DocumentElement.FirstChild);
                }

            }
            if (!SectionExists(configSections, sectionName))
            {
                XmlElement element = configSections.OwnerDocument.CreateElement(configSectionElement);
                element.SetAttribute(nameAttribute, sectionName);
                element.SetAttribute(typeAttribute, typeName);
                configSections.AppendChild(element);
            }
            AppendSectionData(sectionName, doc, objectData);
            if (this.unevaluatedSections == null)
            {
                this.unevaluatedSections = new Hashtable();
            }
            this.unevaluatedSections[sectionName] = typeName;
            doc.Save(this.configurationFile);
        }

        public void RemoveSection(string sectionName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.configurationFile);

            XmlNode configSections = doc.DocumentElement.SelectSingleNode(configSectionsXPath);
            if (configSections == null)
            {
                return;
            }

            if (!SectionExists(configSections, sectionName))
            {
                return;
            }

            XmlNode sectionNode = GetSectionNode(configSections, sectionName);
            configSections.RemoveChild(sectionNode);
            XmlNode sectionInformationNode = doc.DocumentElement.SelectSingleNode(string.Concat("//entlib:", sectionName), GetNamespaceManager(doc));
            doc.DocumentElement.RemoveChild(sectionInformationNode);
            doc.Save(this.configurationFile);
            this.unevaluatedSections.Remove(sectionName);
        }

        public object GetConfig(string configKey)
        {
            if (!this.results.Contains(configKey))
            {
                object objConfig = ResolveConfig(configKey);
                lock (this.results.SyncRoot)
                {
                    this.results[configKey] = objConfig;
                }
                return objConfig;
            }
            return this.results[configKey];
        }

        internal object ResolveConfig(string configKey)
        {
            // get local copy of reference for thread safety 
            // Instance reference could be set to null 
            // between !=null and .Contains().
            Hashtable unevaluatedSections = this.unevaluatedSections;
            if (unevaluatedSections != null && unevaluatedSections.Contains(configKey))
            {
                return Evaluate(configKey);
            }
            if (this.parent != null)
            {
                return this.parent.GetConfig(configKey);
            }
            return null;
        }

        internal IConfigurationSectionHandler GetSectionHandler(string configKey)
        {
            if ((this.sectionHandlers == null) || !(this.sectionHandlers.Contains(configKey)))
            {
                if (!this.shouldInheritSections && this.parent != null)
                {
                    return this.parent.GetSectionHandler(configKey);
                }
                return null;
            }
            object obj = this.sectionHandlers[configKey];
            if (obj == removedSectionObj)
            {
                return null;
            }
            IConfigurationSectionHandler sectionHandler = obj as IConfigurationSectionHandler;
            if (sectionHandler != null)
            {
                return sectionHandler;
            }
            sectionHandler = CreateSectionHandler(obj);
            lock (this.sectionHandlers.SyncRoot)
            {
                this.sectionHandlers[configKey] = sectionHandler;
            }
            return sectionHandler;
        }

        private static string CreateTagKey(string configKey, string tagName)
        {
            string tagKey = (configKey == null) ? tagName : configKey + "/" + tagName;
            return tagKey;
        }

        private static XmlTextReader OpenXmlTextReader(string configurationFile)
        {
            Uri file = new Uri(configurationFile);
            string localFileName = file.LocalPath;
            XmlTextReader reader = null;
            try
            {
                reader = OpenFileXmlTextReader(localFileName);
            }
            catch (XmlException e)
            {
                throw new ConfigurationException(SR.ExceptionLoadingXml(configurationFile, e.Message), localFileName, 0);
            }
            catch (IOException e)
            {
                throw new ConfigurationException(SR.ExceptionLoadingXml(configurationFile, e.Message), localFileName, 0);
            }
            return reader;
        }

        private static XmlTextReader OpenFileXmlTextReader(string localFileName)
        {
            XmlTextReader reader = new XmlTextReader(new FileStream(localFileName, FileMode.Open, FileAccess.Read));
            reader.MoveToContent();
            return reader;
        }

        public IConfigurationChangeWatcher CreateFileWatcher()
        {
            ConfigurationChangeFileWatcher fileWatcher = new ConfigurationChangeFileWatcher(configurationFile, ConfigurationSettings.SectionName);
            return fileWatcher;
        }

        private void RemoveUnresolvedConfig(string configKey)
        {
            Hashtable unevaluatedSections = this.unevaluatedSections;
            // instance reference could have gone away by now
            if (unevaluatedSections != null)
            {
                // only one writer to Hashtable at a time
                lock (unevaluatedSections.SyncRoot)
                {
                    unevaluatedSections.Remove(configKey);
                    if (unevaluatedSections.Count == 0)
                    {
                        // set instance reference to null when done
                        unevaluatedSections = null;
                    }
                }
            }
        }

        private object Evaluate(string configKey)
        {
            IConfigurationSectionHandler sectionHandler = GetSectionHandler(configKey);
            object parentResult = this.parent != null ? this.parent.GetConfig(configKey) : null;
            string[] keys = configKey.Split(new char[] {'/'});
            XmlTextReader reader = null;
            object result = null;
            try
            {
                reader = OpenXmlTextReader(this.configurationFile);
                result = EvaluateRecursive(sectionHandler, parentResult, keys, 0, reader);
            }
            catch (XmlException e)
            {
                throw new ConfigurationException(e.Message, e, this.configurationFile, e.LineNumber);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            RemoveUnresolvedConfig(configKey);
            return result;
        }

        private SectionType GetSectionType(string configKey)
        {
            if (this.sectionHandlers != null)
            {
                if (this.sectionHandlers.Contains(configKey))
                {
                    object obj = this.sectionHandlers[configKey];
                    if (obj == removedSectionObj)
                    {
                        return SectionType.NotFound;
                    }
                    if (obj == groupSectionObj)
                    {
                        return SectionType.Group;
                    }
                    return SectionType.Section;
                }
            }
            if (!this.shouldInheritSections && this.parent != null)
            {
                return this.parent.GetSectionType(configKey);
            }
            return SectionType.NotFound;
        }

        private IConfigurationSectionHandler CreateSectionHandler(object obj)
        {
            // if we still have a type string get the type and create the IConfigurationSectionHandler            
            string sectionHandlerTypeName = (string)obj;
            obj = null;
            try
            {
                Type t = Type.GetType(sectionHandlerTypeName);
                if (t != null)
                {
                    bool validSectionHandler = typeof(IConfigurationSectionHandler).IsAssignableFrom(t);
                    if (!validSectionHandler)
                    {
                        throw new ConfigurationException(SR.ExceptionNotICSH(sectionHandlerTypeName));
                    }
                    obj = Activator.CreateInstance(t, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
                }
            }
            catch (Exception e)
            {
                throw new ConfigurationException(e.Message, e);
            }

            if (obj == null)
            {
                throw new ConfigurationException(SR.ExceptionCouldNotCreateType(sectionHandlerTypeName));
            }
            IConfigurationSectionHandler sectionHandler = obj as IConfigurationSectionHandler;
            if (sectionHandler == null)
            {
                throw new ConfigurationException(SR.ExceptionNotICSH(sectionHandlerTypeName));
            }
            return sectionHandler;
        }

        private object EvaluateRecursive(IConfigurationSectionHandler sectionHandler, object config, string[] keys, int keyIndex, XmlTextReader reader)
        {
            string name = keys[keyIndex];
            int depth = reader.Depth;
            while (reader.Read() && reader.NodeType != XmlNodeType.Element)
            {
                ;
            }
            while (reader.Depth == depth + 1)
            {
                if (reader.Name == name)
                {
                    if (keyIndex < keys.Length - 1)
                    {
                        config = EvaluateRecursive(sectionHandler, config, keys, keyIndex + 1, reader);
                    }
                    else
                    {
                        XmlDocument doc = new XmlDocument();
                        XmlNode section = doc.ReadNode(reader);
                        config = sectionHandler.Create(config, null, section);
                    }
                    continue;
                }
                SkipToNextElement(reader);
            }
            return config;
        }

        private void ReadConfiguration(XmlTextReader reader)
        {
            reader.MoveToContent();
            if (reader.NodeType != XmlNodeType.Element || reader.Name != "configuration")
            {
                throw new ConfigurationException(SR.ExceptionNoRootConfigTag(this.configurationFile), configurationFile, reader.LineNumber);
            }
//            CheckForUnrecognizedAttributes(reader);
            ReadToNextElement(reader);
            if (reader.Depth == 1)
            {
                if (reader.Name == "configSections")
                {
                    CheckForUnrecognizedAttributes(reader);
                    ReadConfigSections(reader, null);
                }
            }
        }

        private void ReadSectionGroup(XmlTextReader reader, string configKey)
        {
            string tagName = null;
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    switch (reader.Name)
                    {
                        case "name":
                            tagName = reader.Value;
                            break;
                        case "type":
                            break;
                        default:
                            throw new ConfigurationException(SR.ExceptionUnrecognizedAttribute(reader.Name), this.configurationFile, reader.LineNumber);
                    }
                }
                reader.MoveToElement();
            }
            CheckRequiredAttribute(tagName, "name", reader);
            VerifySectionName(tagName, reader);
            string tagKey = CreateTagKey(configKey, tagName);
            if (SectionType.Section == GetSectionType(tagKey))
            {
                throw new ConfigurationException(SR.ExceptionSectionGroupIsDefined(tagKey), this.configurationFile, reader.LineNumber);
            }
            SectionHandlers[tagKey] = groupSectionObj;
            ReadConfigSections(reader, tagKey);
        }

        private void ReadSection(XmlTextReader reader, string configKey)
        {
            string tagName = null;
            string typeName = null;
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    switch (reader.Name)
                    {
                        case "name":
                            tagName = reader.Value;
                            break;
                        case "type":
                            typeName = reader.Value;
                            break;
                        case "allowLocation":
                        case "allowDefinition":
                        case "restartOnExternalChanges":
                        case "allowExeDefinition":
                            break;
                        default:
                            throw new ConfigurationException(SR.ExceptionUnrecognizedAttribute(reader.Name), this.configurationFile, reader.LineNumber);
                    }
                }
                reader.MoveToElement();
            }
            CheckRequiredAttribute(tagName, "name", reader);
            CheckRequiredAttribute(typeName, "type", reader);
            VerifySectionName(tagName, reader);
            string tagKey = CreateTagKey(configKey, tagName);
            if (GetSectionType(tagKey) != SectionType.NotFound)
            {
                throw new ConfigurationException(SR.ExceptionSectionGroupIsDefined(tagKey), this.configurationFile, reader.LineNumber);
            }
            SectionHandlers[tagKey] = typeName;
        }

        private void ReadRemove(XmlTextReader reader, string configKey)
        {
            string tagName = null;
            if (reader.HasAttributes)
            {
                while (reader.MoveToNextAttribute())
                {
                    if (reader.Name != "name")
                    {
                        throw new ConfigurationException(SR.ExceptionUnrecognizedAttribute(reader.Name), this.configurationFile, reader.LineNumber);
                    }
                    tagName = reader.Value;
                }
                reader.MoveToElement();
            }
            if (tagName == null)
            {
                throw new ConfigurationException(SR.ExceptionAttributeMissing("name", reader.Name), this.configurationFile, reader.LineNumber);
            }
            VerifySectionName(tagName, reader);
            string tagKey = CreateTagKey(configKey, tagName);
            if (GetSectionType(tagKey) != SectionType.Section)
            {
                throw new ConfigurationException(SR.ExceptionRemoveSectionHandler(tagName), this.configurationFile, reader.LineNumber);
            }
            SectionHandlers[tagName] = removedSectionObj;
        }

        private void ReadConfigSections(XmlTextReader reader, string configKey)
        {
            int depth = reader.Depth;
            ReadToNextElement(reader);
            while (reader.Depth == depth + 1)
            {
                switch (reader.Name)
                {
                    case "sectionGroup":
                        {
                            ReadSectionGroup(reader, configKey);
                            continue;
                        }
                    case "section":
                        {
                            ReadSection(reader, configKey);
                            break;
                        }
                    case "remove":
                        {
                            ReadRemove(reader, configKey);
                            break;
                        }
                    case "clear":
                        {
                            CheckForUnrecognizedAttributes(reader);
                            this.sectionHandlers = null;
                            this.shouldInheritSections = true;
                            break;
                        }
                    default:
                        throw new ConfigurationException(SR.ExceptionUnrecognizedElement, this.configurationFile, reader.LineNumber);
                }
                ReadToNextElement(reader);
                // unrecognized children are not allowed
                if (reader.Depth > depth + 1)
                {
                    throw new ConfigurationException(SR.ExceptionUnrecognizedElement, this.configurationFile, reader.LineNumber);
                }
            }
        }

        private void VerifySectionName(string tagName, XmlTextReader reader)
        {
            if (tagName.StartsWith("config"))
            {
                throw new ConfigurationException(SR.ExceptionConfigReserved, this.configurationFile, reader.LineNumber);
            }
            if (tagName == "location")
            {
                throw new ConfigurationException(SR.ExceptionLocationReserved, this.configurationFile, reader.LineNumber);
            }
        }

        private void VerifySections(XmlTextReader reader, string configKey)
        {
            int depth = reader.Depth;
            // only move to child nodes of not on first level (we've already passed the first <configsections>)
            if (configKey == null)
            {
                depth = 0;
            }
            else
            {
                ReadToNextElement(reader);
            }
            while (reader.Depth == depth + 1)
            {
                string tagName = reader.Name;
                string tagKey = CreateTagKey(configKey, tagName);
                SectionType sectionType = GetSectionType(tagKey);
                if (sectionType == SectionType.Group)
                {
                    VerifySections(reader, tagKey);
                    continue;
                }
                else if (sectionType == SectionType.NotFound)
                {
                    if (tagKey == "location")
                    {
                    }
                    else if (tagKey == "configSections")
                    {
                        throw new ConfigurationException(SR.ExceptionOnlyOneConfigSections, this.configurationFile, reader.LineNumber);
                    }
                    else
                    {
                        throw new ConfigurationException(SR.ExceptionUnregoconizedConfigSection(tagName), this.configurationFile, reader.LineNumber);
                    }
                }
                else
                {
                    if (this.unevaluatedSections == null)
                    {
                        this.unevaluatedSections = new Hashtable();
                    }
                    this.unevaluatedSections[tagKey] = null;
                }
                SkipToNextElement(reader);
            }
        }

        private void ReadToNextElement(XmlTextReader reader)
        {
            while (reader.Read())
            {
                // optimize for the common case
                if (reader.NodeType == XmlNodeType.Element)
                {
                    return;
                }
                CheckIgnorableNodeType(reader);
            }
        }

        private void SkipToNextElement(XmlTextReader reader)
        {
            reader.Skip();
            while (!reader.EOF && reader.NodeType != XmlNodeType.Element)
            {
                CheckIgnorableNodeType(reader);
                reader.Read();
            }
        }

        private void CheckIgnorableNodeType(XmlTextReader reader)
        {
            if (reader.NodeType != XmlNodeType.Comment
                && reader.NodeType != XmlNodeType.EndElement
                && reader.NodeType != XmlNodeType.Whitespace
                && reader.NodeType != XmlNodeType.SignificantWhitespace)
            {
                throw new ConfigurationException(SR.ExceptionUnrecognizedElement, this.configurationFile, reader.LineNumber);
            }
        }

        private void CheckForUnrecognizedAttributes(XmlTextReader reader)
        {
            if (reader.HasAttributes)
            {
                reader.MoveToNextAttribute();
                throw new ConfigurationException(SR.ExceptionUnrecognizedAttribute(reader.Name), this.configurationFile, reader.LineNumber);
            }
        }

        private void CheckRequiredAttribute(object o, string attribute, XmlTextReader reader)
        {
            if (o == null)
            {
                throw new ConfigurationException(SR.ExceptionAttributeMissing(attribute, reader.Name), this.configurationFile, reader.LineNumber);
            }
        }

        private void SetConfigurationFileName(string filename)
        {
            Uri uri = new Uri(filename);
            if (uri.Scheme == "file")
            {
                this.configurationFile = uri.LocalPath;
            }
            else
            {
                this.configurationFile = filename;
            }
        }

        /// <devdoc>
        /// Determines if a section exist in the config sections given the configSections node and the name of the section.
        /// </devdoc>        
        private bool SectionExists(XmlNode configSections, string sectionName)
        {
            return (GetSectionNode(configSections, sectionName) != null);
        }

        private static XmlNode GetSectionNode(XmlNode configSections, string sectionName)
        {
            XmlNode sectionNode = configSections.SelectSingleNode(string.Concat("//section[@name='", sectionName, "']"));
            return sectionNode;
        }

        private static void AppendSectionData(string sectionName, XmlDocument doc, XmlNode objectData)
        {
            XmlNamespaceManager nsmngr = GetNamespaceManager(doc);
            XmlNode root = doc.SelectSingleNode(string.Concat("//", rootNode));
            XmlNode oldNode = root.SelectSingleNode(string.Concat("//entlib:", sectionName), nsmngr);
            if (oldNode != null)
            {
                XmlNode newNode = doc.ImportNode(objectData, true);
                oldNode.ParentNode.ReplaceChild(newNode, oldNode);
            }
            else
            {
                XmlNode newNode = doc.ImportNode(objectData, true);
                root.AppendChild(newNode);
            }
        }

        internal static XmlNamespaceManager GetNamespaceManager(XmlDocument doc)
        {
            XmlNamespaceManager nsmngr = new XmlNamespaceManager(doc.NameTable);
            nsmngr.AddNamespace("entlib", ConfigurationSettings.ConfigurationNamespace);
            return nsmngr;
        }
    }
}