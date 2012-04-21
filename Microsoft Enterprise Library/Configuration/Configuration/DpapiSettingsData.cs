//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// Configuration settings for any DPAPI implementation.
    /// </summary>
    /// <remarks>
    /// See 
    /// <a href="http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnsecure/html/windataprotection-dpapi.asp">Windows Data Protection</a> on
    /// MSDN for more information.
    /// </remarks>
    [XmlRoot("dpapiSettings", Namespace=ConfigurationSettings.ConfigurationNamespace)]
    public class DpapiSettingsData
    {
        private DpapiStorageMode mode;
        private byte[] entropy;

        /// <summary>
        /// <para>Initialize a new instnace of the <see cref="DpapiSettingsData"/> class.</para>
        /// </summary>
        public DpapiSettingsData() : this(null, DpapiStorageMode.User)
        {
        }

        /// <summary>
        /// <para>Initialize a new instnace of the <see cref="DpapiSettingsData"/> class with entropy and a <see cref="DpapiStorageMode"/>.</para>
        /// </summary>
        /// <param name="entropy">
        /// <para>The machine entropy</para>
        /// </param>
        /// <param name="mode">
        /// <para>One of the <see cref="DpapiStorageMode"/> values.</para>
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para>The value provided was out of the excepted valid ranges for <see cref="DpapiStorageMode"/>.</para>
        /// </exception>
        public DpapiSettingsData(byte[] entropy, DpapiStorageMode mode)
        {
            ValidateMode(mode);
            this.mode = mode;
            this.entropy = entropy;
        }

        /// <summary>
        /// <para>Gets or sets the DPAPI storage mode.</para>
        /// </summary>
        /// <value>
        /// <para>One of the <see cref="DpapiStorageMode"/> values.</para>
        /// </value>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para>The value provided was out of the excepted valid ranges for <see cref="DpapiStorageMode"/>.</para>
        /// </exception>
        [XmlAttribute("mode")]
        public DpapiStorageMode Mode
        {
            get { return mode; }
            set
            {
                ValidateMode(mode);
                mode = value;
            }
        }

        /// <summary>
        /// <para>Gets or sets the machine entropy.</para>
        /// </summary>
        /// <value>
        /// <para>The machine entropy.</para>
        /// </value>
        /// <remarks>
        /// <para>This is only required when the mode is set to <seealso cref="DpapiStorageMode.Machine"/>.</para>
        /// </remarks>
        [XmlAttribute("entropy")]
        public byte[] Entropy
        {
            get { return entropy; }
            set { entropy = value; }
        }

        private static void ValidateMode(DpapiStorageMode mode)
        {
            if (!Enum.IsDefined(typeof(DpapiStorageMode), mode))
            {
                throw new ArgumentOutOfRangeException("mode", SR.ExceptionOutOfRangeDpapiStorageMode);
            }
        }
    }
}