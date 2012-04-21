using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;
using System.Reflection;
using System.ComponentModel;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Controls that represent a dialog window derive from this class.
	/// </summary>
	[
	Designer(typeof(DialogDesigner)),
	]
	public abstract class DialogWindowBase : Control, IPostBackEventHandler {

		#region Properties

		/// <summary>
		/// Gets or sets if the user can resize the window.
		/// </summary>
		[
		Description("Gets or sets if the user can resize the window."),
		Category("Dialog"),
		DefaultValue(false),
		Bindable(true),
		]
		public virtual Boolean Resizable {
			get {
				Object savedState = this.ViewState["Resizable"];
				if ( savedState != null ) {
					return (Boolean)savedState;
				}
				return false;
			}
			set {
				this.ViewState["Resizable"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the result returned and used in the <see cref="DialogClosed"/> event when the dialog is closed via OS window methods such as clicking the "X" button, or by calling <see cref="DialogPage.Close"/> with no parameters.
		/// </summary>
		[
		Description("Gets or sets the result returned and used in the DialogClosed event when the dialog is closed via OS window methods such as clicking the &quot;X&quot; button, or by calling DialogPage.Close with no parameters."),
		Category("Dialog"),
		DefaultValue(""),
		Bindable(true),
		]
		public virtual String DefaultResultValue {
			get {
				Object savedState = this.ViewState["DefaultResultValue"];
				if ( savedState != null ) {
					return (String)savedState;
				}
				return "";
			}
			set {
				this.ViewState["DefaultResultValue"] = value;
			}
		}

		#endregion



		/// <summary>
		/// The event that is raised when the user has closed the dialog and results have been returned.
		/// </summary>
		/// <remarks>
		/// This event is not raised when no results are returned or the user closes the dialogwindow via the native window closing mechanisms.
		/// 
		/// </remarks>
		public event DialogResultEventHandler DialogClosed;
		
		/// <summary>
		/// Raises the DialogClosed event.
		/// </summary>
		protected virtual void OnDialogClosed(DialogResultEventArgs e) {
			if ( this.DialogClosed != null ) {
				this.DialogClosed(this,e);
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>
		/// </summary>
		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender (e);
			RegisterClientScript();
		}

		/// <summary>
		/// Registers the neccessary clientscript for the dialog.
		/// </summary>
		protected virtual void RegisterClientScript() {
			this.Page.RegisterArrayDeclaration("MetaBuilders_DialogWindows","'" + this.ClientID + "', \"" + this.Page.GetPostBackEventReference(this,"@dialogResult@") + "\", \"" + this.DefaultResultValue + "\"");

			if ( !Page.IsClientScriptBlockRegistered(scriptKey) ) {
				using (System.IO.StreamReader reader = new System.IO.StreamReader(typeof(DialogWindowBase).Assembly.GetManifestResourceStream(typeof(DialogWindowBase), "DialogWindowBaseScript.js"))) { 

					String script = "<script language='javascript' type='text/javascript' >\r\n<!--\r\n" + reader.ReadToEnd() + "\r\n//-->\r\n</script>";
					this.Page.RegisterClientScriptBlock(scriptKey, script);
				}
			}

			if ( !Page.IsStartupScriptRegistered(scriptKey) ) {
				Page.RegisterStartupScript(scriptKey,@"<script language='javascript' type='text/javascript' >
<!--
MetaBuilders_DialogWindow_Init();
// -->
</script>
");
			}
		}
		private String scriptKey = "MetaBuilders.WebControls.DialogWindowBase";

		/// <summary>
		/// Gets the script neccessary to open the dialog.
		/// </summary>
		/// <remarks>
		/// <p>
		/// When deriving from this class, you generally want to implement the publi GetDialogOpenScript by calling this method with the proper url and features.
		/// </p>
		/// </remarks>
		protected virtual String GetDialogOpenScript(System.Uri url, NameValueCollection features ) {
			System.Text.StringBuilder theScript = new System.Text.StringBuilder();
			theScript.Append("MetaBuilders_DialogWindow_OpenDialog('");
			theScript.Append(url.ToString());
			theScript.Append("', '");
			theScript.Append(this.ClientID);
			if ( this.Resizable ) {
				theScript.Append("', 'resizable=1");
			} else {
				theScript.Append("', 'resizable=0");
			}
			if ( features["resizable"] != null ) {
				features.Remove("resizable");
			}
			foreach( String key in features.AllKeys ) {
				theScript.Append(",");
				theScript.Append(key);
				theScript.Append("=");
				theScript.Append(features[key]);
			}
			theScript.Append("')");
			return theScript.ToString();
		}

		/// <summary>
		/// Creates and returns a NameValueCollection which has the standard features for a dialog window.
		/// </summary>
		/// <remarks>
		/// This is meant for developers creating custom dialogs.
		/// </remarks>
		protected virtual NameValueCollection GetStandardFeatures() {
			NameValueCollection features = new NameValueCollection();
			features["menubar"] = "0";
			features["status"] = "0";
			features["toolbar"] = "0";
			features["scrollbars"] = "1";
			features["location"] = "0";
			features["directories"] = "0";
			features["dependant"] = "1";
			features["dialog"] = "1";
			features["modal"] = "1";
			return features;
		}


		#region IPostBackEventHandler Members
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument){
			this.OnDialogClosed(new DialogResultEventArgs(eventArgument));
		}
		#endregion

		/// <summary>
		/// Generates the script neccessary to open the dialog.
		/// </summary>
		/// <remarks>
		/// <p>Calling <see cref="Open"/> will cause this script to be sent to the client browser.</p>
		/// <p>This method can also be used to open the dialog on the client without postingback.</p>
		/// <p>This method is only public for developers who want to customize how and when the window is opened via code or a custom server control.</p>
		/// </remarks>
		public abstract String GetDialogOpenScript();

		/// <summary>
		/// Opens the dialog.
		/// </summary>
		/// <remarks>
		/// The dialog window is opened via a script run on the client browser.
		/// Attach to the <see cref="DialogClosed"/> event to get the results of the dialog.
		/// </remarks>
		public void Open() {
			Page.RegisterStartupScript(scriptKey, @"<script language='javascript' type='text/javascript'>
<!--
" + this.GetDialogOpenScript() + @";
// -->
</script>");

		}

	}

}


