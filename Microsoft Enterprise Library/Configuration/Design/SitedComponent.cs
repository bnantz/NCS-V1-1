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

using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// <para>Represents a component that wish to implement behavior when sited.</para>
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public abstract class SitedComponent : Component
    {
        /// <summary>
        /// <para>Gets or sets the <see cref="ISite"/> of the <see cref="Component"/>.</para>
        /// </summary>
        /// <value>
        /// <para>The <see cref="ISite"/> of the <see cref="Component"/></para>
        /// </value>
        public override ISite Site
        {
            get { return base.Site; }
            set
            {
                base.Site = value;
                if (value != null)
                {
                    OnSited();
                }
                else
                {
                    OnUnSited();
                }
            }
        }

        /// <summary>
        /// <para>When overridden by a class, allows descendants to perform processing after the component is sited.</para>
        /// </summary>
        protected virtual void OnSited()
        {
        }

        /// <summary>
        /// <para>When overridden by a class, allows descendants to perform processing after the component is unsited.</para>
        /// </summary>
        protected virtual void OnUnSited()
        {
        }
    }
}