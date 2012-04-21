//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if  UNIT_TESTS
using System.Data;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    [TestFixture]
    public abstract class ParameterDiscoveryFixture
    {
        protected Database db;
        protected DBCommandWrapper storedProcedure;
        protected ParameterCache cache;
        protected IDbConnection connection;

        public class TestCache : ParameterCache
        {
            public bool CacheUsed = false;

            protected override void AddParametersFromCache(DBCommandWrapper command)
            {
                CacheUsed = true;
                base.AddParametersFromCache(command);
            }
        }

        [TearDown]
        public void TearDown()
        {
            storedProcedure.Command.Connection.Dispose();
        }

        [Test]
        public void CanCreateStoredProcedureCommandWrapper()
        {
            Assert.AreEqual(storedProcedure.Command.CommandType, CommandType.StoredProcedure);
        }
    }
}

#endif