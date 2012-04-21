using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Opens the specified dialog without a postback to call <see cref="DialogWindowBase.Open"/>
	/// </summary>
	[
	DefaultProperty("DialogToOpen"),
	Designer(typeof(OpenerDesigner)),
	]
	public class DialogOpenButton : WebControl {
		
		/// <summary>
		/// Overrides <see cref="WebControl.TagKey"/>.
		/// </summary>
		protected override System.Web.UI.HtmlTextWriterTag TagKey {
			get {
				return HtmlTextWriterTag.Input;
			}
		}

		/// <summary>
		/// Gets or sets the ID of the <see cref="DialogWindowBase"/> to open.
		/// </summary>
		[
		Description("Gets or sets the ID of the DialogWindowBase to open."),
		Category("Dialog"),
		DefaultValue(""),
		Bindable(true),
		TypeConverter(typeof(DialogControlConverter)),
		]
		public virtual String DialogToOpen {
			get {
				Object savedState = this.ViewState["DialogToOpen"];
				if ( savedState != null ) {
					return (String)savedState;
				}
				return "";
			}
			set {
				this.ViewState["DialogToOpen"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Text of the button.
		/// </summary>
		[
		Description("Gets or sets the Text of the button."),
		Category("Appearance"),
		DefaultValue(""),
		Bindable(true),
		]
		public virtual String Text {
			get {
				Object savedState = this.ViewState["Text"];
				if ( savedState != null ) {
					return (String)savedState;
				}
				return "";
			}
			set {
				this.ViewState["Text"] = value;
			}
		}

		/// <summary>
		/// Overrides <see cref="WebControl.AddAttributesToRender"/>
		/// </summary>
		protected override void AddAttributesToRender(HtmlTextWriter writer) {

			writer.AddAttribute(HtmlTextWriterAttribute.Type,"button");
			writer.AddAttribute("value",this.Text);
			writer.AddAttribute("language","javascript");

			String myOnclick = " return false; ";

			// prepend the dialog open script to the onclick script
			if ( System.Web.HttpContext.Current != null && this.Page != null && this.Enabled && this.DialogToOpen != null && this.DialogToOpen.Length != 0 ) {

				DialogWindowBase dialog = this.NamingContainer.FindControl(this.DialogToOpen) as DialogWindowBase;
				if ( dialog == null ) {
					dialog = this.Page.FindControl(this.DialogToOpen) as DialogWindowBase;
				}
				if ( dialog == null ) {
					throw new InvalidOperationException("Cannot find a DialogWindow with the name '" + this.DialogToOpen + "'");
				}

				myOnclick = dialog.GetDialogOpenScript() + "; " + myOnclick;
			}

			// prepend the user's onclick script to mine
			if ( this.Attributes["onclick"] != null ) {
				myOnclick = this.Attributes["onclick"] + myOnclick;
				this.Attributes.Remove("onclick");
			}

			// write the final onclick
			writer.AddAttribute("onclick", myOnclick);

			base.AddAttributesToRender (writer);
		}



	}
}
