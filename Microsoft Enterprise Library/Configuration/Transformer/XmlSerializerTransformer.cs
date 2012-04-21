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
using System.Globalization;
using System.IO;
using System.Security.Permissions;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Transformer
{
    /// <summary>
    /// <para>Represents a specific configuration data builder for building Xml serializable objects.</para>
    /// </summary>
    [ReflectionPermission(SecurityAction.Demand, MemberAccess=true)]
    public class XmlSerializerTransformer : TransformerProvider
    {
        private Type[] types;

        /// <summary>
        /// <para>
        /// Intialize a new instance of the <see cref="XmlSerializerTransformer"/> class. 
        /// </para>
        /// </summary>
        /// <seealso cref="XmlSerializerTransformerData"/>
        public XmlSerializerTransformer()
        {
        }

        /// <summary>
        /// <para>Initializes this provider to the correct state and context used by the factory creating it.</para>
        /// </summary>
        /// <param name="configurationView">
        /// <para>The cursor to use to get the data specific for the transformer.</para>
        /// </param>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="configurationView"/> must be of type <see cref="RuntimeConfigurationView"/>.</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="configurationView"/> is a <see langword="null"/> reference (Nothing in Visual Basic).</para>
        /// </exception>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationCursor");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(RuntimeConfigurationView));

            RuntimeConfigurationView runtimeConfigurationView = (RuntimeConfigurationView)configurationView;
            types = runtimeConfigurationView.GetXmlIncludeTypes(CurrentSectionName);
        }

        /// <summary>
        /// <para>Gets the types to use when serializing and deserializing objects to Xml.</para>
        /// </summary>
        /// <returns><para>The types to use when serializing and deserializing objects to Xml.</para></returns>
        public Type[] GetTypes()
        {
            return types;
        }

        /// <summary>
        /// <para>
        /// Deserializes the configuration data coming from storage.
        /// </para>
        /// </summary>
        /// <param name="section">
        /// <para>The data that came from storage.</para>
        /// </param>
        /// <returns>
        /// <para>An object that can be consumed by the calling assembly that wants configuration data.</para>
        /// </returns>		
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="section"/> can not be <see langword="null"/>.</para>
        /// </exception>
        /// <exception cref="ConfigurationException">
        /// <para>The type could not be read from the serialized object.</para>
        /// <para>- or -</para>
        /// <para>The 'xmlSerializationSection' could not be read from the given Xml.</para>
        /// </exception>
        public override object Deserialize(object section)
        {
            ArgumentValidation.CheckForNullReference(section, "section");
            ArgumentValidation.CheckExpectedType(section, typeof(XmlNode));

            XmlNode sectionNode = (XmlNode)section;

            XmlNode serializationNode = sectionNode.SelectSingleNode("//xmlSerializerSection");
            if (serializationNode == null)
            {
                throw new ConfigurationException(SR.ExceptionNotInSerializedObj);
            }
            XmlAttribute typeAttribute = serializationNode.Attributes["type"];
            if (typeAttribute == null)
            {
                throw new ConfigurationException(SR.ExceptionSerializationTypeMissing);
            }
            string typeName = typeAttribute.Value;
            Type classType = null;
            try
            {
                classType = Type.GetType(typeName, true);
            }
            catch (TypeLoadException ex)
            {
                throw new ConfigurationException(SR.ExceptionTypeCreateError(typeName), ex);
            }
            catch (FileNotFoundException ex)
            {
                throw new ConfigurationException(SR.ExceptionTypeCreateError(typeName), ex);
            }
            if (serializationNode.ChildNodes.Count == 0)
            {
                throw new ConfigurationException(SR.ExceptionSerializedObjectMissing);
            }
            XmlSerializer xs = CreateXmlSerializer(classType);
            try
            {
                return xs.Deserialize(new XmlNodeReader(serializationNode.ChildNodes[0]));
            }
            catch (InvalidOperationException e)
            {
                string message = e.Message;
                if (null != e.InnerException)
                {
                    message = String.Concat(message, " ", e.InnerException.Message);
                }
                throw new ConfigurationException(message, e);
            }

        }

        /// <summary>
        /// <para>
        /// Serializes the configuration data coming from the calling assembly and maps it into something that the storage provider can understand.
        /// </para>
        /// </summary>
        /// <param name="value">
        /// <para>The data to serialize.</para>
        /// </param>
        /// <returns>
        /// <para>The object that can be consumed by the storage provider.</para>
        /// </returns>
        /// <exception cref="XmlException">
        /// <para>There is a load or parse error in the XML. In this case, the document remains empty.</para>
        /// </exception>
        public override object Serialize(object value)
        {
            XmlSerializer xmlSerializer = CreateXmlSerializer(value.GetType());
            StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
            XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
            XmlDocument doc = new XmlDocument();
            try
            {
                xmlTextWriter.WriteStartElement("xmlSerializerSection");
                xmlTextWriter.WriteAttributeString("type", value.GetType().AssemblyQualifiedName);
                xmlSerializer.Serialize(xmlTextWriter, value);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
                doc.LoadXml(stringWriter.ToString());
            }
            finally
            {
                xmlTextWriter.Close();
                stringWriter.Close();
            }
            return doc.DocumentElement;
        }

        private XmlSerializer CreateXmlSerializer(Type valueType)
        {
            if ((types != null) && (types.Length > 0))
            {
                return new XmlSerializer(valueType, types);
            }
            return new XmlSerializer(valueType);
        }

    }
}