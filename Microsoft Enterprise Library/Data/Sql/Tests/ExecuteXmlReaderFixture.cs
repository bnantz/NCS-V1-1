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
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    [TestFixture]
    public class ExecuteXmlReaderFixture
    {
        private SqlDatabase sqlDatabase;

        [SetUp]
        public void SetUp()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new TestConfigurationContext());
            sqlDatabase = (SqlDatabase)factory.CreateDefaultDatabase();
        }

        [Test]
        public void CanExecuteXmlQueryWithDataAndSchema()
        {
            string knownGoodOutput = "<Schema name=\"Schema1\" xmlns=\"urn:schemas-microsoft-com:xml-data\" " +
                "xmlns:dt=\"urn:schemas-microsoft-com:datatypes\"><ElementType name=\"Region\" content=\"empty\" " +
                "model=\"closed\"><AttributeType name=\"RegionID\" dt:type=\"i4\" />" +
                "<AttributeType name=\"RegionDescription\" dt:type=\"string\" /><attribute type=\"RegionID\" />" +
                "<attribute type=\"RegionDescription\" /></ElementType></Schema>" +
                "<Region xmlns=\"x-schema:#Schema1\" RegionID=\"1\" RegionDescription=\"Eastern                                           \" />" +
                "<Region xmlns=\"x-schema:#Schema1\" RegionID=\"2\" RegionDescription=\"Western                                           \" />" +
                "<Region xmlns=\"x-schema:#Schema1\" RegionID=\"3\" RegionDescription=\"Northern                                          \" />" +
                "<Region xmlns=\"x-schema:#Schema1\" RegionID=\"4\" RegionDescription=\"Southern                                          \" />";

            string queryString = "Select * from Region for xml auto, xmldata";
            SqlCommandWrapper sqlCommand = sqlDatabase.GetSqlStringCommandWrapper(queryString) as SqlCommandWrapper;

            string actualOutput = RetrieveXmlFromDatabase(sqlCommand);

            Assert.AreEqual(ConnectionState.Closed, sqlCommand.Command.Connection.State);
            Assert.AreEqual(knownGoodOutput, actualOutput);
        }

        [Test]
        public void CanExecuteXmlQueryRetrivingDataOnly()
        {
            string knownGoodOutput =
                "<Region RegionID=\"1\" RegionDescription=\"Eastern                                           \" />" +
                    "<Region RegionID=\"2\" RegionDescription=\"Western                                           \" />" +
                    "<Region RegionID=\"3\" RegionDescription=\"Northern                                          \" />" +
                    "<Region RegionID=\"4\" RegionDescription=\"Southern                                          \" />";

            string queryString = "Select * from Region for xml auto";
            SqlCommandWrapper sqlCommand = sqlDatabase.GetSqlStringCommandWrapper(queryString) as SqlCommandWrapper;

            string actualOutput = RetrieveXmlFromDatabase(sqlCommand);

            Assert.AreEqual(ConnectionState.Closed, sqlCommand.Command.Connection.State);
            Assert.AreEqual(knownGoodOutput, actualOutput);
        }

        [Test]
        public void CanExecuteXmlQueryThroughTransaction()
        {
            string knownGoodOutputAfterChange =
                "<Region RegionID=\"1\" RegionDescription=\"Eastern                                           \" />" +
                    "<Region RegionID=\"2\" RegionDescription=\"Western                                           \" />" +
                    "<Region RegionID=\"3\" RegionDescription=\"Northern                                          \" />" +
                    "<Region RegionID=\"4\" RegionDescription=\"Southern                                          \" />" +
                    "<Region RegionID=\"99\" RegionDescription=\"Midwest                                           \" />";

            string knownGoodOutputAfterRollback =
                "<Region RegionID=\"1\" RegionDescription=\"Eastern                                           \" />" +
                    "<Region RegionID=\"2\" RegionDescription=\"Western                                           \" />" +
                    "<Region RegionID=\"3\" RegionDescription=\"Northern                                          \" />" +
                    "<Region RegionID=\"4\" RegionDescription=\"Southern                                          \" />";

            string insertString = "insert into region values (99, 'Midwest')";
            DBCommandWrapper insertCommand = sqlDatabase.GetSqlStringCommandWrapper(insertString);

            string queryString = "Select * from Region for xml auto";
            SqlCommandWrapper sqlCommand = sqlDatabase.GetSqlStringCommandWrapper(queryString) as SqlCommandWrapper;

            string actualOutput = "";

            using (IDbConnection connection = sqlDatabase.GetConnection())
            {
                connection.Open();
                using (RollbackTransactionWrapper transaction = new RollbackTransactionWrapper(connection.BeginTransaction()))
                {
                    sqlDatabase.ExecuteNonQuery(insertCommand, transaction.Transaction);

                    XmlReader results = sqlDatabase.ExecuteXmlReader(sqlCommand, transaction.Transaction);
                    results.MoveToContent();
                    for (string value = results.ReadOuterXml(); value != null && value.Length != 0; value = results.ReadOuterXml())
                    {
                        actualOutput += value;
                    }
                    results.Close();
                }
            }

            Assert.AreEqual(actualOutput, knownGoodOutputAfterChange);

            string confirmationString = "Select * from Region for xml auto";
            SqlCommandWrapper confirmationCommand = sqlDatabase.GetSqlStringCommandWrapper(confirmationString) as SqlCommandWrapper;

            string rollbackResults = RetrieveXmlFromDatabase(confirmationCommand);
            Assert.AreEqual(knownGoodOutputAfterRollback, rollbackResults);
        }

        [Test]
        public void ConfirmConnectionClosedWhenExceptionThrown()
        {
        }

        private string RetrieveXmlFromDatabase(SqlCommandWrapper sqlCommand)
        {
            string actualOutput = "";

            XmlReader reader = null;
            try
            {
                reader = sqlDatabase.ExecuteXmlReader(sqlCommand);
                reader.MoveToContent();
                for (string value = reader.ReadOuterXml(); value != null && value.Length != 0; value = reader.ReadOuterXml())
                {
                    actualOutput += value;
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }

                sqlCommand.Command.Connection.Close();
            }

            return actualOutput;
        }
    }

}

#endif