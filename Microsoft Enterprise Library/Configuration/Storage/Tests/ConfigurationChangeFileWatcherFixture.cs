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

#if UNIT_TESTS
using System;
using System.IO;
using System.Threading;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Storage.Tests
{
    [TestFixture]
    public class ConfigurationChangeFileWatcherFixture
    {
        [SetUp]
        public void SetUp()
        {
            pollingException = null;
            configurationChangeCounter = 0;

            using (FileStream file = File.Create("MyFile.test"))
            {
                file.Close();
            }
        }

        [TearDown]
        public void TearDown()
        {
            File.Delete("MyFile.test");
        }

        [Test]
        public void NotifiesNothingIfNoOneListeningAndFileChanges()
        {
            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher(@".\MyFile.Test", "MySection"))
            {
                watcher.SetPollDelayInMilliseconds(100);
                watcher.StartWatching();

                File.SetLastWriteTime("MyFile.test", DateTime.Now + TimeSpan.FromHours(1.0));
                Thread.Sleep(250);
            }
        }

        [Test]
        public void NotifiesSingleListenerIfFileChanges()
        {
            string fullyQualifiedPath = Path.GetFullPath("MyFile.Test");
            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher(@"MyFile.Test", "MySection"))
            {
                watcher.SetPollDelayInMilliseconds(100);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
                watcher.StartWatching();
                Thread.Sleep(100);

                File.SetLastWriteTime("MyFile.test", DateTime.Now + TimeSpan.FromHours(1.0));
                Thread.Sleep(250);
            }

            Assert.AreEqual(1, configurationChangeCounter);
            Assert.AreEqual("MySection", storedEventArgs.SectionName);
            Assert.AreEqual(fullyQualifiedPath.ToLower(), storedEventArgs.ConfigurationFile.ToLower());
        }

        [Test]
        public void NotifiesMultipleListenersIfFileChanges()
        {
            string fullyQualifiedPath = Path.GetFullPath("MyFile.Test");
            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher(fullyQualifiedPath, "MySection"))
            {
                watcher.SetPollDelayInMilliseconds(100);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
                watcher.StartWatching();
                Thread.Sleep(100);

                File.SetLastWriteTime("MyFile.test", DateTime.Now + TimeSpan.FromHours(1.0));
                Thread.Sleep(250);
            }

            Assert.AreEqual(2, configurationChangeCounter);
        }

        [Test]
        public void WillStopNotifyingAfterObjectIsDisposed()
        {
            ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher("MyFile.Test", "MySection");
            watcher.SetPollDelayInMilliseconds(100);
            watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
            watcher.StartWatching();
            Thread.Sleep(100);
            watcher.Dispose();

            File.SetLastWriteTime("MyFile.Test", DateTime.Now + TimeSpan.FromHours(1.0));
            Thread.Sleep(250);

            Assert.AreEqual(0, configurationChangeCounter);
        }

        [Test]
        public void StopsNotifyingHandlersWhenAnyHandlerThrowsException()
        {
            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher(@"MyFile.Test", "MySection"))
            {
                watcher.SetPollDelayInMilliseconds(100);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(ExceptionThrowingCallback);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
                watcher.StartWatching();
                Thread.Sleep(100);

                File.SetLastWriteTime("MyFile.test", DateTime.Now + TimeSpan.FromHours(1.0));
                Thread.Sleep(250);
            }

            Assert.AreEqual(1, configurationChangeCounter);
        }

        [Test]
        public void WillStopCallingHandlerAfterThatHandlerHasBeenRemoved()
        {
            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher(@"MyFile.Test", "MySection"))
            {
                watcher.SetPollDelayInMilliseconds(100);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(DifferentFileChangedCallback);
                watcher.StartWatching();
                Thread.Sleep(100);

                watcher.ConfigurationChanged -= new ConfigurationChangedEventHandler(DifferentFileChangedCallback);

                File.SetLastWriteTime("MyFile.test", DateTime.Now + TimeSpan.FromHours(1.0));
                Thread.Sleep(250);
            }

            Assert.AreEqual(1, configurationChangeCounter);
        }

        [Test]
        public void NoCallbackWillHappenAfterLastEventHandlerIsRemoved()
        {
            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher(@"MyFile.Test", "MySection"))
            {
                watcher.SetPollDelayInMilliseconds(100);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(DifferentFileChangedCallback);
                watcher.StartWatching();
                watcher.ConfigurationChanged -= new ConfigurationChangedEventHandler(DifferentFileChangedCallback);

                File.SetLastWriteTime("MyFile.test", DateTime.Now + TimeSpan.FromHours(1.0));
                Thread.Sleep(250);
            }

            Assert.AreEqual(0, configurationChangeCounter);
        }

        [Test]
        public void NoCallbackWillHappenOnFileDelete()
        {
            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher(@"MyFile.Test", "MySection"))
            {
                watcher.SetPollDelayInMilliseconds(100);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
                watcher.StartWatching();

                File.Delete("MyFile.Test");

                Thread.Sleep(250);
            }

            Assert.AreEqual(0, configurationChangeCounter);
        }

        [Test]
        public void CreatingNewWatchedFileDoesNotCauseCallback()
        {
            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher(@"Different.Test", "MySection"))
            {
                watcher.SetPollDelayInMilliseconds(100);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
                watcher.StartWatching();

                using (FileStream differentFile = new FileStream("Different.Test", FileMode.Create))
                {
                    Thread.Sleep(50);
                    differentFile.Close();
                    Thread.Sleep(250);
                }
            }

            File.Delete("Different.Test");

            Assert.AreEqual(0, configurationChangeCounter);
        }

        [Test]
        public void OverwritingExistingWatchedFileCausesOnlyOneCallback()
        {
            if (File.Exists("Different.Test"))
            {
                File.Delete("Different.Test");
            }
            using (FileStream differentFile = new FileStream("Different.Test", FileMode.CreateNew))
            {
                differentFile.Close();
            }

            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher(@"Different.Test", "MySection"))
            {
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
                watcher.SetPollDelayInMilliseconds(100);
                watcher.StartWatching();
                Thread.Sleep(100);

                ChangeFile("Different.Test");
                Thread.Sleep(250);
            }

            File.Delete("Different.Test");

            Assert.AreEqual(1, configurationChangeCounter);
        }

        [Test]
        public void ChangesToNewlyCreatedWatchedFileDoCauseCallback()
        {
            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher(@"Different.Test", "MySection"))
            {
                watcher.SetPollDelayInMilliseconds(100);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
                watcher.StartWatching();
                Thread.Sleep(100);

                using (FileStream differentFile = File.Create("Different.Test"))
                {
                    differentFile.Close();
                }

                Thread.Sleep(150);

                ChangeFile("Different.Test");

                Thread.Sleep(250);
            }

            File.Delete("Different.Test");

            Assert.AreEqual(1, configurationChangeCounter);
        }

        [Test]
        public void NotificationsDoNotAccumulateWhileCallbacksAreHappening()
        {
            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher(@"MyFile.Test", "MySection"))
            {
                watcher.SetPollDelayInMilliseconds(300);
                watcher.ConfigurationChanged += new ConfigurationChangedEventHandler(FileChanged);
                watcher.StartWatching();
                Thread.Sleep(100);

                ChangeFile("MyFile.test");
                Thread.Sleep(50);

                ChangeFile("MyFile.test");
                Thread.Sleep(450);
            }

            Assert.AreEqual(1, configurationChangeCounter);
        }

        [Test]
        public void StoppingPollingDoesNotThrowException()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(PollingThreadExceptionCatcher);
            using (ConfigurationChangeFileWatcher watcher = new ConfigurationChangeFileWatcher("MyFile.Test", "MySection"))
            {
                watcher.StartWatching();
                Thread.Sleep(100);
                watcher.StopWatching();
                Thread.Sleep(1000);
            }
            AppDomain.CurrentDomain.UnhandledException -= new UnhandledExceptionEventHandler(PollingThreadExceptionCatcher);

            Assert.IsNull(pollingException);
        }

        private void PollingThreadExceptionCatcher(object sender, UnhandledExceptionEventArgs e)
        {
            pollingException = (Exception)e.ExceptionObject;
        }

        private void FileChanged(object sender, ConfigurationChangedEventArgs eventData)
        {
            configurationChangeCounter++;
            storedEventArgs = eventData;
        }

        private void ExceptionThrowingCallback(object sender, ConfigurationChangedEventArgs eventData)
        {
            throw new Exception();
        }

        private void DifferentFileChangedCallback(object sender, ConfigurationChangedEventArgs eventData)
        {
            configurationChangeCounter++;
        }

        private void ChangeFile(string fileName)
        {
            using (StreamWriter stream = File.AppendText(fileName))
            {
                stream.Write(DateTime.Now.Ticks);
                stream.Flush();
            }
        }

        private int configurationChangeCounter;
        private ConfigurationChangedEventArgs storedEventArgs;
        private Exception pollingException;
    }
}

#endif