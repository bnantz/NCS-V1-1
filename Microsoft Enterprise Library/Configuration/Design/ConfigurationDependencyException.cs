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
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents an exception that occurs while resolving configuration dependencies.</para>
    /// </summary>
    [Serializable]
    public class ConfigurationDependencyException : Exception
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationDependencyException"/> class.</para>
        /// </summary>
        public ConfigurationDependencyException()
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationDependencyException"/> class with a specified error message.</para>
        /// </summary>
        /// <param name="message"><para>A message that describes the error.</para></param>
        public ConfigurationDependencyException(string message) : base(message)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationDependencyException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.</para>
        /// </summary>
        /// <param name="message"><para>The error message that explains the reason for the exception.</para></param>
        /// <param name="innerException"><para>The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</para></param>
        public ConfigurationDependencyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="ConfigurationDependencyException"/> class with serialized data.</para>
        /// </summary>
        /// <param name="info"><para>The object that holds the serialized object data.</para></param>
        /// <param name="context"><para>The contextual information about the source or destination.</para></param>
        protected ConfigurationDependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}