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
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
    /// <summary>
    /// Represents a flat file logging sink.  Writes the message in a text file format 
    /// on the local machine.  Path and text file name are retrieved from the configuration settings.
    /// </summary>
    public class FlatFileSink : LogSink
    {
        private static object syncObject = new object();
        private LoggingConfigurationView loggingConfigurationView;
        private DefaultLogDestination defaultSink;

        /// <summary>
        /// Create an instance of a FlatFileSink.
        /// </summary>        
        public FlatFileSink()
        {
            this.defaultSink = new DefaultLogDestination();
        }

        /// <summary>
        /// Initializes the sink with its name and any additional attributes. 
        /// </summary>  
        /// <param name="configurationView">Dynamic view into the configuration information needed by the <see cref="FlatFileSink"></see></param>     
        public override void Initialize(ConfigurationView configurationView)
        {
            ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
            ArgumentValidation.CheckExpectedType(configurationView, typeof(LoggingConfigurationView));

            this.loggingConfigurationView = (LoggingConfigurationView)configurationView;
        }

        /// <summary>
        /// Append the log entry to the configured text file.
        /// </summary>
        /// <param name="logEntry"><see cref="LogEntry"></see> to be appended to logging file</param>
        protected override void SendMessageCore(LogEntry logEntry)
        {
            if (ValidateParameters(logEntry))
            {
                try
                {
                    WriteMessageToFile(logEntry);
                }
                catch (Exception e)
                {
                    logEntry.AddErrorMessage(SR.SinkFailure(e.ToString()));
                    throw;
                }
                catch
                {
                    logEntry.AddErrorMessage(SR.SinkFailure(SR.UnknownError));
                }
            }
        }

        private bool ValidateParameters(LogEntry logEntry)
        {
            FlatFileSinkData flatFileSinkData = GetFlatFileSinkDataFromCursor();

            bool valid = true;

            if (flatFileSinkData.Header == null)
            {
                flatFileSinkData.Header = "";
            }
            if (flatFileSinkData.Footer == null)
            {
                flatFileSinkData.Footer = "";
            }

            if (flatFileSinkData.FileName == null || flatFileSinkData.FileName.Length == 0)
            {
                valid = false;
                logEntry.AddErrorMessage(SR.FileSinkMissingConfiguration);
                this.defaultSink.SendMessage(logEntry);
            }

            return valid;
        }

        private FlatFileSinkData GetFlatFileSinkDataFromCursor()
        {
            SinkData sinkData = loggingConfigurationView.GetSinkData(ConfigurationName);
            ArgumentValidation.CheckExpectedType(sinkData, typeof (FlatFileSinkData));

            return (FlatFileSinkData) sinkData;
        }

        private void WriteMessageToFile(LogEntry logEntry)
        {
            FlatFileSinkData flatFileSinkData = GetFlatFileSinkDataFromCursor();

            string directory = Path.GetDirectoryName(flatFileSinkData.FileName);
            if (directory.Length == 0)
            {
                directory = AppDomain.CurrentDomain.BaseDirectory;
            }
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (FileStream fileStream = new FileStream(Path.Combine(directory,  flatFileSinkData.FileName),
                                                          FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    lock (syncObject)
                    {
                        if (flatFileSinkData.Header.Length > 0)
                        {
                            writer.WriteLine(flatFileSinkData.Header);
                        }
                        writer.WriteLine(FormatEntry(logEntry));
                        if (flatFileSinkData.Footer.Length > 0)
                        {
                            writer.WriteLine(flatFileSinkData.Footer);
                        }

                        writer.Flush();
                    }
                }
            }
        }
    }
}