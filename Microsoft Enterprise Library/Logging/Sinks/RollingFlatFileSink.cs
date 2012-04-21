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
using System.IO;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks
{
	/// <summary>
	/// Represents a flat file logging sink.  Writes the message in a text file format 
	/// on the local machine.  Path and text file name are retrieved from the configuration settings.
	/// </summary>
	public class RollingFlatFileSink : LogSink
	{
		private static object fileWriteSyncObject = new object();
		private LoggingConfigurationView loggingConfigurationView;
		private DefaultLogDestination defaultSink;

		/// <summary>
		/// Create an instance of a RollingFlatFileSink.
		/// </summary>        
		public RollingFlatFileSink()
		{
			this.defaultSink = new DefaultLogDestination();
		}

		/// <summary>
		/// Initializes the sink with its name and any additional attributes. 
		/// </summary>  
		/// <param name="configurationView">Dynamic view into the configuration information needed by the <see cref="FlatFileSink"></see></param>     
		public override void Initialize(ConfigurationView configurationView)
		{
			ArgumentValidation.CheckForNullReference(configurationView, "configurationView");
			ArgumentValidation.CheckExpectedType(configurationView, typeof(LoggingConfigurationView));

			this.loggingConfigurationView = (LoggingConfigurationView)configurationView;
		}

		/// <summary>
		/// Append the log entry to the configured text file.
		/// </summary>
		/// <param name="logEntry"><see cref="LogEntry"></see> to be appended to logging file</param>
		protected override void SendMessageCore(LogEntry logEntry)
		{
			try
			{
				WriteMessageToFile(logEntry);
			}
			catch (Exception e)
			{
				logEntry.AddErrorMessage(SR.SinkFailure(e.ToString()));
				throw;
			}
			catch
			{
				logEntry.AddErrorMessage(SR.SinkFailure(SR.UnknownError));
			}
		}

		private RollingFlatFileSinkData GetRollingFlatFileSinkDataFromCursor()
		{
			RollingFlatFileSinkData sinkData = loggingConfigurationView.GetSinkData(ConfigurationName) as RollingFlatFileSinkData;
			ArgumentValidation.CheckExpectedType(sinkData, typeof (RollingFlatFileSinkData));

			return (RollingFlatFileSinkData) sinkData;
		}

		/// <summary>
		/// Performs file rollover by renaming the current log file and deleting old files as necessary.
		/// The rename causes the next log request to generate a new file and log to it.
		/// </summary>
		/// <param name="logFile">Base log file name.</param>
		/// <param name="createdDate">The date the file to be renamed was initially created.</param>
		/// <param name="rollOverFilesToKeep">Number of files to keep. Causes deletion of oldest files.</param>
		/// <returns>True if successful.</returns>
		private bool RolloverFile(string logFile, DateTime createdDate, int rollOverFilesToKeep)
		{
			string stringDate = createdDate.GetDateTimeFormats()[103]; //Get the date in a format to use in the file name.
			stringDate = stringDate.Replace(":", "."); 
			string stringTicks = DateTime.Now.Ticks.ToString();
			string directoryName = Path.GetDirectoryName(logFile);
			string rolledLogFile = "";
			bool success = false;
			try
			{	//Rename the file with date and time plus ticks to ensure uniqueness
				rolledLogFile = Path.Combine(directoryName, Path.GetFileNameWithoutExtension(logFile) + "_" + stringDate + "." + stringTicks + Path.GetExtension(logFile));
				File.Move(logFile, rolledLogFile); //Rename the file with a date.
				string[] archivedFiles = Directory.GetFiles(directoryName, Path.GetFileNameWithoutExtension(logFile) + "_*" + Path.GetExtension(logFile));
				if (archivedFiles.Length > (rollOverFilesToKeep))
				{
					Array.Sort(archivedFiles);
					for (int i=0; i < (archivedFiles.Length - rollOverFilesToKeep); i++)
					{
						File.Delete(Path.Combine(directoryName, archivedFiles[i]));
					}
				}
				success = true;
			}
			catch (Exception e)
			{	//Default to continue logging rather than raising an error.  
				//If the file was already moved or other error, we're assuming another process succeeded.
				Logger.Write("Failed rolling over or deleting files: " + logFile + Environment.NewLine + e.Message);
			}
			return success;
		}

		/// <summary>
		/// Performs file rollover by deleting the existing file, recreating it and datetime stamping it.
		/// When this rollover is set in the configuration no other rollover criteria is evaluated.
		/// </summary>
		/// <param name="logFile">Log file name.</param>
		/// <param name="dayOfWeekDate">Date and time to stamp the new log file with.</param>
		private void RolloverFileByDayOfWeek(string logFile, DateTime dayOfWeekDate)
		{
			File.Delete(logFile); //Delete the old and following this recreate and timestamp it.
			using (FileStream fileStream = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
			{
				using (StreamWriter writer = new StreamWriter(fileStream))
				{
					File.SetCreationTime(logFile, dayOfWeekDate);
				}
			}
		}

		/// <summary>
		/// Writes log message after evaluating rollover criteria and rolling over files if needed.
		/// </summary>
		/// <param name="logEntry">Base log file name.</param>
		private void WriteMessageToFile(LogEntry logEntry)
		{
			bool newLog = false;
			string logFile = "";
			string directory = "";
			string header = "";
			string footer = "";
			int rollOverByDays = 0;
			int rollOverByMinutes = 0;
			double rollOverByMegabytes = 0;
			int rollOverFilesToKeep = 0;
			string rollOverOneFilePerWeekDay = null;
			DateTime dayOfWeekDate = DateTime.Now;

			RollingFlatFileSinkData rollingFlatFileSinkData = GetRollingFlatFileSinkDataFromCursor(); //Retrieve sink parameters
			try
			{ //Get configuration data. If any fail then throw exception and block will log to default.
				logFile = rollingFlatFileSinkData.FileName;
				directory = Path.GetDirectoryName(logFile); 
				header = rollingFlatFileSinkData.Header;
				footer = rollingFlatFileSinkData.Footer;
				rollOverByDays = rollingFlatFileSinkData.RolloverByDays; 
				rollOverByMinutes = rollingFlatFileSinkData.RolloverByMinutes;
				rollOverByMegabytes = rollingFlatFileSinkData.RolloverByMegabytes;
				rollOverFilesToKeep = rollingFlatFileSinkData.NumFilesToKeep;
				rollOverOneFilePerWeekDay = rollingFlatFileSinkData.OneFilePerWeekDay;
			}
			catch (Exception e)
			{
				throw new Exception("Configuration data was unable to be retrieved.  Please configure FileName, RolloverByDays, RolloverByMinutes, RolloverByMegabytes, etc.", e);
			}
			if (directory.Length == 0) //If directory is blank then get current directory
			{
				directory = AppDomain.CurrentDomain.BaseDirectory;
				logFile = Path.Combine(directory, logFile); //Append working directory and file name
			}
			lock (fileWriteSyncObject)
			{
				if (rollOverOneFilePerWeekDay.ToLower() == "true")
				{	//Under load, using the current date here might write a message that was constructed milliseconds before day turnover into the next day's log.
					//If this is undesired the dayOfWeekDate = DateTime.Now could be replaced with dayOfWeekDate = logEntry.Timestamp but a caller might not set the logEntry.Timestamp causing log rollover problems.
					dayOfWeekDate = DateTime.Now; 
					logFile = Path.Combine(Path.GetDirectoryName(logFile), Path.GetFileNameWithoutExtension(logFile) + (int)dayOfWeekDate.DayOfWeek + Path.GetExtension(logFile));
				}
				if (File.Exists(logFile)) //Make sure file exists before using it
				{
					DateTime createdDate = File.GetCreationTime(logFile);
					if (rollOverOneFilePerWeekDay.ToLower() == "true") //If special rollover by day of week
					{
						if (createdDate.Day != dayOfWeekDate.Day)
						{
							RolloverFileByDayOfWeek(logFile, dayOfWeekDate); //Delete and recreate the file
						}
					}
					else
					{
						//Check for rollover on days			
						if (rollOverByDays > 0 && (createdDate.AddDays(rollOverByDays).Date < DateTime.Now.Date))
						{
							if (RolloverFile(logFile, createdDate, rollOverFilesToKeep))
							{
								newLog = true;
							}
						}
						//Check for rollover on minutes if not already rolled
						if (!newLog && rollOverByMinutes > 0 && (createdDate.AddMinutes(rollOverByMinutes) < DateTime.Now))
						{	
							if (RolloverFile(logFile, createdDate, rollOverFilesToKeep))
							{
								newLog = true;
							}
						}
						if (!newLog) //Check for rollover on size in MB (1MB = 1048576) if not already rolled
						{
							if (rollOverByMegabytes > 0 && (new FileInfo(logFile).Length / 1048576.0000000000) > rollOverByMegabytes)
							{
								if (RolloverFile(logFile, createdDate, rollOverFilesToKeep))
								{
									newLog = true;
								}
							}
						}
					}
				}
				else //File did not exist
				{
					if (!Directory.Exists(directory)) //Create directory if doesn't exist
					{
						Directory.CreateDirectory(directory);
					}
					newLog = true; //File did not exist so set flag that triggers DateTime stamp
				}

				using (FileStream fileStream = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
				{	
					using (StreamWriter writer = new StreamWriter(fileStream)) //Write message to the log
					{
						if (header.Length > 0)
						{
							writer.WriteLine(header);
						}
						writer.WriteLine(FormatEntry(logEntry));
						if (footer.Length > 0)
						{
							writer.WriteLine(footer);
						}
						writer.Flush();
						if (newLog) //If we created a new file make sure it has a new time stamp
						{
							File.SetCreationTime(logFile, DateTime.Now);
						}
					}
				}
			}
		}
	}
}