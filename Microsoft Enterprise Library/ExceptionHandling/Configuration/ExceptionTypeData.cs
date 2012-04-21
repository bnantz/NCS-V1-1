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
    /// Represents the configuration for an <see cref="System.Exception"/>
    /// that will be handled by an exception policy.
    /// </summary>
    [XmlRoot("exceptionType", Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
    public class ExceptionTypeData
    {
        private ExceptionHandlerDataCollection exceptionHandlers;
        private string name;
        private string type;
        private PostHandlingAction postHandlingAction = PostHandlingAction.NotifyRethrow;

        /// <summary>
        /// Creates a new instance of ExceptionTypeData.
        /// </summary>
        public ExceptionTypeData() : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ExceptionTypeData"/> class.
        /// </summary>
        /// <param name="name">The configured name.</param>
        /// <param name="type">The full type name of any <see cref="System.Exception"/>.</param>
        public ExceptionTypeData(string name, string type) : this(name, type, PostHandlingAction.None)
        {
        }

        /// <summary>
        /// Initialize a new instance with a specified <see cref="PostHandlingAction"/>.
        /// </summary>
        /// <param name="name">The configured name.</param>
        /// <param name="type">The full type name of any <see cref="System.Exception"/>.</param>
        /// <param name="postHandlingAction">One of the <see cref="PostHandlingAction"/> values.</param>
        public ExceptionTypeData(string name, string type, PostHandlingAction postHandlingAction)
        {
            this.name = name;
            this.type = type;
            this.postHandlingAction = postHandlingAction;
            this.exceptionHandlers = new ExceptionHandlerDataCollection();
        }

        /// <summary>
        /// Gets or sets the name of the Exception Type.
        /// </summary>
        [XmlAttribute("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the fully qualified assembly name of the type.
        /// </summary>
        [XmlAttribute("type")]
        public string TypeName
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Gets or sets the mode in which to conduct a rethrow.
        /// </summary>
        [XmlAttribute("postHandlingAction")]
        public PostHandlingAction PostHandlingAction
        {
            get { return postHandlingAction; }
            set { postHandlingAction = value; }
        }

        /// <summary>
        /// Gets or sets the handler chain that will get executed when an exception of this type is thrown.
        /// </summary>
        [XmlArray(ElementName="exceptionHandlers", Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
        [XmlArrayItem(ElementName="exceptionHandler", Type=typeof(ExceptionHandlerData), Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
        public ExceptionHandlerDataCollection ExceptionHandlers
        {
            get { return exceptionHandlers; }
        }
    }
}