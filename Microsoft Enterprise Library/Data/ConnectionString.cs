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
using System.Globalization;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <devdoc>
    /// ConnectionString class constructs a connection string by 
    /// inserting a username and password into a template
    /// </devdoc>
    internal class ConnectionString
    {
        private const char CONNSTRING_DELIM = ';';
        private string connectionString;
        private string connectionStringWithoutCredentials;
        private string userIdTokens;
        private string passwordTokens;

        public ConnectionString(string connectionString, string userIdTokens, string passwordTokens)
        {
            this.connectionString = connectionString;
            this.userIdTokens = userIdTokens;
            this.passwordTokens = passwordTokens;

            this.connectionStringWithoutCredentials = RemoveCredentials(this.connectionString);
        }

        /// <devdoc>
        /// Database username for the connection string
        /// </devdoc>
        public string UserName
        {
            get
            {
                string lowConnString = connectionString.ToLower(CultureInfo.CurrentCulture);
                int uidPos;
                int uidMPos;

                GetTokenPositions(userIdTokens, out uidPos, out uidMPos);
                if (0 <= uidPos)
                {
                    // found a user id, so pull out the value
                    int uidEPos = lowConnString.IndexOf(CONNSTRING_DELIM, uidMPos);
                    return connectionString.Substring(uidMPos, uidEPos - uidMPos);
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                string lowConnString = connectionString.ToLower(CultureInfo.CurrentCulture);
                int uidPos;
                int uidMPos;
                GetTokenPositions(userIdTokens, out uidPos, out uidMPos);
                if (0 <= uidPos)
                {
                    // found a user id, so replace the value
                    int uidEPos = lowConnString.IndexOf(CONNSTRING_DELIM, uidMPos);
                    connectionString = connectionString.Substring(0, uidMPos) +
                        value + connectionString.Substring(uidEPos);

                    //_connectionStringNoCredentials = RemoveCredentials(_connectionString);
                }
                else
                {
                    //no user id in the connection string so just append to the connection string
                    string[] tokens = userIdTokens.Split(',');
                    connectionString += tokens[0] + value + CONNSTRING_DELIM;
                }
            }
        }

        /// <devdoc>
        /// User password for the connection string
        /// </devdoc>
        public string Password
        {
            get
            {
                if (connectionString == null)
                {
                    throw new InvalidOperationException(SR.ExceptionConnectionStringNotSet);
                }

                string lowConnString = connectionString.ToLower(CultureInfo.CurrentCulture);
                int pwdPos;
                int pwdMPos;
                GetTokenPositions(passwordTokens, out pwdPos, out pwdMPos);

                if (0 <= pwdPos)
                {
                    // found a password, so pull out the value
                    int pwdEPos = lowConnString.IndexOf(CONNSTRING_DELIM, pwdMPos);
                    return connectionString.Substring(pwdMPos, pwdEPos - pwdMPos);
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                string lowConnString = connectionString.ToLower(CultureInfo.CurrentCulture);
                int pwdPos;
                int pwdMPos;
                GetTokenPositions(passwordTokens, out pwdPos, out pwdMPos);

                if (0 <= pwdPos)
                {
                    // found a password, so replace the value
                    int pwdEPos = lowConnString.IndexOf(CONNSTRING_DELIM, pwdMPos);
                    connectionString = connectionString.Substring(0, pwdMPos) + value + connectionString.Substring(pwdEPos);

                    //_connectionStringNoCredentials = RemoveCredentials(_connectionString);
                }
                else
                {
                    //no password in the connection string so just append to the connection string
                    string[] tokens = passwordTokens.Split(',');
                    connectionString += tokens[0] + value + CONNSTRING_DELIM;
                }
            }
        }

        /// <devdoc>
        /// Gets the formatted connection string
        /// </devdoc>        
        public override string ToString()
        {
            return connectionString;
        }

        /// <devdoc>
        /// Gets the formatted connection string without the username and password
        /// </devdoc>        
        public string ToStringNoCredentials()
        {
            return connectionStringWithoutCredentials;
        }

        /// <devdoc>
        /// Formats a new connection string with user id and password
        /// </devdoc>        
        public ConnectionString CreateNewConnectionString(string newConnectionString)
        {
            return new ConnectionString(newConnectionString, userIdTokens, passwordTokens);
        }

        private void GetTokenPositions(string tokenString, out int tokenPos, out int tokenMPos)
        {
            string[] tokens = tokenString.Split(',');
            int currentPos = -1;
            int previousPos = -1;
            string lowConnString = connectionString.ToLower(CultureInfo.CurrentCulture);

            //initialze output parameter
            tokenPos = -1;
            tokenMPos = -1;
            foreach (string token in tokens)
            {
                currentPos = lowConnString.IndexOf(token);
                if (currentPos > previousPos)
                {
                    tokenPos = currentPos;
                    tokenMPos = currentPos + token.Length;
                    previousPos = currentPos;
                }
            }
        }

        private string RemoveCredentials(string connectionString)
        {
            StringBuilder connStringNoCredentials = new StringBuilder();
            if (connectionString == null)
            {
                return string.Empty;
            }

            string[] tokens = connectionString.ToLower(CultureInfo.CurrentCulture).Split(CONNSTRING_DELIM);

            string thingsToRemove = userIdTokens + "," + passwordTokens;
            string[] avoidTokens = thingsToRemove.ToLower(CultureInfo.CurrentCulture).Split(',');

            foreach (string section in tokens)
            {
                bool found = false;
                string token = section.Trim();
                if (token.Length != 0)
                {
                    foreach (string avoidToken in avoidTokens)
                    {
                        if (token.StartsWith(avoidToken))
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        connStringNoCredentials.Append(token + CONNSTRING_DELIM);
                    }
                }
            }
            return connStringNoCredentials.ToString();
        }
    }
}