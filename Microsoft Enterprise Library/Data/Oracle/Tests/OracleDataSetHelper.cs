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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Tests
{
    internal sealed class OracleDataSetHelper
    {
        public static void CreateDataAdapterCommands(Database db, ref DBCommandWrapper insertCommand, ref DBCommandWrapper updateCommand, ref DBCommandWrapper deleteCommand)
        {
            insertCommand = db.GetStoredProcCommandWrapper("RegionInsert");
            updateCommand = db.GetStoredProcCommandWrapper("RegionUpdate");
            deleteCommand = db.GetStoredProcCommandWrapper("RegionDelete");

            insertCommand.AddInParameter("vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            insertCommand.AddInParameter("vRegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            updateCommand.AddInParameter("vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
            updateCommand.AddInParameter("vRegionDescription", DbType.String, "RegionDescription", DataRowVersion.Default);

            deleteCommand.AddInParameter("vRegionID", DbType.Int32, "RegionID", DataRowVersion.Default);
        }

        public static void CreateStoredProcedures(Database db)
        {
            DBCommandWrapper command;
            string sql;

            sql = "create procedure RegionSelect (cur_OUT OUT PKGENTLIB_ARCHITECTURE.CURENTLIB_ARCHITECTURE) as " +
                "BEGIN OPEN cur_OUT FOR select * from Region Order By RegionId; END;";

            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);

            sql = "create procedure RegionInsert (vRegionID IN Region.RegionID%TYPE, vRegionDescription IN Region.RegionDescription%TYPE) as " +
                "BEGIN insert into Region values(vRegionID, vRegionDescription); END;";

            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);

            sql = "create procedure RegionUpdate (vRegionID IN Region.RegionID%TYPE, vRegionDescription IN Region.RegionDescription%TYPE) as " +
                "BEGIN update Region set RegionDescription = vRegionDescription where RegionID = vRegionID; END;";

            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);

            sql = "create procedure RegionDelete (vRegionID IN Region.RegionID%TYPE) as " +
                "BEGIN delete from Region where RegionID = vRegionID; END;";

            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);
        }

        public static void DeleteStoredProcedures(Database db)
        {
            DBCommandWrapper command;
            string sql = "drop procedure RegionSelect";
            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);
            sql = "drop procedure RegionInsert";
            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);
            sql = "drop procedure RegionDelete";
            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);
            sql = "drop procedure RegionUpdate";
            command = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(command);
        }

        public static void AddTestData(Database db)
        {
            string sql =
                "BEGIN " +
                    "insert into Region values (99, 'Midwest');" +
                    "insert into Region values (100, 'Central Europe');" +
                    "insert into Region values (101, 'Middle East');" +
                    "insert into Region values (102, 'Australia');" +
                    "END;";
            DBCommandWrapper testDataInsertion = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(testDataInsertion);
        }
    }
}

#endif