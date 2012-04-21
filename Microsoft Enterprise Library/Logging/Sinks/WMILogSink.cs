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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
    /// <summary>
    /// Represents a WMI logging sink.  Fires a LoggingWMISinkEvent event class in the 
    /// root\EnterpriseLibrary namespace in WMI.
    /// </summary>
    public class WMILogSink : LogSink
    {
        /// <summary>
        /// Creates an instance of the <see cref="WMILogSink"/> class.
        /// </summary>
        public WMILogSink()
        {
        }

        /// <summary>
        /// Initializes the sink with its name and any additional attributes. 
        /// </summary>  
        /// <param name="configurationView">Dynamic view into the configuration information needed by the <see cref="WMILogSink"></see></param>     
        public override void Initialize(ConfigurationView configurationView)
        {
        }

        /// <summary>
        /// Fire a new <see cref="LoggingWMISinkEvent"/> with the properties of the log entry.
        /// </summary>
        /// <param name="log">Log message to send.</param>
        protected override void SendMessageCore(LogEntry log)
        {
            try
            {
                LoggingWMISinkEvent.Fire(log);
            }
            catch (Exception e)
            {
                log.AddErrorMessage(SR.SinkFailure(e.ToString()));
                throw;
            }
            catch
            {
                log.AddErrorMessage(SR.SinkFailure(SR.UnknownError));
            }
        }
    }
}