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
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestFixture]
    public class SqlUpdateDataSetBehaviorsFixture : UpdateDataSetBehaviorsFixture
    {
        [TestFixtureSetUp]
        public void Initialize()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            db = factory.CreateDefaultDatabase();
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
                errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);
                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand,
                                 deleteCommand, UpdateBehavior.Transactional);
            }
            catch (SqlException)
            {
                DataSet resultDataSet = GetDataSetFromTable();
                DataTable resultTable = resultDataSet.Tables[0];
                Assert.IsTrue(errRow.HasErrors);
                Assert.AreEqual(4, resultTable.Rows.Count);
                return;
            }

            Assert.Fail();
        }

        [Test]
        public void UpdateWithStandardBehaviorAndBadData()
        {
            DataRow errRow = null;
            try
            {
                errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);
                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, UpdateBehavior.Standard);
            }
            catch (SqlException)
            {
                DataSet resultDataSet = GetDataSetFromTable();
                DataTable resultTable = resultDataSet.Tables[0];
                Assert.IsTrue(errRow.HasErrors);
                Assert.AreEqual(8, resultTable.Rows.Count);
                return;
            }
            Assert.Fail();
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
            DataRow errRow = AddRowsWithErrorsToDataTable(startingData.Tables[0]);

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand,
                             deleteCommand, UpdateBehavior.Continue);

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
            SqlDataSetHelper.CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }

        protected override void CreateStoredProcedures()
        {
            SqlDataSetHelper.CreateStoredProcedures(db);
        }

        protected override void DeleteStoredProcedures()
        {
            SqlDataSetHelper.DeleteStoredProcedures(db);
        }

    }
}

#endif