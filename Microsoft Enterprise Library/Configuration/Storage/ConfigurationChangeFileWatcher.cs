//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Storage
{
    internal class ConfigurationChangeFileWatcher : IConfigurationChangeWatcher
    {
        private static readonly string eventSourceName = SR.FileWatcherEventSource;
        private string configurationSectionName;
        private string configFilePath;
        private int pollDelayInMilliseconds = defaultPollDelayInMilliseconds;
        private static int defaultPollDelayInMilliseconds = 15000;
        private static readonly object configurationChangedKey = new object();
        private Thread pollingThread;
        private EventHandlerList eventHandlers = new EventHandlerList();
        private DateTime lastWriteTime;

        // testing only methods
        internal static void SetDefaultPollDelayInMilliseconds(int newDefaultPollDelayInMilliseconds)
        {
            defaultPollDelayInMilliseconds = newDefaultPollDelayInMilliseconds;
        }

        internal static void ResetDefaultPollDelay()
        {
            defaultPollDelayInMilliseconds = 15000;
        }

        internal void SetPollDelayInMilliseconds(int newDelayInMilliseconds)
        {
            pollDelayInMilliseconds = newDelayInMilliseconds;
        }

        // end testing only methods

        public ConfigurationChangeFileWatcher(string configFilePath, string configurationSectionName)
        {
            this.configurationSectionName = configurationSectionName;
            this.configFilePath = configFilePath;
        }

        ~ConfigurationChangeFileWatcher()
        {
            Disposing(false);
        }

        public event ConfigurationChangedEventHandler ConfigurationChanged
        {
            add { eventHandlers.AddHandler(configurationChangedKey, value); }
            remove { eventHandlers.RemoveHandler(configurationChangedKey, value); }
        }

        public string SectionName
        {
            get { return configurationSectionName; }
        }

        public void StartWatching()
        {
            if (pollingThread == null)
            {
                pollingThread = new Thread(new ThreadStart(Poller));
                pollingThread.IsBackground = true;
                pollingThread.Start();
            }
        }

        public void StopWatching()
        {
            if (pollingThread != null)
            {
                pollingThread.Interrupt();
                pollingThread = null;
            }
        }

        public void Dispose()
        {
            Disposing(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Disposing(bool isDisposing)
        {
            if (isDisposing)
            {
                eventHandlers.Dispose();
                StopWatching();
            }
        }

        private void Poller()
        {
            lastWriteTime = DateTime.MinValue;
            while (true)
            {
                if (File.Exists(configFilePath) == true)
                {
                    if (lastWriteTime.Equals(DateTime.MinValue))
                    {
                        lastWriteTime = File.GetLastWriteTime(configFilePath);
                    }

                    DateTime currentLastWriteTime = File.GetLastWriteTime(configFilePath);
                    if (lastWriteTime.Equals(currentLastWriteTime) == false)
                    {
                        lastWriteTime = currentLastWriteTime;
                        OnConfigurationChanged();
                    }
                }

                try
                {
                    Thread.Sleep(pollDelayInMilliseconds);
                }
                catch (ThreadInterruptedException)
                {
                    return;
                }
            }

        }

        protected virtual void OnConfigurationChanged()
        {
            ConfigurationChangedEventHandler callbacks = (ConfigurationChangedEventHandler)eventHandlers[configurationChangedKey];
            ConfigurationChangedEventArgs eventData = new ConfigurationChangedEventArgs(Path.GetFullPath(configFilePath), configurationSectionName);

            try
            {
                if (callbacks != null)
                {
                    callbacks(this, eventData);
                }
            }
            catch (Exception e)
            {
                EventLog.WriteEntry(eventSourceName, SR.ExceptionEventRaisingFailed + ":" + e.Message);
            }
        }
    }
}