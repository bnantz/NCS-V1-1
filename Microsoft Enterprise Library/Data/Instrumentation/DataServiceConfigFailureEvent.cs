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

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// <para>
    /// This type supports the Data Access infrastructure and is not intended to be used directly from your code.
    /// </para>
    /// </summary>    
    public class DataServiceConfigFailureEvent : DataServiceFailureEvent
    {
        private static EventLogIdentifier[] eventLogIDs = new EventLogIdentifier[] {new EventLogIdentifier(EventLogEntryType.Error, (int)LogIndex.FailureOccurred)};
        private static DataServiceConfigFailureEvent conigFailuerEvent = new DataServiceConfigFailureEvent(eventLogIDs);
        private static object lockObj = new object();

        private string failedConfigurationFile;

        private DataServiceConfigFailureEvent(EventLogIdentifier[] eventLogIDs) : base(eventLogIDs)
        {
            this.failedConfigurationFile = string.Empty;
        }

        /// <summary/>
        /// <param name="message"/>
        /// <param name="failedConfigurationFile"/>
        /// <param name="ex"/>
        /// <exclude/>
        public static void Fire(string message, Exception ex, string failedConfigurationFile)
        {
            conigFailuerEvent.FireEvent(message, ex, failedConfigurationFile);
        }

        /// <summary/>
        /// <param name="message"/>
        /// <param name="ex"/>
        /// <exclude/>
        new public static void Fire(string message, Exception ex)
        {
            conigFailuerEvent.FireEvent(message, ex);
        }

        /// <summary/>
        /// <exclude/>
        public string ConfigurationFilePath
        {
            get { return AppDomain.CurrentDomain.SetupInformation.ConfigurationFile; }
        }

        /// <summary/>
        /// <exclude/>
        public string FailedConfigurationFile
        {
            get { return this.failedConfigurationFile; }
        }

        private void FireEvent(string message, Exception ex)
        {
            lock (lockObj)
            {
                Message = message;
                failedConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
                Exception = ex;
                ExceptionMessage = string.Empty;
                InstrumentedEvent.FireWmiEvent(this);
            }

            FireAuxEvent(string.Concat(message, SR.ExceptionMsg, ExceptionMessage));
        }

        private void FireEvent(string message, Exception ex, string failedConfigurationFile)
        {
            lock (lockObj)
            {
                Message = message;
                this.failedConfigurationFile = failedConfigurationFile;
                Exception = ex;
                ExceptionMessage = string.Empty;
                InstrumentedEvent.FireWmiEvent(this);
            }
            FireAuxEvent(SR.ExceptionMessageConfigurationLoadFailed(failedConfigurationFile, ExceptionMessage));
        }
    }
}