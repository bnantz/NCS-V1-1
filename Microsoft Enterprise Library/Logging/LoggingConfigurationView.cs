//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents a <see cref="LoggingConfigurationView"></see>-typed view into configuration data
    /// </summary>
    public class LoggingConfigurationView : ConfigurationView
    {
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="LoggingConfigurationView"/> with a <see cref="ConfigurationContext"/>.</para>
        /// </summary>
        /// <param name="context">
        /// <para>A <see cref="ConfigurationContext"/> object.</para>
        /// </param>
        public LoggingConfigurationView(ConfigurationContext context) : base(context)
        {
        }

        /// <summary>
        /// Gets the <see cref="DistributionStrategyData"></see> for the given distribution strategy name
        /// </summary>
        /// <param name="distributionName">Name of the <see cref="DistributionStrategyData"></see> found in configuration</param>
        /// <returns><see cref="DistributionStrategyData"></see> as read from configuration</returns>
        public virtual DistributionStrategyData GetDistributionStrategyData(string distributionName)
        {
            ArgumentValidation.CheckForNullReference(distributionName, "distributionName");
            ArgumentValidation.CheckForEmptyString(distributionName, "distributionName");

            LoggingSettings settings = GetLoggingSettings();
            if (settings.DistributionStrategies == null)
            {
                throw new ConfigurationException(SR.ExceptionNoStrategiesDefined);
            }
            DistributionStrategyData distributionStrategyData = settings.DistributionStrategies[distributionName];
            if (null == distributionStrategyData)
            {
                throw new ConfigurationException(SR.ExceptionNoDistributionStragegy(distributionName));
            }
            return distributionStrategyData;
        }

        /// <summary>
        /// Gets the <see cref="LoggingSettings"></see> from configuration
        /// </summary>
        /// <returns><see cref="LoggingSettings"></see> object that contains the logging-specific configuration information</returns>
        public virtual LoggingSettings GetLoggingSettings()
        {
            return (LoggingSettings) ConfigurationContext.GetConfiguration(LoggingSettings.SectionName);
        }

        /// <summary>
        /// Gets the <see cref="DistributorSettings"></see> from configuration
        /// </summary>
        /// <returns><see cref="DistributorSettings"></see> read from configuration</returns>
        public virtual DistributorSettings GetDistributorSettings()
        {
            return (DistributorSettings) ConfigurationContext.GetConfiguration(DistributorSettings.SectionName);
        }

        /// <summary>
        /// Gets the default <see cref="CategoryData"></see> defined for the logging category
        /// </summary>
        /// <returns><see cref="CategoryData"></see> defined for the default logging category</returns>
        public CategoryData GetDefaultCategoryData()
        {
            DistributorSettings settings = GetDistributorSettings();
            CategoryData defaultCategoryData = settings.CategoryDataCollection[settings.DefaultCategory];
            if (defaultCategoryData == null)
            {
                throw new ConfigurationException(SR.ExceptionCannotLoadDefaultCategory(settings.DefaultCategory));
            }
            return defaultCategoryData;
        }

        /// <summary>
        /// Gets the <see cref="CategoryData"></see> defined for the named logging category
        /// </summary>
        /// <param name="categoryName">Name of the logging category to retrieve</param>
        /// <returns><see cref="CategoryData"></see> defined for named logging category</returns>
        public CategoryData GetCategoryData(string categoryName)
        {
            ArgumentValidation.CheckForNullReference(categoryName, "categoryName");
            ArgumentValidation.CheckForEmptyString(categoryName, "categoryName");

            DistributorSettings settings = GetDistributorSettings();
            CategoryDataCollection categoryDataCollection = settings.CategoryDataCollection;

            // ok for the category data to be null
            return categoryDataCollection[categoryName];
        }

        /// <summary>
        /// Gets the <see cref="FormatterData"></see> defined for the named logging formatter
        /// </summary>
        /// <param name="formatterName">Name of the logging formatter to retrieve</param>
        /// <returns><see cref="FormatterData"></see> defined for named logging formatter</returns>
        public FormatterData GetFormatterData(string formatterName)
        {
            ArgumentValidation.CheckForNullReference(formatterName, "formatterName");
            ArgumentValidation.CheckForEmptyString(formatterName, "formatterName");

            DistributorSettings settings = GetDistributorSettings();
            FormatterDataCollection formatterDataCollection = settings.Formatters;

            // it is ok for a formatter to be null
            return formatterDataCollection[formatterName];
        }

        /// <summary>
        /// Gets the default <see cref="FormatterData"></see>
        /// </summary>
        /// <returns><see cref="FormatterData"></see> defined for default logging formatter</returns>
        public FormatterData GetDefaultFormatterData()
        {
            DistributorSettings settings = GetDistributorSettings();
            // it is ok if this is null
            return settings.Formatters[settings.DefaultFormatter];
        }

        /// <summary>
        /// Gets the <see cref="SinkData"></see> associated with the named logging sink
        /// </summary>
        /// <param name="sinkName">Name of the sink as defined in configuration</param>
        /// <returns><see cref="SinkData"></see> defined for named logging sink</returns>
        public virtual SinkData GetSinkData(string sinkName)
        {
            ArgumentValidation.CheckForNullReference(sinkName, "sinkName");
            ArgumentValidation.CheckForEmptyString(sinkName, "sinkName");

            DistributorSettings settings = GetDistributorSettings();
            
            SinkData sinkData = settings.SinkDataCollection[sinkName];
            if (null == sinkData)
            {
                throw new ConfigurationException(SR.ExceptionNoSinkDefined(sinkName));
            }
            return sinkData;
        }
    }
}