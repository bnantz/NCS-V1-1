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
using System;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2.Tests
{
    internal sealed class DB2DataSetHelper
    {
        public static void CreateDataAdapterCommands(Database db, ref DBCommandWrapper insertCommand, ref DBCommandWrapper updateCommand, ref DBCommandWrapper deleteCommand)
        {
            insertCommand = db.GetStoredProcCommandWrapper("RegionInsert2");
            updateCommand = db.GetStoredProcCommandWrapper("RegionUpdate");
            deleteCommand = db.GetStoredProcCommandWrapper("RegionDelete2");

            insertCommand.AddInParameter("vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            insertCommand.AddInParameter("vRegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            updateCommand.AddInParameter("vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            updateCommand.AddInParameter("vRegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            deleteCommand.AddInParameter("vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
        }

        public static void CreateStoredProcedures(Database db)
        {
            string sql =
                "CREATE PROCEDURE RegionSelect2 () " + Environment.NewLine +
                    "DYNAMIC RESULT SETS 1" + Environment.NewLine +
                    "P1: BEGIN " + Environment.NewLine +
                    "DECLARE cursor1 CURSOR WITH RETURN FOR " + Environment.NewLine +
                    "select * from Region Order by RegionId; " + Environment.NewLine +
                    "OPEN cursor1; " + Environment.NewLine +
                    "END P1";

            DBCommandWrapper command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);

            sql =
                "CREATE procedure RegionInsert2 (IN RegionID INTEGER, IN RegionDescription VARCHAR(100)) " + Environment.NewLine +
                    "P1: BEGIN " + Environment.NewLine +
                    "insert into Region values(RegionID, RegionDescription);" + Environment.NewLine +
                    "END P1";

            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);

            sql =
                "CREATE procedure RegionUpdate (IN vRegionID INTEGER, IN vRegionDescription VARCHAR(100)) " + Environment.NewLine +
                    "P1: BEGIN " + Environment.NewLine +
                    "update Region set RegionDescription = vRegionDescription where RegionID = vRegionID;" + Environment.NewLine +
                    "END P1";

            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);

            sql =
                "CREATE procedure RegionDelete2 (IN vRegionID INTEGER) " + Environment.NewLine +
                    "P1: BEGIN " + Environment.NewLine +
                    "delete from Region where RegionID = vRegionID;" + Environment.NewLine +
                    "END P1";

            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);
        }

        public static void DeleteStoredProcedures(Database db)
        {
            DBCommandWrapper command;
            string sql = "drop procedure RegionSelect2; " +
                "drop procedure RegionInsert2; " +
                "drop procedure RegionDelete2; " +
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