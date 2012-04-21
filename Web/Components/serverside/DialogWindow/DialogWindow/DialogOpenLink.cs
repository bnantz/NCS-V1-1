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
	public class DialogOpenLink : WebControl {
		
		/// <summary>
		/// Overrides <see cref="WebControl.TagKey"/>.
		/// </summary>
		protected override System.Web.UI.HtmlTextWriterTag TagKey {
			get {
				return HtmlTextWriterTag.A;
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
				if ( value == null ) {
					throw new ArgumentNullException("value");
				}
				this.ViewState["DialogToOpen"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the Text of the link.
		/// </summary>
		/// <remarks>
		/// If both <see cref="ImageUrl"/> and <see cref="Text"/> are set,
		/// the <see cref="Text"/> value will be used as the alternate text of the image.
		/// </remarks>
		[
		Description("Gets or sets the Text of the link."),
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
				if ( value == null ) {
					throw new ArgumentNullException("value");
				}
				this.ViewState["Text"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the url of an image to display for the link.
		/// </summary>
		/// <remarks>
		/// If both <see cref="ImageUrl"/> and <see cref="Text"/> are set,
		/// the <see cref="Text"/> value will be used as the alternate text of the image.
		/// </remarks>
		[
		Description("Gets or sets the url of an image to display for the link."),
		Category("Appearance"),
		DefaultValue(""),
		Bindable(true),
		EditorAttribute(typeof(System.Web.UI.Design.ImageUrlEditor), typeof(System.Drawing.Design.UITypeEditor)),
		]
		public virtual Uri ImageUrl {
			get {
				Uri savedState = new Uri(this.ViewState["ImageUrl"].ToString());
			   return savedState;
			}
			set {
				if ( value == null ) {
					throw new ArgumentNullException("value");
				}
				this.ViewState["ImageUrl"] = value.ToString();
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.RenderChildren"/>
		/// </summary>
		protected override void RenderChildren(HtmlTextWriter writer) {
			if ( this.ImageUrl.ToString().Length > 0) 
         {
				System.Web.UI.WebControls.Image image = new Image();
				image.ImageUrl = this.ImageUrl.ToString();
				image.AlternateText = this.Text;
				image.RenderControl(writer);
			} else {
				writer.Write(this.Text);
			}
		}


		/// <summary>
		/// Overrides <see cref="WebControl.AddAttributesToRender"/>
		/// </summary>
		protected override void AddAttributesToRender(HtmlTextWriter writer) {

			if ( System.Web.HttpContext.Current != null && this.Page != null && this.Enabled && this.DialogToOpen.Length != 0 ) {

				DialogWindowBase dialog = this.NamingContainer.FindControl(this.DialogToOpen) as DialogWindowBase;
				if ( dialog == null ) {
					dialog = this.Page.FindControl(this.DialogToOpen) as DialogWindowBase;
				}
				if ( dialog == null ) {
					throw new InvalidOperationException("Cannot find a DialogWindow with the name '" + this.DialogToOpen + "'");
				}

				writer.AddAttribute(HtmlTextWriterAttribute.Href,"javascript:void(" + dialog.GetDialogOpenScript() + ");");
			} else {
				writer.AddAttribute(HtmlTextWriterAttribute.Href,"#");
			}

			base.AddAttributesToRender (writer);
		}



	}
}
