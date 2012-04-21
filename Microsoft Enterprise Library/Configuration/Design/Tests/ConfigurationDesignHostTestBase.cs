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

#if UNIT_TESTS
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    public abstract class ConfigurationDesignHostTestBase
    {
        private ConfigurationDesignHost host;
        private IUIHierarchyService hierarchyService;
        private IConfigurationErrorLogService configurationErrorLogService;
        private IContainer container;
        private int validationErrorsCount;
        private int configurationErrorsCount;
        private IXmlIncludeTypeService xmlIncludeTypeService;
        private INodeCreationService nodeCreationService;
        private ApplicationConfigurationNode applicationNode;
        private IUIHierarchy uiHierarchy;
        private ILinkNodeService linkNodeService;

        [SetUp]
        public virtual void SetUp()
        {
            host = new ConfigurationDesignHost();
            hierarchyService = (IUIHierarchyService)host.GetService(typeof(IUIHierarchyService));
            configurationErrorLogService = (IConfigurationErrorLogService)host.GetService(typeof(IConfigurationErrorLogService));
            xmlIncludeTypeService = (IXmlIncludeTypeService)host.GetService(typeof(IXmlIncludeTypeService));
            nodeCreationService = (INodeCreationService)host.GetService(typeof(INodeCreationService));
            container = (IContainer)host.GetService(typeof(IContainer));
            linkNodeService = (ILinkNodeService)host.GetService(typeof(ILinkNodeService));
            host.AddService(typeof(IUIService), new MyUIService(this));
            validationErrorsCount = 0;
            configurationErrorsCount = 0;
            applicationNode = new ApplicationConfigurationNode(ApplicationData.FromCurrentAppDomain());
            uiHierarchy = CreateHierarchyAndAddToHierarchyService(applicationNode, CreateDefaultConfiguration());
        }

        [TearDown]
        public virtual void TearDown()
        {
            host.Dispose();
        }

        protected ApplicationConfigurationNode GeneratedApplicationNode
        {
            get { return applicationNode; }
        }

        protected IUIHierarchy GeneratedHierarchy
        {
            get { return uiHierarchy; }
        }

        protected int ValidationErrorsCount
        {
            get { return validationErrorsCount; }
        }

        public int ConfigurationErrorsCount
        {
            get { return configurationErrorsCount; }
        }

        protected ConfigurationDesignHost Host
        {
            get { return host; }
        }

        protected IUIHierarchyService HierarchyService
        {
            get { return hierarchyService; }
        }

        protected IContainer Container
        {
            get { return container; }
        }

        protected IConfigurationErrorLogService ConfigurationErrorLogService
        {
            get { return configurationErrorLogService; }
        }

        protected IXmlIncludeTypeService XmlIncludeTypeService
        {
            get { return xmlIncludeTypeService; }
        }

        protected INodeCreationService NodeCreationService
        {
            get { return nodeCreationService; }
        }

        protected ILinkNodeService LinkNodeService
        {
            get { return linkNodeService; }
        }

        protected virtual IUIHierarchy CreateHierarchyAndAddToHierarchyService(ConfigurationNode node, ConfigurationContext configurationContext)
        {
            UIHierarchy hierarchy = new UIHierarchy(node, host, configurationContext);
            hierarchyService.AddHierarchy(hierarchy);
            return hierarchy;
        }

        protected object GetService(Type type)
        {
            return host.GetService(type);
        }

        protected ConfigurationContext CreateDefaultConfiguration()
        {
            return new ConfigurationContext(ApplicationData.FromCurrentAppDomain().ConfigurationFilePath);
        }

        protected class MyUIService : IUIService
        {

            public ConfigurationDesignHostTestBase fixture;

            public MyUIService(ConfigurationDesignHostTestBase fixture)
            {
                this.fixture = fixture;
            }

            public virtual void BeginUpdate()
            {
                Trace.WriteLine("BeginUpdate called");
            }

            public virtual void EndUpdate()
            {
                Trace.WriteLine("EndUpdate called");
            }

            public virtual DialogResult ShowSaveDialog(SaveFileDialog dialog)
            {
                Trace.WriteLine("ShowSaveDialog called");
                return DialogResult.OK;
            }

            public virtual DialogResult ShowOpenDialog(OpenFileDialog dialog)
            {
                Trace.WriteLine("ShowOpenDialog called");
                return DialogResult.OK;
            }

            public virtual IWin32Window OwnerWindow
            {
                get { return null; }
            }

            public virtual void ActivateNode(ConfigurationNode node)
            {
                Trace.WriteLine("ActivateNode:" + node.Name);
            }

            public void DisplayErrorLog(IConfigurationErrorLogService errorLogService)
            {
                foreach (ValidationError error in errorLogService.ValidationErrors)
                {
                    Trace.WriteLine("DisplayValidationErrors:" + error.Message);
                }
                fixture.validationErrorsCount = errorLogService.ValidationErrors.Count;

                foreach (ConfigurationError error in errorLogService.ConfigurationErrors)
                {
                    Trace.WriteLine("DisplayConfigurationErrors:" + error.Message);
                }
                fixture.configurationErrorsCount = errorLogService.ConfigurationErrors.Count;
            }

            public virtual void SetUIDirty(IUIHierarchy hierarchy)
            {
                Trace.WriteLine("Dirty called for " + hierarchy.Id.ToString());
            }

            public virtual bool IsDirty(IUIHierarchy hierarchy)
            {
                Trace.WriteLine("IsDirty called for " + hierarchy.Id.ToString());
                return false;
            }

            public virtual void SetStatus(string status)
            {
                Trace.WriteLine("SetStatus: " + status);
            }

            public virtual void ClearErrorDisplay()
            {
                Trace.WriteLine("ClearErrorDisplay called");
            }

            public virtual void ShowError(Exception e)
            {
                Trace.WriteLine("ShowError: " + e.Message);
            }

            public virtual void ShowError(Exception e, string message)
            {
                Trace.WriteLine("ShowError: Exception = " + e.Message + " message = " + message);
            }

            public virtual void ShowError(Exception e, string message, string caption)
            {
                Trace.WriteLine("ShowError: Exception = " + e.Message + " message = " + message + " caption = " + caption);
            }

            public virtual void ShowError(string message)
            {
                Trace.WriteLine("ShowError: message = " + message);
            }

            public virtual void ShowError(string message, string caption)
            {
                Trace.WriteLine("ShowError: message = " + message + " caption = " + caption);
            }

            public virtual void ShowMessage(string message)
            {
                Trace.WriteLine("ShowError: message = " + message);
            }

            public virtual void ShowMessage(string message, string caption)
            {
                Trace.WriteLine("ShowMessage: message = " + message + " caption = " + caption);
            }

            public virtual DialogResult ShowMessage(string message, string caption, MessageBoxButtons buttons)
            {
                Trace.WriteLine("ShowMessage: message = " + message + " caption = " + caption);
                return DialogResult.OK;
            }
        }

    }
}

#endif