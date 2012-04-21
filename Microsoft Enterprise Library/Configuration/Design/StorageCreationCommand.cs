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

using System;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Creates the physical storage for configuraiton.</para>
    /// </summary>
	public abstract class StorageCreationCommand
	{
	    private readonly IServiceProvider serviceProvider;
	    private readonly string name;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="StorageCreationCommand"/> class with a name and <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="name"><para>The name of the storage to create.</para></param>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
	    protected StorageCreationCommand(string name, IServiceProvider serviceProvider)
		{
            ArgumentValidation.CheckForNullReference(serviceProvider, "serviceProvider");
	        ArgumentValidation.CheckForNullReference(name, "name");

	        this.name = name;
	        this.serviceProvider = serviceProvider;
		}

        /// <summary>
        /// <para>Gets the name of the storage to create.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the storage to create.</para>
        /// </value>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// <para>Gets or sets the <see cref="IServiceProvider"/> for the command.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="IServiceProvider"/> for the command.</para>
        /// </value>
	    protected IServiceProvider ServiceProvider
	    {
	        get { return serviceProvider; }
	    }

        /// <summary>
        /// <para>When overriden by a class, executes the command to create the storage.</para>
        /// </summary>
        /// <value>
        /// <para>Executes the command to create the storage.</para>
        /// </value>
        public abstract void Execute();
	}
}
