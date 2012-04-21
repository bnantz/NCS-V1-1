//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <summary>
    /// <para>Represents a <see cref="FileKeyAlgorithmPairStorageProvider"/> node in a <see cref="IUIHierarchy"/> used for encrypting and decrypting configuration files.</para>
    /// </summary>
    public class FileKeyAlgorithmPairStorageProviderNode : KeyAlgorithmStorageProviderNode
    {
        private FileKeyAlgorithmPairStorageProviderData data;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="FileKeyAlgorithmPairStorageProviderNode"/> class.</para>
        /// </summary>
        public FileKeyAlgorithmPairStorageProviderNode() : this(new FileKeyAlgorithmPairStorageProviderData(SR.DefaultFileKeyAlgorithmStorageProviderNodeName, string.Empty))
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="FileKeyAlgorithmPairStorageProviderNode"/> class with a <see cref="FileKeyAlgorithmPairStorageProviderData"/> object.</para>
        /// </summary>
        /// <param name="data">
        /// <para>The runtime configuration data for the <see cref="FileKeyAlgorithmPairStorageProvider"/>..</para>
        /// </param>
        public FileKeyAlgorithmPairStorageProviderNode(FileKeyAlgorithmPairStorageProviderData data) : base(data)
        {
            this.data = data;
        }

        /// <summary>
        /// <para>Gets the name for the node.</para>
        /// </summary>
        /// <value>
        /// <para>The display name for the node.</para>
        /// </value>
        /// <remarks>
        /// <para>The name should be the <seealso cref="ISite.Name"/>.</para>
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// <para>The name already exists in the parent's node collection.</para>
        /// </exception>
        [ReadOnly(true)]
        public override string Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// <para>Gets or sets the path of the file for storage.</para>
        /// </summary>
        /// <value>
        /// <para>The path of the file for storage. The default is an empty string.</para>
        /// </value>
        /// <seealso cref="FileKeyAlgorithmPairStorageProviderData.Path"/>
        [Required]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.FileKeyAlgorithmStorageProviderNodeFileDescription)]
        [ReadOnly(true)]
        public string File
        {
            get { return data.Path; }
            set { data.Path = value; }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="DpapiSettings"/> used to protect the file.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="DpapiSettings"/> used to protect the file or <see langword="null"/> (Nothing in Visual Basic) if no settings exist.</para>
        /// </value>
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.FileKeyAlgorithmStorageProviderNodeDpapiSettingsDescription)]
        [Editor(typeof(FileKeyAlgorithmDpapiSettingsEditor), typeof(UITypeEditor))]
        public DpapiSettings DpapiSettings
        {
            get
            {
                if (data.DpapiSettings == null)
                {
                    return null;
                }
                else
                {
                    return new DpapiSettings(data.DpapiSettings);
                }
            }
            set
            {
                if (value == null)
                {
                    this.data.DpapiSettings = null;
                }
                else
                {
                    if (this.data.DpapiSettings == null)
                    {
                        this.data.DpapiSettings = new DpapiSettingsData();
                    }
                    data.DpapiSettings.Mode = value.Mode;
                    data.DpapiSettings.Entropy = value.Entropy;
                }
            }
        }

        /// <summary>
        /// <para>Gets the fully qualified assembly name for a <see cref="FileKeyAlgorithmPairStorageProvider"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The fully qualified assembly name for a <see cref="FileKeyAlgorithmPairStorageProvider"/>.</para>
        /// </value>
        /// <seealso cref="FileKeyAlgorithmPairStorageProviderData.TypeName"/>
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescriptionAttribute(SR.Keys.TypeNameDescription)]
        [ReadOnly(true)]
        public string TypeName
        {
            get { return data.TypeName; }
        }

        /// <summary>
        /// <para>Gets the <see cref="KeyAlgorithmPair"/> that contains the algorithm and key to protect the configuraiton files.</para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// The editing of a key algorithm pair is not allowed for storage providers.
        /// Therefore a new storage provider must be created in order to swap out the key.</para>
        /// </remarks>
        [ReadOnly(true)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        [SRDescription(SR.Keys.FileKeyAlgorithmStorageProviderNodeKeyAlgorithmPairDescription)]
        public override KeyAlgorithmPair KeyAlgorithmPair
        {
            get { return base.KeyAlgorithmPair; }
        }

    }
}