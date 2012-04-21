namespace MetaBuilders.WebControls {
    using System;
    using System.Configuration;
    using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Drawing;
	using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    
	/// <summary>
	/// Displays a link button control on the Web page.
	/// When the user clicks the link, a dialog box appears asking to confirm the action.
	/// </summary>
	/// <remarks>
	/// Use the ConfirmedLinkButton to create a button on the page which makes sure the user really wanted to push it.
	/// This is a common requirement of buttons which delete or change data.
	/// After clicking the button, but before the page posts back to the server, the user is asked to confirm the action.
	/// The text of the message displayed is set with the <see cref="ConfirmedLinkButton.Message"/> property.
	/// </remarks>
	/// <example>
	/// The following example demonstrates how to create a link Button which confirms an important action.
	/// <code>
	/// <![CDATA[
    /// <%@ Page Language="C#" %>
    /// <%@ Register tagprefix="mbcb" namespace="MetaBuilders.WebControls" assembly="MetaBuilders.WebControls.ConfirmedButtons" %>
    /// <script runat="server">
    ///     protected void MyButton_Click( Object Sender, EventArgs E ) {
    ///         Result.Text = System.DateTime.Now.ToString();
    ///     }
    /// </script>
    /// <html>
    /// 	<body>
    /// 	<form runat="server">
    ///         <mbcb:ConfirmedLinkButton runat="server" id="MyButton" Text="Click Me" Message="Are you sure?" onclick="MyButton_Click" />
    ///         <asp:Label runat="server" id="Result" />
    /// 	</form>
    /// 	</body>
    /// </html>
	/// ]]>
	/// </code>
	/// </example>
	public class ConfirmedLinkButton : LinkButton {
        
		/// <summary>
		/// Initializes a new instance of the <see cref="ConfirmedLinkButton"/> class.
		/// </summary>
		/// <remarks>
		/// Use this constructor to create and initialize a new instance of the <see cref="ConfirmedLinkButton"/> class.
		/// </remarks>
		public ConfirmedLinkButton() : base() {

		}

		/// <summary>
		/// Gets or sets the message displayed to confirm the action before postback.
		/// </summary>
		/// <value>The message displayed to confirm the action before postback. The default value is "Are you sure you want to do this?".</value>
		[
		Description("Gets or sets the message displayed to confirm the action before postback."),
		DefaultValue("Are you sure you want to do this?"),
		Category("Behavior"),
		Bindable(true)
		]
		public String Message {
			get { 
				object savedState;
                
				savedState = this.ViewState["Message"];
				if (savedState != null) {
					return (String) savedState;
				}
				return "Are you sure you want to do this?";
			}
			set { 
				ViewState["Message"] = value; 
			}
		}
 
		/// <summary>
		/// Gets or sets if the message set with <see cref="Message"/> will be displayed.
		/// </summary>
		/// <remarks>There are times when the developer only wants the user to view the message the first time.
		/// This can be done by setting MessageEnabled to false in the click event handler for the button.
		/// </remarks>
		[
		Description("Gets or sets if the message set with Message will be displayed."),
		DefaultValue(true),
		Category("Behavior"),
		Bindable(true)
		]
		public Boolean MessageEnabled {
			get { 
				object savedState;
                
				savedState = this.ViewState["MessageEnabled"];
				if (savedState != null) {
					return (Boolean) savedState;
				}
				return true;
			}
			set { ViewState["MessageEnabled"] = value; }
		}

		/// <summary>
		/// Adds to the specified writer those HTML attributes and styles that need to be rendered. This method is primarily used by control developers.
		/// </summary>
		/// <param name="writer">The output stream that renders HTML content to the client.</param>
		/// <remarks>
		/// Overridden to add the <see cref="ConfirmedImageButton.Message"/> to the Attributes collection.
		/// </remarks>
		protected override void AddAttributesToRender( HtmlTextWriter writer ) {
			base.AddAttributesToRender( writer );
			if ( MessageEnabled ) {
				if ( ClientScriptUtil.RenderUplevel  ) {
					writer.AddAttribute("confirmationmessage", this.Message);
				} else {
					writer.AddAttribute("onclick", "javascript: return confirm( &quot;" + this.Message.Replace( "\"", "&quot;" ) + "&quot; )",false);
				}
			}
		}
        
		/// <summary>
		/// Raises the PreRender event, which notifies the server control that is about to be rendered to the page.
		/// </summary>
		/// <remarks>Overridden to register client script with the <see cref="Page"/></remarks>
		/// <param name="e">An <see cref="EventArgs"/> object that contains the event data. </param>
		protected override void OnPreRender( EventArgs e ) {
			base.OnPreRender( e );
			if ( MessageEnabled ) {
				if ( ClientScriptUtil.RenderUplevel ) {
					this.RegisterClientScript();
				} // don't need an "else" because the script is put in the AddAttributesToRender otherwise
			}
		}

		/// <summary>
		/// Registers the client script used for uplevel browsers
		/// </summary>
		protected virtual void RegisterClientScript() {
			if ( !Page.IsClientScriptBlockRegistered(ClientScriptUtil.LibraryScriptName) ) {
				Page.RegisterClientScriptBlock(ClientScriptUtil.LibraryScriptName, ClientScriptUtil.LibraryScript );
			}

			Page.RegisterArrayDeclaration(ClientScriptUtil.ArrayName, "'" + ClientID + "'" );
			
			if ( !Page.IsStartupScriptRegistered(ClientScriptUtil.StartupScriptName ) ) {
				Page.RegisterStartupScript( ClientScriptUtil.StartupScriptName, ClientScriptUtil.StartupScript );
			}
		}
        
         
    }
}
