using System;
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design
{
    /// <summary>
    /// Default values for design-time controls.
    /// </summary>
    internal class DefaultValues
    {
        private DefaultValues()
        {
        }

        public const bool ClientTracingEnabled = false;
        public const bool ClientLoggingEnabled = true;
        public const string ClientDistributionStrategy = "In Process";
        public const int ClientMinimumPriority = 0;
        public const CategoryFilterMode ClientCategoryFilterMode = CategoryFilterMode.AllowAllExceptDenied;

        public static readonly string DistributorDefaultCategory = SR.DefaultCategory;
        public static readonly string DistributorDefaultFormatter = SR.DefaultFormatter;
        public const string DistributorServiceName = "Enterprise Library Logging Distributor Service";
        public const string DistributorMsmqPath = @".\Private$\myQueue";
        public const int DistributorQueueTimerInterval = 1000;

        public const string FlatFileSinkFileName = "trace.log";
        public const string FlatFileSinkHeader = "----------------------------------------";
        public const string FlatFileSinkFooter = "----------------------------------------";

       public const string RollingFlatFileSinkFileName = "trace.log";
       public const string RollingFlatFileSinkHeader = "----------------------------------------";
       public const string RollingFlatFileSinkFooter = "----------------------------------------";

       public const string ConsoleSinkHeader = "----------------------------------------";
       public const string ConsoleSinkFooter = "----------------------------------------";

        public const string EventLogSinkLogName = "Application";
        public const string EventLogSinkEventSource = "Enterprise Library Logging";

        public static readonly string TextFormatterFormat = SR.DefaultTextFormat;

       public static string WSSinkUrl = "http://localhost/WebService1/Service1.asmx";
    }
}