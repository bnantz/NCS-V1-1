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
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Creates configuraiton stored in a Xml file.</para>
    /// </summary>
	public class XmlFileStorageCreationCommand : FileStorageCreationCommand
	{

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="FileStorageCreationCommand"/> class with a file name and <see cref="IServiceProvider"/>.</para>
        /// </summary>
        /// <param name="fileName"><para>The name of the file to create.</para></param>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
	    public XmlFileStorageCreationCommand(string fileName, IServiceProvider serviceProvider) : base(fileName, serviceProvider)
		{
		}

        /// <summary>
        /// <para>Creates the file if it does not exist, and determines if the file can be overwrriten if it does exist.</para>
        /// </summary>
	    public override void Execute()
	    {
            if (!File.Exists(FileName))
            {
                FileHelper.CreateZeroByteFile(FileName);
            }
            CanOverwriteFile();
	    }
	}
}
