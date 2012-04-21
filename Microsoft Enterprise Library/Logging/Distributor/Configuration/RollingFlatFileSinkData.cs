//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging and Instrumentation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Logging.Sinks;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration
{
	/// <summary>
	/// Represents the configuration settings for an <see cref="EnterpriseLibrary.Logging.Sinks.RollingFlatFileSink"/>.
	/// </summary>
	[XmlRoot("sink", Namespace=DistributorSettings.ConfigurationNamespace)]
	public class RollingFlatFileSinkData : SinkData
	{
      private string fileName = string.Empty;
      private string header = string.Empty;
      private string footer = string.Empty;
      private string oneFilePerWeekDay = "false";
		private int rolloverByMinutes = 0;
		private int rolloverByDays = 0;
		private double rolloverByMegabytes = 50;
		private int numFilesToKeep = 20;

		/// <summary>
		/// Create a new instance of a <see cref="RollingFlatFileSinkData"/>.
		/// </summary>
		public RollingFlatFileSinkData() 
		{
		}

		public RollingFlatFileSinkData(string name) : base(name) {}

      /// <summary>
      /// Initialize a new instance of the <see cref="EventLogSinkData"/> class with a name.
      /// </summary>
      /// <param name="name">
      /// The name of the sink.
      /// </param>
      /// <param name="fileName">
      /// File name to log.  File will be created if it does not exist.
      /// </param>
      public RollingFlatFileSinkData(string name, string fileName) : this(name, string.Empty, string.Empty, string.Empty)
   {
      this.fileName = fileName;
   }

      /// <summary>
      /// Initialize a new instance of the <see cref="EventLogSinkData"/> class with a name.
      /// </summary>
      /// <param name="name">
      /// The name of the sink.
      /// </param>
      /// <param name="fileName">
      /// File name to log.  File will be created if it does not exist.
      /// </param>
      /// <param name="header">
      /// Optional header to write before each log message.
      /// </param>
      /// <param name="footer">
      /// Optional footer to write after each log message.
      /// </param>
      public RollingFlatFileSinkData(string name, string fileName, string header, string footer) : base(name)
   {
      this.fileName = fileName;
      this.header = header;
      this.footer = footer;
   }

		/// <summary>
		/// Filename
		/// </summary>
		[XmlAttribute("FileName")]
		public string FileName
		{
			get { return fileName; }
			set { fileName = value; }
		}

		/// <summary>
		/// Header
		/// </summary>
		[XmlAttribute("Header")]
		public string Header
		{
			get { return header; }
			set { header = value; }
		}

		/// <summary>
		/// Footer
		/// </summary>
		[XmlAttribute("Footer")]
		public string Footer
		{
			get { return footer; }
			set { footer = value; }
		}

		/// <summary>
		/// OneFilePerWeekDay
		/// </summary>
		[XmlAttribute("OneFilePerWeekDay")]
		public string OneFilePerWeekDay
		{
			get { return oneFilePerWeekDay; }
			set { oneFilePerWeekDay = value; }
		}

		/// <summary>
		/// RolloverByMinutes
		/// </summary>
		[XmlAttribute("RolloverByMinutes")]
		public int RolloverByMinutes
		{
			get { return rolloverByMinutes; }
			set { rolloverByMinutes = value; }
		}

		/// <summary>
		/// RolloverByDays
		/// </summary>
		[XmlAttribute("RolloverByDays")]
		public int RolloverByDays
		{
			get { return rolloverByDays; }
			set { rolloverByDays = value; }
		}

		/// <summary>
		/// RolloverByMegabytes
		/// </summary>
		[XmlAttribute("RolloverByMegabytes")]
		public double RolloverByMegabytes
		{
			get { return rolloverByMegabytes; }
			set { rolloverByMegabytes = value; }
		}

		/// <summary>
		/// NumFilesToKeep
		/// </summary>
		[XmlAttribute("NumFilesToKeep")]
		public int NumFilesToKeep
		{
			get { return numFilesToKeep; }
			set { numFilesToKeep = value; }
		}

		/// <summary>
		/// Returns the fully qualified name of the <c>RollingFlatFileSink</c>.
		/// </summary>
		[XmlIgnore]
		public override string TypeName
		{
			get { return typeof(RollingFlatFileSink).AssemblyQualifiedName; }
			set
			{
			}
		}
	}
}
