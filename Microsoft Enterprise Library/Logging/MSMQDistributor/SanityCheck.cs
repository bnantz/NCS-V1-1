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
using System.ServiceProcess;
using System.Timers;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    /// <summary>
    /// Verifies that the distributor service is running as expected and force the service
    /// to shutdown if a shutdown is pending.
    /// </summary>
    internal class SanityCheck : MarshalByRefObject
    {
        private const int SanityInterval = 5000;
        private const string Header = "HEADER";

        private Timer timer;
        private DistributorEventLogger eventLogger;
        private DistributorService distributorService;

        public SanityCheck(DistributorService distributorService)
        {
            this.eventLogger = distributorService.EventLogger;
            this.distributorService = distributorService;
        }

        public void StartCheckTimer()
        {
            // This will stop the service from the moment ServiceStatus = SHUTDOWN.
            this.timer = new Timer();
            this.timer.Elapsed += new ElapsedEventHandler(OnSanityTimedEvent);
            this.timer.Interval = SanityInterval;
            this.timer.Enabled = true;
        }

        /// <summary>
        /// Stop the current service from running. 
        /// This method will only work if called from another application domain.
        /// </summary>
        public void StopService()
        {
            try
            {
                ServiceController myController =
                    new ServiceController(this.distributorService.ApplicationName);
                myController.Stop();
            }
            catch (Exception e)
            {
                this.eventLogger.AddMessage(Header,
                                            SR.ServiceControllerStopError((this.distributorService.ApplicationName)));
                this.eventLogger.WriteToLog(null, Severity.Error);

                throw new LoggingException(
                    SR.ServiceControllerStopException(this.distributorService.ApplicationName), e);
            }
            catch
            {
                throw new LoggingException(SR.ServiceControllerStopException(this.distributorService.ApplicationName));
            }
        }

        /// <devdoc>
        /// Event triggered by sanity event handler. 
        /// This method runs regularly to check sanity of the service.
        /// </devdoc>
        private void OnSanityTimedEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (this.distributorService.Status == ServiceStatus.Shutdown)
                {
                    ShutdownQueueListener();
                }
            }
            catch (Exception err)
            {
                this.eventLogger.AddMessage(Header, SR.ServiceUnableToShutdown);
                this.eventLogger.WriteToLog(err, Severity.Error);
                this.distributorService.Status = ServiceStatus.PendingShutdown;
            }
            catch
            {
                this.distributorService.Status = ServiceStatus.PendingShutdown;
            }
        }

        private void ShutdownQueueListener()
        {
            bool result = this.distributorService.QueueListener.StopListener();
            if (result)
            {
                AppDomain otherDomain = AppDomain.CreateDomain("otherDomain");
                otherDomain.DoCallBack(new CrossAppDomainDelegate(StopService));
            }
            else
            {
                this.eventLogger.AddMessage(Header, SR.ServiceStopError(this.distributorService.ApplicationName));
                this.eventLogger.WriteToLog(null, Severity.Warning);
            }
        }
    }
}