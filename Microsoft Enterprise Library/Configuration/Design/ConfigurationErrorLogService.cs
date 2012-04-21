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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <devdoc>
    /// Implements the IConfigurationErrorLogService for logging errors in the design.
    /// </devdoc>
    internal class ConfigurationErrorLogService : IConfigurationErrorLogService
    {
        private ConfigurationErrorCollection configurationErrorCollection;
        private ValidationErrorCollection validationErrorCollection;

        public ConfigurationErrorLogService()
        {
            configurationErrorCollection = new ConfigurationErrorCollection();
            validationErrorCollection = new ValidationErrorCollection();
        }

        public ReadOnlyConfigurationErrorCollection ConfigurationErrors
        {
            get { return new ReadOnlyConfigurationErrorCollection(this.configurationErrorCollection); }
        }

        public ReadOnlyValidationErrorCollection ValidationErrors
        {
            get { return new ReadOnlyValidationErrorCollection(this.validationErrorCollection); }
        }

        public void ClearErrorLog()
        {
            validationErrorCollection.Clear();
            configurationErrorCollection.Clear();
        }

        public void LogError(ValidationError validationError)
        {
            validationErrorCollection.Add(validationError);
        }

        public void LogError(ConfigurationError configurationError)
        {
            configurationErrorCollection.Add(configurationError);
        }
    }
}