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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tracing
{
	/// <summary>
	/// Represents a performance tracing class to log method entry/exit and duration.
	/// </summary>
	/// <remarks>
	/// <para>Lifetime of the Tracer object will determine the beginning and the end of
	/// the trace.  The trace message will include, method being traced, start time, end time 
	/// and duration.</para>
	/// <para>Since Tracer uses the logging block to log the trace message, you can include application
	/// data as part of your trace message. Configured items from call context will be logged as
	/// part of the message.</para>
	/// <para>Trace message will be logged to the "Trace" category. You must configure the "Trace"
	/// category with desire log sinks to log the trace messages.</para>
	/// </remarks>
	public class Tracer : IDisposable
	{
		internal const int priority = 5;
		internal const int eventId = 1;
		internal const string startTitle = "TracerEnter";
		internal const string endTitle = "TracerExit";
		internal const Severity severity = Severity.Information;
		internal const string defaultCategory = "Trace";
		internal const string ActivityIdPropertyKey = "TracerActivityId";

		private bool tracerDisposed = false;
		private long tracingStartTicks;
		private ConfigurationContext context;

		[ThreadStatic] private static PeekableStringStack activityIdStack;
		[ThreadStatic] private static PeekableStringStack categoryStack;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tracer"/> class.
		/// </summary>
		public Tracer()
			: this(defaultCategory)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Tracer"/> class with the given category.
		/// </summary>
		/// <param name="category">The category for the <see cref="Tracer"/></param>
		public Tracer(string category)
			: this(category, Tracer.CurrentActivityId)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Tracer"/> class with the given category and activityId.
		/// </summary>
		/// <param name="category">The category for the <see cref="Tracer"/></param>
		/// <param name="activityId">The activity ID for the <see cref="Tracer"/></param>
		public Tracer(string category, string activityId)
			: this(ConfigurationManager.GetCurrentContext(), category, activityId)
		{
		}

		internal Tracer(ConfigurationContext context, string category, string activityId)
		{
			this.context = context;
			Initialize(category, activityId);
		}

		/// <summary>
		/// <para>Releases unmanaged resources and performs other cleanup operations before the <see cref="ProviderFactory"/> is 
		/// reclaimed by garbage collection</para>
		/// </summary>
		~Tracer()
		{
			Dispose(false);
		}

		/// <summary>
		/// Causes the <see cref="Tracer"/> to output its closing message.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Gets the current activity Id.
		/// </summary>
		/// <remarks><para>This property is meant to contain a user-specified correlation ID to be used
		/// for associating log and trace messages with a particular usage scenario.</para></remarks>
		public static string CurrentActivityId
		{
			get { return Tracer.activityIdStack != null ? Tracer.activityIdStack.Peek() : null; }
		}

		/// <summary>
		/// Gets the topmost ActivityId in the <see cref="Tracer"/>'s internal stack
		/// </summary>
		public static string RootActivityId
		{
			get { return Tracer.activityIdStack != null ? Tracer.activityIdStack.PeekAt(0) : null; }
		}

		/// <summary>
		/// Gets the current category used by the <see cref="Tracer"/>.
		/// </summary>
		public static string CurrentCategory
		{
			get { return Tracer.categoryStack != null ? Tracer.categoryStack.Peek() : null; }
		}

		/// <summary>
		/// Gets the topmost category in the <see cref="Tracer"/>'s internal stack.
		/// </summary>
		public static string RootCategory
		{
			get { return Tracer.categoryStack != null ? Tracer.categoryStack.PeekAt(0) : null; }
		}

		/// <summary>
		/// <para>Releases the unmanaged resources used by the <see cref="ProviderFactory"/> and optionally releases 
		/// the managed resources.</para>
		/// </summary>
		/// <param name="disposing">
		/// <para><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> 
		/// to release only unmanaged resources.</para>
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !tracerDisposed)
			{
				WriteTraceEndMessage(endTitle);
				Tracer.categoryStack.Pop();
				Tracer.activityIdStack.Pop();
				this.tracerDisposed = true;
			}
		}

		private void Initialize(string category, string activityId)
		{
			InitializeCategoryStack(category);
			InitializeActivityIdStack(activityId);

			NativeMethods.QueryPerformanceCounter(out tracingStartTicks);
			WriteTraceStartMessage(startTitle);
		}

		private static void InitializeCategoryStack(string category)
		{
			if (Tracer.categoryStack == null)
			{
				Tracer.categoryStack = new PeekableStringStack();
			}

			Tracer.categoryStack.Push(category);
		}

		private void InitializeActivityIdStack(string activityId)
		{
			if (Tracer.activityIdStack == null)
			{
				Tracer.activityIdStack = new PeekableStringStack();
			}

			if (activityId == null)
			{
				if (Tracer.activityIdStack.Peek() == null)
				{
					Tracer.activityIdStack.Push(Guid.NewGuid().ToString());
				}
				else
				{
					Tracer.activityIdStack.Push(Tracer.activityIdStack.Peek());
				}
			}
			else
			{
				Tracer.activityIdStack.Push(activityId);
			}
		}

		private void WriteTraceStartMessage(string entryTitle)
		{
			string methodName = GetExecutingMethodName();
			string message = SR.Tracer_StartMessageFormat(Tracer.activityIdStack.Peek(), methodName, tracingStartTicks);
			WriteTraceMessage(message, entryTitle);
		}

		private void WriteTraceEndMessage(string entryTitle)
		{
			long tracingEndTicks = long.MinValue;
			NativeMethods.QueryPerformanceCounter(out tracingEndTicks);
			decimal secondsElapsed = GetSecondsElapsed(tracingStartTicks, tracingEndTicks);

			string methodName = GetExecutingMethodName();
			string message = SR.Tracer_EndMessageFormat(Tracer.activityIdStack.Peek(), methodName, tracingEndTicks, secondsElapsed);
			WriteTraceMessage(message, entryTitle);
		}

		private void WriteTraceMessage(string message, string entryTitle)
		{
			if (IsTracingEnabled())
			{
				Hashtable extendedProperties = new Hashtable();
				LogEntry entry = new LogEntry(message, Tracer.categoryStack.Peek(), priority, eventId, severity, entryTitle, extendedProperties);
				LogWriter writer = new LogWriter(context);
				writer.Write(entry);
			}
		}

		private bool IsTracingEnabled()
		{
			LoggingSettings settings = (LoggingSettings) context.GetConfiguration(LoggingSettings.SectionName);
			return settings.TracingEnabled;
		}

		private string GetExecutingMethodName()
		{
			string result = "Unknown";
			StackTrace trace = new StackTrace(false);

			for (int index = 0; index < trace.FrameCount; ++index)
			{
				StackFrame frame = trace.GetFrame(index);
				MethodBase method = frame.GetMethod();
				if (method.DeclaringType != GetType())
				{
					result = method.Name;
					break;
				}
			}

			return result;
		}

		private decimal GetSecondsElapsed(long start, long stop)
		{
			long queryFrequency;
			NativeMethods.QueryPerformanceFrequency(out queryFrequency);

			decimal result = Convert.ToDecimal(stop - start)/Convert.ToDecimal(queryFrequency);
			return Math.Round(result, 6);
		}
	}
}