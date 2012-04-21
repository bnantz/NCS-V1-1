using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MetaBuilders.WebControls;

namespace MetaBuilders.WebControls {
    /// <summary>
    /// Creates an EditCommandColumn which asks the user for confirmation before executing the command.
    /// </summary>
    /// <remarks>
    /// A common use for this is as a protection on a button which alters the selected row in the datagrid.
    /// A typical Message might be, "Are you sure you want to save changes?".
    /// By default, messages are empty, and don't cause a prompt. Only set the message on those buttons you want to create a prompt.
    /// Except for the addition of the Message properties, the usage is the same as for an EditCommandColumn.
    /// </remarks>
    public class ConfirmedEditCommandColumn : EditCommandColumn {
        
        
        /// <summary>
        /// The confirmation message to ask the user after clicking the Edit button, but before executing the command.
        /// </summary>
		[
		Description("he confirmation message to ask the user after clicking the Edit button, but before executing the command."),
		Category("Behavior"),
		DefaultValue("")
		]
		public virtual String EditMessage {
            get {
                object savedData = this.ViewState["EditMessage"];
                
                if (savedData != null) {
                	return (String) savedData;
                }
                return String.Empty;
            }
            set {
                this.ViewState["EditMessage"] = value;
                this.OnColumnChanged();
            }
        }
        
        /// <summary>
        /// The confirmation message to ask the user after clicking the Cancel button, but before executing the command.
        /// </summary>
		[
		Description("he confirmation message to ask the user after clicking the Cancel button, but before executing the command."),
		Category("Behavior"),
		DefaultValue("")
		]
		public virtual String CancelMessage {
            get {
                object savedData = this.ViewState["CancelMessage"];
                
                if (savedData != null) {
                	return (String) savedData;
                }
                return String.Empty;
            }
            set {
                this.ViewState["CancelMessage"] = value;
                this.OnColumnChanged();
            }
        }
        
        /// <summary>
        /// The confirmation message to ask the user after clicking the Update button, but before executing the command.
        /// </summary>
		[
		Description("he confirmation message to ask the user after clicking the Update button, but before executing the command."),
		Category("Behavior"),
		DefaultValue("")
		]
		public virtual String UpdateMessage {
            get {
                object savedData = this.ViewState["UpdateMessage"];
                
                if (savedData != null) {
                	return (String) savedData;
                }
                return String.Empty;
            }
            set {
                this.ViewState["UpdateMessage"] = value;
                this.OnColumnChanged();
            }
        }
        
		/// <summary>
		/// Overrides <see cref="EditCommandColumn.InitializeCell"/>
		/// </summary>
        public override void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType) {
        
        	base.InitializeCell(cell, columnIndex, itemType);
            if (itemType != ListItemType.Header && itemType != ListItemType.Footer) {
                ArrayList newCollection = new ArrayList();
                
                for( int i = 0; i < cell.Controls.Count; i++ ) {
                    Control innerControl = cell.Controls[i];
                    Control newInnerControl = innerControl;
                    
                    if ( innerControl is LinkButton ) {
                        LinkButton oldLink = (LinkButton)innerControl;
                        if ( 
                          ( oldLink.CommandName == "Edit"   && this.EditMessage.Length > 0   ) ||
                          ( oldLink.CommandName == "Update" && this.UpdateMessage.Length > 0 ) ||
                          ( oldLink.CommandName == "Cancel" && this.CancelMessage.Length > 0 ) ) {
                            
                            ConfirmedLinkButton myLink = new ConfirmedLinkButton();
                            myLink.CommandName = oldLink.CommandName;
                            myLink.Text = oldLink.Text;
                            myLink.CausesValidation = false;
                            myLink.ID = myLink.ClientID;
                            
                            if ( myLink.CommandName == "Edit" ) {
                                myLink.Message = this.EditMessage;
                            }
                            if ( myLink.CommandName == "Update" ) {
                                myLink.Message = this.UpdateMessage;
                            }
                            if ( myLink.CommandName == "Cancel" ) {
                                myLink.Message = this.CancelMessage;
                            }
                            
                            newInnerControl = myLink;
                        }
                        
                    }
                    if ( innerControl is Button ) {
                        Button oldButton = (Button)innerControl;
                        if ( 
                          ( oldButton.CommandName == "Edit"   && this.EditMessage.Length > 0   ) ||
                          ( oldButton.CommandName == "Update" && this.UpdateMessage.Length > 0 ) ||
                          ( oldButton.CommandName == "Cancel" && this.CancelMessage.Length > 0 ) ) {
                            
                            ConfirmedButton myButton = new ConfirmedButton();
                            myButton.CommandName = oldButton.CommandName;
                            myButton.Text = oldButton.Text;
                            myButton.CausesValidation = false;
                            myButton.ID = myButton.ClientID;
                            
                            if ( myButton.CommandName == "Edit" ) {
                                myButton.Message = this.EditMessage;
                            }
                            if ( myButton.CommandName == "Update" ) {
                                myButton.Message = this.UpdateMessage;
                            }
                            if ( myButton.CommandName == "Cancel" ) {
                                myButton.Message = this.CancelMessage;
                            }
                            
                            newInnerControl = myButton;
                        }
                        
                    }
                    newCollection.Add( newInnerControl );
                    
        	    }
        	    cell.Controls.Clear();
        	    foreach( Object aControl in newCollection ) {
        	        cell.Controls.Add( (Control)aControl );
        	    }
        	    
        	}
        }                
        
    }
}