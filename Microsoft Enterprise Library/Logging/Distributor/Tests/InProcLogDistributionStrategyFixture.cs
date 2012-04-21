//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Tests
{
    [TestFixture]
    public class InProcLogDistributionStrategyFixture : ConfigurationContextFixtureBase 
    {
        [TearDown]
        public void TearDown()
        {
            MockLogSink.Clear();
        }

        [Test]
        public void InProcDistStrategyConstructor()
        {
            InProcLogDistributionStrategy ip = new InProcLogDistributionStrategy();
            ip.ConfigurationName = "InProc";
            ip.Initialize(new LoggingConfigurationView(Context));
            Assert.IsNotNull(ip);
        }

        [Test]
        public void SendMessageInProc()
        {
            InProcLogDistributionStrategy ip = new InProcLogDistributionStrategy();
            ip.ConfigurationName = "InProc";
            ip.Initialize(new LoggingConfigurationView(Context));
            LogEntry msg = CommonUtil.GetDefaultLogEntry();
            msg.Category = "MockCategoryOne";
            ip.SendLog(msg);
            Assert.AreEqual(CommonUtil.MsgBody, MockLogSink.GetLastEntry().Message);
        }
    }
}

#endif