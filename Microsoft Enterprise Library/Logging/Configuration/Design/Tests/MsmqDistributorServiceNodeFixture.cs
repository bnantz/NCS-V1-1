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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Tests
{
    [TestFixture]
    public class MsmqDistributorServiceNodeFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void Properties()
        {
            string msmqPath = @".\Private$\entlib";
            string serviceName = @"EntLib Distributor";
            int queueTimerInterval = 1000;

            MsmqDistributorServiceNode node = new MsmqDistributorServiceNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            node.MsmqPath = msmqPath;
            node.ServiceName = serviceName;
            node.QueueTimerInterval = queueTimerInterval;

            Assert.AreEqual(msmqPath, node.MsmqPath);
            Assert.AreEqual(serviceName, node.ServiceName);
            Assert.AreEqual(queueTimerInterval, node.QueueTimerInterval);
        }
    }
}

#endif