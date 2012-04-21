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

using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common
{
    /// <devdoc>
    /// Used for the culture in SR
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