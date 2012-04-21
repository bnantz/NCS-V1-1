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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// <para>A value describing the boundary conditions for a range.</para>
    /// </summary>
    public enum RangeBoundaryType
    {
        /// <summary>
        /// <para>The range should include the boundary.</para>
        /// </summary>
        Inclusive = 0,
        /// <summary>
        /// <para>The range should exclude the boundary.</para>
        /// </summary>
        Exclusive
    }
}