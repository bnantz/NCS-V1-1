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
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Oracle.DataAccess.Client;

namespace Microsoft.Practices.EnterpriseLibrary.Data.ODP10
{
    /// <summary>
    /// <para>Represents an Oracle Database.</para>
    /// </summary>
    /// <remarks> 
    /// <para>
    /// Internally uses Oracle .NET Managed Provider from Microsoft (System.Data.OracleClient) to connect to Oracle 9i database.
    /// </para>  
    /// <para>
    /// When retrieving a result set, it will build the package name. The package name should be set based
    /// on the stored procedure prefix and this should be set via configuration. For 
    /// example, a package name should be set as prefix of "ENTLIB_" and package name of
    /// "pkgENTLIB_ARCHITECTURE". For your applications, this is required only if you are defining your stored procedures returning 
    /// ref cursors.
    /// </para>
    /// </remarks>
    public class ODP10Database : Database
    {
        private const string RefCursorName = "cur_OUT";

        /// <summary>
        /// Default constructor
        /// </summary>
        public ODP10Database() : base()
        {
        }

        /// <summary>
        /// <para>Gets the parameter token used to delimit parameters for the Oracle Database.</para>
        /// </summary>
        /// <value>
        /// <para>The ':' symbol.</para>
        /// </value>
        protected override char ParameterToken
        {
            get { return ':'; }
        }

        /// <summary>
        /// <para>Get the connection for this database.</para>
        /// <seealso cref="IDbConnection"/>
        /// <seealso cref="OracleConnection"/>
        /// </summary>
        /// <returns>
        /// <para>The <see cref="OracleConnection"/> for this database.</para>
        /// </returns>
        public override IDbConnection GetConnection()
        {
            return new OracleConnection(base.ConnectionString);
        }

        /// <summary>
        /// <para>Create an <see cref="ODP10CommandWrapper"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <returns><para>The <see cref="ODP10CommandWrapper"/> for the stored procedure.</para></returns>
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

            ODP10CommandWrapper wrapper = new ODP10CommandWrapper(storedProcedureName, CommandType.StoredProcedure, ParameterToken);
            PreparePackageSchema(wrapper);
            return wrapper;
        }

        /// <summary>
        /// <para>Create an <see cref="ODP10CommandWrapper"/> for a stored procedure.</para>
        /// </summary>
        /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
        /// <param name="parameterValues"><para>The list of parameters for the procedure.</para></param>
        /// <returns><para>The <see cref="ODP10CommandWrapper"/> for the stored procedure.</para></returns>
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

