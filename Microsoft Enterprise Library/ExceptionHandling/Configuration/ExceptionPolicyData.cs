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
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration for an <see cref="ExceptionPolicy"/>.
    /// </summary>
    [XmlRoot("exceptionPolicy", Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
    public class ExceptionPolicyData : ProviderData
    {
        private ExceptionTypeDataCollection exceptionTypes;

        /// <summary>
        /// Creates a new instance of ExceptionPolicyData.
        /// </summary>
        public ExceptionPolicyData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialzie a new instance of the <see cref="ExceptionPolicyData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="ExceptionPolicyData"/>.
        /// </param>
        public ExceptionPolicyData(string name) : base(name)
        {
            exceptionTypes = new ExceptionTypeDataCollection();
        }

        /// <summary>
        /// Overriden so that the XmlSerializer will ignore this property. Get a fixed value which is the type name of the <see cref="ExceptionPolicy"/>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(ExceptionPolicy).AssemblyQualifiedName; }
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the Exception Types associated with this Policy.
        /// </summary>
        [XmlArray(ElementName="exceptionTypes", Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="exceptionType", Type=typeof(ExceptionTypeData), Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
        public ExceptionTypeDataCollection ExceptionTypes
        {
            get { return exceptionTypes; }
        }
    }

}