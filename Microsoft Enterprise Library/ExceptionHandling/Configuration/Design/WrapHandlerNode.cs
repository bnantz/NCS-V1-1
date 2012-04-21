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
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// Represents a node for the specific ExceptionHandler "WrapHandler"
    /// </summary>
    public class WrapHandlerNode : ExceptionHandlerNode, ITypeDependentExceptionHandler
    {
        private WrapHandlerData wrapHandlerData;

        /// <summary>
        /// Constructs node with default values.
        /// </summary>
        public WrapHandlerNode() : this(new WrapHandlerData(SR.DefaultWrapHandlerNodeName, string.Empty, string.Empty))
        {
        }

        /// <summary>
        /// Constructs the node with config data.
        /// </summary>
        /// <param name="wrapHandlerData">The config data to initialize this node.</param>
        public WrapHandlerNode(WrapHandlerData wrapHandlerData) : base(wrapHandlerData)
        {
            this.wrapHandlerData = wrapHandlerData;
        }

        /// <summary>
        /// The message for the Replace Exception
        /// </summary>
        [SRDescription(SR.Keys.WrapHandlerNodeMessageDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string ExceptionMessage
        {
            get { return wrapHandlerData.ExceptionMessage; }
            set { wrapHandlerData.ExceptionMessage = value; }
        }

        /// <summary>
        /// The fully qualified name of the <see cref="WrapHandler"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get { return base.TypeName; }
            set { base.TypeName = value; }
        }

        /// <summary>
        /// The fully qualified name of the wrapping exception.
        /// </summary>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Exception), TypeSelectorIncludeFlags.BaseType)]
        [SRDescription(SR.Keys.ExceptionWrapTypeNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string WrapExceptionTypeName
        {
            get { return wrapHandlerData.WrapExceptionTypeName; }
            set { wrapHandlerData.WrapExceptionTypeName = value; }
        }

        public void SetSelectedType(Type type)
        {
            wrapHandlerData.WrapExceptionTypeName = type.AssemblyQualifiedName;
        }
    }
}