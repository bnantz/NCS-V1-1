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
    public abstract class UpdateDataSetWithTransactionsFixture : UpdateDataSetStoredProcedureBase
    {
        protected IDbTransaction transaction;
        protected bool rollback = true;

        [Test]
        public void ModifyRowWithStoredProcedure()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, transaction);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
        }

        [Test]
        public void AttemptToInsertBadRowInsideOfATransaction()
        {
            AddRowsWithErrorsToDataTable(startingData.Tables[0]);
            try
            {
                db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand, transaction);
            }
            catch
            {
                transaction.Rollback();
                rollback = false;

                DataSet results = GetDataSetFromTableWithoutTransaction();

                Assert.AreEqual(8, results.Tables[0].Rows.Count);

                return;
            }

            Assert.Fail();
        }

        protected virtual DataSet GetDataSetFromTableWithoutTransaction()
        {
            DBCommandWrapper selectCommand = db.GetStoredProcCommandWrapper("RegionSelect");
            return db.ExecuteDataSet(selectCommand);
        }

        protected override void PrepareDatabaseSetup()
        {
            IDbConnection connection = db.GetConnection();
            connection.Open();
            transaction = connection.BeginTransaction();
        }

        protected override DataSet GetDataSetFromTable()
        {
            DBCommandWrapper selectCommand = db.GetStoredProcCommandWrapper("RegionSelect");
            return db.ExecuteDataSet(selectCommand, transaction);
        }

        protected override void ResetDatabase()
        {
            if (rollback)
            {
                transaction.Rollback();
            }
            RestoreRegionTable();
        }

        private void RestoreRegionTable()
        {
            string sql = "delete from Region where RegionID >= 99";
            DBCommandWrapper cleanupCommand = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(cleanupCommand);
        }
    }
}

#endif