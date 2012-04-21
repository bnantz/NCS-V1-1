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
    public abstract class ExecuteScalarFixture
    {
        protected Database db;
        protected string sqlCommand;
        protected DBCommandWrapper command;

        [Test]
        public void ExecuteScalarWithIDbCommand()
        {
            int count = Convert.ToInt32(this.db.ExecuteScalar(this.command));

            Assert.AreEqual(4, count);
        }

        [Test]
        public void ExecuteScalarWithIDbTransaction()
        {
            int count = -1;
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    count = Convert.ToInt32(this.db.ExecuteScalar(this.command, transaction));
                }
            }

            Assert.AreEqual(4, count);
        }

        [Test]
        public void CanExecuteScalarDoAnInsertion()
        {
            string insertCommand = "Insert into Region values (99, 'Midwest')";
            DBCommandWrapper command = this.db.GetSqlStringCommandWrapper(insertCommand);
            using (IDbConnection connection = this.db.GetConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    this.db.ExecuteScalar(command, transaction.Transaction);

                    DBCommandWrapper rowCountCommand = this.db.GetSqlStringCommandWrapper("select count(*) from Region");
                    int count = Convert.ToInt32(this.db.ExecuteScalar(rowCountCommand, transaction.Transaction));
                    Assert.AreEqual(5, count);
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteScalarWithNullIDbCommand()
        {
            int count = Convert.ToInt32(this.db.ExecuteScalar((DBCommandWrapper)null));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteScalarWithNullIDbTransaction()
        {
            int count = Convert.ToInt32(this.db.ExecuteScalar(this.command, null));
        }

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExecuteScalarWithNullIDbCommandAndNullTransaction()
        {
            int count = Convert.ToInt32(this.db.ExecuteScalar(null, (string)null));
        }
    }
}

#endif