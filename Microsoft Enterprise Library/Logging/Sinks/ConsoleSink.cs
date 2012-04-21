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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
	/// <summary>
	/// Represents a flat file logging sink.  Writes the message in a text file format 
	/// on the local machine.  Path and text file name are retrieved from the configuration settings.
	/// </summary>
	public class ConsoleSink : LogSink
	{
		private LoggingConfigurationView loggingConfigurationView;

		/// <summary>
		/// Create an instance of a ConsoleSink.
		/// </summary>        
		public ConsoleSink()
		{
		}

		/// <summary>
		/// Initializes the sink with its name and any additional attributes. 
		/// </summary>  
		/// <param name="configurationView">Dynamic view into the configuration information needed by the <see cref="ConsoleSink"></see></param>     
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
         if (ValidateParameters())
         {
            try
            {
               WriteMessage(logEntry);
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

      private bool ValidateParameters()
      {
         ConsoleSinkData consoleSinkData = GetConsoleSinkDataFromCursor();

         bool valid = true;

         if (consoleSinkData.Header == null)
         {
            consoleSinkData.Header = "";
         }
         if (consoleSinkData.Footer == null)
         {
            consoleSinkData.Footer = "";
         }

         return valid;
      }

      private ConsoleSinkData GetConsoleSinkDataFromCursor()
      {
         SinkData sinkData = loggingConfigurationView.GetSinkData(ConfigurationName);
         ArgumentValidation.CheckExpectedType(sinkData, typeof (ConsoleSinkData));

         return (ConsoleSinkData) sinkData;
      }

      private void WriteMessage(LogEntry logEntry)
      {
         ConsoleSinkData consoleSinkDataData = GetConsoleSinkDataFromCursor();

         if (consoleSinkDataData.Header.Length > 0)
         {
            Debug.WriteLine(consoleSinkDataData.Header);
            Trace.WriteLine(consoleSinkDataData.Header);
         }
         Debug.WriteLine(FormatEntry(logEntry));
         Trace.WriteLine(FormatEntry(logEntry));
         if (consoleSinkDataData.Footer.Length > 0)
         {
            Debug.WriteLine(consoleSinkDataData.Footer);
            Trace.WriteLine(consoleSinkDataData.Footer);
         }
     }
	}
}