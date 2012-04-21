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
using System.Collections;
using System.Data;
using IBM.Data.DB2;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2
{
    /// <summary>
    /// <para>Represents a SQL statement or stored procedure to execute against an DB2 database.</para>
    /// </summary>   
    public class DB2CommandWrapper : DBCommandWrapper
    {
        private DB2Command command;
        private int rowsAffected;
        private Hashtable guidParameters;
        private object[] parameterValues;
        private bool parameterDiscoveryRequired;
        private Hashtable boolParameters;
        private char parameterToken;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DB2CommandWrapper"/> class with the text of a query and the command type.</para>
        /// </summary>        
        /// <param name="commandText"><para>The stored procedure name or SQL sting the command represents.</para></param>
        /// <param name="commandType"><para>One of the <see crer="CommandType"/> values.</para></param>
        /// <param name="parameterToken"><para>The parameter delimeter for database commands.</para></param>
        internal DB2CommandWrapper(string commandText, CommandType commandType, char parameterToken)
        {
            this.parameterToken = parameterToken;
            this.command = CreateCommand(commandText, commandType);
            this.guidParameters = new Hashtable();
            this.boolParameters = new Hashtable();
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="DB2CommandWrapper"/> class with the text of a query the command type, and the parameter values.</para>
        /// </summary>        
        /// <param name="commandText"><para>The stored procedure name or SQL sting the command represents.</para></param>
        /// <param name="commandType"><para>One of the <see crer="CommandType"/> values.</para></param>
        /// <param name="parameterToken"><para>The parameter delimeter for database commands.</para></param>
        /// <param name="parameterValues"><para>The parameter values to assign in positional order.</para></param>
        internal DB2CommandWrapper(string commandText, CommandType commandType, char parameterToken, object[] parameterValues) : this(commandText, commandType, parameterToken)
        {
            this.parameterValues = parameterValues;
            if (commandType == CommandType.StoredProcedure)
            {
                this.parameterDiscoveryRequired = true;
            }
        }

        /// <summary>
        /// <para>Gets the underlying <see cref="IDbCommand"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The underlying <see cref="IDbCommand"/>. The default is <see langword="null"/>.</para>
        /// </value>
        /// <seealso cref="DB2Command"/>
        public override IDbCommand Command
        {
            get { return this.command; }
        }

        /// <summary>
        /// <para>Gets or sets the rows affected by this command.</para>
        /// </summary>
        /// <value>
        /// <para>The rows affected by this command.</para>
        /// </value>
        public override int RowsAffected
        {
            get { return this.rowsAffected; }
            set { this.rowsAffected = value; }
        }

        /// <summary>
        /// <para>Gets or sets the wait time before terminating the attempt to execute a command and generating an error.</para>
        /// </summary>
        /// <value>
        /// <para>The wait time before terminating the attempt to execute a command and generating an error.</para>
        /// </value>      
        public override int CommandTimeout
        {
            get { return this.command.CommandTimeout; }
            set { this.command.CommandTimeout = value; }
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="DB2Parameter"/> object to the command.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>Avalue indicating whether the parameter accepts null values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>        
        public override void AddParameter(string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            this.command.Parameters.Add(CreateParameter(name, dbType, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value));
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="DB2Parameter"/> object to the command.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="db2Type"><para>One of the <see cref="DB2Type"/> values.</para></param>
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>
        /// <param name="nullable"><para>Avalue indicating whether the parameter accepts null values.</para></param>
        /// <param name="precision"><para>The maximum number of digits used to represent the <paramref name="value"/>.</para></param>
        /// <param name="scale"><para>The number of decimal places to which <paramref name="value"/> is resolved.</para></param>
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>      
        [CLSCompliant(false)]
        public void AddParameter(string name, DB2Type db2Type, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DB2Parameter param = CreateParameter(name, DbType.AnsiString, size, direction, nullable, precision, scale, sourceColumn, sourceVersion, value);
            param.DB2Type = db2Type;
            this.command.Parameters.Add(param);
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="DB2Parameter"/> object to the command.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>        
        /// <param name="direction"><para>One of the <see cref="ParameterDirection"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the <paramref name="value"/>.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public override void AddParameter(string name, DbType dbType, ParameterDirection direction, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            this.command.Parameters.Add(CreateParameter(name, dbType, 0, direction, false, 0, 0, sourceColumn, sourceVersion, value));
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="DB2Parameter"/> object to the command set as <see cref="ParameterDirection"/> value of Output.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>        
        /// <param name="size"><para>The maximum size of the data within the column.</para></param>        
        public override void AddOutParameter(string name, DbType dbType, int size)
        {
            this.command.Parameters.Add(CreateParameter(name, dbType, size, ParameterDirection.Output, false, 0, 0, String.Empty, DataRowVersion.Default, DBNull.Value));
        }

       /// <summary>
       /// <para>Does nothing in SQL Server.  Cursors are not used.  Only here for compliance with DBCommandWrapper</para>
       /// </summary>
       /// <param name="CursorName"><para>The name of the cursor</para></param>
       public override void AddCursorOutParameter(string CursorName)
       {
          // Do Nothing
          return;
       }

       /// <summary>
        /// <para>Adds a new instance of an <see cref="DB2Parameter"/> object to the command set as <see cref="ParameterDirection"/> value of Input.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <remarks>
        /// <para>This version of the method is used when you can have the same parameter object multiple times with different values.</para>
        /// </remarks>        
        public override void AddInParameter(string name, DbType dbType)
        {
            this.command.Parameters.Add(CreateParameter(name, dbType, 0, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, null));
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="DB2Parameter"/> object to the command set as <see cref="ParameterDirection"/> value of Input.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <param name="value"><para>The value of the parameter.</para></param>      
        public override void AddInParameter(string name, DbType dbType, object value)
        {
            this.command.Parameters.Add(CreateParameter(name, dbType, 0, ParameterDirection.Input, false, 0, 0, String.Empty, DataRowVersion.Default, value));
        }

        /// <summary>
        /// <para>Adds a new instance of an <see cref="DB2Parameter"/> object to the command set as <see cref="ParameterDirection"/> value of Input.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter.</para></param>
        /// <param name="dbType"><para>One of the <see cref="DbType"/> values.</para></param>                
        /// <param name="sourceColumn"><para>The name of the source column mapped to the DataSet and used for loading or returning the value.</para></param>
        /// <param name="sourceVersion"><para>One of the <see cref="DataRowVersion"/> values.</para></param>
        public override void AddInParameter(string name, DbType dbType, string sourceColumn, DataRowVersion sourceVersion)
        {
            this.command.Parameters.Add(CreateParameter(name, dbType, 0, ParameterDirection.Input, false, 0, 0, sourceColumn, sourceVersion, null));
        }

        /// <summary>
        /// <para>Returns the value of the parameter for the given <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter to get the value.</para></param>
        /// <returns><para>The value of the parameter.</para></returns>
        public override object GetParameterValue(string name)
        {
            string parameterName = BuildParameterName(name);
            DB2Parameter parameter = command.Parameters[parameterName];
            //check for DBNull
            if (parameter.Value is DBNull)
            {
                return DBNull.Value;
            }
            // cast the parameter as Guid if it is a guid parameter
            if (guidParameters.Contains(parameterName))
            {
                byte[] buffer = (byte[])parameter.Value;
                if (buffer.Length == 0)
                {
                    return DBNull.Value;
                }
                else
                {
                    return new Guid(buffer);
                }
            }
            else if (boolParameters.Contains(parameterName))
            {
                byte[] buffer = (byte[])parameter.Value;
                byte firstByte = (byte)buffer.GetValue(0);
                if ((Convert.ToChar(firstByte) == '0') || (firstByte == 0))
                {
                    return false;
                }
                else if (Convert.ToChar(firstByte) == '1' || (firstByte == 1))
                {
                    return true;
                }
                else
                {
                    return parameter.Value;
                }
            }
            return parameter.Value;
        }

        /// <summary>
        /// <para>Sets the value of a parameter for the given <paramref name="name"/>.</para>
        /// </summary>
        /// <param name="name"><para>The name of the parameter to set the value.</para></param>
        /// <param name="value"><para>The new value of the parameter.</para></param>
        public override void SetParameterValue(string name, object value)
        {
            DB2Parameter parameter = command.Parameters[BuildParameterName(name)];
            if (value is Guid)
            {
                parameter.Value = ((Guid)value).ToByteArray();
            }
            string tmpVal = value as string;
            if ((tmpVal != null) && (tmpVal.Length == 0))
            {
                parameter.Value = Convert.DBNull;
            }
            parameter.Value = (value == null) ? DBNull.Value : value;
        }

        /// <summary>
        /// <para>Clean up resources.</para>
        /// </summary>
        public override void Dispose()
        {
            this.command.Dispose();
        }

        /// <summary>
        /// <para>Dicover the parameters for a stored procedure using a separate connection and command.</para>
        /// </summary>
        /// <param name="parameterToken"><para>The parameter delimeter for database commands.</para></param>
        protected override void DoDiscoverParameters(char parameterToken)
        {
            this.parameterToken = parameterToken;
            DB2Command newCommand = CreateNewCommandAndConnectionForDiscovery();
            DB2CommandBuilder.DeriveParameters(newCommand);

            foreach (IDataParameter parameter in newCommand.Parameters)
            {
                IDataParameter cloneParameter = (IDataParameter)((ICloneable)parameter).Clone();
                cloneParameter.ParameterName = BuildParameterName(cloneParameter.ParameterName);
                this.command.Parameters.Add(cloneParameter);
            }

            newCommand.Connection.Close();
        }

        /// <summary>
        /// <para>Assign the values provided by a user to the command parameters discovered in positional order.</para>
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <para>The number of parameters does not match number of values for stored procedure.</para>
        /// </exception>
        protected override void DoAssignParameterValues()
        {
            if (SameNumberOfParametersAndValues() == false)
            {
                throw new InvalidOperationException(SR.ExceptionMessageParameterMatchFailure);
            }

            int returnParameter = 1;
            for (int i = 0; i < this.parameterValues.Length; i++)
            {
                IDataParameter parameter = this.command.Parameters[i + returnParameter];

                // There used to be code here that checked to see if the parameter was input or input/output
                // before assigning the value to it. We took it out because of an operational bug with
                // deriving parameters for a stored procedure. It turns out that output parameters are set
                // to input/output after discovery, so any direction checking was unneeded. Should it ever
                // be needed, it should go here, and check that a parameter is input or input/output before
                // assigning a value to it.
                SetParameterValue(parameter.ParameterName, this.parameterValues[i]);
            }
        }

        /// <summary>
        /// <para>Determine if a stored procedure is using parameter discovery.</para>
        /// </summary>
        /// <returns>
        /// <para><see langword="true"/> if further preparation is needed.</para>
        /// </returns>
        protected override bool DoIsFurtherPreparationNeeded()
        {
            return this.parameterDiscoveryRequired;
        }

        private DB2Parameter CreateParameter(string name, DbType dbType, int size, ParameterDirection direction, bool nullable, byte precision, byte scale, string sourceColumn, DataRowVersion sourceVersion, object value)
        {
            DB2Parameter param = this.command.CreateParameter();
            param.ParameterName = BuildParameterName(name);
            param.DbType = dbType;
            param.Size = size;
            param.Value = value;
            // modify parameter type and value for special cases
            switch (dbType)
            {
                    // for Guid, change to value to byte array
                case DbType.Guid:
                    guidParameters.Add(param.ParameterName, "System.Guid");
                    param.DB2Type = DB2Type.Binary;
                    param.Size = 16;
                    // convert Guid value to byte array only if not null
                    if ((value is DBNull) || (value == null))
                    {
                        param.Value = Convert.DBNull;
                    }
                    else
                    {
                        param.Value = ((Guid)value).ToByteArray();
                    }
                    break;
                case DbType.Boolean: //Map the Boolean type to byte[1]                
                    if ((value != DBNull.Value) && (param.Value != null))
                    {
                        if ((bool)param.Value)
                        {
                            param.Value = new byte[1] {Convert.ToByte('1')};
                        }
                        else
                        {
                            param.Value = new byte[1] {Convert.ToByte('0')};
                        }
                    }
                    param.DB2Type = DB2Type.Binary;
                    boolParameters.Add(param.ParameterName, "System.Bool");
                    break;
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    // for empty string, set it to DBNull
                    if (value == null)
                    {
                        param.Value = Convert.DBNull;
                    }
                    else if ((!(value is DBNull)) && ((string) value).Length == 0)
                    {
                        param.Value = Convert.DBNull;
                    }
                    break;
                default:
                    break;
            }
            param.Direction = direction;
            param.IsNullable = nullable;
            param.Precision = precision;
            param.Scale = scale;
            param.SourceColumn = sourceColumn;
            param.SourceVersion = sourceVersion;
            return param;
        }

        private bool SameNumberOfParametersAndValues()
        {
            int returnParameterCount = 1;
            int numberOfParametersToStoredProcedure = this.command.Parameters.Count - returnParameterCount;
            int numberOfValuesProvidedForStoredProcedure = this.parameterValues.Length;
            return numberOfParametersToStoredProcedure == numberOfValuesProvidedForStoredProcedure;
        }

        /// <devdoc>
        /// Discovery has to be done on its own connection to allow for the case of the
        /// connection being used being enrolled in a transaction. The DB2CommandBuilder.DeriveParameters
        /// method creates a new OracleCommand internally to communicate to the database, and it
        /// reuses the same connection that is passed in on the command object. If this command
        /// object has a connection that is enrolled in a transaction, the DeriveParameters method does not
        /// honor that transaction, and the call fails. To avoid this, create your own connection and
        /// command, and use them. 
        /// 
        /// You then have to clone each of the IDataParameter objects before it can be transferred to 
        /// the original command, or another exception is thrown.
        /// </devdoc>
        private DB2Command CreateNewCommandAndConnectionForDiscovery()
        {
            DB2Connection clonedConnection = (DB2Connection)((ICloneable)this.command.Connection).Clone();
            clonedConnection.Open();
            DB2Command newCommand = CreateCommand(this.command.CommandText, this.command.CommandType);
            newCommand.Connection = clonedConnection;
            return newCommand;
        }

        private static DB2Command CreateCommand(string commandText, CommandType commandType)
        {
            DB2Command newCommand = new DB2Command();
            newCommand.CommandText = commandText;
            newCommand.CommandType = commandType;
            return newCommand;
        }

        private string BuildParameterName(string name)
        {
            return name;
        }
    }
}