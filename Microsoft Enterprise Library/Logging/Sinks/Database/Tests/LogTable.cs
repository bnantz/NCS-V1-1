 //===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

// Using active schema for testing instead as this is error prone due to sync issues.

//#if  UNIT_TESTS
//using System.Data;
//using Microsoft.Practices.EnterpriseLibrary.Data;
//using EntLibData = Microsoft.Practices.EnterpriseLibrary.Data;
//
//namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Tests
//{
//    /// <summary>
//    /// Utility functions to create, drop and read from a database log table
//    /// </summary>
//    public class LogTable
//    {
//        public static DataTable LoadEntLibLogTable()
//        {
//            EntLibData.Database db = DatabaseFactory.CreateDatabase();
//            // open the table and compare
//            string sql = "select * from EntLibLog";
//            DbCommandWrapper cmd = db.GetSqlStringCommandWrapper(sql);
//            DataSet ds = new DataSet();
//            db.LoadDataSet(cmd, ds, "Log");
//
//            return ds.Tables[0];
//        }
//
//        public static void CreateLogTableAndStoredProcs()
//        {
//            DropLogTableAndStoredProcs();
//
//            CreateLogTable();
//            CreateLogInsertStoredProcedure();
//            CreateLogSelectStoredProcedure();
//        }
//
//        public static void DropLogTableAndStoredProcs()
//        {
//            DropLogTable();
//            DropLogInsertStoredProcedure();
//            DropLogSelectStoredProcedure();
//        }
//
//        public static void ClearLogTable()
//        {
//            try
//            {
//                string sql = "delete from EntLibLog";
//                ExecuteSqlString(sql);
//            }
//            catch
//            {
//                // ignore
//            }
//        }
//
//        private static void CreateLogTable()
//        {
//            string sql = "create table EntLibLog (EventID int, Category varchar(100), Priority int," +
//                "Severity varchar(100), Title varchar(100), Message varchar(1000), " +
//                "MachineName varchar(50), Timestamp datetime)";
//            ExecuteSqlString(sql);
//        }
//
//        private static void CreateLogInsertStoredProcedure()
//        {
//            string sql = "create procedure EntLibLogInsert (@EventID int, @Category varchar(100), " +
//                "@Priority int, @Severity varchar(100), @Title varchar(100), @Message varchar(1000), " +
//                "@MachineName varchar(50), @Timestamp datetime)" +
//                "as insert into EntLibLog values (@EventID, @Category, @Priority, @Severity, " +
//                "@Title, @Message, @MachineName, @Timestamp)";
//            ExecuteSqlString(sql);
//        }
//
//        private static void CreateLogSelectStoredProcedure()
//        {
//            string sql = "create procedure EntLibLogSelect as select * from EntLibLog";
//            ExecuteSqlString(sql);
//        }
//
//        private static void DropLogTable()
//        {
//            try
//            {
//                ExecuteSqlString("drop table EntLibLog");
//            }
//            catch
//            {
//                // ignore errors
//            }
//        }
//
//        private static void DropLogInsertStoredProcedure()
//        {
//            try
//            {
//                ExecuteSqlString("drop procedure EntLibLogInsert");
//            }
//            catch
//            {
//                // ignore errors
//            }
//        }
//
//        private static void DropLogSelectStoredProcedure()
//        {
//            try
//            {
//                ExecuteSqlString("drop procedure EntLibLogSelect");
//            }
//            catch
//            {
//                // ignore errors
//            }
//        }
//
//        private static void ExecuteSqlString(string sql)
//        {
//            EntLibData.Database db = DatabaseFactory.CreateDatabase();
//            DbCommandWrapper cw = db.GetSqlStringCommandWrapper(sql);
//            db.ExecuteNonQuery(cw);
//        }
//
//    }
//}
//
//#endif