using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Allows the client to upload a file to the server.
	/// </summary>
	/// <remarks>
	/// Use the FileUpload server control to handle uploading binary or text files from a browser client to the server.
	/// </remarks>
	/// <example>
	/// The following is an example of using the FileUpload control.
	/// <code><![CDATA[
	/// <%@ Register tagprefix="mbf" namespace="MetaBuilders.WebControls" assembly="MetaBuilders.WebControls.FileUpload" %>
	/// <script language="C#" runat="server">
	/// protected void FileUp_FileReceived( Object sender, EventArgs e ) {
	/// 	if ( FileUp.FileName != "" ) {
	/// 		try {
	/// 			FileUp.PostedFile.SaveAs( System.IO.Path.Combine( Server.MapPath("/"), FileUp.FileName ) );
	/// 			Result.Text = "File Saved";
	/// 		} catch ( Exception ex ) {
	/// 			Result.Text = "File Not Saved: " + ex.Message;
	/// 		}
	/// 	} else {
	/// 		Result.Text = "No File Uploaded";
	/// 	}
	/// }
	/// </script>
	/// <html><body><form runat="server">
	/// <mbf:FileUpload runat="server" OnFileReceived="FileUp_FileReceived" id="FileUp" />
	/// <asp:Button runat="server" text="Smack" />
	/// <br />
	/// <asp:Label runat="server" id="Result" runat="server" EnableViewState="false" />
	/// </form></body></html>
	/// ]]></code>
	/// </example>
	[
	DefaultEvent("FileReceived"),
	ValidationProperty("FileName")
	]
	public class FileUpload : System.Web.UI.WebControls.WebControl, IPostBackDataHandler {

		#region Properties
		/// <summary>
		/// Gets access to the uploaded file specified by a client.
		/// </summary>
		[
		Description("Gets access to the uploaded file specified by a client."),
		Browsable(false),
		DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),
		]
		public virtual HttpPostedFile PostedFile {
			get {
				return this.Context.Request.Files[this.UniqueID];
			}
		}

		/// <summary>
		/// Gets the file name, without the path, of the file uploaded.
		/// </summary>
		[
		Description("Gets the file name, without the path, of the file uploaded."),
		Browsable(false),
		DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden),
		]
		public virtual String FileName {
			get {
				if ( this.fileUploaded ) {
					return System.IO.Path.GetFileName(this.PostedFile.FileName);
				}
				return String.Empty;
			}
		}


		/// <summary>
		/// Gets or sets a value indicating whether an automatic postback to the server will occur whenever the user chooses a file to upload.
		/// </summary>
		[
		Description("Gets or sets a value indicating whether an automatic postback to the server will occur whenever the user chooses a file to upload."),
		Bindable(true),
		Category("Behavior"),
		DefaultValue(false),
		]
		public virtual Boolean AutoPostBack {
			get {
				Object savedState = this.ViewState["AutoPostBack"];
				if ( savedState != null ) {
					return (Boolean)savedState;
				}
				return false;
			}
			set {
				this.ViewState["AutoPostBack"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the maximum length of the file path for the file to upload from the client computer.
		/// </summary>
		[
		Description("Gets or sets the maximum length of the file path for the file to upload from the client computer."),
		Bindable(true),
		Category("Behavior"),
		DefaultValue(-1),
		]
		public virtual Int32 MaxLength {
			get {
				Object savedState = this.ViewState["MaxLength"];
				if ( savedState != null ) {
					return (Int32)savedState;
				}
				return -1;
			}
			set {
				this.ViewState["MaxLength"] = value;
			}
		}

		/// <summary>
		/// Gets or sets a comma-separated list of MIME encodings used to constrain the file types the user can select.
		/// </summary>
		/// <remarks>Use this property to specify the file type that can be uploaded to the server.
		/// For example, to restrict the selection to images, set this property to "image/*".</remarks>
		[
		Description("Gets or sets a comma-separated list of MIME encodings used to constrain the file types the user can select."),
		Bindable(true),
		Category("Behavior"),
		DefaultValue(""),
		]
		public virtual String Accept {
			get {
				Object savedState = this.ViewState["Accept"];
				if ( savedState != null ) {
					return (String)savedState;
				}
				return String.Empty;
			}
			set {
				this.ViewState["Accept"] = value;
			}
		}

		/// <summary>
		/// Overrides <see cref="WebControl.TagKey"/>.
		/// </summary>
		protected override System.Web.UI.HtmlTextWriterTag TagKey {
			get {
				return HtmlTextWriterTag.Input;
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.CreateControlCollection"/>.
		/// </summary>
		/// <returns>A ControlCollection which does not allow child controls.</returns>
		protected override ControlCollection CreateControlCollection() {
			return new EmptyControlCollection(this);
		}

		#endregion

		#region Events
		/// <summary>
		/// The event which is raised when the client uploads a file to the server.
		/// </summary>
		public event EventHandler FileReceived;
		/// <summary>
		/// Raises the FileReceived event.
		/// </summary>
		/// <param name="e">The <see cref="EventArgs"/> for the event.</param>
		protected virtual void OnFileReceived(EventArgs e ) {
			if( FileReceived != null ) {
				FileReceived(this,e);
			}
		}
		#endregion

		#region Util
		/// <summary>
		/// Ensures that the server form, which this control belongs to, has an enctype of "multipart/form-data".
		/// </summary>
		protected virtual void EnsureEncType() {
			System.Web.UI.Control parent = this.Parent;
			while( parent != null && parent.GetType() != typeof( System.Web.UI.HtmlControls.HtmlForm ) && parent.GetType() != typeof( System.Web.UI.Page ) ) {
				parent = parent.Parent;
			}
			System.Web.UI.HtmlControls.HtmlForm myForm = parent as System.Web.UI.HtmlControls.HtmlForm;
			if ( myForm != null) {
				myForm.Enctype = "multipart/form-data";
			}
				
		}

		private Boolean fileUploaded {
			get {
				return( this.PostedFile != null && this.PostedFile.ContentLength > 0 );
			}
		}
		#endregion

		#region Implementation of IPostBackDataHandler
		void IPostBackDataHandler.RaisePostDataChangedEvent() {
			OnFileReceived(EventArgs.Empty);
		}

		bool IPostBackDataHandler.LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection) {
			if ( this.fileUploaded && !this.postDataLoaded ) {
				this.postDataLoaded = true;
				return true;
			}
			return false;
		}
		private Boolean postDataLoaded = false;
		#endregion

		#region Rendering
		/// <summary>
		/// Overrides <see cref="Control.OnLoad"/>.
		/// </summary>
		protected override void OnLoad(System.EventArgs e) {
			base.OnLoad(e);
			if ( this.Page != null ) {
				Page.RegisterRequiresPostBack(this);
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>.
		/// </summary>
		protected override void OnPreRender(System.EventArgs e) {
			base.OnPreRender(e);
			if ( this.Page != null ) {
				this.EnsureEncType();
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.Render"/>.
		/// </summary>
		protected override void Render(System.Web.UI.HtmlTextWriter writer) {
			if ( this.Page != null ) {
				this.Page.VerifyRenderingInServerForm(this);
			}
			base.Render(writer);
		}

		/// <summary>
		/// Overrides <see cref="WebControl.AddAttributesToRender"/>.
		/// </summary>
		protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer) {
			base.AddAttributesToRender(writer);
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "file");
			writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
			if ( this.MaxLength != -1 ) {
				writer.AddAttribute(HtmlTextWriterAttribute.Maxlength, this.MaxLength.ToString());
			}
			if ( this.Accept.Length  > 0 ) {
				writer.AddAttribute("accept",this.Accept);
			}
			if (this.AutoPostBack && this.Page != null) {
				writer.AddAttribute(HtmlTextWriterAttribute.Onchange, this.Page.GetPostBackClientEvent(this, ""));
				writer.AddAttribute("language", "javascript");
			}

		}
		#endregion

	}
}
