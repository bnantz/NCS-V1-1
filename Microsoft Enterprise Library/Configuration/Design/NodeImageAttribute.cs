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
using System.Drawing;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Specifies the image to be displayed when a <see cref="ConfigurationNode"/> is viewed in the user interface.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class NodeImageAttribute : ToolboxBitmapAttribute
    {
        private readonly Type componentType;
        private readonly string name;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="NodeImageAttribute"/> class using the specified <see cref="Type"/> and resource entry name. The type is used to locate the assembly from which to load the <see cref="System.Resources.ResourceManager"/> that contains the image.</para>
        /// </summary>
        /// <param name="t"><para>A <see cref="Type"/> defined in the assembly that contains the image as an embedded resource.</para></param>
        /// <param name="name"><para>The name of the embedded resource.</para></param>
        /// <seealso cref="System.Drawing.ToolboxBitmapAttribute"/>
        public NodeImageAttribute(Type t, string name) : base(t, name)
        {
            this.componentType = t;
            this.name = name;
        }

        /// <summary>
        /// <para>
        /// Initializes a new <see cref="NodeImageAttribute"/> object based on a 16 x 16 bitmap that is embedded  as a resource in a specified assembly.
        /// </para>
        /// </summary>
        /// <param name="t">
        /// <para>A <see cref="Type"/> whose defining assembly is searched for the bitmap resource.</para>
        /// </param>
        public NodeImageAttribute(Type t) : base(t)
        {
            this.componentType = t;
        }

        /// <summary>
        /// <para>Gets the name of the embedded bitmap resource.</para>
        /// </summary>
        /// <value>
        /// <para>The name of the embedded bitmap resource.</para>
        /// </value>
        public string ResourceName
        {
            get { return name; }
        }

        /// <summary>
        /// <para>Gets a <see cref="Type"/> whose defining assembly is searched for the bitmap resource.</para>
        /// </summary>
        /// <value>
        /// <para>A <see cref="Type"/> whose defining assembly is searched for the bitmap resource.</para>
        /// </value>
        public Type ComponentType
        {
            get { return componentType; }
        }

        /// <summary>
        /// <para>Gets the large (32 x 32) <see cref="Image"/> associated with this <see cref="NodeImageAttribute"/> object.</para>
        /// </summary>
        /// <returns>The large <see cref="Image"/> associated with this <see cref="NodeImageAttribute"/> object.</returns>
        public Image GetLargeImage()
        {
            return GetImage(true);
        }

        /// <summary>
        /// <para>Gets the small (16 x 16) <see cref="Image"/> associated with this <see cref="NodeImageAttribute"/> object.</para>
        /// </summary>
        /// <returns><para>The small <see cref="Image"/> associated with this <see cref="NodeImageAttribute"/> object.</para></returns>
        public Image GetImage()
        {
            return GetImage(false);
        }

        private Image GetImage(bool large)
        {
            if (name != null)
            {
                return GetImage(componentType, name, large);
            }
            else
            {
                return GetImage(componentType, large);
            }
        }
    }
}