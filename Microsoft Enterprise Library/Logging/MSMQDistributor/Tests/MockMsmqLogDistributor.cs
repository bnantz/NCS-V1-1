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
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.Tests
{
    internal class MockMsmqLogDistributor : MsmqLogDistributor
    {
        private bool stopRecv = false;
        private bool isCompleted = true;

        public bool ReceiveMsgCalled = false;

        public bool ExceptionOnGetIsCompleted = false;

        public override bool StopReceiving
        {
            get { return stopRecv; }
            set { stopRecv = value; }
        }

        public override bool IsCompleted
        {
            get
            {
                if (ExceptionOnGetIsCompleted)
                {
                    throw new Exception("simulated exception");
                }
                return isCompleted;
            }
        }

        public void SetIsCompleted(bool val)
        {
            isCompleted = val;
        }

        public MockMsmqLogDistributor(ConfigurationContext context) : base(context)
        {
        }

        public override void CheckForMessages()
        {
            ReceiveMsgCalled = true;
        }
    }
}

#endif