            ODP10CommandWrapper wrapper = new ODP10CommandWrapper(storedProcedureName, CommandType.StoredProcedure, ParameterToken, parameterValues);
            PreparePackageSchema(wrapper);
            return wrapper;
        }

        /// <summary>
        /// <para>Create an <see cref="ODP10CommandWrapper"/> for a SQL query.</para>
        /// </summary>
        /// <param name="query"><para>The text of the query.</para></param>        
        /// <returns><para>The <see cref="ODP10CommandWrapper"/> for the SQL query.</para></returns>
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

            ODP10CommandWrapper wrapper = new ODP10CommandWrapper(query, CommandType.Text, ParameterToken);
            return wrapper;
        }

        /// <summary>
        /// <para>Create a <see cref="OracleDataAdapter"/> with the given update behavior and connection.</para>
        /// </summary>
        /// <param name="updateBehavior">
        /// <para>One of the <see cref="UpdateBehavior"/> values.</para>
        /// </param>
        /// <param name="connection">
        /// <para>The open connection to the database.</para>
        /// </param>
        /// <returns>An <see cref="OracleDataAdapter"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="connection"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        protected override DbDataAdapter GetDataAdapter(UpdateBehavior updateBehavior, IDbConnection connection)
        {
            ArgumentValidation.CheckForNullReference(connection, "connection");

            OracleDataAdapter adapter = new OracleDataAdapter(String.Empty, (OracleConnection)connection);
            if (updateBehavior == UpdateBehavior.Continue)
            {
                adapter.RowUpdated += new OracleRowUpdatedEventHandler(OnOracleRowUpdated);
            }
            return adapter;
        }

        /// <summary>
        /// Creates and <see cref="OracleDataReader"/> based on the <paramref name="commandWrapper"/>.
        /// </summary>
        /// <param name="commandWrapper">The command wrapper to execute.</param>        
        /// <returns>An <see cref="OracleDataReader"/> object.</returns>        
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="commandWrapper"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        public override IDataReader ExecuteReader(DBCommandWrapper commandWrapper)
        {
            PrepareCWRefCursor(commandWrapper);
            return new ODP10DataReaderWrapper((OracleDataReader)base.ExecuteReader(commandWrapper));
        }

        /// <summary>
        /// <para>Creates and <see cref="OracleDataReader"/> based on the <paramref name="commandWrapper"/>.</para>
        /// </summary>        
        /// <param name="commandWrapper"><para>The command wrapper to execute.</para></param>        
        /// <param name="transaction"><para>The transaction to participate in when executing this reader.</para></param>        
        /// <returns><para>An <see cref="OracleDataReader"/> object.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="commandWrapper"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="transaction"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        public override IDataReader ExecuteReader(DBCommandWrapper commandWrapper, IDbTransaction transaction)
        {
            PrepareCWRefCursor(commandWrapper);
            return new ODP10DataReaderWrapper((OracleDataReader)base.ExecuteReader(commandWrapper, transaction));
        }

        /// <summary>
        /// <para>Execute a command and return the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="commandWrapper"><para>The command to execute to fill the <see cref="DataSet"/></para></param>
        /// <returns><para>A <see cref="DataSet"/> filed with records and, if necessary, schema.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="commandWrapper"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        public override DataSet ExecuteDataSet(DBCommandWrapper commandWrapper)
        {
            PrepareCWRefCursor(commandWrapper);
            return base.ExecuteDataSet(commandWrapper);
        }

        /// <summary>
        /// <para>Execute a command and return the results in a new <see cref="DataSet"/>.</para>
        /// </summary>
        /// <param name="commandWrapper"><para>The command to execute to fill the <see cref="DataSet"/></para></param>
        /// <param name="transaction"><para>The transaction to participate in when executing this reader.</para></param>        
        /// <returns><para>A <see cref="DataSet"/> filed with records and, if necessary, schema.</para></returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="commandWrapper"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="commandWrapper"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// <para>- or -</para>
        /// <para><paramref name="transaction"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
        /// </exception>
        public override DataSet ExecuteDataSet(DBCommandWrapper commandWrapper, IDbTransaction transaction)
        {
            PrepareCWRefCursor(commandWrapper);
            return base.ExecuteDataSet(commandWrapper, transaction);
        }

        /// <summary>
        /// <para>Load a <see cref="DataSet"/> from a <see cref="DBCommandWrapper"/>.</para>
        /// </summary>
        /// <param name="commandWrapper">
        /// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        public override void LoadDataSet(DBCommandWrapper commandWrapper, DataSet dataSet, string[] tableNames)
        {
            PrepareCWRefCursor(commandWrapper);
            base.LoadDataSet(commandWrapper, dataSet, tableNames);
        }

        /// <summary>
        /// <para>Load a <see cref="DataSet"/> from a <see cref="DBCommandWrapper"/> in  a transaction.</para>
        /// </summary>
        /// <param name="commandWrapper">
        /// <para>The command to execute to fill the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="dataSet">
        /// <para>The <see cref="DataSet"/> to fill.</para>
        /// </param>
        /// <param name="tableNames">
        /// <para>An array of table name mappings for the <see cref="DataSet"/>.</para>
        /// </param>
        /// <param name="transaction">
        /// <para>The <see cref="IDbTransaction"/> to execute the command in.</para>
        /// </param>
        public override void LoadDataSet(DBCommandWrapper commandWrapper, DataSet dataSet, string[] tableNames, IDbTransaction transaction)
        {
            PrepareCWRefCursor(commandWrapper);
            base.LoadDataSet(commandWrapper, dataSet, tableNames, transaction);
        }

        /// <devdoc>
        /// This is a private method that will build the Oracle package name if your stored procedure
        /// has proper prefix and postfix. 
        /// This functionality is include for
        /// the portability of the architecture between SQL and Oracle datbase.
        /// This method will also add the reference cursor to the command writer if not added already. This
        /// is required for Oracle .NET managed data provider.
        /// </devdoc>        
        private static void PrepareCWRefCursor(DBCommandWrapper commandWrapper)
        {
            ArgumentValidation.CheckForNullReference(commandWrapper, "commandWrapper");

            ODP10CommandWrapper oracleCommandWrapper = (ODP10CommandWrapper)commandWrapper;
            if (!(oracleCommandWrapper.ParameterDiscoveryRequired))
            {
                if (CommandType.StoredProcedure == commandWrapper.Command.CommandType)
                {
                    // Check for ref. cursor in the command writer, if it does not exist, add a know reference cursor out
                    // of "cur_OUT"
                    if (!oracleCommandWrapper.IsRefCursorAdded)
                    {
                        oracleCommandWrapper.AddParameter(RefCursorName, OracleDbType.RefCursor, 0, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Default, Convert.DBNull);
                    }
                }
            }
        }

        /// <devdoc>
        /// Look into configuration and get the information how the command wrapper should be updated if calling a package on this
        /// connection.
        /// </devdoc>        
        private void PreparePackageSchema(ODP10CommandWrapper commandWrapper)
        {
            const string allPrefix = "*";
            string packageName = String.Empty;
            string prefix = String.Empty;
            string commandText = commandWrapper.Command.CommandText;

            if (CommandType.StoredProcedure == commandWrapper.Command.CommandType)
            {
                OracleConnectionStringData oraConnectionString = this.DatabaseProviderData.ConnectionStringData as OracleConnectionStringData;
                if (oraConnectionString != null)
                {
                    foreach (OraclePackageData oraPackage in oraConnectionString.OraclePackages)
                    {
                        if ((oraPackage.Prefix == allPrefix) || (commandText.StartsWith(prefix)))
                        {
                            //use the package name for the matching prefix
                            packageName = oraPackage.Name;
                            break;
                        }
                    }
                }
                if (0 != packageName.Length)
                {
                    commandWrapper.Command.CommandText = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", packageName, commandText);
                }
            }
        }

        /// <devdoc>
        /// Listens for the RowUpdate event on a data adapter to support UpdateBehavior.Continue
        /// </devdoc>
        private void OnOracleRowUpdated(object sender, OracleRowUpdatedEventArgs args)
        {
            if (args.RecordsAffected == 0)
            {
                if (args.Errors != null)
                {
                    args.Row.RowError = SR.ExceptionMessageUpdateDataSetRowFailure;
                    args.Status = UpdateStatus.SkipCurrentRow;
                }
            }
        }
    }
}