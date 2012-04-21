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
using System.Collections;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
    /// <summary>
    /// Represents a template based formatter for <see cref="LogEntry"/> messages.
    /// </summary>
    public class TextFormatter : ConfigurationProvider, ILogFormatter
    {
        private TextFormatterData textFormatterData;

        /// <summary>
        /// Message template containing tokens.
        /// </summary>
        private StringBuilder templateBuilder;

        /// <summary>
        /// Array of token formatters.
        /// </summary>
        private ArrayList tokenFunctions;

        private const string timeStampToken = "{timestamp}";
        private const string messageToken = "{message}";
        private const string categoryToken = "{category}";
        private const string priorityToken = "{priority}";
        private const string eventIdToken = "{eventid}";
        private const string severityToken = "{severity}";
        private const string titleToken = "{title}";

        private const string machineToken = "{machine}";
        private const string appDomainNameToken = "{appDomain}";
        private const string processIdToken = "{processId}";
        private const string processNameToken = "{processName}";
        private const string threadNameToken = "{threadName}";
        private const string win32ThreadIdToken = "{win32ThreadId}";

        private const string NewLineToken = "{newline}";
        private const string TabToken = "{tab}";

        /// <summary>
        /// Internal only for testing purposes
        /// </summary>
        internal StringBuilder TemplateBuilder
        {
            get { return templateBuilder; }
            set { templateBuilder = value; }
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="TextFormatter"></see>
        /// </summary>
        public TextFormatter() : this(new TextFormatterData())
        {
        }

        /// <summary>
        /// Initializes a new instance of a <see cref="TextFormatter"></see> with the given 
        /// <see cref="TextFormatterData"></see> configuration data
        /// </summary>
        /// <param name="textFormatterData">
        /// <para>A <see cref="TextFormatterData"/> object.</para>
        /// </param>
        public TextFormatter(TextFormatterData textFormatterData)
        {
            ArgumentValidation.CheckForNullReference(textFormatterData, "textFormatterData");

            this.textFormatterData = textFormatterData;
            RegisterTemplate();
        }

        /// <summary>
        /// Initialize a new instance of a <see cref="TextFormatter"/>.  Reads the template from
        /// configuration parameters.
        /// </summary>
        /// <param name="configurationView">Dynamic view into the configuration information needed by the <see cref="TextFormatter"></see></param>     
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(LoggingConfigurationView));

            LoggingConfigurationView loggingConfigurationView = (LoggingConfigurationView)configurationView;
            FormatterData formatterData = loggingConfigurationView.GetFormatterData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(textFormatterData, typeof(TextFormatterData));

            textFormatterData = (TextFormatterData)formatterData;
            RegisterTemplate();
        }

        private void RegisterTemplate()
        {
            this.templateBuilder = new StringBuilder(textFormatterData.Template.Value);
            RegisterTokenFunctions();
        }

        /// <summary>
        /// Gets the <see cref="TextFormatterData"/> from which
        /// this object was initialized.
        /// </summary>
        protected TextFormatterData FormatterData
        {
            get { return textFormatterData; }
        }

        /// <summary>
        /// Format the <see cref="LogEntry"/> object by replacing tokens with values.
        /// </summary>
        /// <param name="log">Log entry to format.</param>
        /// <returns>Formatted string with tokens replaced with property values.</returns>
        public virtual string Format(LogEntry log)
        {
            if (templateBuilder.Length == 0)
            {
                templateBuilder = new StringBuilder(SR.DefaultTextFormat);
            }

            templateBuilder.Replace(timeStampToken, log.TimeStampString);
            templateBuilder.Replace(titleToken, log.Title);
            templateBuilder.Replace(messageToken, log.Message);
            templateBuilder.Replace(categoryToken, log.Category);
            templateBuilder.Replace(eventIdToken, log.EventId.ToString(CultureInfo.InvariantCulture));
            templateBuilder.Replace(priorityToken, log.Priority.ToString(CultureInfo.InvariantCulture));
            templateBuilder.Replace(severityToken, log.Severity.ToString(CultureInfo.InvariantCulture));

            templateBuilder.Replace(machineToken, log.MachineName);
            templateBuilder.Replace(appDomainNameToken, log.AppDomainName);
            templateBuilder.Replace(processIdToken, log.ProcessId);
            templateBuilder.Replace(processNameToken, log.ProcessName);
            templateBuilder.Replace(threadNameToken, log.ManagedThreadName);
            templateBuilder.Replace(win32ThreadIdToken, log.Win32ThreadId);

            FormatTokenFunctions(log);

            templateBuilder.Replace(NewLineToken, Environment.NewLine);
            templateBuilder.Replace(TabToken, "\t");

            return templateBuilder.ToString();
        }

        private void FormatTokenFunctions(LogEntry log)
        {
            foreach (TokenFunction token in tokenFunctions)
            {
                token.Format(templateBuilder, log);
            }
        }

        private void RegisterTokenFunctions()
        {
            tokenFunctions = new ArrayList();
            tokenFunctions.Add(new DictionaryToken());
            tokenFunctions.Add(new KeyValueToken());
            tokenFunctions.Add(new TimeStampToken());
        }
    }
}