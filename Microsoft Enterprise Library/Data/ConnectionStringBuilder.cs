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

using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <devdoc>
    /// Builds a connection string out of individual parameters (username, password, server, database, etc).
    /// Reads configuration information from a connectionString node
    /// </devdoc>
    internal sealed class ConnectionStringBuilder
    {
        private ConnectionStringBuilder()
        {
        }

        /// <devdoc>
        /// Creates a connection string by reading a connectionString node from configuration.
        /// </devdoc>        
        public static string Build(ConnectionStringData connectionString)
        {
            StringBuilder connection = new StringBuilder();
            foreach (ParameterData setting in connectionString.Parameters)
            {
                connection.AppendFormat("{0}={1};", setting.Name, setting.Value);
            }
            return connection.ToString();
        }
    }
}