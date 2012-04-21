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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tracing
{
	/// <summary>
	/// Provides a common interface for a logger which can be consumed
	/// by the Tracer class.
	/// </summary>
	internal interface ITraceLogger
	{
		/// <summary>
		/// Writes the given log entry to the log.
		/// </summary>
		/// <param name="entry">The log entry to write.</param>
		void Write( LogEntry entry );
	}
}
