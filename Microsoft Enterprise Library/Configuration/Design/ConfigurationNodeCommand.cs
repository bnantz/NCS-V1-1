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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a command to execute against a <see cref="ConfigurationNode"/>.</para>
    /// </summary>
    public abstract class ConfigurationNodeCommand : IDisposable
    {
        private static readonly object ExecutingEvent = new object();
        private static readonly object ExecutedEvent = new object();

        private EventHandlerList events;
        private IServiceProvider serviceProvider;
        private bool clearErrorLog;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationNodeCommand"/> class with an <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        protected ConfigurationNodeCommand(IServiceProvider serviceProvider) : this(serviceProvider, true)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="clearErrorLog">
        /// <para>Determines if all the messages in the <see cref="IConfigurationErrorLogService"/> should be cleared when the command has executed.</para>
        /// </param>
        protected ConfigurationNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }
            this.serviceProvider = serviceProvider;
            this.clearErrorLog = clearErrorLog;
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="ConfigurationBuilder"/> and optionally releases the managed resources.</para>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// <para>Releases the unmanaged resources used by the <see cref="ConfigurationBuilder"/> and optionally releases the managed resources.</para>
        /// </summary>
        /// <param name="disposing">
        /// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.</para>
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (null != events)
                {
                    events.Dispose();
                }
            }
        }

        /// <summary>
        /// <para>Occurs as the command is about to be executed.</para>
        /// </summary>
        public event CommandExecutingEventHandler Executing
        {
            add { Events.AddHandler(ExecutingEvent, value); }
            remove { Events.RemoveHandler(ExecutingEvent, value); }
        }

        /// <summary>
        /// <para>Occurs after the commmand has completed execution.</para>
        /// </summary>
        public event EventHandler Executed
        {
            add { Events.AddHandler(ExecutedEvent, value); }
            remove { Events.RemoveHandler(ExecutedEvent, value); }
        }

        /// <summary>
        /// <para>Determines if the <see cref="IConfigurationErrorLogService"/> is cleared after the command executes.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the log should be cleared; otherwise <see langword="false"/>.</para>
        /// </value>
        public bool ClearErrorLog
        {
            get { return clearErrorLog; }
            set { clearErrorLog = value; }
        }

        /// <summary>
        /// <para>Gets the list of event handlers that are attached to this command.</para>
        /// </summary>
        /// <value>
        /// <para>An <see cref="EventHandlerList"/> that provides the delegates for this component</para>
        /// </value>
        protected EventHandlerList Events
        {
            get
            {
                if (events == null)
                {
                    events = new EventHandlerList();
                }
                return events;
            }
        }

        /// <summary>
        /// <para>Gets the service provider for the command.</para>
        /// </summary>
        /// <value>
        /// <para>The service provider for the command.</para>
        /// </value>
        protected IServiceProvider ServiceProvider
        {
            get { return serviceProvider; }
        }

        /// <summary>
        /// <para>Gets the <see cref="IUIService"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="IUIService"/>.</para>
        /// </value>
        protected IUIService UIService
        {
            get { return ServiceHelper.GetUIService(serviceProvider); }
        }

        /// <summary>
        /// <para>Gets the <see cref="IUIHierarchyService"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="IUIHierarchyService"/>.</para>
        /// </value>
        protected IUIHierarchyService UIHierarchyService
        {
            get { return ServiceHelper.GetUIHierarchyService(serviceProvider); }
        }

        /// <summary>
        /// <para>Gets the currently selected <see cref="IUIHierarchy"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The currently selected <see cref="IUIHierarchy"/>.</para>
        /// </value>
        protected IUIHierarchy CurrentHierarchy
        {
            get { return ServiceHelper.GetCurrentHierarchy(serviceProvider); } 
        }

        /// <summary>
        /// <para>Gets the <see cref="IXmlIncludeTypeService"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="IXmlIncludeTypeService"/>.</para>
        /// </value>
        protected IXmlIncludeTypeService XmlIncludeTypeService
        {
            get { return ServiceHelper.GetXmlIncludeTypeService(serviceProvider); }
        }

        /// <summary>
        /// <para>Gets the <see cref="IConfigurationErrorLogService"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="IConfigurationErrorLogService"/>.</para>
        /// </value>
        protected IConfigurationErrorLogService ConfigurationErrorLogService
        {
            get { return ServiceHelper.GetConfigurationErrorService(serviceProvider); }
        }

        /// <summary>
        /// <para>Gets the <see cref="INodeNameCreationService"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="INodeNameCreationService"/>.</para>
        /// </value>
        protected INodeNameCreationService NodeNameCreationService
        {
            get { return ServiceHelper.GetNodeNameCreationService(serviceProvider); }
        }

        /// <summary>
        /// <para>Gets the <see cref="INodeCreationService"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="INodeCreationService"/>.</para>
        /// </value>
        protected INodeCreationService NodeCreationService
        {
            get { return ServiceHelper.GetNodeCreationService(serviceProvider); }
        }

        /// <summary>
        /// <para>Gets the <see cref="ILinkNodeService"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ILinkNodeService"/>.</para>
        /// </value>
        protected ILinkNodeService LinkNodeService
        {
            get { return ServiceHelper.GetLinkService(serviceProvider); }
        }

        /// <summary>
        /// <para>Executes the command.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to execute on the command on.</para>
        /// </param>
        public void Execute(ConfigurationNode node)
        {
            CommandExecutingEventArgs e = new CommandExecutingEventArgs();
            OnExecuting(e);
            if (e.Cancel)
            {
                return;
            }
            UIService.ClearErrorDisplay();
            ExecuteCore(node);
            OnExecuted(new EventArgs());
            ClearErrorsBeforeExitingCommand();
        }

        /// <summary>
        /// <para>When overridden by a class, executes the core logic of the command.</para>
        /// </summary>
        /// <param name="node">
        /// <para>The node to execute the command upon.</para>
        /// </param>
        protected abstract void ExecuteCore(ConfigurationNode node);

        /// <summary>
        /// <para>Raises the <see cref="Executing"/> event.</para>
        /// </summary>
        /// <param name="e">
        /// <para>The <see cref="CommandExecutingEventArgs"/> containing the event data.</para></param>
        protected virtual void OnExecuting(CommandExecutingEventArgs e)
        {
            if (events != null)
            {
                EventHandler handler = (EventHandler)events[ExecutingEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        /// <summary>
        /// <para>Raises the <see cref="Executed"/> event.</para>
        /// </summary>
        /// <param name="e"><para>An <see cref="EventArgs"/> containing the event data.</para></param>
        protected virtual void OnExecuted(EventArgs e)
        {
            if (events != null)
            {
               EventHandler handler = (EventHandler)events[ExecutedEvent];
                if (handler != null)
                {
                    handler(this, e);
                }
            }
        }

        /// <summary>
        /// <para>Retrieves the requested service.</para>
        /// </summary>
        /// <param name="serviceClass">
        /// <para>The type of the service to retrieve.</para>
        /// </param>
        /// <returns>
        /// <para>An instance of the service or <c>null</c> if no such service exists.</para>
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="serviceClass"/> is a null reference (Nothing in Visual Basic).</para>
        /// </exception>
        protected object GetService(Type serviceClass)
        {
            return serviceProvider.GetService(serviceClass);
        }

        /// <summary>
        /// <para>Log a <see cref="ConfigurationError"/> to the <see cref="IConfigurationErrorLogService"/>.</para>
        /// </summary>
        /// <param name="error">
        /// <para>The <see cref="ConfigurationError"/> to log.</para>
        /// </param>
        protected void LogError(ConfigurationError error)
        {
            ServiceHelper.LogError(serviceProvider, error);
        }

        /// <summary>
        /// <para>Log a <see cref="ValidationError"/> to the <see cref="IConfigurationErrorLogService"/>.</para>
        /// </summary>
        /// <param name="error">
        /// <para>The <see cref="ValidationError"/> to log.</para>
        /// </param>
        protected void LogError(ValidationError error)
        {
            ServiceHelper.LogError(serviceProvider, error);
        }

        private void ClearErrorsBeforeExitingCommand()
        {
            if (clearErrorLog == false)
            {
                return;
            }
            ConfigurationErrorLogService.ClearErrorLog();
        }

    }
}