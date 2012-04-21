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
    /// Represents the configuration settings for the distributor and the 
    /// MSMQ distributor windows service.
    /// </summary>
    [XmlRoot("enterpriseLibrary.loggingDistributorSettings", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class DistributorSettings
    {
        private string defaultCategory;
        private string defaultFormatter;

        private MsmqDistributorServiceData distributorServiceData;

        private SinkDataCollection sinkDataCollection;
        private CategoryDataCollection categoryDataCollection;
        private FormatterDataCollection formatters;

        /// <summary>
        /// Configuration section name for logging distributor settings.
        /// </summary>
        public const string SectionName = "loggingDistributorConfiguration";

        /// <summary>
        /// <para>Gets the Xml namespace for this root node.</para>
        /// </summary>
        /// <value>
        /// <para>The Xml namespace for this root node.</para>
        /// </value>
        public const string ConfigurationNamespace = "http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/loggingdistributor";

        /// <summary>
        /// Initialize a new instance of the <see cref="DistributorSettings"/>.
        /// </summary>
        public DistributorSettings() : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="DistributorSettings"/> using the given default category and formatter.
        /// </summary>
        /// <param name="defaultCategory">The name of the default category to use.</param>
        /// <param name="defaultFormatter">The name of the default formatter to use.</param>
        public DistributorSettings(string defaultCategory, string defaultFormatter)
        {
            this.defaultCategory = defaultCategory;
            this.defaultFormatter = defaultFormatter;
            this.sinkDataCollection = new SinkDataCollection();
            this.categoryDataCollection = new CategoryDataCollection();
            this.formatters = new FormatterDataCollection();
            this.distributorServiceData = new MsmqDistributorServiceData();
        }

        /// <summary>
        /// Default category to use if a category is not defined or is typed incorrectly.
        /// </summary>
        [XmlAttribute("defaultCategory")]
        public string DefaultCategory
        {
            get
            {
                if (this.defaultCategory == null)
                {
                    return "";
                }
                else
                {
                    return this.defaultCategory;
                }
            }
            set { this.defaultCategory = value; }
        }

        /// <summary>
        /// Default formatter to use if a formatter is not defined or is typed incorrectly.
        /// </summary>
        [XmlAttribute("defaultFormatter")]
        public string DefaultFormatter
        {
            get
            {
                if (this.defaultFormatter == null)
                {
                    return "";
                }
                else
                {
                    return this.defaultFormatter;
                }
            }
            set { this.defaultFormatter = value; }
        }

        /// <summary>
        /// Timer interval to poll the message queue in the MSMQ distributor windows service.
        /// </summary>
        [XmlElement("distributorService")]
        public MsmqDistributorServiceData DistributorService
        {
            get { return this.distributorServiceData; }
            set { this.distributorServiceData = value; }
        }

        /// <summary>
        /// Collection of sinks represented as <see cref="SinkData"/> configuration settings.
        /// </summary>
        [XmlArray(ElementName="sinks", Namespace=DistributorSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="sink", Type=typeof(SinkData), Namespace=DistributorSettings.ConfigurationNamespace)]
        public SinkDataCollection SinkDataCollection
        {
            get { return sinkDataCollection; }
        }

        /// <summary>
        /// Collection of categories represented as <see cref="CategoryData"/> configuration settings.
        /// </summary>
        [XmlArray(ElementName="categories", Namespace=DistributorSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="category", Type=typeof(CategoryData), Namespace=DistributorSettings.ConfigurationNamespace)]
        public CategoryDataCollection CategoryDataCollection
        {
            get { return categoryDataCollection; }
        }

        /// <summary>
        /// Collection of formatters represented as <see cref="FormatterData"/> configuration settings.
        /// </summary>
        [XmlArray(ElementName="formatters", Namespace=DistributorSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="formatter", Type=typeof(FormatterData), Namespace=DistributorSettings.ConfigurationNamespace)]
        public FormatterDataCollection Formatters
        {
            get { return formatters; }
        }
    }
}