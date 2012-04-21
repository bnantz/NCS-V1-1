//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// Represents an authentication credential consisting of a
    /// name and password associated with a user or system account.
    /// </summary>
    public class NamePasswordCredential
    {
        private string name;
        private byte[] password;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamePasswordCredential"/>
        /// class with the specified name and password.
        /// </summary>
        /// <param name="name">A name associated with a user or system account.</param>
        /// <param name="password">A password associated with a user or system</param>
        public NamePasswordCredential(string name, byte[] password)
        {
            if (name == null || name.Length == 0)
            {
                throw new ArgumentNullException("name");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            this.name = name;
            this.password = password;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamePasswordCredential"/>
        /// class with the specified name and password.
        /// </summary>
        /// <param name="name">A name associated with a user or system account.</param>
        /// <param name="password">A password associated with a user or system</param>
        /// <remarks>
        /// It is recommended that you use <see cref="NamePasswordCredential(string, byte[])"/>
        /// so that you can overwrite the password bytes when you are done using them.
        /// </remarks>
        public NamePasswordCredential(string name, string password)
        {
            if (name == null || name.Length == 0)
            {
                throw new ArgumentNullException("name");
            }

            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            this.name = name;
            this.password = Encoding.Unicode.GetBytes(password);
        }

        /// <summary>
        /// Gets the name of this credential.
        /// </summary>
        /// <value>A string.</value>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the password of this credential as a byte array.
        /// </summary>
        public byte[] PasswordBytes
        {
            get { return this.password; }
        }

        /// <summary>
        /// Gets the password of this credential as a string.
        /// </summary>
        /// <remarks>
        /// It is recommended that you use PasswordBytes so that you can overwrite the password bytes
        /// when you are done using them.
        /// </remarks>
        public string Password
        {
            get { return Encoding.Unicode.GetString(this.password); }
        }
    }
}