using Microsoft.Practices.EnterpriseLibrary.Configuration.Protection;
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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Protection
{
    /// <summary>
    /// <para>Represents the key settings produced by an <see cref="IKeyCreator"/>.</para>
    /// </summary>
    public class KeySettings
    {
        private IKeyCreator keyCreator;
        private byte[] key;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="KeySettings"/> class with a <see cref="IKeyCreator"/></para>
        /// </summary>
        /// <param name="keyCreator">The key creator that created the key.</param>
        public KeySettings(IKeyCreator keyCreator)
        {
            this.keyCreator = keyCreator;
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="IKeyCreator"/> that created this key.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="IKeyCreator"/> that created this key.</para>
        /// </value>
        public IKeyCreator KeyCreator
        {
            get { return keyCreator; }
            set { keyCreator = value; }
        }

        /// <summary>
        /// <para>Gets or sets the key.</para>
        /// </summary>
        /// <value>
        /// <para>The key created by the <see cref="IKeyCreator"/>.</para>
        /// </value>
        public byte[] Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// <para>Returns a message stating that the key is encrypted.</para>
        /// </summary>
        /// <returns><para>A message stating that the key is encrypted.</para></returns>
        public override string ToString()
        {
            return SR.KeyInfoLabelToString;
        }

        /// <summary>
        /// <para>Provides a shallow copy of this instance.</para>
        /// </summary>
        /// <returns><para>A new instance of a <see cref="KeySettings"/> object.</para></returns>
        public KeySettings Clone()
        {
            KeySettings clone = new KeySettings(this.keyCreator);
            clone.Key = this.key;
            return clone;
        }
    }
}