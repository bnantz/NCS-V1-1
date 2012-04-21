//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Enterprise Library Shared Library
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if    UNIT_TESTS
using System.Collections;
using System.Threading;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests
{
    public abstract class ThreadStressFixtureBase
    {
        public void ThreadStress(ThreadStart testMethod, int threadCount)
        {
            ArrayList threads = new ArrayList();
            for (int i = 0; i < threadCount; i++)
            {
                threads.Add(new Thread(testMethod));
            }

            foreach (Thread thread in threads)
            {
                thread.Start();
            }
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
    }
}

#endif