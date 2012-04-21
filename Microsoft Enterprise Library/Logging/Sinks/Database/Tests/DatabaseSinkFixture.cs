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

#if  UNIT_TESTS
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Tests;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Tests;
using NUnit.Framework;
using EntLibData = Microsoft.Practices.EnterpriseLibrary.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Tests
{
    [TestFixture]
    public class DatabaseSinkFixture : ConfigurationContextFixtureBase
    {
        private DatabaseSink sink;
        private Data.Database db;


        public override void FixtureSetup()
        {
            base.FixtureSetup ();
            this.db = DatabaseFactory.CreateDatabase("LoggingDb");
        }
        

        public override void FixtureTeardown()
        {
            string sql = "delete from Log";
            DBCommandWrapper cmd = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(cmd);
            base.FixtureTeardown ();
        }

        [SetUp]
        public void Setup()
        {
            DatabaseSinkData sinkParams = new DatabaseSinkData();
            sinkParams.DatabaseInstanceName = "LoggingDb";
            sinkParams.StoredProcName = "WriteLog";
            this.sink = CreateSink(sinkParams);

            string sql = "delete from Log";
            DBCommandWrapper cmd = db.GetSqlStringCommandWrapper(sql);
            db.ExecuteNonQuery(cmd);

            CommonUtil.ResetEventLogCounter();
        }

        [TearDown]
        public void TearDown()
        {
            CommonUtil.ResetEventLogCounter();
        }

        [Test]
        public void Constructor()
        {
            DatabaseSinkData sinkParams = new DatabaseSinkData();
            sinkParams.DatabaseInstanceName = "LoggingDb";
            sinkParams.StoredProcName = "WriteLog";
            this.sink = CreateSink(sinkParams);
            DatabaseSink sink = CreateSink(sinkParams);

            Assert.IsNotNull(sink);
        }

        [Test]
        public void GetDatabaseSinkFromConfigFile()
        {
            DistributorSettings settings = (DistributorSettings)ConfigurationManager.GetConfiguration(DistributorSettings.SectionName);
            DatabaseSinkData dbData = settings.SinkDataCollection["DatabaseSink2"] as DatabaseSinkData;
            Assert.AreEqual("LoggingDb", dbData.DatabaseInstanceName);
            Assert.AreEqual("WriteLog", dbData.StoredProcName);
        }

        [Test]
        public void LogMessageToDatabase()
        {
            DataTable dt = LoadEntLibLogTable(this.db);
            int initialRowCount = dt.Rows.Count;
            CommonUtil.SendTestMessage(this.sink);

            // confirm no messages written to event log
            Assert.AreEqual(0, CommonUtil.EventLogEntryCount());

            dt = LoadEntLibLogTable(this.db);
            DataRow[] rows = dt.Select("Message = '" + CommonUtil.MsgBody + "'");

            Assert.AreEqual(initialRowCount + 1, dt.Rows.Count);
            Assert.AreEqual(1, rows.Length, "Only one row should have matched with this message body");
        }

        public static DataTable LoadEntLibLogTable(Data.Database db)
        {
            // open the table and compare
            string sql = "select * from Log";
            DBCommandWrapper cmd = db.GetSqlStringCommandWrapper(sql);
            DataSet ds = new DataSet();
            db.LoadDataSet(cmd, ds, "Log");

            return ds.Tables[0];
        }

        [Test]
        public void MissingRequiredParameters()
        {
            // create a flatfile sink without the required parameters
            DatabaseSink badSink = CreateSink(new DatabaseSinkData());

            CommonUtil.SendTestMessage(badSink);

            // confirm an error message was written to event log
            Assert.AreEqual(1, CommonUtil.EventLogEntryCount());

            string expected = SR.DatabaseSinkMissingParameters;
            string entry = CommonUtil.GetLastEventLogEntry();
            Assert.IsTrue(entry.IndexOf(expected) > -1, "confirm error message");
        }

        [Test]
        public void NullStoredProcName()
        {
            DatabaseSinkData sinkParams = new DatabaseSinkData();
            sinkParams.DatabaseInstanceName = "Service_Dflt";
            sinkParams.StoredProcName = null;

            // create a flatfile sink without the required parameters
            DatabaseSink badSink = CreateSink(new DatabaseSinkData());

            CommonUtil.SendTestMessage(badSink);

            // confirm an error message was written to event log
            Assert.AreEqual(1, CommonUtil.EventLogEntryCount());

            string expected = SR.DatabaseSinkMissingParameters;
            string entry = CommonUtil.GetLastEventLogEntry();
            Assert.IsTrue(entry.IndexOf(expected) > -1, "confirm error message");
        }

        [Test]
        public void NullServiceName()
        {
            DatabaseSinkData sinkParams = new DatabaseSinkData();
            sinkParams.DatabaseInstanceName = null;
            sinkParams.StoredProcName = "stored proc";

            // create a flatfile sink without the required parameters
            DatabaseSink badSink = CreateSink(new DatabaseSinkData());

            CommonUtil.SendTestMessage(badSink);

            // confirm an error message was written to event log
            Assert.AreEqual(1, CommonUtil.EventLogEntryCount());

            string expected = SR.DatabaseSinkMissingParameters;
            string entry = CommonUtil.GetLastEventLogEntry();
            Assert.IsTrue(entry.IndexOf(expected) > -1, "confirm error message");
        }

        [Test]
        public void LogMessageDuringATransaction()
        {
            using (IDbConnection connection = db.GetConnection())
            {
                DataTable dt = LoadEntLibLogTable(this.db);
                int initialRowCount = dt.Rows.Count;

                connection.Open();
                IDbTransaction trans = connection.BeginTransaction();
                string sql = "select * from Log";
                DBCommandWrapper cmd = this.db.GetSqlStringCommandWrapper(sql);
                cmd.Command.Connection = connection;
                cmd.Command.Transaction = trans;
                cmd.Command.ExecuteNonQuery();

                CommonUtil.SendTestMessage(this.sink);

                trans.Commit();

                dt = LoadEntLibLogTable(this.db);
                DataRow[] rows = dt.Select("Message = '" + CommonUtil.MsgBody + "'");

                Assert.AreEqual(initialRowCount + 1, dt.Rows.Count);
                Assert.AreEqual(1, rows.Length);

            }
        }

        [Test]
        public void LogToLoggingDatabase()
        {
            Data.Database loggingDb = DatabaseFactory.CreateDatabase("LoggingDb");
            string sql = "delete [Log]";
            DBCommandWrapper cmd = loggingDb.GetSqlStringCommandWrapper(sql);
            loggingDb.ExecuteNonQuery(cmd);

            Logger.Write("test message", "LoggingDbCategory");

            sql = "select * from [Log]";
            cmd = loggingDb.GetSqlStringCommandWrapper(sql);
            DataSet ds = loggingDb.ExecuteDataSet(cmd);
            Assert.AreEqual(1, ds.Tables[0].Rows.Count);
        }

        private DatabaseSink CreateSink(SinkData sinkData)
        {
            DatabaseSink databaseSink = new DatabaseSink();
            databaseSink.Initialize(new TestLoggingConfigurationView(sinkData, Context));

            return databaseSink;
        }

        private class TestLoggingConfigurationView : LoggingConfigurationView
        {
            private SinkData data;

            public TestLoggingConfigurationView(SinkData data, ConfigurationContext context) : base(context)
            {
                this.data = data;
            }

            public override SinkData GetSinkData(string name)
            {
                return data;
            }

        }
    }
}

#endif