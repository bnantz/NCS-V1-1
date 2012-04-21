//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration
{
    /// <summary>
    /// Configuration information for DataCacheStorageData. This class represents the extra information, over and
    /// above what is defined in <see cref="CacheStorageData" />, needed to connect caching to the Data block.
    /// </summary>
    [XmlRoot("cacheStorage", Namespace=CacheManagerSettings.ConfigurationNamespace)]
    public class DataCacheStorageData : CacheStorageData
    {
        private string databaseInstanceName;
        private string partitionName;

        /// <summary>
        /// Initialize a new instance of the <see cref="DataCacheStorageData"/> class.
        /// </summary>
        public DataCacheStorageData()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="DataCacheStorageData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the provider.
        /// </param>
        public DataCacheStorageData(string name) : base(name)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="DataCacheStorageData"/> class with a name, database instance name, and partion name.
        /// </summary>
        /// <param name="name">
        /// The name of the provider.
        /// </param>
        /// <param name="databaseInstanceName">
        /// Name of the database instance to use for storage. Instance must be defined in Data configuration.
        /// </param>
        /// <param name="partitionName">
        /// Name of the particular section inside of a database used to store this provider's data. This 
        /// field allows different applications to share the same database safely, preventing any modification of 
        /// one application's data by a provider from another application.
        /// </param>
        public DataCacheStorageData(string name, string databaseInstanceName, string partitionName) : base(name)
        {
            this.databaseInstanceName = databaseInstanceName;
            this.partitionName = partitionName;
        }

        /// <summary>
        /// Name of the database instance to use for storage. Instance must be defined in Data configuration.
        /// </summary>
        [XmlAttribute("databaseInstanceName")]
        public string DatabaseInstanceName
        {
            get { return databaseInstanceName; }
            set { databaseInstanceName = value; }
        }

        /// <summary>
        /// Name of the particular section inside of a database used to store this provider's data. This 
        /// field allows different applications to share the same database safely, preventing any modification of 
        /// one application's data by a provider from another application.
        /// </summary>
        [XmlAttribute("partitionName")]
        public string PartitionName
        {
            get { return partitionName; }
            set { partitionName = value; }
        }

        /// <summary>
        /// String representation of the type of the DataBackingStore object to instantiate
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(DataBackingStore).AssemblyQualifiedName; }
            set
            {
            }
        }
    }
}