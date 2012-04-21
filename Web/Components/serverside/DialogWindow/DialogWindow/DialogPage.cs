using System;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Collections.Specialized;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// DialogPage is the base class for pages which are functioning as dialogs.
	/// </summary>
	/// <remarks>
	/// When using the <see cref="DialogWindow"/> control, 
	/// the <see cref="DialogWindow.TargetUrl"/> should point to a page which derives,
	/// directly or indirectly, from <see cref="DialogPage"/>.
	/// </remarks>
	/// <example>
	/// The following example shows you a typical <see cref="DialogPage"/>.
	/// <code>
	/// <![CDATA[
	/// <%@ Page Language="C#" Inherits="MetaBuilders.WebControls.DialogPage"  %>
	/// <script runat="server">
	/// protected void Smack_Click(Object sender, EventArgs e ) {
	///		this.Close(Results.Text);
	/// }
	/// </script>
	/// <html><body><form runat="server">
	///		<asp:TextBox runat="server" Id="Results" />
	///		<asp:Button runat="server" Id="Smack" Text="Smack" OnClick="Smack_Click" />
	/// </form></body></html>
	/// ]]>
	/// </code>
	/// </example>
	public class DialogPage : Page {

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>
		/// </summary>
		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender (e);
			RegisterScript();
		}

		/// <summary>
		/// Registers the neccessary script for the dialog to function.
		/// </summary>
		protected virtual void RegisterScript() {
			Page.RegisterClientScriptBlock(scriptKey, @"<script language='javascript' type='text/javascript' >
<!--
function MetaBuilders_DialogWindow_Close(results) {
	if ( window.opener != null && typeof( window.opener.MetaBuilders_DialogWindow_DoDialogPostBack) != 'undefined' ) {
		window.opener.MetaBuilders_DialogWindow_DoDialogPostBack(results);
	} else {
		window.alert('The page that this dialog belonged to is no longer available. The results may not be what was expected');
	}
	window.close();
}
// -->
</script>");
		}

		/// <summary>
		/// Gets the script required to close the dialog with the given results.
		/// </summary>
		/// <remarks>
		/// Simply call <see cref="DialogPage.Close"/> to close the dialog from the server code.
		/// Only call this method to get the script neccessary to close the dialog from a client script event handler.
		/// </remarks>
		/// <param name="results">The results which will be available in the <see cref="DialogWindowBase.DialogClosed"/> event.</param>
		public String GetDialogCloseScript(String results) {
			return @"MetaBuilders_DialogWindow_Close('" + results.Replace("'","\\'") + @"');";
		}

		/// <summary>
		/// Gets the script required to close the dialog with default results.
		/// </summary>
		/// <remarks>
		/// Simply call <see cref="DialogPage.Close"/> to close the dialog from the server code.
		/// Only call this method to get the script neccessary to close the dialog from a client script event handler.
		/// </remarks>
		public String GetDialogCloseScript() {
			return @"MetaBuilders_DialogWindow_Close(null);";
		}

		/// <summary>
		/// Closes the dialog, and returns the given results for the <see cref="DialogWindowBase.DialogClosed"/> event.
		/// </summary>
		/// <param name="results">The results which will be available in the <see cref="DialogWindowBase.DialogClosed"/> event.</param>
		public void Close(String results) {
			String script = this.GetDialogCloseScript(results);
			Page.RegisterStartupScript(scriptKey,"<script language='javascript' type='text/javascript' >\r\n<!--\r\n" + script + "\r\n// -->\r\n</script>");
		}

		/// <summary>
		/// Closes the dialog, and returns the default results.
		/// </summary>
		public void Close(){
			Page.RegisterStartupScript(scriptKey, "<script language='javascript' type='text/javascript' >\r\n<!--\r\n" + GetDialogCloseScript() + "\r\n// -->\r\n</script>");
		}

		private String scriptKey = "MetaBuilders.WebControls.DialogPage";

	}
}
