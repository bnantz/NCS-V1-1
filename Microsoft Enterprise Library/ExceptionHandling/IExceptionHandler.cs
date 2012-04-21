//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Defines the contract for an ExceptionHandler.  An ExceptionHandler contains specific handling
    /// logic (i.e. logging the exception, replacing the excepiton, etc.) that is executed in a chain of multiple
    /// ExceptionHandlers.  A chain of one or more ExceptionHandlers is executed based on the exception type being 
    /// handled, as well as the <see cref="ExceptionPolicy"/>.  <seealso cref="ExceptionPolicy.HandleException"/>
    /// </summary>
    public interface IExceptionHandler : IConfigurationProvider
    {
        /// <summary>
        /// <para>When implemented by a class, gets or sets the current name of the <see cref="ExceptionPolicy"/> using this handler.</para>
        /// </summary>
        /// <value>
        /// <para>The current name of the <see cref="ExceptionPolicy"/> using this handler.</para>
        /// </value>
        string CurrentPolicyName { get; set; }

        /// <summary>
        /// <para>When implemented by a class, gets or sets the current <see cref="Exception"/> <see cref="Type.AssemblyQualifiedName"/> that will be handled.</para>
        /// </summary>
        /// <value>
        /// <para>The current <see cref="Exception"/> <see cref="Type.AssemblyQualifiedName"/> that will be handled.</para>
        /// </value>
        string CurrentExceptionTypeName {get; set; }

        /// <summary>
        /// <para>When implemented by a class, handles an <see cref="Exception"/>.</para>
        /// </summary>
        /// <param name="exception"><para>The exception to handle.</para></param>
        /// <param name="policyName"><para>The name of the <see cref="ExceptionPolicy"/>.</para></param>
        /// <param name="handlingInstanceId">
        /// <para>The unique ID attached to the handling chain for this handling instance.</para>
        /// </param>
        /// <returns><para>Modified exception to pass to the next handler in the chain.</para></returns>
        Exception HandleException(Exception exception, string policyName, Guid handlingInstanceId);
    }
}