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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor
{
    /// <summary>
    /// Represents a factory for creating instances of classes that implement the <see cref="ILogDistributionStrategy"/> interface.
    /// </summary>
    public class LogDistributionStrategyFactory : ProviderFactory
    {
        /// <summary>
        /// Default constructor for a LogDistributionStrategy
        /// </summary>
        public LogDistributionStrategyFactory() : this(ConfigurationManager.GetCurrentContext())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="LogDistributionStrategyFactory"/> class
        /// with the specified <see cref="ConfigurationContext"/>.
        /// </summary>
        /// <param name="context"><see cref="ConfigurationContext"></see> provided by caller. The context should contain
        /// all configuration info needed to construct individual <see cref="ILogDistributionStrategy"></see> instances</param>
        public LogDistributionStrategyFactory(ConfigurationContext context) : base(SR.DistributionStrategyFactoryName, context, typeof (ILogDistributionStrategy))
        {
        }

        /// <summary>
        /// Creates the <see cref="ILogDistributionStrategy"/> object from the configuration 
        /// data associated with the specified name.
        /// </summary>
        /// <param name="distributionName">The name of the distribution strategy to create, as defined in configuration</param>
        /// <returns>An <see cref="ILogDistributionStrategy"/> object.</returns>
        public ILogDistributionStrategy CreateDistributionStrategy(string distributionName)
        {
            ArgumentValidation.CheckForNullReference(distributionName, "distributionName");
            ArgumentValidation.CheckForEmptyString(distributionName, "distributionName");

            return (ILogDistributionStrategy) base.CreateInstance(distributionName);
        }

        /// <summary>
        /// Factory method used to create the LoggingConfigurationView
        /// </summary>
        /// <returns>Specific LoggingConfigurationView used to configure the LogDistributor</returns>
        protected override ConfigurationView CreateConfigurationView()
        {
            return new LoggingConfigurationView(ConfigurationContext);
        }

        /// <summary>
        /// Get the configuration type for the type of <see cref="ILogDistributionStrategy"></see>
        /// </summary>
        /// <param name="distributionName">Name of distribution strategy as defined in configuration</param>
        /// <returns>A <see cref="Type"></see> instance describing the type of <see cref="ILogDistributionStrategy"></see> to be instantiated</returns>
        protected override Type GetConfigurationType(string distributionName)
        {
            LoggingConfigurationView loggingConfigurationView = (LoggingConfigurationView) CreateConfigurationView();
            DistributionStrategyData data = loggingConfigurationView.GetDistributionStrategyData(distributionName);
            return GetType(data.TypeName);
        }
    }
}