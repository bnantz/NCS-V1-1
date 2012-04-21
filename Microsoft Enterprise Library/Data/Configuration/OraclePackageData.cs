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

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary>
    /// <para>Represents the package information to use when calling a stored procedure for Oracle.</para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// A package name can be appended to the stored procedure name of a command if the prefix of the stored procedure
    /// matchs the prefix defined. This allows the caller of the stored procedure to use stored procedures
    /// in a more database independent fashion.
    /// </para>
    /// </remarks>
    [XmlRoot("package", Namespace=DatabaseSettings.ConfigurationNamespace)]
    public class OraclePackageData
    {
        private string prefix;
        private string name;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="OraclePackageData"/> class.</para>
        /// </summary>
        public OraclePackageData()
        {
            prefix = string.Empty;
            name = string.Empty;
        }

        /// <summary>
        /// <para>Initilaize a new instance of the <see cref="OraclePackageData"/> class given the prefix to search for and the name of the package.</para>
        /// </summary>
        /// <param name="name">
        /// <para>The name of the package to append to any found procedure that has the <paramref name="prefix"/>.</para>
        /// </param>
        /// <param name="prefix">
        /// <para>The prefix of the stored procedures used in this package.</para>
        /// </param>
        public OraclePackageData(string name, string prefix) : this()
        {
            this.prefix = prefix;
            this.name = name;
        }

        /// <summary>
        /// <para>Gets or sets the prefix of the stored procedures that are in the package in Oracle.</para>
        /// </summary>
        /// <value>
        /// <para>The prefix of the stored procedures that are in the package in Oracle.</para>
        /// </value>
        [XmlAttribute("prefix")]
        public string Prefix
        {
            get { return this.prefix; }
            set { this.prefix = value; }
        }

        /// <summary>
        /// <para>Gets or sets the name of the package to append to a stored procedure.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the package to append to a stored procedure.</para>
        /// </value>
        /// <remarks>
        /// <para>
        /// This name will be appended to the begining of a stored procedure name when calling <b>any</b> stored procedure in the 
        /// Oracle instance that this connection string represents.
        /// </para>
        /// </remarks>
        [XmlAttribute("name")]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}