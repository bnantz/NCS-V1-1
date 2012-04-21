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

#if UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Distributor.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests
{
    public class TestLogSinkConfigurationView : LoggingConfigurationView
    {
        private SinkData data;

        public TestLogSinkConfigurationView(SinkData data) : base(new ConfigurationContext(new ConfigurationDictionary()))
        {
            this.data = data;
        }

        public override SinkData GetSinkData(string sinkName)
        {
            return data;
        }

    }
}

#endif