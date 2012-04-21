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

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration
{
    /// <summary>
    /// <para>The name value pair used in the <see cref="NameValueItemCollection"/>.</para>
    /// </summary>
    [XmlRoot("extension")]
    public class NameValueItem
    {
        private string name;
        private string value;
        private NameValueItemCollection owner;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="NameValueItem"/> class.</para>
        /// </summary>
        public NameValueItem()
        {
        }

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="NameValueItem"/> class with a name and value.</para>
        /// </summary>
        /// <param name="name">
        /// <para>
        /// <para>The name of the item.</para>
        /// </para>
        /// </param>
        /// <param name="value">
        /// <para>The value of the item.</para>
        /// </param>
        public NameValueItem(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// <para>Gets or sets the name of the item.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the item.</para>
        /// </value>
        [XmlAttribute("name")]
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                if (this.Owner != null)
                {
                    this.Owner.Set(this);
                }
            }
        }

        /// <summary>
        /// <para>Gets or sets the value of the item.</para>
        /// </summary>
        /// <value>
        /// <para>The value of the item.</para>
        /// </value>
        [XmlAttribute("value")]
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        internal NameValueItemCollection Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /// <summary>
        /// <para>Returns a <see cref="System.String"/> that represents the current <see cref="NameValueItem"/>.</para>
        /// </summary>
        /// <returns>
        /// <para>The <seealso cref="Name"/> of the item.</para>
        /// </returns>
        public override string ToString()
        {
            return this.name;
        }
    }
}