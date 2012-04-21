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

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Categories;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design
{
    /// <summary>
    /// Represents a node for the specific ExceptionHandler "<see cref="LoggingExceptionHandler"/>".
    /// </summary>
    public class LoggingExceptionHandlerNode : ExceptionHandlerNode
    {
        private const int eventID = 100;
        private static readonly string category = SR.DefaultCategory;
        private const Severity logSeverity = Severity.Error;
        private static readonly string formatterTypeName = typeof(TextExceptionFormatter).AssemblyQualifiedName;

        private LoggingExceptionHandlerData data;
        private CategoryNode defaultCategoryNode;
        private ConfigurationNodeChangedEventHandler onDefaultCategoryNodeRemoved;

        /// <summary>
        /// Constructs the node with default values.
        /// </summary>
        public LoggingExceptionHandlerNode() : this(new LoggingExceptionHandlerData(SR.LoggingHandlerName, category, eventID, logSeverity, SR.DefaultTitle, formatterTypeName))
        {
        }

        /// <summary>
        /// Constructs the node with config data.
        /// </summary>
        /// <param name="data">The config data to initialize this node.</param>
        public LoggingExceptionHandlerNode(LoggingExceptionHandlerData data) : base(data)
        {
            this.data = data;
            this.onDefaultCategoryNodeRemoved = new ConfigurationNodeChangedEventHandler(OnDefaultCategoryNodeRemoved);
        }

        /// <summary>
        /// Gets the assembly qualified type name of the logging exception handler.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return data.TypeName; }
        }

        /// <summary>
        /// <seealso cref="LoggingExceptionHandlerData.DefaultLogCategory"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.DefaultLogCategoryDescription)]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(CategoryNode))]
        public CategoryNode LogCategory
        {
            get { return defaultCategoryNode; }
            set
            {
                ILinkNodeService service = GetService(typeof(ILinkNodeService)) as ILinkNodeService;
                Debug.Assert(service != null, "Could not get the ILinkNodeService");
                this.defaultCategoryNode = (CategoryNode)service.CreateReference(defaultCategoryNode, value, onDefaultCategoryNodeRemoved, null);
                data.DefaultLogCategory = (defaultCategoryNode == null) ? String.Empty : defaultCategoryNode.Name;
            }
        }

        /// <summary>
        /// <seealso cref="LoggingExceptionHandlerData.DefaultEventID"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.DefaultEventIdDescription)]
        public int EventID
        {
            get { return data.DefaultEventID; }
            set { data.DefaultEventID = value; }
        }

        /// <summary>
        /// <seealso cref="LoggingExceptionHandlerData.DefaultSeverity"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.DefaultSeverityDescription)]
        public Severity Severity
        {
            get { return data.DefaultSeverity; }
            set { data.DefaultSeverity = value; }
        }

        /// <summary>
        /// <seealso cref="LoggingExceptionHandlerData.DefaultTitle"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.DefaultTitleDescription)]
        public string Title
        {
            get { return data.DefaultTitle; }
            set { data.DefaultTitle = value; }
        }

        /// <summary>
        /// <seealso cref="LoggingExceptionHandlerData.FormatterTypeName"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.FormatterTypeNameDescription)]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(ExceptionFormatter))]
        public string FormatterTypeName
        {
            get { return data.FormatterTypeName; }
            set { data.FormatterTypeName = value; }
        }

        /// <summary>
        /// <seealso cref="LoggingExceptionHandlerData.MinimumPriority"/>.
        /// </summary>
        [Required]
        [SRDescription(SR.Keys.MinimumPriorityDescription)]
        public int Priority
        {
            get { return data.MinimumPriority; }
            set { data.MinimumPriority = value; }
        }

        /// <summary>
        /// Initializes the Logging block if applicable.
        /// </summary>
        protected override void AddDefaultChildNodes()
        {
            CreateLoggingSettingsNode();
            ResolveNodeReferences();
            base.AddDefaultChildNodes();
        }

        /// <summary>
        /// Resolves the node references.
        /// </summary>
        public override void ResolveNodeReferences()
        {
            CategoryCollectionNode categoryCollectionNode = Hierarchy.FindNodeByType(typeof(CategoryCollectionNode)) as CategoryCollectionNode;
            Debug.Assert(categoryCollectionNode != null, "How is it that the categories are not there?");
            LogCategory = Hierarchy.FindNodeByName(categoryCollectionNode, this.data.DefaultLogCategory) as CategoryNode;
        }

        private void CreateLoggingSettingsNode()
        {
            if (!LoggingSettingsNodeExists())
            {
                AddConfigurationSectionCommand cmd = new AddConfigurationSectionCommand(Site, typeof(LoggingSettingsNode), LoggingSettings.SectionName);
                cmd.Execute(Hierarchy.RootNode);
            }
        }

        private bool LoggingSettingsNodeExists()
        {
            LoggingSettingsNode node = Hierarchy.FindNodeByType(typeof(LoggingSettingsNode)) as LoggingSettingsNode;
            return (node != null);
        }

        private void OnDefaultCategoryNodeRemoved(object sender, ConfigurationNodeChangedEventArgs e)
        {
            this.defaultCategoryNode = null;
        }
    }
}