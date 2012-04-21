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

#if  UNIT_TESTS
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Tests
{
    internal class MockDistributionStrategy : ConfigurationProvider, ILogDistributionStrategy
    {
        public static int HitCount = 0;
        public static string MessageXml;
        public static LogEntry Entry;

        public MockDistributionStrategy()
        {
        }

        public override void Initialize(ConfigurationView configurationView)
        {
        }

        public void SendLog(LogEntry logEntry)
        {
            SoapFormatter formatter = new SoapFormatter();

            string serializedEntry = String.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                formatter.Serialize(ms, logEntry);
                ms.Position = 0;
                byte[] buffer = new byte[ms.Length];
                ms.Read(buffer, 0, (int)ms.Length);
                serializedEntry = Encoding.UTF8.GetString(buffer);
            }

            MessageXml = serializedEntry;
            Entry = logEntry;

            HitCount++;
        }
    }
}

#endif