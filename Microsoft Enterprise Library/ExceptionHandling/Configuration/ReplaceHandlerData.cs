//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration
{
    /// <summary>
    /// Represents the configuration data for a <see cref="ReplaceHandler"/>.
    /// </summary>
    [XmlRoot("exceptionHandler", Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
    public class ReplaceHandlerData : ExceptionHandlerData
    {
        private string exceptionMessage;
        private string replaceExceptionTypeName;

        /// <summary>
        /// Initialize a new instance of the <see cref="ReplaceHandlerData"/> class.
        /// </summary>
        public ReplaceHandlerData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ReplaceHandlerData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="ReplaceHandlerData"/>.
        /// </param>
        public ReplaceHandlerData(string name) : this(name, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ReplaceHandlerData"/> class with a name, exception message, and replace exception type name.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="ReplaceHandlerData"/>.
        /// </param>
        /// <param name="exceptionMessage">
        /// The exception message replacement.
        /// </param>
        /// <param name="replaceExceptionTypeName">
        /// The fully qualified assembly name the type of the replacing exception.
        /// </param>
        public ReplaceHandlerData(string name, string exceptionMessage, string replaceExceptionTypeName) : base(name)
        {
            this.exceptionMessage = exceptionMessage;
            this.replaceExceptionTypeName = replaceExceptionTypeName;
        }

        /// <summary>
        /// Gets or sets the optional for exception message replacement.
        /// </summary>
        [XmlAttribute("exceptionMessage")]
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }

        /// <summary>
        /// Gets or sets the fully qualified assembly name the type of the replacing exception.
        /// </summary>
        [XmlAttribute("replaceExceptionTypeName")]
        public string ReplaceExceptionTypeName
        {
            get { return replaceExceptionTypeName; }
            set { replaceExceptionTypeName = value; }
        }

        /// <summary>
        /// Returns the fully qualified assembly name for a <c>ReplaceHandler</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(ReplaceHandler).AssemblyQualifiedName; }
            set
            {
            }
        }

    }
}