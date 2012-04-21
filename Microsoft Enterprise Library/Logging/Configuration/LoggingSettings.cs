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

using System.ComponentModel;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Configuration settings for client-side logging applications.
    /// </summary>
    [XmlRoot("enterpriseLibrary.loggingSettings", Namespace=LoggingSettings.ConfigurationNamespace)]
    public class LoggingSettings
    {
        private CategoryFilterDataCollection categoryFilters;
        private DistributionStrategyDataCollection distributionStrategies;

        private string name;
        private bool tracingEnabled;
        private bool loggingEnabled;
        private string distributionStrategy;
        private int minimumPriority;
        private CategoryFilterMode categoryFilterMode;

        /// <summary>
        /// Configuration section name for logging client settings.
        /// </summary>
        public const string SectionName = "loggingConfiguration";

        /// <summary>
        /// <para>Gets the Xml namespace for this root node.</para>
        /// </summary>
        /// <value>
        /// <para>The Xml namespace for this root node.</para>
        /// </value>
        public const string ConfigurationNamespace = "http://www.microsoft.com/practices/enterpriselibrary/08-31-2004/logging";

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingSettings"/> with default values.
        /// </summary>
        public LoggingSettings() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingSettings"/> using the given name.
        /// </summary>
        /// <param name="name">The name to use for this instance</param>
        public LoggingSettings(string name) : this(name, false, false, string.Empty, 0, CategoryFilterMode.AllowAllExceptDenied)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingSettings"/> using the given values
        /// </summary>
        /// <param name="name">The name to use for this instance</param>
        /// <param name="tracingEnabled">Should tracing be enabled?</param>
        /// <param name="loggingEnabled">Should logging be enabled?</param>
        /// <param name="distributionStrategy">The distribution strategy to use.</param>
        /// <param name="minimumPriority">The minimum value for messages to be processed.</param>
        /// <param name="categoryFilterMode">The category filter mode to use.</param>
        public LoggingSettings(string name, bool tracingEnabled, bool loggingEnabled, string distributionStrategy, int minimumPriority, CategoryFilterMode categoryFilterMode)
        {
            this.categoryFilters = categoryFilters;
            this.distributionStrategies = distributionStrategies;
            this.name = name;
            this.tracingEnabled = tracingEnabled;
            this.loggingEnabled = loggingEnabled;
            this.distributionStrategy = distributionStrategy;
            this.minimumPriority = minimumPriority;
            this.categoryFilterMode = categoryFilterMode;
            categoryFilters = new CategoryFilterDataCollection();
            distributionStrategies = new DistributionStrategyDataCollection();
        }

        /// <summary>
        /// Enable or disable all logging.
        /// </summary>
        [XmlAttribute("loggingEnabled")]
        public bool LoggingEnabled
        {
            get { return this.loggingEnabled; }
            set { this.loggingEnabled = value; }
        }

        /// <summary>
        /// Enable or disable trace logging.
        /// </summary>
        [XmlAttribute("tracingEnabled")]
        public bool TracingEnabled
        {
            get { return this.tracingEnabled; }
            set { this.tracingEnabled = value; }
        }

        /// <summary>
        /// One of <see cref="CategoryFilterMode"/> enumeration.
        /// </summary>
        [XmlAttribute("categoryFilterMode")]
        public CategoryFilterMode CategoryFilterMode
        {
            get { return this.categoryFilterMode; }
            set { this.categoryFilterMode = value; }
        }

        /// <summary>
        /// Distribution strategy determines how messages are processed.
        /// Either synchronously with a <see cref="EnterpriseLibrary.Logging.Distributor.InProcLogDistributionStrategy"/>
        /// or asychronously with a <see cref="EnterpriseLibrary.Logging.Distributor.MsmqLogDistributionStrategy"/>.
        /// </summary>
        [XmlAttribute("distributionStrategy")]
        public string DistributionStrategy
        {
            get { return this.distributionStrategy; }
            set { this.distributionStrategy = value; }
        }

        /// <summary>
        /// The minimum value for messages to be processed.  Messages with a priority
        /// below the minimum are dropped immediately on the client.
        /// </summary>
        [XmlAttribute("minimumPriority")]
        public int MinimumPriority
        {
            get { return this.minimumPriority; }
            set { this.minimumPriority = value; }
        }

        /// <summary>
        /// Name of the configuration node.
        /// </summary>
        [XmlAttribute("name")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Collection of <see cref="CategoryFilterData"/>.
        /// </summary>
        [XmlArray(ElementName="categoryFilters", Namespace=LoggingSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="categoryFilter", Type=typeof(CategoryFilterData), Namespace=LoggingSettings.ConfigurationNamespace)]
        public CategoryFilterDataCollection CategoryFilters
        {
            get { return this.categoryFilters; }
            set { this.categoryFilters = value; }
        }

        /// <summary>
        /// Collection of <see cref="DistributionStrategyData"/>.
        /// </summary>
        [XmlArray(ElementName="distributionStrategies", Namespace=LoggingSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="distributionStrategy", Type=typeof(DistributionStrategyData), Namespace=LoggingSettings.ConfigurationNamespace)]
        public DistributionStrategyDataCollection DistributionStrategies
        {
            get { return this.distributionStrategies; }
        }
    }
}