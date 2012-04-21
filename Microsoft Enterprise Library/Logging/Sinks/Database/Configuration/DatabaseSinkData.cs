//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Database.Configuration
{
    /// <summary>
    /// Represents the configuration settings for an <see cref="EnterpriseLibrary.Logging.Sinks.Database.DatabaseSink"/>.
    /// </summary>
    [XmlRoot("sink", Namespace=DistributorSettings.ConfigurationNamespace)]
    public class DatabaseSinkData : SinkData
    {
        private string databaseInstanceName = String.Empty;
        private string storedProcName = String.Empty;

        /// <summary>
        /// Create a new instance of a <see cref="DatabaseSinkData"/>.
        /// </summary>
        public DatabaseSinkData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="DatabaseSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        public DatabaseSinkData(string name) : this(name, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="DatabaseSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        /// <param name="storedProcName">
        /// Stored procedure to execute.
        /// </param>
        public DatabaseSinkData(string name, string storedProcName) : this(name, storedProcName, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="DatabaseSinkData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the sink.
        /// </param>
        /// <param name="databaseInstanceName">
        /// Enterprise Library Data service key name.
        /// </param>
        /// <param name="storedProcName">
        /// Stored procedure to execute.
        /// </param>
        public DatabaseSinkData(string name, string storedProcName, string databaseInstanceName) : base(name)
        {
            this.databaseInstanceName = databaseInstanceName;
            this.storedProcName = storedProcName;
        }

        /// <summary>
        /// Enterprise Library Data service key name.
        /// </summary>
        [XmlAttribute("databaseInstanceName")]
        public string DatabaseInstanceName
        {
            get { return databaseInstanceName; }
            set { databaseInstanceName = value; }
        }

        /// <summary>
        /// Stored procedure to execute.
        /// </summary>
        [XmlAttribute("storedProcName")]
        public string StoredProcName
        {
            get { return storedProcName; }
            set { storedProcName = value; }
        }

        /// <summary>
        /// Returns the fully qualified name of the <c>DatabaseSink</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(DatabaseSink).AssemblyQualifiedName; }
            set
            {
            }
        }
    }
}