//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
    /// <summary>
    /// Represents a factory for creating <see cref="ILogFormatter"/> objects.
    /// </summary>
    internal class LogFormatterFactory : ProviderFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogFormatterFactory"/> class.
        /// </summary>
        public LogFormatterFactory(ConfigurationContext context) : base(SR.FormatterFactoryName, context, typeof(ILogFormatter))
        {
        }

        /// <summary>
        /// Creates the <see cref="ILogFormatter"/> from the configuration data associated with the specified name.
        /// </summary>
        /// <returns>An <see cref="ILogFormatter"/>.</returns>
        public ILogFormatter CreateFormatter(string formatterName)
        {
            return (ILogFormatter)CreateInstance(formatterName);
        }

        protected override ConfigurationView CreateConfigurationView()
        {
            return new LoggingConfigurationView(ConfigurationContext);
        }

        protected override Type GetConfigurationType(string formatterName)
        {
        	LoggingConfigurationView loggingConfigurationView = (LoggingConfigurationView)CreateConfigurationView();
            FormatterData data = loggingConfigurationView.GetFormatterData(formatterName);
            return GetType(data.TypeName);
        }
    }

}