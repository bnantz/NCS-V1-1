//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright � Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright � Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common;
using MySql.Data.MySqlClient;

namespace Microsoft.Practices.EnterpriseLibrary.Data.MySql
{
    /// <summary>
    /// <para>Represents a MySql Server Database.</para>
    /// </summary>
    /// <remarks> 
    /// <para>
    /// Internally uses MySql Server .NET Managed Provider from Microsoft (System.Data.MySqlClient) to connect to the database.
    /// </para>  
    /// </remarks>
    public class MySqlDatabase : Database
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="MySqlDatabase"/> class.
        /// </summary>
        public MySqlDatabase() : base()
        {
        }

        /// <summary>
        /// <para>Gets the parameter token used to delimit parameters for the MySql Database.</para>
        /// </summary>
        /// <value>
        /// <para>The '@' symbol.</para>
        /// </value>
        protected override char ParameterToken
        {
            get { return '@'; }
        }

        /// <summary>
        /// <para>Get the connection for this database.</para>
        /// <seealso cref="IDbConnection"/>
        /// <seealso cref="MySqlConnection"/>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="MySqlConnection"/> for this database.</para>
        /// </returns>
        public override IDbConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// <para>Create a <see cref="MySqlCommandWrapper"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <returns><para>The <see cref="MySqlCommandWrapper"/> for the stored procedure.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="storedProcedureName"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="storedProcedureName"/> hast not been initialized.</para>
        /// </exception>
        public override DBCommandWrapper GetStoredProcCommandWrapper(string storedProcedureName)
        {
            ArgumentValidation.CheckForNullReference(storedProcedureName, "storedProcedureName");
            ArgumentValidation.CheckForEmptyString(storedProcedureName, "storedProcedureName");

            return new MySqlCommandWrapper(storedProcedureName, CommandType.StoredProcedure, ParameterToken);
        }

        /// <summary>
        /// <para>Create an <see cref="MySqlCommandWrapper"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <param name="parameterValues"><para>The list of parameters for the procedure.</para></param>
        /// <returns><para>The <see cref="MySqlCommandWrapper"/> for the stored procedure.</para></returns>
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
            ArgumentValidation.CheckForNullReference(storedProcedureName, "storedProcedureName");
            ArgumentValidation.CheckForEmptyString(storedProcedureName, "storedProcedureName");
            ArgumentValidation.CheckForNullReference(parameterValues, "parameterValues");

            return new MySqlCommandWrapper(storedProcedureName, CommandType.StoredProcedure, ParameterToken, parameterValues);
        }

        /// <summary>
        /// <para>Create an <see cref="MySqlCommandWrapper"/> for a SQL query.</para>
        /// </summary>
        /// <param name="query"><para>The text of the query.</para></param>        
        /// <returns><para>The <see cref="MySqlCommandWrapper"/> for the SQL query.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="query"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="query"/> hast not been initialized.</para>
        /// </exception>
        public override DBCommandWrapper GetSqlStringCommandWrapper(string query)
        {
            ArgumentValidation.CheckForNullReference(query, "query");
            ArgumentValidation.CheckForEmptyString(query, "query");

            return new MySqlCommandWrapper(query, CommandType.Text, ParameterToken);
        }

        /// <summary>
        /// <para>Create a <see cref="MySqlDataAdapter"/> with the given update behavior and connection.</para>
        /// </summary>
        /// <param name="updateBehavior">
        /// <para>One of the <see cref="UpdateBehavior"/> values.</para>
        /// </param>
        /// <param name="connection">
        /// <para>The open connection to the database.</para>
        /// </param>
        /// <returns>An <see cref="MySqlDataAdapter"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="connection"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        protected override DbDataAdapter GetDataAdapter(UpdateBehavior updateBehavior, IDbConnection connection)
        {
            string queryStringToBeFilledInLater = String.Empty;
            MySqlDataAdapter adapter = new MySqlDataAdapter(queryStringToBeFilledInLater, (MySqlConnection)connection);

            if (updateBehavior == UpdateBehavior.Continue)
            {
                adapter.RowUpdated += new MySqlRowUpdatedEventHandler(OnMySqlRowUpdated);
            }
            return adapter;
        }

        /// <devdoc>
        /// Listens for the RowUpdate event on a dataadapter to support UpdateBehavior.Continue
        /// </devdoc>
        private void OnMySqlRowUpdated(object sender, MySqlRowUpdatedEventArgs rowThatCouldNotBeWritten)
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