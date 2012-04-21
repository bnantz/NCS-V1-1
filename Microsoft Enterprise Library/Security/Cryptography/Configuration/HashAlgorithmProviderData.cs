//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// Configuration settings for the <c>HashAlgorithm</c> hash provider.
    /// </summary>
    [XmlRoot("hashProvider", Namespace=CryptographySettings.ConfigurationNamespace)]
    public class HashAlgorithmProviderData : HashProviderData
    {
        private string algorithmType;
        private bool saltEnabled;

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public HashAlgorithmProviderData()
        {
        }

        /// <summary>
        /// Initialized a new HashAlgorithmProviderData with the given name
        /// </summary>
        /// <param name="name">The name for this object</param>
        public HashAlgorithmProviderData(string name) : this(name, string.Empty, false)
        {
        }

        /// <summary>
        /// Initialized a new HashAlgorithmProviderData with the given name
        /// </summary>
        /// <param name="name">The name for this object</param>
        /// <param name="algorithmType">The hash algorithm to use</param>
        /// <param name="saltEnabled">Should a salt be used?</param>
        public HashAlgorithmProviderData(string name, string algorithmType, bool saltEnabled) : base(name)
        {
            this.algorithmType = algorithmType;
            this.saltEnabled = saltEnabled;
        }

        /// <summary>
        /// Gets or sets the type of <see cref="System.Security.Cryptography.HashAlgorithm"/>.
        /// </summary>
        [XmlAttribute("algorithmType")]
        public string AlgorithmType
        {
            get { return algorithmType; }
            set { algorithmType = value; }
        }

        /// <summary>
        /// Gets or sets the salt enabled flag.
        /// </summary>
        [XmlAttribute("saltEnabled")]
        public virtual bool SaltEnabled
        {
            get { return saltEnabled; }
            set { saltEnabled = value; }
        }

        /// <summary>
        /// Gets the assembly qualified name of this provider.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(HashAlgorithmProvider).AssemblyQualifiedName; }
            set
            {
            }
        }
    }
}