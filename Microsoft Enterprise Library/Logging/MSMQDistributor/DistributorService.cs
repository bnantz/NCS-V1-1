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
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    /// <summary>
    /// <para>This type supports the Data Access Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>    
    public class DistributorService : ServiceBase
    {
        internal const string DefaultApplicationName = "Enterprise Library Logging Distributor Service";

        private const string Header = "HEADER";

        private DistributorEventLogger eventLogger;
        private string applicationName;
        private ServiceStatus status;

        private MsmqListener queueListener;

        /// <summary/>
        /// <exclude/>
        public DistributorService()
        {
            InitializeComponent();

            base.CanStop = true;
            base.CanPauseAndContinue = true;
            base.CanStop = true;
            base.AutoLog = false;
        }

        /// <summary/>
        /// <exclude/>
        private static void Main()
        {
            ServiceBase[] servicesToRun = new ServiceBase[] {new DistributorService()};
            ServiceBase.Run(servicesToRun);
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Gets or sets the current status of the service.  Values are defined in <see cref="ServiceStatus"/> enumeration.
        /// </devdoc>
        public virtual ServiceStatus Status
        {
            get { return this.status; }
            set { this.status = value; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Gets or sets the name of the windows service.
        /// </devdoc>
        public string ApplicationName
        {
            get { return this.applicationName; }
            set { this.applicationName = value; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Gets the logger used to log events for this service.
        /// </devdoc>
        public DistributorEventLogger EventLogger
        {
            get { return this.eventLogger; }
        }

        internal MsmqListener QueueListener
        {
            get { return this.queueListener; }
            set { this.queueListener = value; }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// Initialization of the service.  Start the queue listener and write status to event log.
        /// </devdoc>
        public void InitializeComponent()
        {
            try
            {
                // Use the default settings for log name and application name.
                // This is done to ensure the windows service starts correctly.
                this.ApplicationName = DefaultApplicationName;

                this.eventLogger = new DistributorEventLogger();
                this.eventLogger.AddMessage(SR.InitializeComponentStartedMessage, SR.InitializeComponentStarted);
                this.status = ServiceStatus.OK;

                DistributorSettings distributorSettings = (DistributorSettings)ConfigurationManager.GetConfiguration(DistributorSettings.SectionName);

                this.queueListener = new MsmqListener(this, distributorSettings.DistributorService.QueueTimerInterval);

                //this.ApplicationName = this.ServiceName;
                this.ApplicationName = distributorSettings.DistributorService.ServiceName;
                this.eventLogger.AddMessage("name", this.ApplicationName);

                this.eventLogger.ApplicationName = this.ApplicationName;
                this.eventLogger.AddMessage(SR.InitializeComponentCompletedMessage, SR.InitializeComponentCompleted);
            }
            catch (LoggingException loggingException)
            {
                this.eventLogger.AddMessage(Header, SR.ServiceStartError(this.ApplicationName));

                this.eventLogger.WriteToLog(loggingException, Severity.Error);
                throw;
            }
            catch (Exception ex)
            {
                this.eventLogger.AddMessage(Header, SR.ServiceStartError(this.ApplicationName));

                this.eventLogger.WriteToLog(ex, Severity.Error);
                throw new LoggingException(SR.ErrorInitializingService, ex);
            }
            catch
            {
                throw new LoggingException(SR.ErrorInitializingService);
            }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The windows service start event.
        /// </devdoc>
        protected override void OnStart(string[] args)
        {
            try
            {
                SanityCheck sanityCheck = new SanityCheck(this);
                sanityCheck.StartCheckTimer();

                if (this.Status == ServiceStatus.OK)
                {
                    StartMsmqListener();

                    this.eventLogger.AddMessage(Header, SR.ServiceStartComplete(this.ApplicationName));

                    LoggingDistributorEvent.Fire(this.eventLogger.GetMessage(null), true);
                    this.eventLogger.WriteToLog(null, Severity.Information);
                }
            }
            catch (Exception e)
            {
                this.eventLogger.AddMessage(Header, SR.ServiceStartError(this.ApplicationName));

                this.eventLogger.WriteToLog(e, Severity.Error);
                this.Status = ServiceStatus.Shutdown;
            }
            catch
            {
                this.Status = ServiceStatus.Shutdown;
            }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The windows service stop event.
        /// </devdoc>
        protected override void OnStop()
        {
            try
            {
                StopMsmqListener();
            }
            catch (Exception e)
            {
                this.eventLogger.AddMessage(Header, SR.ServiceStopError(this.ApplicationName));

                this.eventLogger.WriteToLog(e, Severity.Error);
                this.Status = ServiceStatus.Shutdown;
            }
            catch
            {
                this.Status = ServiceStatus.Shutdown; 
            }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The windows service pause event.
        /// </devdoc>
        protected override void OnPause()
        {
            try
            {
                if (this.queueListener.StopListener())
                {
                    this.eventLogger.AddMessage(Header, SR.ServicePausedSuccess(this.ApplicationName));
                    LoggingDistributorEvent.Fire(this.eventLogger.GetMessage(null), false);
                    this.eventLogger.WriteToLog(null, Severity.Information);
                }
                else
                {
                    this.eventLogger.AddMessage(Header, SR.ServicePauseWarning(this.ApplicationName));

                    this.eventLogger.WriteToLog(null, Severity.Warning);
                }
            }
            catch (Exception e)
            {
                this.eventLogger.AddMessage(Header, SR.ServicePauseError(this.ApplicationName));

                this.eventLogger.WriteToLog(e, Severity.Error);
                this.Status = ServiceStatus.Shutdown;
            }
            catch
            {
                this.status = ServiceStatus.Shutdown;
            }
        }

        /// <summary/>
        /// <exclude/>
        /// <devdoc>
        /// The windows service resume event.
        /// </devdoc>
        protected override void OnContinue()
        {
            try
            {
                this.queueListener.StartListener();
                this.eventLogger.AddMessage(Header, SR.ServiceResumeComplete(this.ApplicationName));

                LoggingDistributorEvent.Fire(this.eventLogger.GetMessage(null), true);
                this.eventLogger.WriteToLog(null, Severity.Information);
            }
            catch (Exception e)
            {
                this.eventLogger.AddMessage(Header, SR.ServiceResumeError(this.ApplicationName));

                this.eventLogger.WriteToLog(e, Severity.Error);
                this.Status = ServiceStatus.Shutdown;
            }
            catch
            {
                this.status = ServiceStatus.Shutdown;
            }
        }

        private void StartMsmqListener()
        {
            try
            {
                this.eventLogger.AddMessage(SR.InitializeStartupSequenceStartedMessage, SR.ValidationStarted);

                this.queueListener.StartListener();

                this.eventLogger.AddMessage(SR.InitializeStartupSequenceFinishedMessage, SR.ValidationComplete);
            }
            catch
            {
                this.eventLogger.AddMessage(SR.InitializeStartupSequenceErrorMessage, SR.ValidationError);

                this.Status = ServiceStatus.Shutdown;
                throw;
            }
        }

        private void StopMsmqListener()
        {
            if (this.queueListener.StopListener())
            {
                this.eventLogger.AddMessage(Header, SR.ServiceStopComplete(this.ApplicationName));

                LoggingDistributorEvent.Fire(this.eventLogger.GetMessage(null), false);
                this.eventLogger.WriteToLog(null, Severity.Information);
            }
            else
            {
                this.eventLogger.AddMessage(Header, SR.ServiceStopWarning(this.ApplicationName));

                this.eventLogger.WriteToLog(null, Severity.Warning);
            }
        }
    }
}