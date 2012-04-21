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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    /// <summary>
    /// 
    /// </summary>
    internal class MockDbCommand : IDbCommand
    {
        public CommandType CommandType
        {
            get { return new CommandType(); }
            set
            {
            }
        }

        public int CommandTimeout
        {
            get { return 0; }
            set
            {
            }
        }

        public IDbConnection Connection
        {
            get { return null; }
            set
            {
            }
        }

        public UpdateRowSource UpdatedRowSource
        {
            get { return new UpdateRowSource(); }
            set
            {
            }
        }

        public virtual string CommandText
        {
            get { return null; }
            set
            {
            }
        }

        public IDataParameterCollection Parameters
        {
            get { return null; }
        }

        public IDbTransaction Transaction
        {
            get { return null; }
            set
            {
            }
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return null;
        }

        IDataReader IDbCommand.ExecuteReader()
        {
            return null;
        }

        public virtual object ExecuteScalar()
        {
            return 0;
        }

        public int ExecuteNonQuery()
        {
            return 0;
        }

        public IDbDataParameter CreateParameter()
        {
            return null;
        }

        public void Cancel()
        {
        }

        public void Prepare()
        {
        }

        public void Dispose()
        {
        }
    }
}

#endif