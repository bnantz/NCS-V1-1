//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging
{
    /// <summary>
    /// Represents an <see cref="IExceptionHandler"/> that formats
    /// the exception into a log message sends it to
    /// the Enterprise Library Logging Block.
    /// </summary>	
    public class LoggingExceptionHandler : ExceptionHandler
    {
        private string defaultLogCategory;
        private int defaultEventId;
        private Severity defaultSeverity;
        private string defaultTitle;
        private string formatterTypeName;
        private int minimumPriority;
        private ConstructorInfo constructor;
        private LogWriter writer;
        private ExceptionHandlingConfigurationView exceptionHandlingConfigurationView;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="LoggingExceptionHandler"/> class.</para>
        /// </summary>
        public LoggingExceptionHandler()
        {
        }

        /// <summary>
        /// <para>When overridden by a class, initializes the provider with a <see cref="ExceptionHandlingConfigurationView"/>.</para>
        /// </summary>
        /// <param name="configurationView">
        /// <para>A <see cref="ExceptionHandlingConfigurationView"/> object.</para>
        /// </param>
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(ExceptionHandlingConfigurationView));

            exceptionHandlingConfigurationView = (ExceptionHandlingConfigurationView)configurationView;
        }

        /// <summary>
        /// <para>Handles the specified <see cref="Exception"/> object by formatting it and writting to the configured log.</para>
        /// </summary>
        /// <param name="exception"><para>The exception to handle.</para></param>
        /// <param name="policyName"><para>The name of the <see cref="ExceptionPolicy"/>.</para></param>
        /// <param name="handlingInstanceId">
        /// <para>The unique ID attached to the handling chain for this handling instance.</para>
        /// </param>
        /// <returns><para>Modified exception to pass to the next handler in the chain.</para></returns>
        public override Exception HandleException(Exception exception, string policyName, Guid handlingInstanceId)
        {
            SetupEnvironment();
            WriteToLog(CreateMessage(exception, handlingInstanceId));
            return exception;
        }

        /// <summary>
        /// Writes the specified log message using 
        /// the Logging framework's <see cref="Logger.Write(LogEntry)"/>
        /// method.
        /// </summary>
        /// <param name="logMessage">The message to write to the log.</param>
        protected virtual void WriteToLog(string logMessage)
        {
            LogEntry entry = new LogEntry(
                logMessage,
                defaultLogCategory,
                minimumPriority,
                defaultEventId,
                defaultSeverity,
                defaultTitle,
                null);
            this.writer.Write(entry);
        }

        /// <summary>
        /// Creates an instance of the <see cref="StringWriter"/>
        /// class using its default constructor.
        /// </summary>
        /// <returns>A newly created <see cref="StringWriter"></see></returns>
        protected virtual StringWriter CreateStringWriter()
        {
            return new StringWriter(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Creates an <see cref="ExceptionFormatter"/>
        /// object based on the configured ExceptionFormatter
        /// type name.
        /// </summary>
        /// <param name="writer">The stream to write to.</param>
        /// <param name="exception">The <see cref="Exception"/> to pass into the formatter.</param>
        /// <returns>A newly created <see cref="ExceptionFormatter"></see></returns>
        protected virtual ExceptionFormatter CreateFormatter(StringWriter writer, Exception exception)
        {
            return (ExceptionFormatter) constructor.Invoke(
                new object[] {writer, exception}
                );
        }

        private void SetupEnvironment()
        {
            ExceptionHandlerData exceptionHandlerData = exceptionHandlingConfigurationView.GetExceptionHandlerData(CurrentPolicyName, CurrentExceptionTypeName, ConfigurationName);
            ArgumentValidation.CheckExpectedType(exceptionHandlerData, typeof (LoggingExceptionHandlerData));

            InitProperties((LoggingExceptionHandlerData) exceptionHandlerData);

            Type formatterType = Type.GetType(formatterTypeName, true);
            Type[] types = new Type[] {typeof (TextWriter), typeof (Exception)};
            constructor = formatterType.GetConstructor(types);
            if (constructor == null)
            {
                throw new ExceptionHandlingException(
                    SR.MissingConstructor(formatterType.AssemblyQualifiedName)
                    );
            }

            this.writer = new LogWriter(exceptionHandlingConfigurationView.ConfigurationContext);
        }

        private void InitProperties(LoggingExceptionHandlerData data)
        {
            defaultLogCategory = data.DefaultLogCategory;
            defaultEventId = data.DefaultEventID;
            defaultSeverity = data.DefaultSeverity;
            defaultTitle = data.DefaultTitle;
            formatterTypeName = data.FormatterTypeName;
            minimumPriority = data.MinimumPriority;
        }

        private string CreateMessage(Exception exception, Guid handlingInstanceID)
        {
            StringWriter writer = null;
            StringBuilder stringBuilder = null;
            try
            {
                writer = CreateStringWriter();
                writer.WriteLine("HandlingInstanceID: {0}", handlingInstanceID.ToString());
                ExceptionFormatter formatter = CreateFormatter(writer, exception);
                formatter.Format();
                stringBuilder = writer.GetStringBuilder();

            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }

            return stringBuilder.ToString();
        }
    }
}