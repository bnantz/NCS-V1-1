using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.UI;
using System.ComponentModel;
using System.Resources;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// The ResizeMonitor control raises server-side events when the user changes their browser window size.
	/// </summary>
	/// <example>
	/// This is an example usage of the ResizeMonitor control.
	/// <code>
	/// <![CDATA[
	/// <%@ Page language="C#" %>
	/// <%@ Register tagprefix="mbrp" namespace="MetaBuilders.WebControls" assembly="MetaBuilders.WebControls.ResizeMonitor" %>
	/// <script runat="server">
	///		protected virtual void ClientResize( Object sender, EventArgs e ) {
	///			Result.Text = "Resized to " + rpb.Width.ToString() + "X" + rpb.Height.ToString();
	///		}
	/// </script>
	/// <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
	/// <html><body><form runat="server">
	///		<mbrp:ResizeMonitor runat="server" id="rpb" OnResize="ClientResize" />
	///		<asp:Label runat="server" id="Result" EnableViewState="false" />
	///		<div style="border:1px solid black;height:90%;width:90%;">foo</div>
	/// </form></body></html>
	/// ]]>
	/// </code>
	/// </example>
	[
	Designer(typeof(ResizeMonitorDesigner))
	]
	public class ResizeMonitor : System.Web.UI.Control, INamingContainer, IPostBackEventHandler {

		#region Properties
		/// <summary>
		/// The length of time, in milliseconds, that the control will wait before firing a postback after a window resize.
		/// </summary>
		[
		Description("The length of time, in milliseconds, that the control will wait before firing a postback after a window resize."),
		Category("Behavior"),
		DefaultValue(500)
		]
		public virtual Int32 Delay {
			get {
				Object savedState = this.ViewState["Delay"];
				if ( savedState != null ) {
					return (Int32)savedState;
				}
				return 500;
			}
			set {
				this.ViewState["Delay"] = value;
			}
		}

		/// <summary>
		/// Gets the resized width of the client browser.
		/// </summary>
		[
		Description(""),
		Browsable(false)
		]
		public virtual Int32 Width {
			get {
				Object savedState = this.ViewState["Width"];
				if ( savedState != null ) {
					return (Int32)savedState;
				}
				return -1;
			}
		}
		/// <summary>
		/// Sets the resized width of the client browser.
		/// </summary>
		protected virtual void SetWidth( Int32 value ) {
			this.ViewState["Width"] = value;
		}

		/// <summary>
		/// Gets the resized height of the client browser.
		/// </summary>
		[
		Description(""),
		Browsable(false)
		]
		public virtual Int32 Height {
			get {
				Object savedState = this.ViewState["Height"];
				if ( savedState != null ) {
					return (Int32)savedState;
				}
				return -1;
			}
		}
		/// <summary>
		/// Sets the resized height of the client browser.
		/// </summary>
		protected virtual void SetHeight( Int32 value ) {
			this.ViewState["Height"] = value;
		}
		#endregion

		#region Events
		/// <summary>
		/// The event which fires when the user resizes the browser window.
		/// </summary>
		public event EventHandler Resize;
		/// <summary>
		/// Raises the Resize event.
		/// </summary>
		protected virtual void OnResize( EventArgs e ) {
			if ( Resize != null ) {
				Resize( this, e );
			}
		}
		#endregion

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>.
		/// </summary>
		protected override void OnPreRender(System.EventArgs e) {
			base.OnPreRender(e);
			RegisterClientScript();
			this.widthHolder.Value = this.Width.ToString();
			this.heightHolder.Value = this.Height.ToString();
		}

		/// <summary>
		/// Overrides <see cref="Control.Render"/>.
		/// </summary>
		protected override void Render(System.Web.UI.HtmlTextWriter writer) {
			this.RenderChildren(writer);
		}


		#region Client Script

		/// <summary>
		/// Registers the clientside script with the page.
		/// </summary>
		protected virtual void RegisterClientScript() {
			RegisterLibraryScript();
			RegisterPostbackScript();
			RegisterStartupScript();
		}

		/// <summary>
		/// Register the clientside script library with the page.
		/// </summary>
		protected virtual void RegisterLibraryScript() {
			if ( !Page.IsClientScriptBlockRegistered(LibraryScriptName) ) {
				Page.RegisterClientScriptBlock(LibraryScriptName, LibraryScript );
			} else {
				throw new InvalidOperationException("Only one ResizeMonitor control allowed per page.");
			}
		}

		/// <summary>
		/// Registers the clientside postback script with the page.
		/// </summary>
		protected virtual void RegisterPostbackScript() {
			if ( !Page.IsClientScriptBlockRegistered(PostbackScriptName) ) {
				System.Text.StringBuilder script = new System.Text.StringBuilder();
				script.Append("<script language='javascript'>\n<!--\n");
				script.Append("function ResizeMonitor_Postback() {\n");
				script.Append("ResizeMonitor_SaveDimensions();\n");
				script.Append(Page.GetPostBackEventReference(this));
				script.Append(";\n}\n");
				script.Append(this.GetClientVariableDeclaration("TimeoutLength", this.Delay.ToString() ) );
				script.Append(this.GetClientVariableDeclaration("WidthHolder", this.widthHolder.UniqueID, true ) );
				script.Append(this.GetClientVariableDeclaration("HeightHolder", this.heightHolder.UniqueID, true ) );
				script.Append("//-->\n</script>");

				Page.RegisterClientScriptBlock(PostbackScriptName, script.ToString() );
			}
		}

		/// <summary>
		/// Creates a clientside variable with the given name and value.
		/// </summary>
		/// <param name="name">The name of the variable.</param>
		/// <param name="value">The initial value of the variable.</param>
		/// <returns>A string containing the script variable declaration.</returns>
		protected virtual String GetClientVariableDeclaration( String name, String value ) {
			return GetClientVariableDeclaration( name, value, false );
		}

		/// <summary>
		/// Creates a clientside variable with the given name and value, quoted or not.
		/// </summary>
		/// <param name="name">The name of the variable.</param>
		/// <param name="value">The initial value of the variable.</param>
		/// <param name="quote">Whether the value should be quoted or not.</param>
		/// <returns>A string containing the script variable declaration.</returns>
		protected virtual String GetClientVariableDeclaration( String name, String value, Boolean quote ) {
			if ( quote ) {
				return "document.ResizeMonitor_" + name + " = '" + value + "';\n";
			} else {
				return "document.ResizeMonitor_" + name + " = " + value + ";\n";
			}
		}

		/// <summary>
		/// Registers the startup script with the page.
		/// </summary>
		protected virtual void RegisterStartupScript() {
			if ( !Page.IsStartupScriptRegistered(StartupScriptName ) ) {
				Page.RegisterStartupScript( StartupScriptName, StartupScript );
			}
		}



		/// <summary>
		/// The startup script for the control.
		/// </summary>
		protected virtual String StartupScript {
			get {
				ResourceManager manager = new ResourceManager( typeof(ResizeMonitor) );
				return manager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true).GetString("StartupScript");
			}
		}

		/// <summary>
		/// The library script for the control.
		/// </summary>
		protected virtual String LibraryScript {
			get {
				if( UseReferenceLibrary ) {
					return "<script language=\"javascript\" src=\"" + ReferenceLibraryUrl + "\"></script>";
				} else {
					ResourceManager manager = new ResourceManager( typeof(ResizeMonitor) );
					return manager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true).GetString("LibraryScript");
				}
			}
		}


		/// <summary>
		/// Gets a boolean value indicating whether an external script library should be used,
		/// instead of the library being emitted as a whole.
		/// </summary>
		protected virtual bool UseReferenceLibrary {
			get {
				return ( ReferenceLibraryUrl.ToString().Length > 0 );
			}
		}
	    
		/// <summary>
		/// The path to the script library folder
		/// </summary>
		/// <example>
		/// The following example demonstrates how to set the script library path via web.config.
		/// <code>
		/// <![CDATA[
		/// <configSections>
		///    <sectionGroup name="metaBuilders.webControls">
		///       <section name="ResizeMonitor"
		///          type="System.Configuration.NameValueSectionHandler,system, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, Custom=null" />
		///    </sectionGroup>
		/// </configSections>
		/// 
		/// <metaBuilders.webControls>
		///    <ResizeMonitor>
		///       <add key="ReferenceLibraryUrl" value="/MetaBuilders_WebControls_client/ResizeMonitor/1_0/ResizeMonitor.js" />
		///    </ResizeMonitor>
		/// </metaBuilders.webControls>
		/// ]]>
		/// </code>
		/// </example>
		protected virtual Uri ReferenceLibraryUrl {
			get {
				NameValueCollection config = ConfigurationSettings.GetConfig("metaBuilders.webControls/ResizeMonitor") as NameValueCollection;
				if( config != null ) {
					if ( config["ReferenceLibraryUrl"]  != null ) {
						return new Uri(config["ReferenceLibraryUrl"].ToString());
					}
				}
				return new Uri("");
			}
		}



		private String LibraryScriptName = "MetaBuilders.WebControls.ResizeMonitor Library";
		private String StartupScriptName = "MetaBuilders.WebControls.ResizeMonitor Startup";
		private String PostbackScriptName = "MetaBuilders.WebControls.ResizeMonitor Postback";
		#endregion

		#region Implementation of IPostBackEventHandler
		void IPostBackEventHandler.RaisePostBackEvent(string eventArgument) {
			OnResize( EventArgs.Empty );
		}
		#endregion

		#region Size Tracking
		/// <summary>
		/// Overrides <see cref="Control.CreateChildControls"/>
		/// </summary>
		protected override void CreateChildControls() {
			widthHolder = new System.Web.UI.HtmlControls.HtmlInputHidden();
			widthHolder.ID = "ResizeWidth";
			this.Controls.Add(widthHolder);
			widthHolder.ServerChange += new EventHandler( widthChanged );

			heightHolder = new System.Web.UI.HtmlControls.HtmlInputHidden();
			heightHolder.ID = "ResizeHeight";
			this.Controls.Add(heightHolder);
			heightHolder.ServerChange += new EventHandler( heightChanged );
		}

		/// <summary>
		/// Handles the ServerChange event of widthHolder
		/// </summary>
		private void widthChanged( Object sender, EventArgs e ) {
			this.SetWidth( Int32.Parse(this.widthHolder.Value) );
		}

		/// <summary>
		/// Handles the ServerChange event of heightHolder
		/// </summary>
		private void heightChanged( Object sender, EventArgs e ) {
			this.SetHeight( Int32.Parse(this.heightHolder.Value) );
		}

		private System.Web.UI.HtmlControls.HtmlInputHidden widthHolder;
		private System.Web.UI.HtmlControls.HtmlInputHidden heightHolder;
		#endregion
	}
}
