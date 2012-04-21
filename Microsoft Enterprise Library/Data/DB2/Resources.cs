//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Data.DB2
{
    /// <devdoc>
    /// Used for the common ui culture in SR
    /// </devdoc>
    internal sealed class Resources
    {
        private Resources()
        {
        }

        public static CultureInfo CultureInfo
        {
            get { return CultureInfo.InvariantCulture; }
        }
    }
}