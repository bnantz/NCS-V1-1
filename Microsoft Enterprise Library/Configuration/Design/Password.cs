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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a password and renders it as asterisks for situations in which the password should not be displayed to the user.</para>
    /// </summary>
    [Serializable]
    public class Password
    {
        private string passwordText;

        /// <summary>
        /// <para>Initialzie a new instance of the <see cref="Password"/> class with the password text.</para>
        /// </summary>
        /// <param name="passwordText">
        /// <para>The password text.</para>
        /// </param>
        public Password(string passwordText)
        {
            this.passwordText = passwordText;
        }

        /// <summary>
        /// <para>Gets the password text.</para>
        /// </summary>
        /// <value>
        /// <para>The password text.</para>
        /// </value>
        public string PasswordText
        {
            get { return passwordText; }
        }

        /// <summary>
        /// <para>Creates and returns a string representation of the current password.</para>
        /// </summary>
        /// <returns><para>A string representation of the current password.</para></returns>
        public override string ToString()
        {
            return "***********";
        }
    }
}