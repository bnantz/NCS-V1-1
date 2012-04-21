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
    /// Represents a node for the specific ExceptionHandler "<see cref="WrapHandler"/>".
    /// </summary>
    public class ReplaceHandlerNode : ExceptionHandlerNode, ITypeDependentExceptionHandler
    {
        private ReplaceHandlerData replaceHandlerData;

        /// <summary>
        /// Constructs the node with default values.
        /// </summary>
        public ReplaceHandlerNode() : this(new ReplaceHandlerData(SR.DefaultReplaceHandlerNodeName, string.Empty, string.Empty))
        {
        }

        /// <summary>
        /// Constructs the node with config data.
        /// </summary>
        /// <param name="replaceHandlerData">The config data to initialize this node.</param>
        public ReplaceHandlerNode(ReplaceHandlerData replaceHandlerData) : base(replaceHandlerData)
        {
            this.replaceHandlerData = replaceHandlerData;
        }

        /// <summary>
        /// The message for the Replace Exception
        /// </summary>
        [SRDescription(SR.Keys.WrapHandlerNodeMessageDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string ExceptionMessage
        {
            get { return replaceHandlerData.ExceptionMessage; }
            set { replaceHandlerData.ExceptionMessage = value; }
        }


        /// <summary>
        /// The fully qualified name of the <see cref="ReplaceHandler"/>.
        /// </summary>
        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return base.TypeName;
            }
            set
            {
                base.TypeName = value;
            }
        }

        /// <summary>
        /// The fully qualified name of the replacing exception.
        /// </summary>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(Exception), TypeSelectorIncludeFlags.BaseType)]
        [SRDescription(SR.Keys.ExceptionReplaceTypeNameDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public string ReplaceExceptionTypeName
        {
            get { return replaceHandlerData.ReplaceExceptionTypeName; }
            set { replaceHandlerData.ReplaceExceptionTypeName = value; }
        }

        public void SetSelectedType(Type type)
        {
            replaceHandlerData.ReplaceExceptionTypeName = type.AssemblyQualifiedName;
        }
    }
}