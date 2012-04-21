using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Resources;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;

namespace MetaBuilders.WebControls {
    
	/// <summary>
	/// A variation on the DropDownList control with the added ability to type directly into the text portion of the control.
	/// </summary>
	/// <remarks>
	/// <p>On modern browsers that support javascript and css properly, the ComboBox has a similar appearance to the standard DropDownList,
	/// except that the top area accepts direct text entry.
	/// On older browsers, or those that don't support modern standards, 
	/// the ComboBox will appear as two seperate controls, a TextBox, followed by a DropDownList.
	/// </p>
	/// <p>
	/// If you are using a visual designer with the ComboBox, be aware that Grid Mode layout support,
	/// while existing, is limited by the same constraints as the DropDownList control.
	/// Most 'alternate' browsers will not display the drop-down portion of the control correctly.
	/// </p>
	/// </remarks>
	/// <example>
	/// The following example demonstrates how to use the combobox on a page.
	/// <code>
	/// <![CDATA[
	/// <%@ Register TagPrefix="mbcbb" Namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.ComboBox" %>
	/// <html><body><form runat="server">
	///  <mbcbb:ComboBox id="ComboBox1" runat="server" >
	///   <asp:ListItem Value="1">Item 1</asp:ListItem>
	///   <asp:ListItem Value="2">Item 2</asp:ListItem>
	///   <asp:ListItem Value="3">Item 3</asp:ListItem>
	///  </mbcbb:ComboBox>
	/// </form></body></html>
	/// ]]>
	/// </code>
	/// </example>
	[
	Designer(typeof(MetaBuilders.WebControls.ComboBoxDesigner)),
	ValidationProperty("Text"),
	]
	public class ComboBox : System.Web.UI.WebControls.ListControl, IPostBackDataHandler, INamingContainer {

		/// <summary>
		/// Creates a new instance of the ComboBox control.
		/// </summary>
		public ComboBox() : base() {}
        
		#region Events
		/// <summary>
		/// The event which occurs when the <see cref="Text"/> property is changed.
		/// </summary>
		public event EventHandler TextChanged;
		/// <summary>
		/// Raises the <see cref="TextChanged"/> event.
		/// </summary>
		/// <param name="e">The data for the event.</param>
		protected virtual void OnTextChanged( EventArgs e ) {
			if ( TextChanged != null ) {
				TextChanged( this, e );
			}
		}
		#endregion

		#region Implementation of IPostBackDataHandler
		void IPostBackDataHandler.RaisePostDataChangedEvent() {
			OnSelectedIndexChanged(System.EventArgs.Empty);
		}

	   bool IPostBackDataHandler.LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection) {
			// No need to check for the text portion changing. That is handled automagically

			bool listIndexChanged = false;
		    
			if( TextIsInList ) {
		        
				ListItem selectedItem = this.Items.FindByText(text.Text);
				Int32 selectedIndex = Items.IndexOf( selectedItem );
	            
				if ( this.SelectedIndex != selectedIndex ) {
					listIndexChanged = true;
					this.SelectedIndex = selectedIndex;
				}
			} else {
				if ( this.SelectedIndex != -1  ) {
					listIndexChanged = true;
					this.SelectedIndex = -1;
				}
			}
			isLoaded = true;
			return listIndexChanged;
		}
		#endregion
        
		#region New Properties
		/// <summary>
		/// Gets or sets the number of rows displayed in the dropdown portion of the <see cref="ComboBox"/> control.
		/// </summary>
		/// <value>
		/// The number of rows displayed in the <see cref="ComboBox"/> control. The default value is 4.
		/// </value>
		[
		Description("Gets or sets the number of rows of the dropdown portion of the ComboBox control."),
		Category("Appearance"),
		DefaultValue(4),
		Bindable(true),
		]
		public virtual Int32 Rows {
			get {
				object savedRows;

				savedRows = this.ViewState["Rows"];
				if (savedRows != null)
					return (Int32) savedRows;
				return 4;
			}
			set {
				if ( value < 1 ) {
					throw new ArgumentOutOfRangeException();
				}
				this.ViewState["Rows"] = value;
			}

		}

		/// <summary>
		/// Gets or sets the text content of the text box portion of the <see cref="ComboBox"/> control.
		/// </summary>
		[
		Description("Gets or sets the text content of the text box portion of the ComboBox control."),
		Category("Appearance"),
		Bindable(true),
		DefaultValue("")
		]
		public virtual String Text {
			get {
				this.EnsureChildControls();
				if ( text.Text == null ) {
					return String.Empty;
				}
				return text.Text;
			}
			set {
				this.EnsureChildControls();
				text.Text = value;
			}
		}

