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
    [TestFixture]
    public abstract class ExecuteReaderFixture
    {
        protected Database db;
        protected string queryString;
        protected string insertString;
        protected DBCommandWrapper queryCommand;
        protected DBCommandWrapper insertCommand;

        [Test]
        public void CanExecuteReaderFromDbCommand()
        {
            IDataReader reader = this.db.ExecuteReader(this.queryCommand);
            string accumulator = "";
            while (reader.Read())
            {
                accumulator += ((string)reader["RegionDescription"]).Trim();
            }
            reader.Close();

            Assert.AreEqual("EasternWesternNorthernSouthern", accumulator);
            Assert.AreEqual(ConnectionState.Closed, this.queryCommand.Command.Connection.State);
        }

        [Test]
        public void WhatGetsReturnedWhenWeDoAnInsertThroughDbCommandExecute()
        {
            int count = -1;
            IDataReader reader = null;
            try
            {
                reader = this.db.ExecuteReader(this.insertCommand);
                count = reader.RecordsAffected;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                string deleteString = "Delete from Region where RegionId = 99";
                DBCommandWrapper cleanupCommand = this.db.GetSqlStringCommandWrapper(deleteString);
                this.db.ExecuteNonQuery(cleanupCommand);
            }

            Assert.AreEqual(1, count);
        }

        [Test]
        public void CanExecuteQueryThroughDataReaderUsingTransaction()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    IDataReader reader = this.db.ExecuteReader(this.insertCommand, transaction.Transaction);
                    Assert.AreEqual(1, reader.RecordsAffected);
                    reader.Close();
                }

                Assert.AreEqual(ConnectionState.Open, connection.State);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanExecuteQueryThroughDataReaderUsingNullCommand()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                this.insertCommand = null;
                IDataReader reader = this.db.ExecuteReader(this.insertCommand, null);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanExecuteQueryThroughDataReaderUsingNullCommandAndNullTransaction()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                IDataReader reader = this.db.ExecuteReader(null, (string)null);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CanExecuteQueryThroughDataReaderUsingNullTransaction()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                IDataReader reader = this.db.ExecuteReader(this.insertCommand, null);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteReaderWithNullCommand()
        {
            IDataReader reader = this.db.ExecuteReader((DBCommandWrapper)null);
            Assert.AreEqual(null, this.insertCommand);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullQueryStringTest()
        {
            DBCommandWrapper myCommand = this.db.GetSqlStringCommandWrapper(null);
            IDataReader reader = this.db.ExecuteReader(myCommand);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void EmptyQueryStringTest()
        {
            DBCommandWrapper myCommand = this.db.GetSqlStringCommandWrapper(string.Empty);
            IDataReader reader = this.db.ExecuteReader(myCommand);
        }
    }
}

#endif