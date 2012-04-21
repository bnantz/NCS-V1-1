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

using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Distributor.Sinks
{
	/// <summary>
	///  Represents a node for a "<see cref="RollingFlatFileSinkData"/>".
	/// </summary>
	public class RollingFlatFileSinkNode : SinkNode
	{
		private RollingFlatFileSinkData rollingFlatFileSinkData;

		/// <summary>
		/// Creates node with initial configuration data.
		/// </summary>
		public RollingFlatFileSinkNode() : this(new RollingFlatFileSinkData(SR.RollingFlatFileSink, DefaultValues.RollingFlatFileSinkFileName, DefaultValues.RollingFlatFileSinkHeader, DefaultValues.RollingFlatFileSinkFooter)) // TODO - SR
		{
		}

		/// <summary>
		/// Creates node with specified configuration data.
		/// </summary>
		/// <param name="rollingFlatFileSinkData">Configuration data.</param>
		public RollingFlatFileSinkNode(RollingFlatFileSinkData rollingFlatFileSinkData) : base(rollingFlatFileSinkData)
		{
			this.rollingFlatFileSinkData = rollingFlatFileSinkData;
		}

		/// <summary>
		/// Read only. Returns the type name of a <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.RollingFlatFileSink"/>.
		/// </summary>
		[Browsable(false)]
		public override string TypeName
		{
			get { return rollingFlatFileSinkData.TypeName; }
		}

      /// <summary>
      /// Fully qualified file name to save the log output.
      /// </summary>
      [Required]
      [Editor(typeof(SaveFileEditor), typeof(UITypeEditor))]
      [FilteredFileNameEditor("All files (*.*)|*.*")]
      [SRDescription(SR.Keys.RollingFlatFileSinkFlatFileName)]
      [SRCategory(SR.Keys.CategoryGeneral)]
      public string Filename
      {
         get { return rollingFlatFileSinkData.FileName; }
         set { rollingFlatFileSinkData.FileName = value; }
      }

      /// <summary>
      /// Optional header to write before each log message.
      /// </summary>
      [SRDescription(SR.Keys.RollingFlatFileSinkHeader)]
      [SRCategory(SR.Keys.CategoryGeneral)]
      public string Header
      {
         get { return rollingFlatFileSinkData.Header; }
         set { rollingFlatFileSinkData.Header = value; }
      }

      /// <summary>
      /// Optional footer to write after each log message.
      /// </summary>
      [SRDescription(SR.Keys.RollingFlatFileSinkFooter)]
      [SRCategory(SR.Keys.CategoryGeneral)]
      public string Footer
      {
         get { return rollingFlatFileSinkData.Footer; }
         set { rollingFlatFileSinkData.Footer = value; }
      }

		/// <summary>
		/// See <see cref="RollingFlatFileSinkData.OneFilePerWeekDay"/>.
		/// </summary>
		[Required]
		[SRDescription(SR.Keys.OneFilePerWeekDay)]
		[SRCategory(SR.Keys.CategorySpecial)]
		public string OneFilePerWeekDay
		{
			get { return rollingFlatFileSinkData.OneFilePerWeekDay; }
			set { rollingFlatFileSinkData.OneFilePerWeekDay = value; }
		}

		/// <summary>
		/// See <see cref="RollingFlatFileSinkData.RolloverByMinutes"/>.
		/// </summary>
		[Required]
		[SRDescription(SR.Keys.RolloverByMinutes)]
		[SRCategory(SR.Keys.CategoryRollover)]
		public int RolloverByMinutes
		{
			get { return rollingFlatFileSinkData.RolloverByMinutes; }
			set { rollingFlatFileSinkData.RolloverByMinutes = value; }
		}

		/// <summary>
		/// See <see cref="RollingFlatFileSinkData.RolloverByMinutes"/>.
		/// </summary>
		[Required]
		[SRDescription(SR.Keys.RolloverByDays)]
		[SRCategory(SR.Keys.CategoryRollover)]
		public int RolloverByDays
		{
			get { return rollingFlatFileSinkData.RolloverByDays; }
			set { rollingFlatFileSinkData.RolloverByDays = value; }
		}

		/// <summary>
		/// See <see cref="RollingFlatFileSinkData.RolloverByMegabytes"/>.
		/// </summary>
		[Required]
		[SRDescription(SR.Keys.RolloverByMegabytes)]
		[SRCategory(SR.Keys.CategoryRollover)]
		public double RolloverByMegabytes
		{
			get { return rollingFlatFileSinkData.RolloverByMegabytes; }
			set { rollingFlatFileSinkData.RolloverByMegabytes = value; }
		}

		/// <summary>
		/// See <see cref="RollingFlatFileSinkData.NumFilesToKeep"/>.
		/// </summary>
		[Required]
		[SRDescription(SR.Keys.NumFilesToKeep)]
		[SRCategory(SR.Keys.CategoryRollover)]
		public int NumFilesToKeep
		{
			get { return rollingFlatFileSinkData.NumFilesToKeep; }
			set { rollingFlatFileSinkData.NumFilesToKeep = value; }
		}
	}
}