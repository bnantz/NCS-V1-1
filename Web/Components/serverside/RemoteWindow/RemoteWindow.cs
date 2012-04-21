using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// A control that displays a link to another web page, which opens in a new, minimally adorned window.
	/// </summary>
	/// <remarks>
	/// Only browsers which have javascript will open the window with the properties set on the control.
	/// Other browsers will open a new window with default adornments and attributes.
	/// </remarks>
	/// <example>
	/// The following example demonstrates how to create a simple remote window.
	/// <code>
	/// <![CDATA[
	/// <html>
	///		<body>
	///		<form runat="server">
	///			<h3>RemoteWindow Example</h3>
	///
	///			Click on the link.<br><br>
	///
	///			<mb:RemoteWindow id="RemoteWindow1"
	///				NavigateUrl="http://www.google.com"
	///				Text="Quick Search"
	///				runat="server"/>
	///       
	///		</form>
	///		</body>
	/// </html>
	/// ]]>
	/// </code>
	/// </example>
	public class RemoteWindow : HyperLink {

		/// <summary>
		/// Initializes a new instance of the <see cref="RemoteWindow"/> class.
		/// </summary>
		public RemoteWindow(): base() {
			
			Random ran = new Random(this.GetHashCode());
			WindowName = "newWindow"+ ran.Next().ToString();
			
			base.Target = "_blank";
		}
	
		/// <summary>
		/// Gets or sets the <see cref="WindowName"/> of the <see cref="RemoteWindow"/>.
		/// </summary>
		/// <remarks>
		/// The WindowName is largly irrelevant unless client-side script is going to access it.
		/// The control will make up its own name, which is likely to be unique, if no name is given.
		/// </remarks>
		[
			Bindable(true),
			Category("Misc"),
			Description("Gets or sets the WindowName of the RemoteWindow."),
			DefaultValue("")
		]
		public String WindowName {
			get {
				object savedState;
                
				savedState = this.ViewState["WindowName"];
				if (savedState != null) {
					return (String) savedState;
				}
				return "";
			}
            set { 
				ViewState["WindowName"] = value; 
			}
		}

		/// <summary>
		/// Gets or sets if the user can resize the <see cref="RemoteWindow"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Window Attributes"),
		Description("Gets or sets if the user can resize the RemoteWindow."),
		DefaultValue(true)
		]
		public Boolean Resizable {
			get { 
				object savedState;
                
				savedState = this.ViewState["Resizable"];
				if (savedState != null) {
					return (Boolean) savedState;
				}
				return true;
			}
			set { ViewState["Resizable"] = value; }
		}

		/// <summary>
		/// Gets or sets the distance, in pixels, from the top of the screen to the top of the <see cref="RemoteWindow"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Window Attributes"),
		Description("Gets or sets the distance, in pixels, from the top of the screen to the top of the RemoteWindow."),
		DefaultValue("")
		]
		public Unit Top {
			get {
				object savedState;
                
				savedState = this.ViewState["Top"];
				if (savedState != null) {
					return (Unit) savedState;
				}
				return Unit.Empty;
			}
			set { 
				ViewState["Top"] = value; 
			}
		}

		/// <summary>
		/// Gets or sets the distance, in pixels, from the left of the screen to the left of the <see cref="RemoteWindow"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Window Attributes"),
		Description("Gets or sets the distance, in pixels, from the left of the screen to the left of the RemoteWindow."),
		DefaultValue("")
		]
		public Unit Left {
			get {
				object savedState;
                
				savedState = this.ViewState["Left"];
				if (savedState != null) {
					return (Unit) savedState;
				}
				return Unit.Empty;
			}
			set { 
				ViewState["Left"] = value; 
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="WindowHeight"/>, in pixels, of the <see cref="RemoteWindow"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Window Attributes"),
		Description("Gets or sets the WindowHeight, in pixels, of the RemoteWindow."),
		DefaultValue("")
		]
		public Unit WindowHeight {
			get {
				object savedState;
                
				savedState = this.ViewState["WindowHeight"];
				if (savedState != null) {
					return (Unit) savedState;
				}
				return Unit.Empty;
			}
			set { 
				ViewState["WindowHeight"] = value; 
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="WindowWidth"/>, in pixels, of the <see cref="RemoteWindow"/>.
		/// </summary>
		[
		Bindable(true),
		Category("Window Attributes"),
		Description("Gets or sets the WindowWidth, in pixels, of the RemoteWindow."),
		DefaultValue("")
		]
		public Unit WindowWidth {
			get {
				object savedState;
                
				savedState = this.ViewState["WindowWidth"];
				if (savedState != null) {
					return (Unit) savedState;
				}
				return Unit.Empty;
			}
			set { 
				ViewState["WindowWidth"] = value; 
			}
		}

		/// <summary>
		/// Adds to the specified writer those HTML attributes and styles that need to be rendered. This method is primarily used by control developers.
		/// </summary>
		/// <param name="writer">The output stream that renders HTML content to the client.</param>
		/// <remarks>
		/// Overridden to set additional attributes.
		/// </remarks>
		protected override void AddAttributesToRender( HtmlTextWriter writer ) {
			base.AddAttributesToRender( writer );

			StringBuilder Options = new StringBuilder();

			//Standard Thingies
			Options.Append ("javascript:window.open( '");
			Options.Append (this.ResolveUrl(NavigateUrl));
			Options.Append ("', '");
			Options.Append (WindowName);
			Options.Append ("', 'menubar=0,toolbar=0,resizable=");
			
			if ( Resizable ) {
				Options.Append( "1," );
			}
			else {
				Options.Append( "0," );
			}

			//window sizes
			if ( Top != Unit.Empty ) {
				Options.Append ("top=");
				Options.Append ( Top.Value.ToString() );
				Options.Append (",screenY=");
				Options.Append ( Top.Value.ToString() );
				Options.Append (",");
			}
			
			if ( Left != Unit.Empty ) {
				Options.Append ("left=");
				Options.Append ( Left.Value.ToString() );
				Options.Append (",screenX=");
				Options.Append ( Left.Value.ToString() );
				Options.Append (",");
			}	

			if ( WindowHeight != Unit.Empty ) {
				Options.Append ("height=");
				Options.Append ( WindowHeight.Value.ToString() );
				Options.Append (",innerHeight=");
				Options.Append ( WindowHeight.Value.ToString() );
				Options.Append (",");
			}	

			if ( WindowWidth != Unit.Empty ) {
				Options.Append ("width=");
				Options.Append ( WindowWidth.Value.ToString() );
				Options.Append (",innerWidth=");
				Options.Append ( WindowWidth.Value.ToString() );
			}	

			Options.Append( "'); return false;" );
			writer.AddAttribute("onclick", Options.ToString(), true );
		}
		

	}

}