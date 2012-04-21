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
    public abstract class ExecuteNonQueryFixture
    {
        protected Database db;
        protected string insertString;
        protected string countQuery;
        protected DBCommandWrapper insertionCommand;
        protected DBCommandWrapper countCommand;

        [Test]
        public void CanExecuteNonQueryWithDbCommand()
        {
            this.db.ExecuteNonQuery(this.insertionCommand);

            int count = Convert.ToInt32(this.db.ExecuteScalar(this.countCommand));

            string cleanupString = "delete from Region where RegionId = 77";
            DBCommandWrapper cleanupCommand = this.db.GetSqlStringCommandWrapper(cleanupString);
            this.db.ExecuteNonQuery(cleanupCommand);

            Assert.AreEqual(5, count);
            Assert.AreEqual(1, this.insertionCommand.RowsAffected);
        }

        [Test]
        public void CanExecuteNonQueryThroughTransaction()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    this.db.ExecuteNonQuery(this.insertionCommand, transaction.Transaction);

                    int count = Convert.ToInt32(this.db.ExecuteScalar(this.countCommand, transaction.Transaction));
                    Assert.AreEqual(5, count);
                    Assert.AreEqual(1, this.insertionCommand.RowsAffected);
                }
            }
        }

        [Test]
        public void TransactionActuallyRollsBack()
        {
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    this.db.ExecuteNonQuery(this.insertionCommand, transaction.Transaction);
                }
            }

            DBCommandWrapper wrapper = this.db.GetSqlStringCommandWrapper(this.countQuery);
            int count = Convert.ToInt32(this.db.ExecuteScalar(wrapper));
            Assert.AreEqual(4, count);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteNonQueryWithNullDbTransaction()
        {
            this.db.ExecuteNonQuery(this.insertionCommand, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteNonQueryWithNullDbCommandAndTransaction()
        {
            this.db.ExecuteNonQuery(null, (string)null);
        }
    }
}

#endif