//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Authorization
{
    /// <summary>
    /// Exception throw by an 
    /// <see cref="AuthorizationRuleProvider"/> 
    /// when the requested rule is not found.
    /// </summary>
    [Serializable]
    public class AuthorizationRuleNotFoundException : ConfigurationException
    {
        private const string RuleNameKey = "ruleName";
        private string ruleName;

        /// <summary>
        /// Initializes an <c>AuthorizationRuleNotFoundException</c> 
        /// with a specified error message.
        /// </summary>
        public AuthorizationRuleNotFoundException()
        {
        }

        /// <summary>
        /// Initializes an <c>AuthorizationRuleNotFoundException</c> with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public AuthorizationRuleNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes an <c>AuthorizationRuleNotFoundException</c> with a specified error message
        /// and inner exception.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public AuthorizationRuleNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <c>AuthorizationRuleNotFoundException</c> class with a specified error message.
        /// </summary>
        /// <param name="ruleName">The name of the rule provider.</param>
        /// <param name="message">A message that describes the error.</param>
        public AuthorizationRuleNotFoundException(string ruleName, string message) : base(message)
        {
            this.ruleName = ruleName;
        }

        /// <summary>
        /// Initializes a new instance of the <c>AuthorizationRuleNotFoundException</c> class with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.
        /// </param>
        protected AuthorizationRuleNotFoundException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            this.ruleName = info.GetString(RuleNameKey);
        }

        /// <summary>
        /// Gets the requested rule name
        /// that was not found.
        /// </summary>
        /// <value>The name of the rule provider.</value>
        public string RuleName
        {
            get { return this.ruleName; }
        }

        /// <summary>
        /// Sets the SerializationInfo object 
        /// with the rule name and 
        /// additional exception information.
        /// </summary>
        /// <param name="info">The object that holds 
        /// the serialized object data.</param>
        /// <param name="context">The contextual information 
        /// about the source or destination. 
        /// </param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(RuleNameKey, this.ruleName);
        }
    }
}