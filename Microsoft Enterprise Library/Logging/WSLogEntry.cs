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

using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Management.Instrumentation;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// Represents a log message.  Contains the common properties that are required for all log messages.
	/// </summary>
	[XmlRoot("logEntry")]
	[Serializable]
	[InstrumentationClass(InstrumentationType.Event)]
    public class WSLogEntry : ICloneable
	{
		private string message = string.Empty;
		private string title = string.Empty;
		private string category = string.Empty;
		private int priority = -1;
		private int eventId = 0;
		private Severity severity = Severity.Unspecified;

		private string machineName = string.Empty;
		private DateTime timeStamp = DateTime.MaxValue;

		private StringBuilder errorMessages;
		private object [][] extendedProperties;

		private string appDomainName;
		private string processId;
		private string processName;
		private string threadName;
		private string win32ThreadId;

		/// <summary>
		/// Initialize a new instance of a <see cref="WSLogEntry"/> class.
		/// </summary>
		public WSLogEntry()
		{
			CollectIntrinsticProperties();
		}

		/// <summary>
		/// Create a new instance of <see cref="WSLogEntry"/> with a full set of constructor parameters
		/// </summary>
		/// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
		/// <param name="category">Category name used to route the log entry to a one or more sinks.</param>
		/// <param name="priority">Only messages must be above the minimum priority are processed.</param>
		/// <param name="eventId">Event number or identifier.</param>
		/// <param name="severity">Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).</param>
		/// <param name="title">Additional description of the log entry message.</param>
		/// <param name="properties">Dictionary of key/value pairs to record.</param>
		public WSLogEntry(object message, string category, int priority, int eventId,
		                Severity severity, string title, IDictionary properties)
		{
			this.Message = message.ToString();
			this.Priority = priority;
			this.Category = category;
			this.EventId = eventId;
			this.Severity = severity;
			this.Title = title;
			this.ExtendedProperties = ToJaggedArray((Hashtable)properties);

			CollectIntrinsticProperties();
		}

      /// <summary>
      /// Create a new instance of <see cref="WSLogEntry"/> with a full set of constructor parameters
      /// </summary>
      /// <param name="entry" cref="LogEntry"></param>
      public WSLogEntry(LogEntry entry)
      {
         this.Message = entry.Message;
         this.Priority = entry.Priority;
         this.Category = entry.Category;
         this.EventId = entry.EventId;
         this.Severity = entry.Severity;
         this.Title = entry.Title;
         this.ExtendedProperties = ToJaggedArray((Hashtable)entry.ExtendedProperties);

         CollectIntrinsticProperties();
      }

      public LogEntry ToLogEntry()
      {
         LogEntry entry = new LogEntry();
         entry.Message = this.Message;
         entry.Priority = this.Priority;
         entry.Category = this.Category;
         entry.EventId = this.EventId;
         entry.Severity = this.Severity;
         entry.Title= this.Title;
         entry.ExtendedProperties = ToHashtable(this.ExtendedProperties);
      
         return entry;
      }

      public WSLogEntry FromLogEntry(LogEntry entry)
      {
           return new WSLogEntry(entry);
      }

		/// <summary>
		/// Message body to log.  Value from ToString() method from message object.
		/// </summary>
		public string Message
		{
			get { return this.message; }
			set { this.message = value; }
		}

		/// <summary>
		/// Category name used to route the log entry to a one or more sinks.
		/// </summary>
		public string Category
		{
			get
			{
				if (this.category == null)
				{
					return "";
				}
				else
				{
					return this.category;
				}
			}
			set { this.category = value; }
		}

		/// <summary>
		/// Importance of the log message.  Only messages must be above the minimum priority are processed.
		/// </summary>
		public int Priority
		{
			get { return this.priority; }
			set { this.priority = value; }
		}

		/// <summary>
		/// Event number or identifier.
		/// </summary>
		public int EventId
		{
			get { return this.eventId; }
			set { this.eventId = value; }
		}

		/// <summary>
		/// Log entry severity as a <see cref="Severity"/> enumeration. (Unspecified, Information, Warning or Error).
		/// </summary>
		[IgnoreMember]
        public Severity Severity
		{
			get { return this.severity; }
			set { this.severity = value; }
		}

        /// <summary>
        /// <para>Gets or sets the <see cref="System.Int32"/> value of the <see cref="Severity"/> enumeration.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="System.Int32"/> value of the <see cref="Severity"/> enumeration.</para>
        /// </value>
        /// <summary>
        /// <para>Gets or sets the <see cref="System.Int32"/> value of the <see cref="Severity"/> enumeration.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="System.Int32"/> value of the <see cref="Severity"/> enumeration.</para>
        /// </value>
        public int LoggedSeverity
        {
            get { return (int)this.severity; }
            set
            {
                if (Enum.IsDefined(typeof(Severity), value))
                {
                    throw new ArgumentException(SR.ExceptionSeverityNotDefined);
                }
                severity = (Severity)Enum.Parse(typeof(Severity), value.ToString(CultureInfo.InvariantCulture));
            }
        }

		/// <summary>
		/// Additional description of the log entry message.
		/// </summary>
		public string Title
		{
			get { return this.title; }
			set { this.title = value; }
		}

		/// <summary>
		/// Date and time of the log entry message.
		/// </summary>
		public DateTime TimeStamp
		{
			get { return this.timeStamp; }
			set { this.timeStamp = value; }
		}

		/// <summary>
		/// Name of the computer.
		/// </summary>
		public string MachineName
		{
			get { return this.machineName; }
			set { this.machineName = value; }
		}

		/// <summary>
		/// The AppDomain in which we are running
		/// </summary>
		public string AppDomainName
		{
			get { return this.appDomainName; }
			set { this.appDomainName = value; }
		}

		/// <summary>
		/// The Win32 process ID for the current running process.
		/// </summary>
		public string ProcessId
		{
			get { return processId; }
			set { processId = value; }
		}

		/// <summary>
		/// The name of the current running process.
		/// </summary>
		public string ProcessName
		{
			get { return processName; }
			set { processName = value; }
		}

		/// <summary>
		/// The name of the .NET thread. <seealso cref="Win32ThreadId"/>
		/// </summary>
		public string ManagedThreadName
		{
			get { return threadName; }
			set { threadName = value; }
		}

		/// <summary>
		/// The Win32 Thread ID for the current thread.
		/// </summary>
		public string Win32ThreadId
		{
			get { return win32ThreadId; }
			set { win32ThreadId = value; }
		}

		/// <summary>
		/// Dictionary of key/value pairs to record.
		/// </summary>
		[IgnoreMember]
        public object [][] ExtendedProperties
		{
            get
            {
                if (extendedProperties == null)
                {
                    extendedProperties = new object[1][];
                }
                return this.extendedProperties;
            }
			set { this.extendedProperties = value; }
		}

		/// <summary>
		/// Read-only property that returns the timeStamp formatted using the current culture.
		/// </summary>
		public string TimeStampString
		{
			get { return TimeStamp.ToString(CultureInfo.CurrentCulture); }
		}

		/// <summary>
		/// Creates a new <see cref="LogEntry"/> that is a copy of the current instance.
		/// </summary>
		/// <remarks>
		/// If the dictionary contained in <see cref="ExtendedProperties"/> implements <see cref="ICloneable"/>, the resulting
		/// <see cref="LogEntry"/> will have its ExtendedProperties set by calling <c>Clone()</c>. Otherwise the resulting
		/// <see cref="LogEntry"/> will have its ExtendedProperties set to null.
		/// </remarks>
		/// <implements>ICloneable.Clone</implements>
		/// <returns>A new <c>LogEntry</c> that is a copy of the current instance.</returns>
		public object Clone()
		{
			WSLogEntry result = new WSLogEntry();

			result.Message = this.Message;
			result.Category = this.Category;
			result.EventId = this.EventId;
			result.Title = this.Title;
			result.Severity = this.Severity;
			result.Priority = this.Priority;

			result.TimeStamp = this.TimeStamp;
			result.MachineName = this.MachineName;
			result.AppDomainName = this.AppDomainName;
			result.ProcessId = this.ProcessId;
			result.ProcessName = this.ProcessName;
			result.ManagedThreadName = this.ManagedThreadName;

		   result.ExtendedProperties = ToJaggedArray((Hashtable)(IDictionary) ((ICloneable) extendedProperties).Clone());

			return result;
		}

      public static object[][] ToJaggedArray(Hashtable ht)
      {
         object [][] oo = new object[ht.Count][];
         int i=0;
         foreach(Object key in ht.Keys)
         {
            oo[i] = new object[]{key, ht[key]};
            i++;
         }

         return oo;
      }

      public static Hashtable ToHashtable(object [][] oo)
      {
         Hashtable ht = new Hashtable(oo.Length);
         object key;
         object value;

         foreach(object [] pair in oo)
         {
            key = pair[0];
            value = pair[1];
            ht[key]=value;
         }

         return ht;
      }

		/// <summary>
		/// Add an error or warning message to the start of the messages string builder.
		/// Used by the distributor to record problems.
		/// </summary>
		/// <param name="message">Message to be added to this instance</param>
		public virtual void AddErrorMessage(string message)
		{
			if (errorMessages == null)
			{
				errorMessages = new StringBuilder();
			}
			errorMessages.Insert(0, Environment.NewLine);
			errorMessages.Insert(0, Environment.NewLine);
			errorMessages.Insert(0, message);
		}

        /// <summary>
        /// Gets the error message with the <see cref="LogEntry"></see>
        /// </summary>
		public string ErrorMessages
		{
			get
			{
				if( errorMessages == null )
					return null;
				else
					return errorMessages.ToString();
			}
		}

		/// <summary>
		/// Set the intrinsic properties such as MachineName and UserIdentity.
		/// </summary>
		private void CollectIntrinsticProperties()
		{
			this.TimeStamp = DateTime.Now;

			try
			{
				MachineName = Environment.MachineName;
			}
			catch (Exception e)
			{
				this.MachineName = SR.IntrinsicPropertyError(e.Message);
			}
            catch
            {
                this.machineName = SR.IntrinsicPropertyError(SR.UnknownError);
            }

			try
			{
				appDomainName = AppDomain.CurrentDomain.FriendlyName;
			}
			catch (Exception e)
			{
				appDomainName = SR.IntrinsicPropertyError(e.Message);
			}
            catch
            {
                appDomainName = SR.IntrinsicPropertyError(SR.UnknownError);
            }

			try
			{
				processId = NativeMethods.GetCurrentProcessId().ToString(NumberFormatInfo.InvariantInfo);
			}
			catch (Exception e)
			{
				processId = SR.IntrinsicPropertyError(e.Message);
			}
            catch
            {
                processId = SR.IntrinsicPropertyError(SR.UnknownError);
            }

			try
			{
				processName = GetProcessName();
			}
			catch (Exception e)
			{
				processName = SR.IntrinsicPropertyError(e.Message);
			}
            catch
            {
                processName = SR.IntrinsicPropertyError(SR.UnknownError);
            }

			try
			{
				threadName = Thread.CurrentThread.Name;
			}
			catch (Exception e)
			{
				threadName = SR.IntrinsicPropertyError(e.Message);
			}
            catch
            {
                threadName = SR.IntrinsicPropertyError(SR.UnknownError);
            }

			try
			{
				win32ThreadId = NativeMethods.GetCurrentThreadId().ToString(NumberFormatInfo.InvariantInfo);
			}
			catch (Exception e)
			{
				win32ThreadId = SR.IntrinsicPropertyError(e.Message);
			}
            catch
            {
                win32ThreadId = SR.IntrinsicPropertyError(SR.UnknownError);
            }
		}

		internal static string GetProcessName()
		{
			StringBuilder buffer = new StringBuilder(1024);
			int length = NativeMethods.GetModuleFileName(NativeMethods.GetModuleHandle(null), buffer, buffer.Capacity);
			Debug.Assert(length > 0);
			return buffer.ToString();
		}
	}
}
