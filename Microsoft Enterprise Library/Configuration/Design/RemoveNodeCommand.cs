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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a command that will remove a node from it's associated <see cref="IUIHierarchy"/>.</para>
    /// </summary>
    public class RemoveNodeCommand : ConfigurationNodeCommand
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="RemoveNodeCommand"/> class with an <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public RemoveNodeCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="RemoveApplicationConfigurationNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="clearErrorLog">
        /// <para>Determines if all the messages in the <see cref="IConfigurationErrorLogService"/> should be cleared when the command has executed.</para>
        /// </param>
        public RemoveNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog) : base(serviceProvider, clearErrorLog)
        {
        }

        /// <summary>
        /// <para>Removes the node from the <see cref="IUIHierarchy"/>.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to execute the command upon.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            try
            {
                UIService.BeginUpdate();
                UIService.SetUIDirty(node.Hierarchy);
                node.Remove();
                using(RemoveXmlIncludeTypesCommand cmd = new RemoveXmlIncludeTypesCommand(ServiceProvider))
                {
                    cmd.Execute(node);
                }
            }
            finally
            {
                UIService.EndUpdate();
            }
        }
    }
}