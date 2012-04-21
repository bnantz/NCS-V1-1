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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>
    /// Represents a command that will run the validation for the node the command encapsulates.
    /// </para>
    /// </summary>
    public class ValidateNodeCommand : ConfigurationNodeCommand
    {
        private bool validationSucceeded;
        private bool reportErrorsOnFailure;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ValidateNodeCommand"/> class with an <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public ValidateNodeCommand(IServiceProvider serviceProvider) : base(serviceProvider, true)
        {
            reportErrorsOnFailure = true;
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ValidateNodeCommand"/> class with an <see cref="IServiceProvider"/> and if the error service should be cleared after the command executes.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="clearErrorLog">
        /// <para>Determines if all the messages in the <see cref="IConfigurationErrorLogService"/> should be cleared when the command has executed.</para>
        /// </param>
        public ValidateNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog) : base(serviceProvider, clearErrorLog)
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ValidateNodeCommand"/> class with an <see cref="IServiceProvider"/>, if the error service should be cleared after the command executes and if the command should report the failures after executing.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        /// <param name="clearErrorLog">
        /// <para>Determines if all the messages in the <see cref="IConfigurationErrorLogService"/> should be cleared when the command has executed.</para>
        /// </param>
        /// <param name="reportErrorsOnFailure">
        /// <para>Determines if the command should report errors on failure.</para>
        /// </param>
        public ValidateNodeCommand(IServiceProvider serviceProvider, bool clearErrorLog, bool reportErrorsOnFailure) : base(serviceProvider, clearErrorLog)
        {
            this.reportErrorsOnFailure = reportErrorsOnFailure;
        }

        /// <summary>
        /// <para>Determines if the validation succeeded.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if the validation succeeded; otherwise, <see langword="false"/>.</para>
        /// <para>The default value is <see langword="false"/>.</para>
        /// </value>
        public bool ValidationSucceeded
        {
            get { return validationSucceeded; }
        }

        /// <summary>
        /// <para>Determines if a message should be shown when validation fails.</para>
        /// </summary>
        /// <value>
        /// <para><see langword="true"/> if a message should be shown when validation fails, <see langword="false"/>.</para>
        /// <para>The default value is <see langword="true"/>.</para>
        /// </value>
        public bool ReportErrorsOnFailure
        {
            get { return reportErrorsOnFailure; }
            set { reportErrorsOnFailure = value; }
        }

        /// <summary>
        /// <para>
        /// Executes the validation for the current node and all the child nodes.
        /// </para>
        /// </summary>
        /// <param name="node">
        /// <para>The <see cref="ConfigurationNode"/> to validate.</para>
        /// </param>
        protected override void ExecuteCore(ConfigurationNode node)
        {
            ValidationErrorCollection errors = new ValidationErrorCollection();
            Validate(node, errors);
            if (errors.Count > 0)
            {
                foreach(ValidationError error in errors)
                {
                    ConfigurationErrorLogService.LogError(error);    
                }
                
            }
            if (ConfigurationErrorLogService.ValidationErrors.Count > 0)
            {
                if (ConfigurationErrorLogService.ValidationErrors.Count > 0)
                {
                    UIService.DisplayErrorLog(ConfigurationErrorLogService);
                }

                if (reportErrorsOnFailure)
                {
                    UIService.ShowMessage(SR.ValidationErrorsMessage, SR.ValidationCaption);
                }
                ConfigurationErrorLogService.ClearErrorLog();
                validationSucceeded = false;
            }
            else
            {
                validationSucceeded = true;
            }
        }

        private void Validate(ConfigurationNode node, ValidationErrorCollection errors)
        {
            Type t = node.GetType();
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            ValidateProperties(node, properties, errors);
            ValidateChildNodeProperties(node, errors);
        }

        private void ValidateProperties(ConfigurationNode node, PropertyInfo[] properties, ValidationErrorCollection errors)
        {
            foreach (PropertyInfo property in properties)
            {
                ValidationAttribute[] validationAttributes = (ValidationAttribute[])property.GetCustomAttributes(typeof(ValidationAttribute), true);
                foreach (ValidationAttribute validationAttribute in validationAttributes)
                {
                    if (validationAttribute.IsActive)
                    {
                        validationAttribute.Validate(node, property, errors);
                    }
                }
            }
        }

        private void ValidateChildNodeProperties(ConfigurationNode node, ValidationErrorCollection errors)
        {
            foreach (ConfigurationNode childNode in node.Nodes)
            {
                Validate(childNode, errors);
            }
        }
    }
}