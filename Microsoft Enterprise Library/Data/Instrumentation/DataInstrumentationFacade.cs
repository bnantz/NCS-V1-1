//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation
{
    /// <summary>
    /// <para>This type supports the Data Access Instrumentation infrastructure and is not intended to be used directly from your code.</para>
    /// </summary>    
    internal class DataInstrumentationFacade
    {
        /// <summary/>
        /// <param name="executionTime"/>
        /// <exclude/>
        public virtual void CommandExecuted(DateTime executionTime)
        {
            DataCommandExecutedEvent.Fire(executionTime);
        }

        /// <summary/>
        /// <param name="commandText"/>        
        /// <param name="connectionString"/>
        /// <exclude/>
        public virtual void CommandFailed(string commandText, string connectionString)
        {
            DataCommandFailedEvent.Fire(commandText, connectionString);
        }

        /// <summary/>
        /// <exclude/>
        public void TransactionCommitted()
        {
            DataTransactionCommittedEvent.Fire();
        }

        /// <summary/>
        /// <param name="connectionString"/>
        /// <exclude/>
        public void TransactionRolledBack(string connectionString)
        {
            DataTransactionRolledBackEvent.Fire(connectionString);
        }

        /// <summary/>
        /// <exclude/>
        public void TransactionOpened()
        {
            DataTransactionOpenedEvent.Fire();
        }

        /// <summary/>
        /// <param name="connectionString"/>
        /// <exclude/>
        public void TransactionFailed(string connectionString)
        {
            DataTransactionFailedEvent.Fire(connectionString);
        }

        /// <summary/>
        /// <param name="connectionString"/>
        /// <exclude/>
        public void ConnectionOpened(string connectionString)
        {
            DataConnectionOpenedEvent.Fire(connectionString);
        }

        /// <summary/>
        /// <param name="connectionString"/>
        /// <exclude/>
        public void ConnectionFailed(string connectionString)
        {
            DataConnectionFailedEvent.Fire(connectionString);
        }
    }

}