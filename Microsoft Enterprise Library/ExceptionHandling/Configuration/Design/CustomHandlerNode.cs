//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// Configuration node for custom handlers.
    /// </summary>
    public class CustomHandlerNode : ExceptionHandlerNode, ITypeDependentExceptionHandler
    {
        private CustomHandlerData customHandlerData;

        /// <summary>
        /// Initializes node with default data.
        /// </summary>
        public CustomHandlerNode() : this(new CustomHandlerData(SR.DefaultCustomHandlerNodeName))
        {
        }

        /// <summary>
        /// Initializes node with specified data.
        /// </summary>
        /// <param name="customHandlerData">Sepecified configuration data.</param>
        public CustomHandlerNode(CustomHandlerData customHandlerData) : base(customHandlerData)
        {
            this.customHandlerData = customHandlerData;
        }

        /// <summary>
        /// See <see cref="CustomHandlerData.Attributes"/>.
        /// </summary>
        [SRDescription(SR.Keys.ExceptionHandlerAdditionalPropertiesDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public NameValueItemCollection Attributes
        {
            get { return customHandlerData.Attributes; }
        }

        public void SetSelectedType(Type type)
        {
            customHandlerData.TypeName = type.AssemblyQualifiedName;
        }
    }
}