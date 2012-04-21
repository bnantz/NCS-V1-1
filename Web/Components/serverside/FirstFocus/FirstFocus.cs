using System;
using System.ComponentModel;
using System.Web.UI;

namespace MetaBuilders.WebControls {
	/// <summary>
	/// A control which gives another control focus on the webform as soon as it loads.
	/// </summary>
	/// <example>
	/// The following is an example page using the FirstFocus control
	/// <![CDATA[
	/// <%@ Register TagPrefix="mbff" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.FirstFocus" %>
	/// <html><body><form runat="server">
	/// <asp:TextBox runat="server" Id="TextBox1" />
	/// <asp:TextBox runat="server" Id="TextBox2" />
	/// <mbff:FirstFocus runat="server" ControlToFocus="TextBox2" />
	/// </form></body></html>
	/// ]]>
	/// </example>
	[
	Designer(typeof(FirstFocusDesigner)),
	]
	public class FirstFocus : Control {

		/// <summary>
		/// Gets or sets the ID of the control which will receive focus.
		/// </summary>
		/// <remarks>The control to focus must exist in the same <see cref="INamingContainer"/> as the <see cref="FirstFocus"/> control.</remarks>
		[
		Description("The ID of the control which will receive focus."),
		DefaultValue(""),
		Bindable(true),
		Category("Behavior"),
		TypeConverter(typeof(FirstFocusControlIdConverter)),
		]
		public virtual String ControlToFocus {
			get {
				Object savedState = this.ViewState["ControlToFocus"];
				if ( savedState != null ) {
					return (String)savedState;
				}
				return "";
			}
			set {
				this.ViewState["ControlToFocus"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether focus will be applied on postbacks.
		/// </summary>
		[
		Description("Gets or sets whether focus will be applied on postbacks."),
		DefaultValue(true),
		Bindable(true),
		Category("Behavior"),
		]
		public virtual Boolean EnableForPostBack {
			get {
				Object savedState = this.ViewState["EnableForPostBack"];
				if ( savedState != null ) {
					return (Boolean)savedState;
				}
				return true;
			}
			set {
				this.ViewState["EnableForPostBack"] = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the focusing behavior is enabled.
		/// </summary>
		[
		Description("Gets or sets whether the focusing behavior is enabled."),
		DefaultValue(true),
		Bindable(true),
		Category("Behavior"),
		]
		public virtual Boolean Enabled {
			get {
				Object savedState = this.ViewState["Enabled"];
				if ( savedState != null ) {
					return (Boolean)savedState;
				}
				return true;
			}
			set {
				this.ViewState["Enabled"] = value;
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/> to register client script.
		/// </summary>
		protected override void OnPreRender( EventArgs e ) {
			base.OnPreRender(e);

			if ( this.ControlToFocus.Length == 0 || !this.Enabled || ( this.Page.IsPostBack && !this.EnableForPostBack ) ) {
				return;
			}

			Control targetControl = this.NamingContainer.FindControl(this.ControlToFocus);
			if ( targetControl != null ) {
				RegisterScript(targetControl);
			}
		}

		private void RegisterScript(Control targetControl) {
			String pageScriptName = "MetaBuilders.WebControls.FirstFocusScript";
			if ( !this.Page.IsClientScriptBlockRegistered(pageScriptName) ) {
				using (System.IO.StreamReader reader = new System.IO.StreamReader(typeof(FirstFocus).Assembly.GetManifestResourceStream(typeof(FirstFocus), "FirstFocusScript.js"))) { 
					String script = "<script language='javascript' type='text/javascript' >\r\n<!--\r\n" + reader.ReadToEnd() + "\r\n//-->\r\n</script>";
					this.Page.RegisterClientScriptBlock(pageScriptName, script);
				}
			}
			targetControl.Page.RegisterStartupScript(pageScriptName, @"<script language='javascript' type='text/javascript'>
<!--
MetaBuilders_FirstFocus_Focus('" + targetControl.ClientID + "', '" + targetControl.UniqueID + @"');
// -->
</script>");
		}


	}
}
