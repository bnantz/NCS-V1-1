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
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Storage
{
    /// <summary>
    /// <para>
    /// Represents a storage provider writer for configuration data that saves the configuration data to an external XML file.
    /// </para>
    /// </summary>
    public class XmlFileStorageProvider : StorageProvider, IStorageProviderWriter
    {
        private string applicationDocumentPath;
        private string currentDirectory;
        private RuntimeConfigurationView runtimeConfigurationView;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="XmlFileStorageProvider"/> class.</para>
        /// </summary>
        /// <seealso cref="XmlFileStorageProviderData"/>
        public XmlFileStorageProvider()
        {
            this.currentDirectory = string.Empty;
        }

        /// <summary>
        /// <para>Gets the name of the Xml file used as storage.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the Xml file used as storage.</para>
        /// </value>
        public string XmlFileName
        {
            get { return this.applicationDocumentPath; }
        }

        /// <summary>
        /// <para>Initializes this provider to the correct state and context used by the factory creating it.</para>
        /// </summary>
        /// <param name="configurationView">
        /// <para>The cursor to use to get the data specific for the transformer.</para>
        /// </param>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="configurationView"/> must be of type <see cref="EnterpriseLibrary.Configuration.RuntimeConfigurationView"/>.</para>
        /// <para>- or -</para>
        /// <para>The <see cref="StorageProviderData"/> from the cursor should be of type <see cref="XmlFileStorageProviderData"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="configurationView"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");

            GetStorageCursor(configurationView);
            StorageProviderData storageProviderData = runtimeConfigurationView.GetStorageProviderData(CurrentSectionName);
            ArgumentValidation.CheckExpectedType(storageProviderData, typeof(XmlFileStorageProviderData));

            XmlFileStorageProviderData xmlFileStorageProvider = (XmlFileStorageProviderData)storageProviderData;
            SetCurrentDirectory(runtimeConfigurationView);
            SetApplicationDocumentPathSetting(xmlFileStorageProvider);
        }

        /// <summary>
        /// <para>Creates a new <see cref="IConfigurationChangeWatcher"/> for this configuration storage.</para>
        /// </summary>
        /// <returns>
        /// <para>An <see cref="IConfigurationChangeWatcher"/> instance.</para>
        /// </returns>
        public override IConfigurationChangeWatcher CreateConfigurationChangeWatcher()
        {
            return new ConfigurationChangeFileWatcher(XmlFileName, CurrentSectionName);
        }

        private void SetCurrentDirectory(RuntimeConfigurationView RuntimeConfigurationView)
        {
            if (RuntimeConfigurationView.ConfigurationFile.Length == 0)
            {
                this.currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                this.currentDirectory = Path.GetDirectoryName(RuntimeConfigurationView.ConfigurationFile);
            }
        }

        /// <summary>
        /// <para>Reads the configuration from storage. The data will come back as an <see cref="XmlNode"/>.</para>
        /// </summary>        
        /// <returns>
        /// <para>The configuration data for the section.</para>
        /// </returns>
        /// <exception cref="ConfigurationException">
        /// <para>The signature check has failed for the section.</para>
        /// <para>- or -</para>
        /// <para>The data in the section could not be decrypted.</para>
        /// </exception>
        public override object Read()
        {
            XmlDocument xmlApplicationDocument = LoadXmlFile(this.applicationDocumentPath);
            XmlNode sectionNode = xmlApplicationDocument.SelectSingleNode(CurrentSectionName);
            if (sectionNode == null)
            {
                throw new ConfigurationException(SR.ExceptionXmlStorageSectionNotFoundError(CurrentSectionName, this.applicationDocumentPath));
            }
            sectionNode = sectionNode.CloneNode(true);
            // since we wrapped the section name here in the file storage provider
            return sectionNode.FirstChild;
        }

        /// <summary>
        /// Writes the configuration data to storage.
        /// </summary>
        /// <param name="value">
        /// <para>The value to write to storage. The type must be <see cref="XmlNode"/></para>
        /// </param>
        /// <exception cref="ConfigurationException">        
        /// <para>The data in the section could not be encrypted.</para>
        /// </exception>
        public void Write(object value)
        {
            ArgumentValidation.CheckForNullReference(value, "value");
            ArgumentValidation.CheckExpectedType(value, typeof(XmlNode));

            XmlNode valueNode = (XmlNode)value;
            XmlDocument xmlDoc = new XmlDocument();
            CreateXmlDeclaration(xmlDoc);
            XmlNode sectionNode = xmlDoc.CreateElement(CurrentSectionName);
            xmlDoc.AppendChild(sectionNode);
            CloneNode(valueNode, sectionNode);
            SaveData(xmlDoc);
        }

        private static void CreateXmlDeclaration(XmlDocument xmlDoc)
        {
            if (!(xmlDoc.FirstChild is XmlDeclaration))
            {
                XmlDeclaration declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                if (xmlDoc.HasChildNodes)
                {
                    xmlDoc.InsertBefore(declaration, xmlDoc.ChildNodes[0]);
                }
                else
                {
                    xmlDoc.AppendChild(declaration);
                }

            }
        }

        /// <devdoc>
        /// Load the xml file storage.
        /// </devdoc>        
        private XmlDocument LoadXmlFile(string fileName)
        {
            XmlDocument doc = new XmlDocument();
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (ConfigurationProtector protector = runtimeConfigurationView.GetConfigurationProtector(CurrentSectionName))
                    {
                        byte[] fileBytes = new byte[fs.Length];
                        fs.Read(fileBytes, 0, fileBytes.Length);
                        if (fileBytes.Length > 0)
                        {
                            fileBytes = protector.Decrypt(fileBytes);
                            doc.Load(new MemoryStream(fileBytes));
                        }
                    }
                }
                catch (XmlException e)
                {
                    throw new ConfigurationException(SR.ExceptionInvalidXmlStorageFile(fileName), e);
                }
            }
            

            return doc;
        }

        private void SaveData(XmlDocument xmlDoc)
        {
            
            using (ConfigurationProtector protector = this.runtimeConfigurationView.GetConfigurationProtector(CurrentSectionName))
            {
                if (!protector.Encrypted)
                {
                    xmlDoc.Save(this.applicationDocumentPath);
                }
                else
                {
                    using (FileStream fs = new FileStream(this.applicationDocumentPath, FileMode.Truncate, FileAccess.Write))
                    {
                        byte[] xmlBytes = GetEncoding(xmlDoc).GetBytes(xmlDoc.OuterXml);
                        xmlBytes = protector.Encrypt(xmlBytes);
                        fs.Write(xmlBytes, 0, xmlBytes.Length);
                        fs.Flush();
                    }
                }
            }
        }

        private static Encoding GetEncoding(XmlDocument xmlDoc)
        {
            if (xmlDoc.HasChildNodes)
            {
                XmlDeclaration dec = xmlDoc.FirstChild as XmlDeclaration;
                if (null != dec)
                {
                    return Encoding.GetEncoding(dec.Encoding);
                }
            }
            return new UTF8Encoding(false, true);
        }

        private static void CloneNode(XmlNode value, XmlNode sectionNode)
        {
            XmlNode cloneNode = sectionNode.OwnerDocument.ImportNode(value, true);
            sectionNode.AppendChild(cloneNode);
        }

        private void GetStorageCursor(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckExpectedType(configurationView, typeof(RuntimeConfigurationView));

            runtimeConfigurationView = (RuntimeConfigurationView)configurationView;
        }

        /// <devdoc>
        /// If no path is specified for the configuration file the default is to use the application configuration file.
        /// </devdoc>        
        private void SetApplicationDocumentPathSetting(XmlFileStorageProviderData storageProviderData)
        {
            this.applicationDocumentPath = storageProviderData.Path;
            if (this.applicationDocumentPath == null || this.applicationDocumentPath.Length == 0)
            {
                throw new ConfigurationException(SR.ExceptionConfigurationFileNotFound(string.Empty, CurrentSectionName));
            }
            try
            {
                if (!Path.IsPathRooted(this.applicationDocumentPath))
                {
                    this.applicationDocumentPath = Path.Combine(this.currentDirectory, this.applicationDocumentPath);
                }

            }
            catch (ArgumentException ex)
            {
                throw new ConfigurationException(SR.ExceptionConfigurationFileNotFound(applicationDocumentPath, CurrentSectionName), ex);
            }
            if (!File.Exists(this.applicationDocumentPath))
            {
                throw new ConfigurationException(SR.ExceptionConfigurationFileNotFound(applicationDocumentPath, CurrentSectionName));
            }
        }
    }
}
