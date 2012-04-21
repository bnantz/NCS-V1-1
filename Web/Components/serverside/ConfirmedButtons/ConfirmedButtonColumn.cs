using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MetaBuilders.WebControls;

namespace MetaBuilders.WebControls {
    /// <summary>
    /// Creates a button column which asks the user for confirmation before commiting the click
    /// </summary>
    /// <remarks>
    /// A common use for this is as a protection on a button which deletes the selected row from the datagrid.
    /// A typical Message might be, "Are you sure you want to delete this data?".
    /// Except for the addition of the Message property, the usage is the same as for a <see cref="ButtonColumn"/>.
    /// </remarks>
    public class ConfirmedButtonColumn : ButtonColumn {
        
        /// <summary>
        /// The confirmation message to ask the user after clicking the button, but before posting back.
        /// </summary>
        [
			Description("he confirmation message to ask the user after clicking the button, but before posting back."),
			Category("Behavior"),
			DefaultValue("Are you sure you want to do this?")
		]
		public virtual String Message {
            get {
                object savedData = this.ViewState["Message"];
                
                if (savedData != null) {
                	return (String) savedData;
                }
                return "Are you sure you want to do this?";
            }
            set {
                this.ViewState["Message"] = value;
                this.OnColumnChanged();
            }
        }

		/// <summary>
		/// Overrides <see cref="ButtonColumn.Initialize"/>.
		/// </summary>
		public override void Initialize() {
			base.Initialize();
			this.textFieldDesc = null;
		}
        
        /// <summary>
        /// Overridden to replace the button with a confirmed button
        /// </summary>
        public override void InitializeCell(TableCell cell, int columnIndex, ListItemType itemType) {
        	base.InitializeCell(cell, columnIndex, itemType);
        	
        	if ( itemType != ListItemType.Footer && itemType != ListItemType.Header ) {
				WebControl theButton = null;
        		if ( this.ButtonType == ButtonColumnType.LinkButton ) {
        			ConfirmedLinkButton theLinkButton = new ConfirmedLinkButton();
        			theLinkButton.Text = this.Text;
        			theLinkButton.CommandName = this.CommandName;
        			theLinkButton.CausesValidation = false;
        			theLinkButton.Message = this.Message;
        			theLinkButton.ID = theLinkButton.ClientID;
        			theButton = theLinkButton;
        		} else {
        			ConfirmedButton theNormalButton = new ConfirmedButton();
        			theNormalButton.Text = this.Text;
        			theNormalButton.CommandName = this.CommandName;
        			theNormalButton.CausesValidation = false;
        			theNormalButton.Message = this.Message;
        			theNormalButton.ID = theNormalButton.ClientID;
        			theButton = theNormalButton;
        		}

				if (this.DataTextField.Length != 0) {
					theButton.DataBinding += new EventHandler( OnDataBindColumn );
				}

				cell.Controls.Clear();
				cell.Controls.Add(theButton);
        	}
        }                
		
		private void OnDataBindColumn(object sender, EventArgs e) {
			Control theButton = (Control) sender;
			DataGridItem gridItem = (DataGridItem) theButton.NamingContainer;
			Object dataItem = gridItem.DataItem;

			if (this.textFieldDesc == null) {
				this.textFieldDesc = TypeDescriptor.GetProperties(dataItem).Find(this.DataTextField, true);
				if (this.textFieldDesc == null && !(this.DesignMode)) {
					throw new HttpException("Field Not Found '"+ this.DataTextField + "'" );
				}
			}

			String dataString;
			if (this.textFieldDesc != null) {
				Object dataValue = this.textFieldDesc.GetValue(dataItem);
				dataString = this.FormatDataTextValue(dataValue);
			} else {
				dataString = "Sample Databound Text";
			}
			if (theButton is LinkButton ) {
				((ConfirmedLinkButton) theButton).Text = dataString;
			} else if ( theButton is ConfirmedButton ) {
				((ConfirmedButton) theButton).Text = dataString;
			}
		}
        
		private PropertyDescriptor textFieldDesc;

    }
}