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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents an error that occurs while operating on a <see cref="ConfigurationNode"/> object.</para>
    /// </summary>   
    public class ConfigurationError
    {
        private readonly ConfigurationNode node;
        private readonly string message;

        /// <summary>
        /// <para>
        /// Initializes a new instance of the <see cref="ConfigurationError"/> class with the <see cref="ConfigurationNode"/> and an error message.
        /// </para>
        /// </summary>
        /// <param name="node"><para>The <see cref="ConfigurationNode"/> object.</para></param>
        /// <param name="message"><para>The. error message.</para></param>
        public ConfigurationError(ConfigurationNode node, string message)
        {
            this.node = node;
            this.message = message;
        }

        /// <summary>
        /// <para>Gets the <see cref="ConfigurationNode"/> where the error originated.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ConfigurationNode"/> where the error originated.</para>
        /// </value>
        public ConfigurationNode ConfigurationNode
        {
            get { return node; }
        }

        /// <summary>
        /// <para>Gets the error message.</para>
        /// </summary>
        /// <value>
        /// <para>The error message.</para>
        /// </value>
        public string Message
        {
            get { return message; }
        }

        /// <summary>
        /// <para>Creates and returns a string representation of the current error.</para>
        /// </summary>
        /// <returns><para>A string representation of the current error.</para></returns>
        public override string ToString()
        {
            return SR.ConfigurationErrorToString(ConfigurationNode.Name,
                                                 ConfigurationNode.Path,
                                                 Message);
        }

    }
}