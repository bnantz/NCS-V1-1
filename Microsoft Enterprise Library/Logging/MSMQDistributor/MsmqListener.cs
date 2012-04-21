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
using System.Globalization;
using System.Threading;
using System.Timers;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor;
using Timer = System.Timers.Timer;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    /// <summary>
    /// Represents the message queue polling timer.  Uses an <see cref="MsmqLogDistributor"/> 
    /// to check for new log messages each timer interval.
    /// </summary>
    internal class MsmqListener
    {
        internal int QueueListenerRetries = 30;

        private const int DefaultQueueTimerInterval = 20000;
        private const int QueueListenerRetrySleepTime = 1000;
        private static int queueTimerInterval;

        private System.Timers.Timer queueTimer = null;
        private DistributorEventLogger eventLogger = null;

        private DistributorService distributorService;
        private MsmqLogDistributor logDistributor;

        /// <summary>
        /// Initialize a new instance of the <see cref="MsmqListener"/>.
        /// </summary>
        /// <param name="distributorService">Distributor service inheriting from <see cref="System.ServiceProcess.ServiceBase"/>.</param>
        /// <param name="timerInterval">Interval to check for new messages.</param>
        public MsmqListener(DistributorService distributorService, int timerInterval)
        {
            this.distributorService = distributorService;
            this.QueueTimerInterval = timerInterval;
            this.eventLogger = distributorService.EventLogger;

            this.logDistributor = new MsmqLogDistributor(ConfigurationManager.GetCurrentContext(), this.eventLogger);
        }

        /// <summary>
        /// Polling interval to check for new log messages.
        /// </summary>
        public int QueueTimerInterval
        {
            get
            {
                if (queueTimerInterval == 0)
                {
                    return DefaultQueueTimerInterval;
                }
                else
                {
                    return queueTimerInterval;
                }
            }

            set
            {
                if (value == 0)
                {
                    queueTimerInterval = DefaultQueueTimerInterval;
                }
                else
                {
                    queueTimerInterval = value;
                }
            }
        }

        /// <summary>
        /// Start the queue listener and begin polling the message queue.
        /// </summary>
        public virtual void StartListener()
        {
            try
            {
                this.eventLogger.AddMessage(SR.ListenerStartingMessage, SR.ListenerStarting);

                this.logDistributor.StopReceiving = false;

                if (this.queueTimer == null)
                {
                    this.queueTimer = new Timer();
                    this.queueTimer.Elapsed += new ElapsedEventHandler(OnQueueTimedEvent);
                }
                this.queueTimer.Interval = this.QueueTimerInterval;
                this.queueTimer.Enabled = true;

                this.eventLogger.AddMessage(SR.ListenerStartCompleteMessage, SR.ListenerStartComplete(this.QueueTimerInterval));
            }
            catch (Exception e)
            {
                this.eventLogger.AddMessage(SR.ListenerStartErrorMessage, SR.ListenerStartError);
                this.eventLogger.AddMessage("exception", e.Message);
                throw;
            }
            catch
            {
                this.eventLogger.AddMessage(SR.ListenerStartErrorMessage, SR.ListenerStartError);
                this.eventLogger.AddMessage("exception", SR.UnknownError);
                throw;
            }
        }

        /// <summary>
        /// Attempt to stop the queue listener and discontinue polling the message queue.
        /// </summary>
        /// <returns>True if the listener stopped succesfully.</returns>
        public virtual bool StopListener()
        {
            try
            {
                if (this.queueTimer != null)
                {
                    this.eventLogger.AddMessage(SR.ListenerStopStartedMessage, SR.ListenerStopStarted);

                    this.queueTimer.Enabled = false;
                    this.logDistributor.StopReceiving = true;

                    if (WaitUntilListenerStopped())
                    {
                        return true;
                    }

                    this.queueTimer.Enabled = true;
                    this.logDistributor.StopReceiving = false;
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                this.eventLogger.AddMessage(SR.ListenerStopErrorMessage, SR.ListenerStopError);
                this.eventLogger.AddMessage("exception", e.Message);
                throw;
            }
            catch
            {
                this.eventLogger.AddMessage(SR.ListenerStopErrorMessage, SR.ListenerStopError);
                this.eventLogger.AddMessage("exception", SR.UnknownError);
                throw;
            }
        }

        private bool WaitUntilListenerStopped()
        {
            int timeOut = 0;
            while (timeOut < QueueListenerRetries)
            {
                // Try to stop for QueueListenerRetries retries (1 second per retry)
                if (this.logDistributor.IsCompleted)
                {
                    this.eventLogger.AddMessage(SR.ListenerStopCompletedMessage,
                                                SR.ListenerStopCompleted(timeOut.ToString(CultureInfo.InvariantCulture)));

                    return true;
                }
                Thread.Sleep(QueueListenerRetrySleepTime);
                ++timeOut;
            }

            this.eventLogger.AddMessage(SR.StopListenerWarningMessage,
                                        SR.ListenerCannotStop(timeOut.ToString(CultureInfo.InvariantCulture)));

            return false;
        }

        /// <devdoc>
        /// support unit tests - allows for a mock object
        /// </devdoc>
        internal void SetMsmqLogDistributor(MsmqLogDistributor logDistributor)
        {
            this.logDistributor = logDistributor;
        }

        /// <devdoc>
        /// Event triggered by the queue timer event handler. 
        /// This method runs regularly to check the queue for pending queue messages.
        /// </devdoc>
        private void OnQueueTimedEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (this.logDistributor.IsCompleted &&
                    (this.distributorService.Status == ServiceStatus.OK))
                {
                    this.logDistributor.CheckForMessages();
                }
            }
            catch (Exception err)
            {
                this.eventLogger.AddMessage("HEADER", SR.QueueTimedEventError);
                this.eventLogger.WriteToLog(err, Severity.Error);
                this.distributorService.Status = ServiceStatus.Shutdown;
            }
            catch
            {
                this.distributorService.Status = ServiceStatus.Shutdown;
            }
        }
    }
}