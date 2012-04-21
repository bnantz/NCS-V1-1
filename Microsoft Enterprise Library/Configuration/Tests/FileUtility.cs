//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Configuration Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

#if UNIT_TESTS
using System;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Tests
{
    internal sealed class FileUtility
    {
        private FileUtility()
        {
        }

        public static void OpenAndChangeFile(string configFile)
        {
            FileAttributes attributes = File.GetAttributes(configFile);
            try
            {
                File.SetAttributes(configFile, FileAttributes.Normal);
                File.SetLastWriteTime(configFile, DateTime.Now + TimeSpan.FromHours(1));
            }
            finally
            {
                File.SetAttributes(configFile, attributes);
            }
        }
    }
}

#endif