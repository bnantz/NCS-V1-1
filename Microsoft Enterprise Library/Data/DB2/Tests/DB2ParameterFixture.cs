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

#if  LONG_RUNNING_TESTS
using System.Data;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2.Tests
{
    [TestFixture]
    public class DB2ParameterFixture
    {
        [Test]
        public void CanInsertNullStringParameter()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new Db2TestConfigurationContext());
            Database db = factory.CreateDatabase("DB2Test");
            using (IDbConnection connection = db.GetConnection())
            {
                connection.Open();
                using (IDbTransaction transaction = connection.BeginTransaction())
                {
                    string sqlString = "insert into Orders (OrderID, ShipName) Values (?, ?)";
                    DBCommandWrapper insert = db.GetSqlStringCommandWrapper(sqlString);
                    insert.AddInParameter("@Param1", DbType.Int32, 1);
                    insert.AddInParameter("@Param2", DbType.String, null);

                    db.ExecuteNonQuery(insert, transaction);
                    transaction.Rollback();
                }
            }
        }
    }
}

#endif