//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration
{
    /// <summary>
    /// Represents the configuration settings for a logging category. 
    /// </summary>
    [XmlRoot("category", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class CategoryData
    {
        private string name;
        private DestinationDataCollection destinationDataCollection;

        /// <summary>
        /// Initialize a new instance of <see cref="CategoryData"/>.
        /// </summary>
        public CategoryData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of <see cref="CategoryData"/>.
        /// </summary>
        /// <param name="name">Name associated with this logging category</param>
        public CategoryData(string name)
        {
            this.name = name;
            destinationDataCollection = new DestinationDataCollection();
        }

        /// <summary>
        /// Name of the category.
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Collection of <see cref="DestinationData"/> configuration settings. 
        /// </summary>
        [XmlArray(ElementName="destinations")]
        [XmlArrayItem(ElementName="destination", Type=typeof(DestinationData))]
        public DestinationDataCollection DestinationDataCollection
        {
            get { return destinationDataCollection; }
        }
    }
}