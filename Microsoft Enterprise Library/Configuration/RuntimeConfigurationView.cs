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
using System.Security.Permissions;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>Represents a view of the <see cref="ConfigurationSettings"/>.</para>
    /// </summary>
    public class RuntimeConfigurationView : ConfigurationView
    {

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="RuntimeConfigurationView"/> class with a <see cref="ConfigurationContext"/> object.</para>
        /// </summary>
        /// <param name="context">
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </param>
        public RuntimeConfigurationView(ConfigurationContext context) : base(context)
        {
        }

        /// <summary>
        /// <para>Gets the configuration file containing the configuration meta-data.</para>
        /// </summary>
        /// <value>
        /// <para>The configuration file containing the configuration meta-data.</para>
        /// </value>
        public string ConfigurationFile
        {
            get { return ConfigurationContext.ConfigurationFile; }
        }

        /// <summary>
        /// <para>Retrieves the <see cref="TransformerData"/> for the section.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the section to get the <see cref="TransformerData"/></para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="TransformerData"/> for the section.</para>
        /// </returns>
        public virtual TransformerData GetTransformerProviderData(string sectionName)
        {
            ConfigurationSettings settings = GetConfigurationSettings();

            ConfigurationSectionData configurationSectionData = settings.ConfigurationSections[sectionName];
            if (configurationSectionData == null)
            {
                InvalidSectionExceptionBuilder builder = new InvalidSectionExceptionBuilder(sectionName, ConfigurationContext.ConfigurationFile);
                throw builder.ThrowException();
            }

            return configurationSectionData.Transformer;
        }

        /// <summary>
        /// <para>Gets the <see cref="ConfigurationSettings"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="ConfigurationSettings"/>.</para>
        /// </returns>
        public virtual ConfigurationSettings GetConfigurationSettings()
        {
            return ConfigurationContext.GetMetaConfiguration();
        }

        /// <summary>
        /// <para>Gets the types used in the <see cref="XmlSerializer"/> for the section.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the section for the include types.</para>
        /// </param>
        /// <returns>
        /// <para>An array of types to use in the <see cref="XmlSerializer"/>.</para>
        /// </returns>
        public virtual Type[] GetXmlIncludeTypes(string sectionName)
        {
            XmlSerializerTransformerData xmlSerializerTransformerData = GetXmlSerializerTransformerData(sectionName);
            Type[] types = CollectTypes(xmlSerializerTransformerData.XmlIncludeTypes);

            return types;
        }

        /// <devdoc>
        /// Full demand needed to protect Type.GetType. LinkDemand insufficient because that only
        /// checks permissions of calling code, which would be GetXmlIncludeTypes, which has full 
        /// trust
        /// </devdoc>
        [ReflectionPermission(SecurityAction.Demand, TypeInformation = true)]
        internal Type[] CollectTypes(XmlIncludeTypeDataCollection xmlIncludeTypes)
        {
            Type[] types = new Type[xmlIncludeTypes.Count];

            int index = 0;
            foreach (XmlIncludeTypeData includeType in xmlIncludeTypes)
            {
                types[index++] = Type.GetType(includeType.TypeName, true);
            }

            return types;
        }

        private XmlSerializerTransformerData GetXmlSerializerTransformerData(string sectionName)
        {
            TransformerData transformerData = GetTransformerProviderData(sectionName);
            ArgumentValidation.CheckExpectedType(transformerData, typeof(XmlSerializerTransformerData));
            return (XmlSerializerTransformerData)transformerData;
        }

        /// <summary>
        /// <para>Retrieves the <see cref="StorageProviderData"/> for the section.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the section to get the <see cref="StorageProviderData"/></para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="StorageProviderData"/> for the section.</para>
        /// </returns>
        public virtual StorageProviderData GetStorageProviderData(string sectionName)
        {
            ConfigurationSettings settings = GetConfigurationSettings();
            
            ConfigurationSectionData configurationSectionData = settings.ConfigurationSections[sectionName];
            if (configurationSectionData == null)
            {
                InvalidSectionExceptionBuilder builder = new InvalidSectionExceptionBuilder(sectionName, ConfigurationFile);
                throw builder.ThrowException();
            }

            return configurationSectionData.StorageProvider;
        }

        /// <summary>
        /// <para>Retrieves the <see cref="ConfigurationProtector"/> for the section.</para>
        /// </summary>
        /// <param name="sectionName">
        /// <para>The name of the section to get the <see cref="ConfigurationProtector"/></para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="ConfigurationProtector"/> for the section.</para>
        /// </returns>
        public virtual ConfigurationProtector GetConfigurationProtector(string sectionName)
        {
            ConfigurationProtector protector = new ConfigurationProtector();
            protector.Load(ConfigurationContext, sectionName);
            return protector;
        }

        /// <summary>
        /// <para>Gets the <see cref="KeyAlgorithmPairStorageProviderData"/> for the configuration.</para>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="KeyAlgorithmPairStorageProviderData"/> for the configuration.</para>
        /// </returns>
        public virtual KeyAlgorithmPairStorageProviderData GetKeyAlgorithmPairStorageProviderData()
        {
            ConfigurationSettings settings = GetConfigurationSettings();
            return settings.KeyAlgorithmPairStorageProviderData;
        }
    }
}