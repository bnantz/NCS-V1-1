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
	/// Provides a dialog that shows a custom <see cref="DialogPage"/>.
	/// Call <see cref="DialogWindowBase.Open"/> to display the dialog window, or point a <see cref="DialogOpenButton"/> at it.
	/// </summary>
	/// <remarks>
	/// <p>
	/// The DialogWindow control requires javascript support in order to function.
	/// </p>
	/// </remarks>
	/// <example>
	/// The following example shows you how to use the <see cref="DialogWindow"/> on your page.
	/// <code>
	/// <![CDATA[
	/// <%@ Register TagPrefix="mbdw" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.DialogWindow" %>
	/// <script runat="server" language="c#" >
	/// protected void MyDialogWindow_DialogClosed( Object sender, DialogResultEventArgs e ) {
	///		Entry.Text = e.Results;
	/// }
	/// </script>
	/// <form runat="server">
	///		<asp:TextBox runat="server" Id="Entry" />
	///		<mbdw:DialogWindow runat="server" Id="MyDialog" 
	///			OnDialogClosed="MyDialog_DialogClosed" 
	///			Resizable="True"
	///			TargetUrl="DialogPage.aspx"
	///			WindowHeight="150"
	///			WindowWidth="400" 
	///		/>
	///		<mbdw:DialogOpenButton runat="server" 
	///			DialogToOpen="MyDialog" 
	///			Text="Open A Custom Dialog Window" 
	///		/>
	/// </form>
	/// ]]>
	/// </code>
	/// </example>
	public class DialogWindow : DialogWindowBase {

		#region Properties
		/// <summary>
		/// Gets or sets the url for the <see cref="DialogPage"/> serving as the UI of the dialog.
		/// </summary>
		[
		Description("Gets or sets the url for the DialogPage serving as the UI of the dialog."),
		Category("Dialog"),
		DefaultValue(""),
		Bindable(true),
		EditorAttribute(typeof(System.Web.UI.Design.UrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
		]
		public virtual Uri TargetUrl {
			get {
				Object savedState = this.ViewState["TargetUrl"];
				if ( savedState != null ) {
					return new Uri((string)savedState);
				}
				return new Uri("");
			}
			set {
				this.ViewState["TargetUrl"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the height of the dialog window.
		/// </summary>
		[
		Description("Gets or sets the height of the dialog window."),
		Category("Dialog"),
		DefaultValue(typeof(Unit),""),
		Bindable(true),
		]
		public virtual Unit WindowHeight {
			get {
				Object savedState = this.ViewState["WindowHeight"];
				if ( savedState != null ) {
					return (Unit)savedState;
				}
				return Unit.Empty;
			}
			set {
				this.ViewState["WindowHeight"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the width of the dialog window.
		/// </summary>
		[
		Description("Gets or sets the width of the dialog window."),
		Category("Dialog"),
		DefaultValue(typeof(Unit),""),
		Bindable(true),
		]
		public virtual Unit WindowWidth {
			get {
				Object savedState = this.ViewState["WindowWidth"];
				if ( savedState != null ) {
					return (Unit)savedState;
				}
				return Unit.Empty;
			}
			set {
				this.ViewState["WindowWidth"] = value;
			}
		}

		/// <summary>
		/// Gets a collection of name/value pairs that are added to the querystring of the <see cref="TargetUrl"/>.
		/// </summary>
		[
		Description("Gets a collection of name/value pairs that are added to the querystring of the TargetUrl."),
		Browsable(false),
		]
		public virtual NameValueCollection DialogArguments {
			get {
				if ( dialogArguments == null ) {
					dialogArguments = new NameValueCollection();
				}
				return dialogArguments;
			}
		}
		private NameValueCollection dialogArguments;

		#endregion

		/// <summary>
		/// Overrides <see cref="Control.SaveViewState"/>
		/// </summary>
		protected override object SaveViewState() {
			Triplet vstate = new Triplet();
			vstate.First = base.SaveViewState();
			if ( this.dialogArguments == null || this.dialogArguments.Count == 0 ) {
				vstate.Second = null;
				vstate.Third = null;
			} else {
				String[] keys = this.dialogArguments.AllKeys;
				vstate.Second = keys;

				String[] values = new String[keys.Length];
				for(Int32 i = 0; i < keys.Length; i++ ) {
					values[i] = this.dialogArguments[i];
				}
				vstate.Third = values;
			}
			return vstate;
		}

		/// <summary>
		/// Overrides <see cref="Control.LoadViewState"/>
		/// </summary>
		protected override void LoadViewState(object savedState) {
			Triplet vstate = savedState as Triplet;
			base.LoadViewState(vstate.First);
			if ( vstate.Second != null ) {
				String[] keys = vstate.Second as String[];
				String[] values = vstate.Third as String[];
				for( Int32 i = 0; i < keys.Length; i++ ) {
					this.DialogArguments[keys[i]] = values[i];
				}
			}
		}

		/// <summary>
		/// Generates the script neccessary to open the dialog.
		/// </summary>
		/// <remarks>
		/// <p>Calling <see cref="DialogWindowBase.Open"/> will cause this script to be sent to the client browser.</p>
		/// <p>This method can also be used to open the dialog on the client without postingback.</p>
		/// <p>This method is only public for developers who want to customize how and when the window is opened via code or a custom server control.</p>
		/// </remarks>
		public override String GetDialogOpenScript() {
			StringBuilder finalUrl = new StringBuilder();
			finalUrl.Append(this.ResolveUrl(this.TargetUrl.ToString()));
			String argAppender = "&";
			if ( TargetUrl.ToString().IndexOf("?") == -1 ) {
				argAppender = "?";
			}
			if ( this.dialogArguments != null ) {
				foreach( String key in this.dialogArguments.Keys ) {
					finalUrl.Append(argAppender);
					if ( argAppender == "?" ) {
						argAppender = "&";
					}
					finalUrl.Append(HttpUtility.UrlEncode(key));
					finalUrl.Append("=");
					finalUrl.Append(HttpUtility.UrlEncode(this.dialogArguments[key]));
				}
			}
			NameValueCollection features = this.GetStandardFeatures();
			if ( this.WindowHeight != Unit.Empty ) {
				features["height"] = this.WindowHeight.Value.ToString();
				features["innerHeight"] = this.WindowHeight.Value.ToString();
			}
			if ( this.WindowWidth != Unit.Empty ) {
				features["width"] = this.WindowWidth.Value.ToString();
				features["innerWidth"] = this.WindowWidth.Value.ToString();
			}

			return this.GetDialogOpenScript(new Uri(finalUrl.ToString()),features);
		}

	}

}


