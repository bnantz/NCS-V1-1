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

// REMOVED:
// 1) Perf / thread testing should be more complete
// 2) Test is brittle - changes to the schema require too much work to keep test up to date.
// 3) Test is slow (30 - 60 seconds) while providing little value.

//#if UNIT_TESTS
//using System.Data;
//using System.Threading;
//using Microsoft.Practices.EnterpriseLibrary.Common.Tests;
//using Microsoft.Practices.EnterpriseLibrary.Data;
//using NUnit.Framework;
//using EntLibData=Microsoft.Practices.EnterpriseLibrary.Data;
//
//namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Tests 
//{
//	[TestFixture]
//	public class DatabaseSinkStressFixture  : ThreadStressFixtureBase
//	{
//		private int threadCount = 100;
//		private int loopCount = 10;
//		private Data.Database db;
//
//		[TestFixtureSetUp]
//		public void TestSetup()
//		{
//			LogTable.CreateLogTableAndStoredProcs();
//			this.db = DatabaseFactory.CreateDatabase();
//		}
//
//		[TestFixtureTearDown]
//		public void TestTearDown()
//		{
//			LogTable.DropLogTableAndStoredProcs();
//		}
//
//		[Test]
//		public void MultiThreadToDatabaseSink()
//		{
//			ThreadStart testMethod = new ThreadStart(SendTestMessages);
//
//			base.ThreadStress(testMethod, threadCount);
//
//			Thread.Sleep(500);
//
//			DataTable dt = DatabaseSinkFixture.LoadEntLibLogTable(this.db);
//
//			int count = dt.Rows.Count;
//
//			Assert.AreEqual(threadCount*loopCount, count);
//		}
//
//		private void SendTestMessages()
//		{
//			for (int i = 0; i < loopCount; i++)
//			{
//				Logger.Write("test", "LoggingDefaultDatabaseCategory");
//			}
//		}
//	}
//}
//#endif