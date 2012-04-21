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

using System;
using System.Runtime.InteropServices;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Cryptography
{
    /// <devdoc>
    /// Native methods for interop with crypt32.dll and kernel32.dll.
    /// </devdoc>
    internal sealed class NativeMethods
    {
        private NativeMethods()
        {
        }

        [DllImport("Crypt32.dll", SetLastError=true, CharSet=CharSet.Auto)]
        public static extern bool CryptProtectData(
            ref DataBlob dataInPointer,
            String dataDescr,
            ref DataBlob optionalEntropyPointer,
            IntPtr reservedVoidPointer,
            ref CryptProtectPromptStruct promptStructPointer,
            int flags,
            ref DataBlob dataOutPointer);

        [DllImport("Crypt32.dll", SetLastError=true, CharSet=CharSet.Auto)]
        public static extern bool CryptUnprotectData(
            ref DataBlob dataInPointer,
            String dataDescr,
            ref DataBlob optionalEntropyPointer,
            IntPtr reservedVoidPointer,
            ref CryptProtectPromptStruct promptStructPointer,
            int flags,
            ref DataBlob dataOutPointer);

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        public struct DataBlob
        {
            public int Size;
            public IntPtr DataPointer;
        }

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        public struct CryptProtectPromptStruct
        {
            public int Size;
            public int PromptFlags;
            public IntPtr AppWindowHandle;
            public String Prompt;
        }
    }
}