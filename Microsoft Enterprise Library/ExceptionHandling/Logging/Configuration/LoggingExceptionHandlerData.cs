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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration
{
    /// <summary>
    /// Represents configuration for <see cref="LoggingExceptionHandler"/>s.
    /// </summary>
    [XmlRoot("exceptionHandler", Namespace=ExceptionHandlingSettings.ConfigurationNamespace)]
    public class LoggingExceptionHandlerData : ExceptionHandlerData
    {
        private string defaultLogCategory;
        private int defaultEventID;
        private Severity defaultSeverity;
        private string defaultTitle;
        private string formatterTypeName;
        private int minimumPriority;

        /// <summary>
        /// Initializes with default values.
        /// </summary>
        public LoggingExceptionHandlerData() : this(string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingExceptionHandlerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the handler.
        /// </param>
        public LoggingExceptionHandlerData(string name) : this(name, string.Empty, 0, Severity.Unspecified, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingExceptionHandlerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the handler.
        /// </param>
        /// <param name="defaultLogCategory">
        /// The default log category.
        /// </param>
        /// <param name="defaultEventId">
        /// The default eventID.
        /// </param>
        /// <param name="defaultSeverity">
        /// The default severity.
        /// </param>
        /// <param name="defaultTitle">
        /// The default title.
        /// </param>
        /// <param name="formatterTypeName">
        /// The formatter fully qualified assembly type name
        /// </param>
        public LoggingExceptionHandlerData(string name, string defaultLogCategory, int defaultEventId, Severity defaultSeverity, string defaultTitle, string formatterTypeName) : this(name, string.Empty, 0, Severity.Unspecified, string.Empty, string.Empty, 0)
        {
            this.defaultLogCategory = defaultLogCategory;
            defaultEventID = defaultEventId;
            this.defaultSeverity = defaultSeverity;
            this.defaultTitle = defaultTitle;
            this.formatterTypeName = formatterTypeName;
            this.minimumPriority = minimumPriority;
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingExceptionHandlerData"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the handler.
        /// </param>
        /// <param name="defaultLogCategory">
        /// The default log category.
        /// </param>
        /// <param name="defaultEventId">
        /// The default eventID.
        /// </param>
        /// <param name="defaultSeverity">
        /// The default severity.
        /// </param>
        /// <param name="defaultTitle">
        /// The default title.
        /// </param>
        /// <param name="formatterTypeName">
        /// The formatter fully qualified assembly type name
        /// </param>
        /// <param name="minimumPriority">
        /// The minimum value for messages to be processed.  Messages with a priority below the minimum are dropped immediately on the client.
        /// </param>
        public LoggingExceptionHandlerData(string name, string defaultLogCategory, int defaultEventId, Severity defaultSeverity, string defaultTitle, string formatterTypeName, int minimumPriority) : base(name)
        {
            this.defaultLogCategory = defaultLogCategory;
            defaultEventID = defaultEventId;
            this.defaultSeverity = defaultSeverity;
            this.defaultTitle = defaultTitle;
            this.formatterTypeName = formatterTypeName;
            this.minimumPriority = minimumPriority;
        }

        /// <summary>
        /// Returns the fully qualified name of the <c>LoggingExceptionHandler</c>.
        /// </summary>
        [XmlIgnore]
        public override string TypeName
        {
            get { return typeof(LoggingExceptionHandler).AssemblyQualifiedName; }
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the default log category.
        /// </summary>
        [XmlAttribute("defaultLogCategory")]
        public string DefaultLogCategory
        {
            get { return defaultLogCategory; }
            set { defaultLogCategory = value; }
        }

        /// <summary>
        /// Gets or sets the default eventID.
        /// </summary>
        [XmlAttribute("defaultEventID")]
        public int DefaultEventID
        {
            get { return defaultEventID; }
            set { defaultEventID = value; }
        }

        /// <summary>
        /// Gets or sets the default severity.
        /// </summary>
        [XmlAttribute("defaultSeverity")]
        public Severity DefaultSeverity
        {
            get { return defaultSeverity; }
            set { defaultSeverity = value; }
        }

        /// <summary>
        ///  Gets or sets the default title.
        /// </summary>
        [XmlAttribute("defaultTitle")]
        public string DefaultTitle
        {
            get { return defaultTitle; }
            set { defaultTitle = value; }
        }

        /// <summary>
        /// Gets or sets the formatter fully qualified assembly type name
        /// </summary>
        [XmlAttribute("formatterTypeName")]
        public string FormatterTypeName
        {
            get { return formatterTypeName; }
            set { formatterTypeName = value; }
        }

        /// <summary>
        /// Gets or sets the minimum value for messages to be processed.  Messages with a priority
        /// below the minimum are dropped immediately on the client.
        /// </summary>
        [XmlAttribute("minimumPriority")]
        public int MinimumPriority
        {
            get { return minimumPriority; }
            set { minimumPriority = value; }
        }
    }
}