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
    /// Represents the configuration data for a <see cref="WrapHandler"/>.
    /// </summary>
    [XmlRoot("exceptionHandler", Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
    public class WrapHandlerData : ExceptionHandlerData
    {
        private string exceptionMessage;
        private string wrapExceptionTypeName;

        /// <summary>
        /// Initialize a new instance of the <see cref="WrapHandlerData"/> class.
        /// </summary>
        public WrapHandlerData()
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="WrapHandlerData"/> class with a name.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="WrapHandlerData"/>.
        /// </param>
        public WrapHandlerData(string name) : base(name)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="WrapHandlerData"/> class with a name, an exception message, the fully qualified assembly name of type of the wrapping exception.
        /// </summary>
        /// <param name="name">
        /// The name of the <see cref="WrapHandlerData"/>.
        /// </param>
        /// <param name="exceptionMessage">
        /// The exception message replacement.
        /// </param>
        /// <param name="wrapExceptionTypeName">
        /// The fully qualified assembly name of type of the wrapping exception
        /// </param>
        public WrapHandlerData(string name, string exceptionMessage, string wrapExceptionTypeName) : base(name)
        {
            this.exceptionMessage = exceptionMessage;
            this.wrapExceptionTypeName = wrapExceptionTypeName;
        }

        /// <summary>
        /// Gets or sets optional for exception message replacement.
        /// </summary>
        [XmlAttribute("exceptionMessage")]
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
            set { exceptionMessage = value; }
        }

        /// <summary>
        /// Gets or sets the fully qualified assembly name of type of the wrapping exception.
        /// </summary>
        [XmlAttribute("wrapExceptionTypeName")]
        public string WrapExceptionTypeName
        {
            get { return wrapExceptionTypeName; }
            set { wrapExceptionTypeName = value; }
        }

        /// <summary>
        /// Returns the fully qualified assembly name for a <c>WrapHandler</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(WrapHandler).AssemblyQualifiedName; }
            set
            {
            }
        }
    }
}