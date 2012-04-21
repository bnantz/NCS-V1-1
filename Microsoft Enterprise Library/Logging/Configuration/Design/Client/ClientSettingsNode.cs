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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.CategoryFilters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client.DistributionStrategies;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Client
{
    /// <summary>
    /// Node for LoggingSettings
    /// </summary>
    [Image(typeof(ClientSettingsNode))]
    [ServiceDependency(typeof(ILinkNodeService))]
    public class ClientSettingsNode : ConfigurationNode
    {
        private DistributionStrategyNode distributionStrategyNode;
        private LoggingSettings loggingSettings;
        private ConfigurationNodeChangedEventHandler onDistributionStrategyNodeRemoved;
        private ConfigurationNodeChangedEventHandler onDistributionStrategyNodeRenamed;

        /// <summary>
        /// Creates node with initial configuration data.
        /// </summary>
        public ClientSettingsNode() : this(new LoggingSettings(SR.ClientSettings, DefaultValues.ClientTracingEnabled, DefaultValues.ClientLoggingEnabled, DefaultValues.ClientDistributionStrategy, DefaultValues.ClientMinimumPriority, DefaultValues.ClientCategoryFilterMode))
        {
        }

        /// <summary>
        /// Creates node with specified configuration data.
        /// </summary>
        /// <param name="loggingSettings">The specified configuration data.</param>
        public ClientSettingsNode(LoggingSettings loggingSettings) : base()
        {
            if (loggingSettings == null)
            {
                throw new ArgumentNullException("loggingSettings");
            }
            this.onDistributionStrategyNodeRemoved = new ConfigurationNodeChangedEventHandler(OnDistributionStrategyNodeRemoved);
            this.onDistributionStrategyNodeRenamed = new ConfigurationNodeChangedEventHandler(OnDistributionStrategyNodeRenamed);
            this.loggingSettings = loggingSettings;

        }

        /// <summary>
        /// The configured name.
        /// </summary>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <returns>Configuration data for this node.</returns>
        [Browsable(false)]
        public virtual LoggingSettings LoggingSettings
        {
            get
            {
                GetDistributionStrategies();
                return loggingSettings;
            }
        }


        /// <summary>
        /// Contains category filter settings.
        /// </summary>
        [Editor(typeof(CategoryFilterEditor), typeof(UITypeEditor))]
        [SRDescription(SR.Keys.CategoryFilterSettingsDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public CategoryFilterSettings CategoryFilterSettings
        {
            get
            {
                return new CategoryFilterSettings(
                    loggingSettings.CategoryFilterMode,
                    loggingSettings.CategoryFilters
                    );
            }
            set
            {
                loggingSettings.CategoryFilterMode = value.CategoryFilterMode;
                loggingSettings.CategoryFilters = value.CategoryFilters;
            }
        }

        /// <summary>
        /// Enables or disables trace logging.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.TracingEnabledDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public bool TracingEnabled
        {
            get { return loggingSettings.TracingEnabled; }
            set { loggingSettings.TracingEnabled = value; }
        }

        /// <summary>
        /// Enables or disables all logging.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.LoggingEnabledDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public bool LoggingEnabled
        {
            get { return loggingSettings.LoggingEnabled; }
            set { loggingSettings.LoggingEnabled = value; }
        }

        /// <summary>
        /// Defines the distribution strategy.
        /// <seealso cref="EnterpriseLibrary.Logging.Configuration.LoggingSettings.DistributionStrategy"/>
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.DistributionStrategyDescription)]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(DistributionStrategyNode))]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public DistributionStrategyNode DistributionStrategy
        {
            get { return distributionStrategyNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                distributionStrategyNode = (DistributionStrategyNode)service.CreateReference(distributionStrategyNode, value, onDistributionStrategyNodeRemoved, onDistributionStrategyNodeRenamed);
                loggingSettings.DistributionStrategy = distributionStrategyNode == null ? String.Empty : distributionStrategyNode.Name;
            }
        }

        /// <summary>
        /// The minimum value for messages to be processed.  Messages with a priority
        /// below the minimum are dropped immediately on the client.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.MinimumPriority)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public int MinimumPriority
        {
            get { return loggingSettings.MinimumPriority; }
            set { loggingSettings.MinimumPriority = value; }
        }

        /// <summary>
        /// Adds a <see cref="DistributionStrategyCollectionNode"/>.
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            base.AddDefaultChildNodes();
            DistributionStrategyCollectionNode node = new DistributionStrategyCollectionNode(loggingSettings.DistributionStrategies);
            Nodes.AddWithDefaultChildren(node);
            this.ResolveNodeReferences();
        }

        /// <summary>
        /// See <see cref="ConfigurationNode.ResolveNodeReferences"/>.
        /// </summary>
        public override void ResolveNodeReferences()
        {
            base.ResolveNodeReferences();
            DistributionStrategyCollectionNode nodes = Hierarchy.FindNodeByType(typeof(DistributionStrategyCollectionNode)) as DistributionStrategyCollectionNode;
            if (nodes == null) return;
            DistributionStrategy = Hierarchy.FindNodeByName(nodes, loggingSettings.DistributionStrategy) as DistributionStrategyNode;
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = this.loggingSettings.Name;
            if (loggingSettings.DistributionStrategies.Count > 0)
            {
                Nodes.Add(new DistributionStrategyCollectionNode(loggingSettings.DistributionStrategies));
            }
        }

        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems();
            ConfigurationMenuItem item = ConfigurationMenuItem.CreateAddChildNodeMenuItem(Site, this, typeof(DistributionStrategyCollectionNode), SR.DistributionStrategies, false);
            AddMenuItem(item);

        }


        protected override void OnChildAdding(ConfigurationNodeChangedEventArgs e)
        {
            base.OnChildAdding (e);
            if (Hierarchy.ContainsNodeType(this, e.Node.GetType()))
            {
                throw new InvalidOperationException(SR.ExceptionCollectionAlreadyExists);
            }
        }

        private void GetDistributionStrategies()
        {   
            DistributionStrategyCollectionNode distributionStrategyCollectionNode = Hierarchy.FindNodeByType(this, typeof(DistributionStrategyCollectionNode)) as DistributionStrategyCollectionNode;
            if (distributionStrategyCollectionNode == null) return;
            if (Object.ReferenceEquals(loggingSettings.DistributionStrategies, distributionStrategyCollectionNode.DistributionStrategyDataCollection)) return;

            loggingSettings.DistributionStrategies.Clear();
            foreach (DistributionStrategyData strategyNode in distributionStrategyCollectionNode.DistributionStrategyDataCollection)
            {
               loggingSettings.DistributionStrategies[strategyNode.Name] = strategyNode;
            }
        }

        private void OnDistributionStrategyNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.distributionStrategyNode = null;
        }

        private void OnDistributionStrategyNodeRenamed(object sender, ConfigurationNodeChangedEventArgs e)
        {
            loggingSettings.DistributionStrategy = e.Node.Name;
        }
    }
}