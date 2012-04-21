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
    public abstract class DataAccessTestsFixture
    {
        protected Database db;
        protected DataSet dataSet;
        protected string sqlQuery = "SELECT * FROM Region";
        protected DBCommandWrapper command;

        [Test]
        public void CanGetNonEmptyResultSet()
        {
            this.db.LoadDataSet(this.command, this.dataSet, "Foo");
            Assert.AreEqual(4, this.dataSet.Tables["Foo"].Rows.Count);
        }

        [Test]
        public void CanGetTablePositionally()
        {
            this.db.LoadDataSet(this.command, this.dataSet, "Foo");
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        [Test]
        public void CanGetNonEmptyResultSetUsingTransaction()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(this.command, this.dataSet, "Foo", transaction);
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        [Test]
        public void CanGetNonEmptyResultSetUsingTransactionWithNullTableName()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(this.command, this.dataSet, "Foo", transaction);
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        [Test]
        public void ExecuteDataSetWithCommandWrapper()
        {
            this.db.LoadDataSet(this.command, this.dataSet, "Foo");
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        [Test]
        public void ExecuteDataSetWithDbTransaction()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(this.command, this.dataSet, "Foo", transaction);
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CannotLoadDataSetWithEmptyTableName()
        {
            db.LoadDataSet(command, dataSet, "");
            Assert.Fail("Cannot call LoadDataSet with empty SourceTable name");
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteNullCommand()
        {
            this.db.LoadDataSet(null, null, (string)null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteCommandNullTransaction()
        {
            this.db.LoadDataSet(this.command, this.dataSet, "Foo", null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteCommandNullDataset()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(this.command, null, "Foo", transaction);
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteCommandNullCommand()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(null, this.dataSet, "Foo", transaction);
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteCommandNullTableName()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    this.db.LoadDataSet(this.command, this.dataSet, (string)null, transaction);
                }
            }
            Assert.AreEqual(4, this.dataSet.Tables[0].Rows.Count);
        }

    }
}

#endif