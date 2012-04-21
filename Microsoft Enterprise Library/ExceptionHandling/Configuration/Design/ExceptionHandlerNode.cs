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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
    /// <summary>
    /// The designtime node for the Exception Handler.
    /// </summary>
    [Image(typeof(ExceptionHandlerNode))]
    public abstract class ExceptionHandlerNode : ConfigurationNode
    {
        private ExceptionHandlerData exceptionHandlerData;

        /// <summary>
        /// Constructs a new instance of the <see cref="ExceptionHandlerNode"/> object with
        /// a corresponding runtime configuration data.
        /// </summary>
        /// <param name="exceptionHandlerData">The cooresponding configuration data.</param>
        protected ExceptionHandlerNode(ExceptionHandlerData exceptionHandlerData) : base( /*SR.ExceptionHandler*/)
        {
            if (exceptionHandlerData == null) throw new ArgumentNullException("exceptionHandlerData");
            this.exceptionHandlerData = exceptionHandlerData;
        }

        /// <summary>
        /// The fully qualified assembly name of the <see cref="Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler"/>.
        /// </summary>
        [Required]
        [Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
        [BaseType(typeof(IExceptionHandler))]
        [SRDescription(SR.Keys.ExceptionHandlerTypeDescription)]
        [SRCategory(SR.Keys.CategoryGeneral)]
        public virtual string TypeName
        {
            get { return exceptionHandlerData.TypeName; }
            set { exceptionHandlerData.TypeName = value; }
        }

        /// <summary>
        /// Retrieves configuration data based on the current state of the node.
        /// </summary>
        /// <value>Configuration data for this node.</value>
        [Browsable(false)]
        public virtual ExceptionHandlerData ExceptionHandlerData
        {
            get
            {
                return exceptionHandlerData;
            }
        }

        protected override void OnSited()
        {
            base.OnSited();
            Site.Name = exceptionHandlerData.Name;
        }

        protected override void OnRenamed(ConfigurationNodeChangedEventArgs e)
        {
            base.OnRenamed (e);
            exceptionHandlerData.Name = e.Node.Name;
        }

        protected override void OnAddMenuItems()
        {
            base.OnAddMenuItems ();
            CreateMoveUpMenuItem();

            CreateMoveDownMenuItem();

        }

        private void CreateMoveDownMenuItem()
        {
            if (this.NextSibling == null) return;

            ConfigurationMenuItem moveDownMenuItem = new ConfigurationMenuItem(SR.MoveDownMenuItemText, 
                                                                               new MoveNodeAfterCommand(Site, this, this.NextSibling), 
                                                                               this.Parent, 
                                                                               Shortcut.None, 
                                                                               SR.MoveDownStatusText, 
                                                                               InsertionPoint.Action);
            AddMenuItem(moveDownMenuItem);
        }

        private void CreateMoveUpMenuItem()
        {
            if (this.PreviousSibling == null) return;

            ConfigurationMenuItem moveUpMenuItem = new ConfigurationMenuItem(SR.MoveUpMenuItemText, 
                                                                             new MoveNodeBeforeCommand(Site, this, this.PreviousSibling), 
                                                                             this.Parent, 
                                                                             Shortcut.None, 
                                                                             SR.MoveUpStatusText, 
                                                                             InsertionPoint.Action);
            AddMenuItem(moveUpMenuItem);
        }

    }
}