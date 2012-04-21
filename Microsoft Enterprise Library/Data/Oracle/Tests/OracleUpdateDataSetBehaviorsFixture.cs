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
using System.Data.OracleClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    /// <summary>
    /// Tests executing a batch of commands with insert, delete and update 
    /// using ExecuteUpdateDataTable
    /// </summary>
    [TestFixture]
    public class OracleUpdateDataSetBehaviorsFixture : UpdateDataSetBehaviorsFixture
    {
        [TestFixtureSetUp]
        public void Initialize()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            this.db = factory.CreateDatabase("OracleTest");
            // ensure that stored procedures are dropped before trying to create them
            try
            {
                DeleteStoredProcedures();
            }
            catch
            {
            }
            CreateStoredProcedures();
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            DeleteStoredProcedures();
        }

        [Test]
        public void UpdateWithTransactionalBehaviorAndBadData()
        {
            DataRow errRow = null;
            try
            {
                // insert a few rows, some with errors
                errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);

                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Transactional);
            }
            catch (OracleException)
            {
                //ensure that any changes were rolled back
                DataSet resultDataSet = GetDataSetFromTable();
                DataTable resultTable = resultDataSet.Tables[0];

                Assert.IsTrue(errRow.HasErrors);
                Assert.AreEqual(4, resultTable.Rows.Count);
                return;
            }

            Assert.Fail(); // Exception must be thrown and caught
        }

        [Test]
        public void UpdateWithStandardBehaviorAndBadData()
        {
            DataRow errRow = null;
            try
            {
                // insert a few rows, some with errors
                errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);

                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Standard);
            }
            catch (OracleException)
            {
                //ensure that changes up to the error were written
                DataSet resultDataSet = GetDataSetFromTable();
                DataTable resultTable = resultDataSet.Tables[0];

                Assert.IsTrue(errRow.HasErrors);
                Assert.AreEqual(8, resultTable.Rows.Count);
                return;
            }
            Assert.Fail(); // Exception must be thrown and caught
        }

        [Test]
        public void UpdateWithContinueBehavior()
        {
            AddRowsToDataTable(startingData.Tables[0]);

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Continue);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual(502, resultTable.Rows[6]["RegionID"]);
            Assert.AreEqual("Washington", resultTable.Rows[6]["RegionDescription"].ToString().Trim());
        }

        [Test]
        public void UpdateWithContinueBehaviorAndBadData()
        {
            // insert a few rows, some with errors
            DataRow errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Continue);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.IsTrue(errRow.HasErrors);
            Assert.AreEqual(SR.ExceptionMessageUpdateDataSetRowFailure, errRow.RowError);
            Assert.AreEqual(10, resultTable.Rows.Count);
            Assert.AreEqual(500, resultTable.Rows[4]["RegionID"]);
            Assert.AreEqual(502, resultTable.Rows[6]["RegionID"]);
        }

        protected override void CreateDataAdapterCommands()
        {
            OracleDataSetHelper.CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }

        protected override void CreateStoredProcedures()
        {
            OracleDataSetHelper.CreateStoredProcedures(db);
        }

        protected override void DeleteStoredProcedures()
        {
            OracleDataSetHelper.DeleteStoredProcedures(db);
        }
    }
}

#endif