		/// <summary>
		/// Gets a boolean value indicating whether the string in the text portion of the <see cref="ComboBox"/>
		/// can be found in the text property of any of the ListItems in the Items collection.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public virtual bool TextIsInList {
			get {
				this.EnsureChildControls();
				return ( Items.FindByText( text.Text ) != null );
			}
		}
	    
		/// <summary>
		/// Gets a boolean value indicating whether an external script library should be used,
		/// instead of the library being emitted as a whole.
		/// </summary>
		protected virtual bool UseReferenceLibrary {
			get {
				return ( ReferenceLibraryUrl.ToString().Length  > 0);
			}
		}
	    
		/// <summary>
		/// Queries the web.config file to get the external reference script library path, if available.
		/// </summary>
		protected virtual Uri ReferenceLibraryUrl {
			get {
				NameValueCollection config = ConfigurationSettings.GetConfig("MetaBuilders.WebControls.ComboBox") as NameValueCollection;
				if( config != null ) {
					return new Uri(config["ReferenceLibraryUrl"]);
				}
				return new Uri("");
			}
		}
	    

		#endregion

		#region Hidden Base Properties
		/// <summary>
		/// Hides the BorderColor property, as it not relevant on this control.
		/// </summary>
		[
		EditorBrowsableAttribute(EditorBrowsableState.Never),
		BindableAttribute(BindableSupport.No),
		BrowsableAttribute(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override System.Drawing.Color BorderColor {
			get {
				return base.BorderColor;
			}
			set {
				
			}
		}

		/// <summary>
		/// Hides the BorderStyle property, as it not relevant on this control.
		/// </summary>
		[
		EditorBrowsableAttribute(EditorBrowsableState.Never),
		BindableAttribute(BindableSupport.No),
		BrowsableAttribute(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override System.Web.UI.WebControls.BorderStyle BorderStyle {
			get {
				return base.BorderStyle;
			}
			set {
				
			}
		}

		/// <summary>
		/// Hides the BorderWidth property, as it not relevant on this control.
		/// </summary>
		[
		EditorBrowsableAttribute(EditorBrowsableState.Never),
		BindableAttribute(BindableSupport.No),
		BrowsableAttribute(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override System.Web.UI.WebControls.Unit BorderWidth {
			get {
				return base.BorderWidth;
			}
			set {
				
			}
		}

		/// <summary>
		/// Hides the ToolTip property, as it not relevant on this control.
		/// </summary>
		[
		EditorBrowsableAttribute(EditorBrowsableState.Never),
		BindableAttribute(BindableSupport.No),
		BrowsableAttribute(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override string ToolTip {
			get {
				return base.ToolTip;
			}
			set {
				
			}
		}

		/// <summary>
		/// Hides the Height property, as it not relevant on this control.
		/// </summary>
		[
		EditorBrowsableAttribute(EditorBrowsableState.Never),
		BindableAttribute(BindableSupport.No),
		BrowsableAttribute(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public override Unit Height {
			get {
				return Unit.Empty;
			}
			set {
				base.Height = Unit.Empty;
			}
		}
		#endregion

		
		#region Render Overrides
		/// <summary>
		/// Overrides the LoadViewState method.
		/// </summary>
		protected override void LoadViewState(object savedState) {
			base.LoadViewState(savedState);
			if ( this.ViewState["-1Saved"] != null && (Boolean)this.ViewState["-1Saved"] ) { // not really sure why I have to do this
				this.SelectedIndex = -1;
			}
		}

		/// <summary>
		/// Overrides the CreateChildControls method.
		/// </summary>
		protected override void CreateChildControls() {
			container = new WebControl();
			container.ID = "Container";
			this.Controls.Add( container );
            
			text = new TextBox();
			text.ID = "Text";
			text.Attributes["autocomplete"] = "off";
			container.Controls.Add( text );
			text.TextChanged += new EventHandler( this.raiseTextChanged );
            
			button = new WebControl();
			button.ID = "Button";
			container.Controls.Add( button );
		}

		/// <summary>
		/// Overrides the OnPreRender method.
		/// </summary>
		protected override void OnPreRender(System.EventArgs e) {

			base.OnPreRender(e);

			if( this.AutoPostBack ) {
				this.AutoPostBack = false;
				this.text.AutoPostBack = true;
			}
		    
			ResourceManager manager = new ResourceManager( this.GetType() );
			ResourceSet resources = manager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true);

			if( !Page.IsClientScriptBlockRegistered( "MetaBuilders.WebControls.ComboBox Library" ) ) {
				if( UseReferenceLibrary ) {
					Page.RegisterClientScriptBlock( "MetaBuilders.WebControls.ComboBox Library", "<script language=\"javascript\" src=\"" + ResolveUrl( ReferenceLibraryUrl.ToString() ) + "\"></script>" );
				} else {
					String script = resources.GetString("ClientScript");
					this.Page.RegisterClientScriptBlock("MetaBuilders.WebControls.ComboBox Library", script );
				}
			}
            
			String initScript = resources.GetString("InitScript");
			this.Page.RegisterStartupScript("MetaBuilders.WebControls.ComboBox Init", initScript );
            
			Page.RegisterArrayDeclaration( "ComboBoxes", "'" + this.ClientID + "'" );

		}

		/// <summary>
		/// Overrides the AddAttributesToRender method.
		/// </summary>
		protected override void AddAttributesToRender( HtmlTextWriter writer ) {

			if (this.Page != null) {
				this.Page.VerifyRenderingInServerForm(this);
			}

			writer.AddAttribute(HtmlTextWriterAttribute.Name, this.UniqueID);
			writer.AddAttribute( HtmlTextWriterAttribute.Onchange, "ComboBox_SimpleAttach(this, this.form['" + this.text.UniqueID + "']); " );
			writer.AddAttribute( "listSize", this.Rows.ToString() );

			base.AddAttributesToRender(writer);
		}
        
		/// <summary>
		/// Overrides the Render method.
		/// </summary>
		protected override void Render( HtmlTextWriter writer ) {
			this.EnsureChildControls();
			this.TextControl.ControlStyle.CopyFrom(this.ControlStyle);
			this.ContainerControl.ControlStyle.CopyFrom(this.ControlStyle);

			this.TextControl.Enabled = this.Enabled;
			this.ContainerControl.Style["POSITION"] = this.Style["POSITION"];
			this.ContainerControl.Style["TOP"] = this.Style["TOP"];
			this.ContainerControl.Style["LEFT"] = this.Style["LEFT"];
			this.ContainerControl.Style["Z-INDEX"] = this.Style["Z-INDEX"];

			base.Render( getCorrectTagWriter( writer )  );
		}
		
		/// <summary>
		/// Overrides the RenderBeginTag method.
		/// </summary>
		public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer) {
			container.RenderControl( writer );
			base.RenderBeginTag( writer );
		}
		
		/// <summary>
		/// Overrides the RenderContents method.
		/// </summary>
		protected override void RenderContents(System.Web.UI.HtmlTextWriter writer) {
			Boolean oneSelected = false;

			foreach( ListItem item in this.Items ) {
				writer.WriteBeginTag("option");
				if ( item.Selected ) {
					if ( !oneSelected ) {
						writer.WriteAttribute( "selected", "selected", false );
					}
					oneSelected = true;
				}

				writer.WriteAttribute( "value", item.Value, true );
				writer.Write(">");
				HttpUtility.HtmlEncode(item.Text, writer );
				writer.WriteEndTag("option");
				writer.WriteLine();
			}

		}

		/// <summary>
		/// Overrides the SaveViewState method.
		/// </summary>
		protected override object SaveViewState() {
			this.ViewState["-1Saved"] = ( this.SelectedIndex == -1 ); // not really sure why I have to do this
			return  base.SaveViewState();
		}

		#endregion
		
		#region Util
		private HtmlTextWriter getCorrectTagWriter( HtmlTextWriter writer ) {

			HtmlTextWriter tagWriter = writer;

			if ( writer is System.Web.UI.Html32TextWriter ) {
				HttpBrowserCapabilities browser = this.Page.Request.Browser;
				if( browser.W3CDomVersion.Major > 0 ) {
					tagWriter =  new HtmlTextWriter( writer.InnerWriter );
				} else if ( String.Compare( browser.Browser, "netscape", true ) == 0 ) {
					if ( browser.MajorVersion >= 5 ) {
						tagWriter = new HtmlTextWriter( writer.InnerWriter );
					}
				}
			}

			return tagWriter;
		}

		private void raiseTextChanged( Object sender, EventArgs e ){
			OnTextChanged( e );
		}

		/// <summary>
		/// The container control of ComboBox's controls.
		/// </summary>
		/// <remarks>
		/// This is used by the designer.
		/// </remarks>
		protected internal WebControl ContainerControl {
			get {
				EnsureChildControls();
				return container;
			}
		}

		/// <summary>
		/// The button which activates the dropdownlist portion of the ComboBox.
		/// </summary>
		/// <remarks>This is used by the designer.</remarks>
		protected internal WebControl ButtonControl {
			get {
				EnsureChildControls();
				return button;
			}
		}

		/// <summary>
		/// The text area of the ComboBox.
		/// </summary>
		/// <remarks>This is used by the designer.</remarks>
		protected internal TextBox TextControl {
			get {
				EnsureChildControls();
				return text;
			}
		}

		private WebControl container;
		private WebControl button;
		private TextBox text;

		private Boolean isLoaded = false;
		#endregion

		/// <summary>
		/// Overrides <see cref="ListControl.SelectedIndex"/>.
		/// </summary>
		public override int SelectedIndex {
			get {
				return base.SelectedIndex;
			}
			set {
				base.SelectedIndex = value;
				if ( this.isLoaded ) {
					if ( this.SelectedItem != null ) {
						this.Text = this.SelectedItem.Text;
					} else {
						this.Text = String.Empty;
					}
				}
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.OnLoad"/>
		/// </summary>
		protected override void OnLoad(System.EventArgs e) {
			this.isLoaded = true;
			base.OnLoad(e);
		}

	}
}
