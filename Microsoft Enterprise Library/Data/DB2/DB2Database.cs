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
using System.Data;
using System.Data.Common;
using IBM.Data.DB2;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2
{
    /// <summary>
    /// <para>Represents a DB2 Database.</para>
    /// </summary>
    /// <remarks> 
    /// <para>
    /// Internally uses DB2 .NET Managed Provider from IBM (IBM.Data.DB2) to connect to the database.
    /// </para>  
    /// </remarks>
    public class DB2Database : Database
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public DB2Database() : base()
        {
        }

        /// <summary>
        /// <para>Gets the parameter token used to delimit parameters for the Sql Database.</para>
        /// </summary>
        /// <value>
        /// <para>The '' symbol.</para>
        /// </value>
        protected override char ParameterToken
        {
            get { return '@'; }
        }

        /// <summary>
        /// <para>Get the connection for this database.</para>
        /// <seealso cref="IDbConnection"/>
        /// <seealso cref="DB2Connection"/>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="DB2Connection"/> for this database.</para>
        /// </returns>
        public override IDbConnection GetConnection()
        {
            return new DB2Connection(ConnectionString);
        }

        /// <summary>
        /// <para>Create a <see cref="DB2CommandWrapper"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <returns><para>The <see cref="DB2CommandWrapper"/> for the stored procedure.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="storedProcedureName"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        public override DBCommandWrapper GetStoredProcCommandWrapper(string storedProcedureName)
        {
            if (storedProcedureName == null)
            {
                throw new ArgumentNullException("storedProcedureName");
            }
            if (storedProcedureName.Length == 0)
            {
                throw new ArgumentException(SR.ExceptionMessageNoDefault, "storedProcedureName");
            }
            return new DB2CommandWrapper(storedProcedureName, CommandType.StoredProcedure, ParameterToken);
        }

        /// <summary>
        /// <para>Create an <see cref="DB2CommandWrapper"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <param name="parameterValues"><para>The list of parameters for the procedure.</para></param>
        /// <returns><para>The <see cref="DB2CommandWrapper"/> for the stored procedure.</para></returns>
        /// <remarks>
        /// <para>The parameters for the stored procedure will be discovered and the values are assigned in positional order.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="storedProcedureName"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="parameterValues"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="storedProcedureName"/> hast not been initialized.</para>
        /// </exception>
        public override DBCommandWrapper GetStoredProcCommandWrapper(string storedProcedureName, params object[] parameterValues)
        {
            if (storedProcedureName == null)
            {
                throw new ArgumentNullException("storedProcedureName");
            }
            if (storedProcedureName.Length == 0)
            {
                throw new ArgumentException(SR.ExceptionMessageNoDefault, "storedProcedureName");
            }
            if (parameterValues == null)
            {
                throw new ArgumentNullException("parameterValues");
            }
            return new DB2CommandWrapper(storedProcedureName, CommandType.StoredProcedure, ParameterToken, parameterValues);
        }

        /// <summary>
        /// <para>Create an <see cref="DB2CommandWrapper"/> for a SQL query.</para>
        /// </summary>
        /// <param name="query"><para>The text of the query.</para></param>        
        /// <returns><para>The <see cref="DB2CommandWrapper"/> for the SQL query.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="query"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="query"/> hast not been initialized.</para>
        /// </exception>       
        public override DBCommandWrapper GetSqlStringCommandWrapper(string query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if (query.Length == 0)
            {
                throw new ArgumentException(SR.ExceptionMessageNoDefault, "query");
            }
            return new DB2CommandWrapper(query, CommandType.Text, ParameterToken);
        }

        /// <summary>
        /// <para>Create a <see cref="DB2DataAdapter"/> with the given update behavior and connection.</para>
        /// </summary>
        /// <param name="updateBehavior">
        /// <para>One of the <see cref="UpdateBehavior"/> values.</para>
        /// </param>
        /// <param name="connection">
        /// <para>The open connection to the database.</para>
        /// </param>
        /// <returns>An <see cref="DB2DataAdapter"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="connection"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        protected override DbDataAdapter GetDataAdapter(UpdateBehavior updateBehavior, IDbConnection connection)
        {
            string queryStringToBeFilledInLater = String.Empty;
            DB2DataAdapter adapter = new DB2DataAdapter(queryStringToBeFilledInLater, (DB2Connection)connection);

            if (updateBehavior == UpdateBehavior.Continue)
            {
                adapter.RowUpdated += new DB2RowUpdatedEventHandler(OnDB2RowUpdated);
            }
            return adapter;
        }

        /// <devdoc>
        /// Listens for the RowUpdate event on a data adapter to support UpdateBehavior.Continue
        /// </devdoc>
        private void OnDB2RowUpdated(object sender, DB2RowUpdatedEventArgs rowThatCouldNotBeWritten)
        {
            if (rowThatCouldNotBeWritten.RecordsAffected == 0)
            {
                if (rowThatCouldNotBeWritten.Errors != null)
                {
                    rowThatCouldNotBeWritten.Row.RowError = SR.ExceptionMessageUpdateDataSetRowFailure;
                    rowThatCouldNotBeWritten.Status = UpdateStatus.SkipCurrentRow;
                }
            }
        }
    }
}