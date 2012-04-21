//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Facade for writing a log entry to one or more log sinks.  This class is sealed.
    /// </summary>
    public sealed class Logger
    {
        private static object sync = new object();
        private static volatile LogWriter writer;

        private const int DefaultPriority = -1;
        private const Severity DefaultSeverity = Severity.Unspecified;
        private const int DefaultEventId = 1;
        private const string DefaultTitle = "";

        /// <summary>
        /// No instances allowed.  Use the static methods on this class.
        /// </summary>
        private Logger()
        {
        }

        /// <summary>
        /// Add a key/value pair to the <see cref="System.Runtime.Remoting.Messaging.CallContext"/> dictionary.  
        /// Context items will be recorded with every log entry.
        /// </summary>
        /// <param name="key">Hashtable key</param>
        /// <param name="value">Value.  Objects will be serialized.</param>
        /// <example>The following example demonstrates use of the AddContextItem method.
        /// <code>Logger.SetContextItem("SessionID", myComponent.SessionId);</code></example>
        public static void SetContextItem(object key, object value)
        {
            Writer.SetContextItem(key, value);
        }

        /// <summary>
        /// Empty the context items dictionary.
        /// </summary>
        public static void FlushContextItems()
        {
            Writer.FlushContextItems();
        }

        /// <overloads>
        /// Write a new log entry to the default category.
        /// </overloads>
        /// <summary>
        /// Write a new log entry to the default category.
        /// </summary>
        /// <example>The following example demonstrates use of the Write method with
        /// one required parameter, message.
        /// <code>Logger.Write("My message body");</code></example>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        public static void Write(object message)
        {
            Write(message, "", DefaultPriority,
                  DefaultEventId, DefaultSeverity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry to a specific category.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more sinks.</param>
        public static void Write(object message, string category)
        {
            Write(message, category, DefaultPriority, DefaultEventId, DefaultSeverity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific category and priority.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more sinks.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        public static void Write(object message, string category, int priority)
        {
            Write(message, category, priority, DefaultEventId, DefaultSeverity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority and event id.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more sinks.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        public static void Write(object message, string category, int priority, int eventId)
        {
            Write(message, category, priority, eventId, DefaultSeverity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority, event id and severity.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more sinks.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        public static void Write(object message, string category, int priority, int eventId, Severity severity)
        {
            Write(message, category, priority, eventId, severity, DefaultTitle, null);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority, event id, severity
        /// and title.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more sinks.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log message severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message</param>
        public static void Write(object message, string category, int priority, int eventId,
                                 Severity severity, string title)
        {
            Write(message, category, priority, eventId, severity, title, null);
        }

        /// <summary>
        /// Write a new log entry and a dictionary of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public static void Write(object message, IDictionary properties)
        {
            Write(message, "", DefaultPriority,
                  DefaultEventId, DefaultSeverity, DefaultTitle, properties);
        }

        /// <summary>
        /// Write a new log entry to a specific category with a dictionary 
        /// of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more sinks.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public static void Write(object message, string category, IDictionary properties)
        {
            Write(message, category, DefaultPriority, DefaultEventId, DefaultSeverity,
                  DefaultTitle, properties);
        }

        /// <summary>
        /// Write a new log entry to with a specific category, priority and a dictionary 
        /// of extended properties.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more sinks.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public static void Write(object message, string category, int priority, IDictionary properties)
        {
            Write(message, category, priority, DefaultEventId, DefaultSeverity, DefaultTitle, properties);
        }

        /// <summary>
        /// Write a new log entry with a specific category, priority, event Id, severity
        /// title and dictionary of extended properties.
        /// </summary>
        /// <example>The following example demonstrates use of the Write method with
        /// a full set of parameters.
        /// <code></code></example>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Category name used to route the log entry to a one or more sinks.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log message severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).</param>
        /// <param name="title">Additional description of the log entry message.</param>
        /// <param name="properties">Dictionary of key/value pairs to log.</param>
        public static void Write(object message, string category, int priority, int eventId,
                                 Severity severity, string title, IDictionary properties)
        {
            LogEntry log = new LogEntry();
            log.Message = message.ToString();
            log.Category = category;
            log.Priority = priority;
            log.EventId = eventId;
            log.Severity = severity;
            log.Title = title;
            log.ExtendedProperties = properties;

            Write(log);
        }

        /// <summary>
        /// Write a new log entry as defined in the <see cref="LogEntry"/> parameter.
        /// </summary>
        /// <example>The following examples demonstrates use of the Write method using
        /// a <see cref="LogEntry"/> type.
        /// <code>
        /// LogEntry log = new LogEntry();
        /// log.Category = "MyCategory1";
        /// log.Message = "My message body";
        /// log.Severity = Severity.Error;
        /// log.Priority = 100;
        /// Logger.Write(log);</code></example>
        /// <param name="log">Log entry object to write.</param>
        public static void Write(LogEntry log)
        {
            Writer.Write(log);
        }

       /// <summary>
       /// Write a new log entry as defined in the <see cref="WSLogEntry"/> parameter.
       /// </summary>
       /// <example>The following examples demonstrates use of the Write method using
       /// <code>
       /// LogEntry log = new LogEntry();
       /// log.Category = "MyCategory1";
       /// log.Message = "My message body";
       /// log.Severity = Severity.Error;
       /// log.Priority = 100;
       /// Logger.Write(log);</code></example>
       /// <param name="log">Log entry object to write.</param>
       public static void Write(WSLogEntry log)
       {
          Writer.Write(log.ToLogEntry());
       }

        private static LogWriter Writer
        {
            get
            {
                if (writer == null)
                {
                    lock (sync)
                    {
                        if (writer == null)
                        {
                            ConfigurationContext context = ConfigurationManager.GetCurrentContext();
                            writer = new LogWriter(context);
                        }
                    }
                }
                return writer;
            }
        }
    }
}