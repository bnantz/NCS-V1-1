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

using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// Reads the "enterpriselibrary.configurationSettings" section of the configuration file. 
    /// </summary>
    public class ConfigurationManagerSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// <para>
        /// Initializes a new instance of the <see cref="ConfigurationManagerSectionHandler"/> class.
        /// </para>
        /// </summary>
        public ConfigurationManagerSectionHandler()
        {
        }

        /// <summary>
        /// <para>
        /// Evaluates the given XML section and returns a <see cref="ConfigurationSettings"/> instance that contains the results of the evaluation.
        /// </para>
        /// </summary>
        /// <param name="parent">
        /// <para>The configuration settings in a corresponding parent configuration section. </para>
        /// </param>
        /// <param name="configContext">
        /// <para>An HttpConfigurationContext when <see cref="Create"/> is called from the ASP.NET configuration system. Otherwise, this parameter is reserved and is a null reference (Nothing in Visual Basic). </para>
        /// </param>
        /// <param name="section">
        /// <para>The <see cref="XmlNode"/> that contains the configuration information to be handled. Provides direct access to the XML contents of the configuration section. </para>
        /// </param>
        /// <returns>
        /// <para>
        /// A <see cref="ConfigurationSettings"/> instance that contains the section's configuration settings.
        /// </para>
        /// </returns>
        public object Create(object parent, object configContext, XmlNode section)
        {
            ConfigurationSettings configurationSettings = new ConfigurationSettings();
            if (section == null)
            {
                return configurationSettings;
            }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConfigurationSettings));
            configurationSettings = (ConfigurationSettings)xmlSerializer.Deserialize(new XmlTextReader(new StringReader(section.OuterXml)));
            return configurationSettings;
        }
    }
}