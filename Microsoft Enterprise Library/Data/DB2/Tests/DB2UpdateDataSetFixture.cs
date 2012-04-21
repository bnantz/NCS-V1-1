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
using Microsoft.Practices.EnterpriseLibrary.Data.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2.Tests
{
    [TestFixture]
    public class DB2UpdateDataSetFixture : UpdateDataSetFixture
    {
        [TestFixtureSetUp]
        public void Initialize()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory(new Db2TestConfigurationContext());
            this.db = factory.CreateDatabase("DB2Test");
            try
            {
                DeleteStoredProcedures();
            }
            catch
            {
            }
            CreateStoredProcedures();
        }

        [TestFixtureTearDown]
        public void Dispose()
        {
            DeleteStoredProcedures();
        }

        protected override DataSet GetDataSetFromTable()
        {
            DBCommandWrapper selectCommand = db.GetStoredProcCommandWrapper("RegionSelect2");
            return db.ExecuteDataSet(selectCommand);
        }

        protected override void CreateDataAdapterCommands()
        {
            DB2DataSetHelper.CreateDataAdapterCommands(db, ref insertCommand, ref updateCommand, ref deleteCommand);
        }

        protected override void CreateStoredProcedures()
        {
            DB2DataSetHelper.CreateStoredProcedures(db);
        }

        protected override void DeleteStoredProcedures()
        {
            DB2DataSetHelper.DeleteStoredProcedures(db);
        }

        protected override void AddTestData()
        {
            DB2DataSetHelper.AddTestData(db);
        }
    }
}

#endif