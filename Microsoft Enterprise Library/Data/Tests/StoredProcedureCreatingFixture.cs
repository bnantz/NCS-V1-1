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
using System;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    /// <summary>
    /// Use the Data block to execute a create a stored procedure script using ExecNonQuery.
    /// </summary>    
    public abstract class StoredProcedureCreatingFixture
    {
        protected Database db;

        [SetUp]
        public virtual void SetUp()
        {
            this.db.ClearParameterCache();
            CreateStoredProcedure();
        }

        [TearDown]
        public void TearDown()
        {
            DeleteStoredProcedure();
            this.db.ClearParameterCache();
        }

        [Test]
        public void CanGetOutputValueFromStoredProcedure()
        {
            DBCommandWrapper storedProcedure = this.db.GetStoredProcCommandWrapper("TestProc", null, "ALFKI");
            this.db.ExecuteNonQuery(storedProcedure);

            int resultCount = Convert.ToInt32(storedProcedure.GetParameterValue("vCount"));
            Assert.AreEqual(6, resultCount);
        }

        [Test]
        public void CanGetOutputValueFromStoredProcedureWithCachedParameters()
        {
            DBCommandWrapper storedProcedure = this.db.GetStoredProcCommandWrapper("TestProc", null, "ALFKI");
            this.db.ExecuteNonQuery(storedProcedure);

            DBCommandWrapper duplicateStoredProcedure = this.db.GetStoredProcCommandWrapper("TestProc", null, "CHOPS");
            this.db.ExecuteNonQuery(duplicateStoredProcedure);

            int resultCount = Convert.ToInt32(duplicateStoredProcedure.GetParameterValue("vCount"));
            Assert.AreEqual(8, resultCount);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void ArgumentExceptionWhenThereAreTooFewParameters()
        {
            DBCommandWrapper storedProcedure = this.db.GetStoredProcCommandWrapper("TestProc", "ALFKI");
            this.db.ExecuteNonQuery(storedProcedure);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void ArgumentExceptionWhenThereAreTooManyParameters()
        {
            DBCommandWrapper invalidCommandWrapper = this.db.GetStoredProcCommandWrapper("TestProc", "ALFKI", "EIEIO", "Hello");
            this.db.ExecuteNonQuery(invalidCommandWrapper);
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [Test]
        public void ExceptionThrownWhenReadingParametersFromCacheWithTooFewParameterValues()
        {
            DBCommandWrapper storedProcedure = this.db.GetStoredProcCommandWrapper("TestProc", null, "ALFKI");
            this.db.ExecuteNonQuery(storedProcedure);

            DBCommandWrapper duplicateStoredProcedure = this.db.GetStoredProcCommandWrapper("TestProc", "CHOPS");
            this.db.ExecuteNonQuery(duplicateStoredProcedure);
        }

        protected abstract void CreateStoredProcedure();

        protected abstract void DeleteStoredProcedure();
    }
}

#endif