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
using System.Data.OleDb;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Data.OleDb
{
   /// <summary>
   /// <para>Represents a OleDb Server Database.</para>
   /// </summary>
   /// <remarks> 
   /// <para>
   /// Internally uses OleDb Server .NET Managed Provider from Microsoft (System.Data.OleDbClient) to connect to the database.
   /// </para>  
   /// </remarks>
   public class OleDbDatabase : Database
   {
      /// <summary>
      /// Initialize a new instance of the <see cref="OleDbDatabase"/> class.
      /// </summary>
      public OleDbDatabase() : base()
      {
      }

      /// <summary>
      /// <para>Gets the parameter token used to delimit parameters for the OleDb Database.</para>
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
      /// <seealso cref="OleDbConnection"/>
      /// </summary>
      /// <returns>
      /// <para>The <see cref="OleDbConnection"/> for this database.</para>
      /// </returns>
      public override IDbConnection GetConnection()
      {
         return new OleDbConnection(ConnectionString);
      }

      /// <summary>
      /// <para>Create a <see cref="OleDbCommandWrapper"/> for a stored procedure.</para>
      /// </summary>
      /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
      /// <returns><para>The <see cref="OleDbCommandWrapper"/> for the stored procedure.</para></returns>
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

         return new OleDbCommandWrapper(storedProcedureName, CommandType.StoredProcedure, ParameterToken);
      }

      /// <summary>
      /// <para>Create an <see cref="OleDbCommandWrapper"/> for a stored procedure.</para>
      /// </summary>
      /// <param name="storedProcedureName"><para>The name of the stored procedure.</para></param>
      /// <param name="parameterValues"><para>The list of parameters for the procedure.</para></param>
      /// <returns><para>The <see cref="OleDbCommandWrapper"/> for the stored procedure.</para></returns>
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

         return new OleDbCommandWrapper(storedProcedureName, CommandType.StoredProcedure, ParameterToken, parameterValues);
      }

      /// <summary>
      /// <para>Create an <see cref="OleDbCommandWrapper"/> for a SQL query.</para>
      /// </summary>
      /// <param name="query"><para>The text of the query.</para></param>        
      /// <returns><para>The <see cref="OleDbCommandWrapper"/> for the SQL query.</para></returns>
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

         return new OleDbCommandWrapper(query, CommandType.Text, ParameterToken);
      }

      /// <summary>
      /// <para>Create a <see cref="OleDbDataAdapter"/> with the given update behavior and connection.</para>
      /// </summary>
      /// <param name="updateBehavior">
      /// <para>One of the <see cref="UpdateBehavior"/> values.</para>
      /// </param>
      /// <param name="connection">
      /// <para>The open connection to the database.</para>
      /// </param>
      /// <returns>An <see cref="OleDbDataAdapter"/>.</returns>
      /// <exception cref="ArgumentNullException">
      /// <para><paramref name="connection"/> can not be <see langword="null"/> (Nothing in Visual Basic).</para>
      /// </exception>
      protected override DbDataAdapter GetDataAdapter(UpdateBehavior updateBehavior, IDbConnection connection)
      {
         string queryStringToBeFilledInLater = String.Empty;
         OleDbDataAdapter adapter = new OleDbDataAdapter(queryStringToBeFilledInLater, (OleDbConnection)connection);

         if (updateBehavior == UpdateBehavior.Continue)
         {
            adapter.RowUpdated += new OleDbRowUpdatedEventHandler(OnOleDbRowUpdated);
         }
         return adapter;
      }

      /// <devdoc>
      /// Listens for the RowUpdate event on a dataadapter to support UpdateBehavior.Continue
      /// </devdoc>
      private void OnOleDbRowUpdated(object sender, OleDbRowUpdatedEventArgs rowThatCouldNotBeWritten)
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