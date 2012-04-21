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
    /// <para>Represents a handler for exceptions.</para>
    /// </summary>
	public abstract class ExceptionHandler : ConfigurationProvider, IExceptionHandler
	{
	    private string currentPolicyName;
	    private string currentExceptionTypeName;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ExceptionHandler"/> class.</para>
        /// </summary>
	    protected ExceptionHandler() : base()
		{
            currentPolicyName = string.Empty;
            currentExceptionTypeName = string.Empty;
		}

        /// <summary>
        /// <para>Gets or sets the current name of the <see cref="ExceptionPolicy"/> using this handler.</para>
        /// </summary>
        /// <value>
        /// <para>The current name of the <see cref="ExceptionPolicy"/> using this handler.</para>
        /// </value>
	    public string CurrentPolicyName
	    {
	        get { return currentPolicyName; }
	        set { currentPolicyName = value;  }
	    }

        /// <summary>
        /// <para>Gets or sets the current <see cref="Exception"/> <see cref="Type.AssemblyQualifiedName"/> that will be handled.</para>
        /// </summary>
        /// <value>
        /// <para>The current <see cref="Exception"/> <see cref="Type.AssemblyQualifiedName"/> that will be handled.</para>
        /// </value>
	    public string CurrentExceptionTypeName
	    {
	        get { return currentExceptionTypeName; }
	        set { currentExceptionTypeName = value; }
	    }

        /// <summary>
        /// <para>When overriden by a class, handles an <see cref="Exception"/>.</para>
        /// </summary>
        /// <param name="exception"><para>The exception to handle.</para></param>
        /// <param name="policyName"><para>The name of the <see cref="ExceptionPolicy"/>.</para></param>
        /// <param name="handlingInstanceId">
        /// <para>The unique ID attached to the handling chain for this handling instance.</para>
        /// </param>
        /// <returns><para>Modified exception to pass to the next handler in the chain.</para></returns>
	    public abstract Exception HandleException(Exception exception, string policyName, Guid handlingInstanceId);

	}
}
