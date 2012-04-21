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

using Microsoft.Practices.EnterpriseLibrary.Common.Cryptography;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <summary>
    /// <para>Represents the settings used when encrypting / decrypting data with DPAPI.</para>
    /// </summary>
    public sealed class DpapiSettings
    {
        private DpapiStorageMode mode;
        private byte[] entropy;

        private DpapiSettings()
        {
            mode = DpapiStorageMode.None;
        }

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="DpapiSettings"/> class with a <see cref="DpapiSettingsData"/> object.</para>
        /// </summary>
        /// <param name="settingsData">A <see cref="DpapiSettingsData"/> object containing the configuration data from storage.</param>
        public DpapiSettings(DpapiSettingsData settingsData)
        {
            if (settingsData != null)
            {
                this.Mode = settingsData.Mode;
                this.Entropy = settingsData.Entropy;
            }
            else
            {
                this.Mode = DpapiStorageMode.Machine;
                this.Entropy = null;
            }
        }

        /// <summary>
        /// <para>Gets or sets the DPAPI storage mode used to encrypt and decrypt data.</para>
        /// </summary>
        /// <value>
        /// <para>One of the <see cref="DpapiStorageMode"/> values.</para>
        /// </value>
        public DpapiStorageMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        /// <summary>
        /// Gets or sets the entropy for machine mode.
        /// </summary>
        public byte[] Entropy
        {
            get { return entropy; }
            set { entropy = value; }
        }

        /// <summary>
        /// Determines whether or not two DpapiSettings objects have equal properties.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><c>true</c> if equal, otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }

            DpapiSettings compareObj = obj as DpapiSettings;

            if (compareObj == null)
            {
                return false;
            }
            if (compareObj.Mode != this.Mode)
            {
                return false;
            }
            if (!CryptographyUtility.CompareBytes(compareObj.Entropy, this.Entropy))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Calls base. Two objects that are determined as "equal" will not 
        /// necessarily return the same hashcode.
        /// </summary>
        /// <returns>A unique hash representing this object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Renders a summary of these settings to a string.
        /// </summary>
        /// <returns>A summary or initialization status.</returns>
        public override string ToString()
        {
            if (this.mode != DpapiStorageMode.None)
            {
                return SR.DpapiModeToString(this.Mode.ToString());
            }
            else
            {
                return SR.DefaultDpapiModeToString;
            }
        }
    }
}