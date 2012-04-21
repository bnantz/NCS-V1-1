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
using System.Data;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    public abstract class UpdateDataSetFixture : UpdateDataSetStoredProcedureBase
    {
        [Test]
        public void ModifyRowWithStoredProcedure()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Continue);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
        }

        [Test]
        public void DeleteRowWithStoredProcedure()
        {
            startingData.Tables[0].Rows[5].Delete();

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Continue);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(7, resultTable.Rows.Count);
        }

        [Test]
        public void InsertRowWithStoredProcedure()
        {
            DataRow newRow = startingData.Tables[0].NewRow();
            newRow["RegionID"] = 1000;
            newRow["RegionDescription"] = "Moon Base Alpha";
            startingData.Tables[0].Rows.Add(newRow);

            db.UpdateDataSet(startingData, "Table", insertCommand, updateCommand, deleteCommand,
                             UpdateBehavior.Continue);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(9, resultTable.Rows.Count);
            Assert.AreEqual(1000, resultTable.Rows[8]["RegionID"]);
            Assert.AreEqual("Moon Base Alpha", resultTable.Rows[8]["RegionDescription"].ToString().Trim());
        }

        [Test]
        public void DeleteRowWithMissingInsertAndUpdateCommands()
        {
            startingData.Tables[0].Rows[5].Delete();

            db.UpdateDataSet(startingData, "Table", null, null, deleteCommand,
                             UpdateBehavior.Standard);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(7, resultTable.Rows.Count);
        }

        [Test]
        public void UpdateRowWithMissingInsertAndDeleteCommands()
        {
            startingData.Tables[0].Rows[4]["RegionDescription"] = "South America";

            db.UpdateDataSet(startingData, "Table", null, updateCommand, null,
                             UpdateBehavior.Standard);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(8, resultTable.Rows.Count);
            Assert.AreEqual("South America", resultTable.Rows[4]["RegionDescription"].ToString().Trim());
        }

        [Test]
        public void InsertRowWithMissingUpdateAndDeleteCommands()
        {
            DataRow newRow = startingData.Tables[0].NewRow();
            newRow["RegionID"] = 1000;
            newRow["RegionDescription"] = "Moon Base Alpha";
            startingData.Tables[0].Rows.Add(newRow);

            db.UpdateDataSet(startingData, "Table", insertCommand, null, null,
                             UpdateBehavior.Standard);

            DataSet resultDataSet = GetDataSetFromTable();
            DataTable resultTable = resultDataSet.Tables[0];

            Assert.AreEqual(9, resultTable.Rows.Count);
            Assert.AreEqual(1000, resultTable.Rows[8]["RegionID"]);
            Assert.AreEqual("Moon Base Alpha", resultTable.Rows[8]["RegionDescription"].ToString().Trim());
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void UpdateDataSetWithAllCommandsMissing()
        {
            DataRow newRow = startingData.Tables[0].NewRow();
            newRow["RegionID"] = 1000;
            newRow["RegionDescription"] = "Moon Base Alpha";
            startingData.Tables[0].Rows.Add(newRow);

            db.UpdateDataSet(startingData, "Table", null, null, null,
                             UpdateBehavior.Standard);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateDataSetWithNullTable()
        {
            db.UpdateDataSet(null, null, null, null, null, UpdateBehavior.Standard);
        }

        protected override void PrepareDatabaseSetup()
        {
        }

        protected override DataSet GetDataSetFromTable()
        {
            DBCommandWrapper selectCommand = db.GetStoredProcCommandWrapper("RegionSelect");
            return db.ExecuteDataSet(selectCommand);
        }

        protected override void ResetDatabase()
        {
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