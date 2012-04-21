//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the root node for Exception Handling Settings
    /// </summary>
    [XmlRoot("enterpriseLibrary.exceptionHandlingSettings", Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
    public class ExceptionHandlingSettings
    {
        /// <summary>
        /// Configuration section name for exception handling settings.
        /// </summary>
        public const string SectionName = "exceptionHandlingConfiguration";

        private ExceptionPolicyDataCollection exceptionPolicies;

        /// <summary>
        /// <para>Gets the Xml namespace for this root node.</para>
        /// </summary>
        /// <value>
        /// <para>The Xml namespace for this root node.</para>
        /// </value>
        public const string ConfigurationNamespace = "http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/exceptionhandling";

        /// <summary>
        /// Creates a new instance of ExceptionHandlingSettings.
        /// </summary>
        public ExceptionHandlingSettings()
        {
            exceptionPolicies = new ExceptionPolicyDataCollection();
        }

//        /// <summary>
//        /// Gets the <see cref="ExceptionHandlingSettings"/> from the 
//        /// specified <see cref="ConfigurationContext"/>.
//        /// </summary>
//        /// <param name="context">A <see cref="ConfigurationContext"/>.</param>
//        /// <returns>An <see cref="ExceptionHandlingSettings"/> object.</returns>
//        public static ExceptionHandlingSettings GetSettings(ConfigurationContext context)
//        {
//            try
//            {
//                return context.GetConfig(SectionName) as ExceptionHandlingSettings;
//            }
//            catch (ConfigurationException ex)
//            {
//                ExceptionUtility.LogHandlingException(ex);
//                throw new ExceptionHandlingException(ex.Message, ex);
//            }
//        }

        /// <summary>
        /// Gets or sets the configured Exception Policies. 
        /// </summary>
        [XmlArray(ElementName="exceptionPolicies", Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="exceptionPolicy", Type=typeof(ExceptionPolicyData), Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
        public ExceptionPolicyDataCollection ExceptionPolicies
        {
            get { return exceptionPolicies; }
        }
    }
}