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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Sinks.Tests
{
    public class MyCustomLogEntry : LogEntry
    {
        private string myName;

        public MyCustomLogEntry()
        {
            myName = "MyCustomLogEntry";
        }

        public string MyName
        {
            get { return myName; }
            set { myName = value; }
        }
    }
}

#endif