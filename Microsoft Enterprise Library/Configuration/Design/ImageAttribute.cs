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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Defines the image to be associated with the <see cref="ConfigurationNode"/>.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ImageAttribute : NodeImageAttribute
    {
        /// <summary>
        /// <para>Initializes a new instance of the <see cref="NodeImageAttribute"/> class based on a 16 x 16 bitmap that is embedded as a resource in a specified assembly.</para>
        /// </summary>
        /// <param name="t">
        /// <para>A <para>Type</para> whose defining assembly is searched for the bitmap resource.</para>
        /// </param>
        public ImageAttribute(Type t) : base(t)
        {
        }

        /// <summary>
        /// <para>
        /// <para>Initializes a new instance of the <see cref="NodeImageAttribute"/> class using the specified <see cref="Type"/> and resource entry name. The type is used to locate the assembly from which to load the <see cref="System.Resources.ResourceManager"/> that contains the image.</para></para>
        /// </summary>
        /// <param name="t"><para>A <see cref="Type"/> defined in the assembly that contains the image as an embedded resource.</para></param> 
        /// <param name="name"><para>The name of the embedded resource.</para></param>        
        /// <seealso cref="System.Drawing.ToolboxBitmapAttribute"/>        
        public ImageAttribute(Type t, string name) : base(t, name)
        {
        }
    }
}