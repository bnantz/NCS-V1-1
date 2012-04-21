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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Sql.Tests
{
    internal sealed class SqlDataSetHelper
    {
        public static void CreateDataAdapterCommands(Database db, ref DBCommandWrapper insertCommand, ref DBCommandWrapper updateCommand, ref DBCommandWrapper deleteCommand)
        {
            insertCommand = db.GetStoredProcCommandWrapper("RegionInsert");
            updateCommand = db.GetStoredProcCommandWrapper("RegionUpdate");
            deleteCommand = db.GetStoredProcCommandWrapper("RegionDelete");

            insertCommand.AddInParameter("@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            insertCommand.AddInParameter("@RegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            updateCommand.AddInParameter("@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            updateCommand.AddInParameter("@RegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            deleteCommand.AddInParameter("@RegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
        }

        public static void CreateStoredProcedures(Database db)
        {
            string sql = "create procedure RegionSelect as " +
                "select * from Region Order by RegionId";

            DBCommandWrapper command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);

            sql = "create procedure RegionInsert (@RegionID int, @RegionDescription varchar(100) ) as " +
                "insert into Region values(@RegionID, @RegionDescription)";

            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);

            sql = "create procedure RegionUpdate (@RegionID int, @RegionDescription varchar(100) ) as " +
                "update Region set RegionDescription = @RegionDescription where RegionID = @RegionID";

            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);

            sql = "create procedure RegionDelete (@RegionID int) as " +
                "delete from Region where RegionID = @RegionID";

            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);
        }

        public static void DeleteStoredProcedures(Database db)
        {
            DBCommandWrapper command;
            string sql = "drop procedure RegionSelect; " +
                "drop procedure RegionInsert; " +
                "drop procedure RegionDelete; " +
                "drop procedure RegionUpdate";
            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);
        }

        public static void AddTestData(Database db)
        {
            string sql =
                "insert into Region values (99, 'Midwest');" +
                    "insert into Region values (100, 'Central Europe');" +
                    "insert into Region values (101, 'Middle East');" +
                    "insert into Region values (102, 'Australia')";
            DBCommandWrapper testDataInsertion = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(testDataInsertion);
        }
    }
}

#endif