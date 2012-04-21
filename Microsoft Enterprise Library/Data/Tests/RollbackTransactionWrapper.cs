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
using System;
using System.Data;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Tests
{
    /// <summary>
    /// Used by a few test fixtures to simplify the code to rollback a transaction
    /// </summary>
    public class RollbackTransactionWrapper : IDisposable
    {
        private IDbTransaction transaction;

        public RollbackTransactionWrapper(IDbTransaction transaction)
        {
            this.transaction = transaction;
        }

        public void Dispose()
        {
            transaction.Rollback();
        }

        public IDbTransaction Transaction
        {
            get { return transaction; }
        }
    }
}

#endif