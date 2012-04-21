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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
	/// <summary>
	/// Represents a factory for creating <see cref="LogSink"/> objects.
	/// </summary>
    internal class LogSinkFactory : ProviderFactory
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LogSinkFactory"/> class.
		/// </summary>
        public LogSinkFactory(ConfigurationContext context) : base(SR.SinkFactoryName, context, typeof(ILogSink))
		{
		}

		/// <summary>
		/// Creates the <see cref="ILogSink"/> from the configuration data associated with the specified name.
		/// </summary>
        /// <returns>An <see cref="ILogSink"/>.</returns>
        public ILogSink CreateSink(string sinkName)
		{
            return (ILogSink)CreateInstance(sinkName);
		}

        protected override ConfigurationView CreateConfigurationView()
        {
            return new LoggingConfigurationView(ConfigurationContext);
	}

        protected override Type GetConfigurationType(string sinkName)
        {
        	LoggingConfigurationView loggingConfigurationView = (LoggingConfigurationView)CreateConfigurationView();
            SinkData data = loggingConfigurationView.GetSinkData(sinkName);
            return GetType(data.TypeName);
}
    }
}
