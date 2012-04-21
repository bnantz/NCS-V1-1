using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Resources;


namespace MetaBuilders.WebControls {

	/// <summary>
	/// Allows the developer to assign default buttons to per control on a web form.
	/// </summary>
	/// <remarks>
	/// This is a non-visual control, using the IExtenderProvider to add properties to other controls.
	/// With a designer, after the DefaultButtons control is added to the page,
	/// input controls will have a new property named "DefaultButton", where the ID of a button should go.
	/// Without a designer, a page developer can simply add DefaultButtonSetting tags inside the DefaultButtons tag.
	/// </remarks>
	/// <example>
	/// Here is an example page using the DefaultButtons control.
	/// <code><![CDATA[
	///	<%@ Register tagprefix="mbdb" namespace="MetaBuilders.WebControls" Assembly="MetaBuilders.WebControls.DefaultButtons" %>
	/// <script language="C#" runat="server">
	///		protected void Smack_Click( Object sender, EventArgs e ) {
	///			Button clicked = sender as Button;
	///			if ( clicked != null ) {
	///				Message.Text = clicked.ID + " was clicked";
	///			}
	///		}
	/// </script>
	/// <html><body><form id="Form1" runat="server" >
	///		<mbdb:DefaultButtons runat="server" id="DefaultButtons1" >
	///			<mbdb:DefaultButtonSetting parent="Text1" button="Smack1" />
	///			<mbdb:DefaultButtonSetting parent="Text2" button="Smack2" />
	///		</mbdb:DefaultButtons>
	/// 	
	///		Pressing Enter while in this textbox should click "Smack1"<br />
	///		<asp:TextBox id="Text1" runat="server" /><asp:Button id="Smack1" runat="server" Text="Smack1" onclick="Smack_Click" />
	///		<br />
	///		
	///		Pressing Enter while in this textbox should click "Smack2"<br />
	///		<asp:TextBox id="Text2" runat="server" /><asp:Button id="Smack2" runat="server" Text="Smack2" onclick="Smack_Click" />
	///		<br />
	///		
	///		<asp:Label runat="server" id="Message" EnableViewState="false" />
	/// </form></body></html>
	/// ]]></code>
	/// </example>
	[
	ProvideProperty("DefaultButton",typeof(Control)), // Names the Property to show on other controls in the desinger
	ParseChildren(true,"Settings"), // makes Settings the default property
	PersistChildren(false), // persist properties, not controls
	DefaultProperty("Settings"), // Settings is the default property
	Designer(typeof(DefaultButtonsDesigner)), // the designer type
	]
	public class DefaultButtons : System.Web.UI.Control, IExtenderProvider {

		/// <summary>
		/// Creates a new instance of the DefaultButtons control.
		/// </summary>
		public DefaultButtons() {}

		#region IExtenderProvider
		/// <summary>
		/// Defines the controls which can be extended.
		/// </summary>
		/// <remarks>
		/// In general, the Page, the Form, and buttons cannot be extended.
		/// </remarks>
		Boolean IExtenderProvider.CanExtend(Object target) {
			if ( target is System.Web.UI.Page ) { return false; }
			if ( target is System.Web.UI.HtmlControls.HtmlForm ) { return false; }
			if ( target is System.Web.UI.HtmlControls.HtmlInputButton ) { return false; }
			if ( target is System.Web.UI.HtmlControls.HtmlButton ) { return false; }
			if ( target is System.Web.UI.WebControls.Button ) { return false; }
			if ( target is System.Web.UI.WebControls.LinkButton ) { return false; }
			if ( target is System.Web.UI.WebControls.ImageButton ) { return false; }

			if (target is Control && !(target is DefaultButtons)) {
				return true;
			}
			return false;
		}

		/// <summary>
		/// gets the property value for the given control
		/// </summary>
		[
		DefaultValue("")
		]
		public String GetDefaultButton(Control control) {
			foreach( DefaultButtonSetting setting in this.Settings ) {
				if ( control.ID == setting.Parent ) {
					return setting.Button;
				}
			}
			return String.Empty;
		}


		/// <summary>
		/// sets the property value for the given control
		/// </summary>
		public void SetDefaultButton(Control control, String value) {
			if (value == null) {
				value = String.Empty;
			}

			DefaultButtonSetting newSetting = null;

			// search the current settings for the control given
			foreach( DefaultButtonSetting setting in Settings ) {
				if ( setting.Parent == control.ID ) {
					newSetting = setting;
					break;
				}
			}

			if ( newSetting == null ) {

				// the setting wasn't found, so add it
				if ( value.Length > 0 ) {
					newSetting = new DefaultButtonSetting();
					newSetting.Parent = control.ID;
					newSetting.Button = value;
					this.Settings.Add( newSetting );
				}
			} else {

				// change or remove the setting as needed.
				if ( value.Length  > 0) {
					newSetting.Button = value;
				} else {
					this.Settings.Remove( newSetting );
				}
			}

			notifyDesignerOfChange();
		}

		private void notifyDesignerOfChange() {
			// Thanks to Paul Easter for this code on microsoft.public.dotnet.framework.aspnet.buildingcontrols

			// Tell the designer that the component has changed
			if ( this.Site != null && this.Site.DesignMode ) {
				try {
					IDesignerHost host = (IDesignerHost)this.Site.GetService(typeof(IDesignerHost));
					if ( host != null ) {
						IComponentChangeService changer = (IComponentChangeService)host.GetService(typeof(IComponentChangeService));
						if ( changer != null ) {
							changer.OnComponentChanged(this,null,null,null);
						}
					}
				} catch ( Exception ex ) {
					System.Diagnostics.Debug.WriteLine(ex.ToString());
				}
			}

		}

		#endregion

		#region Control Implementation
		/// <summary>
		/// Gets the collection of settings which map input controls to their default buttons.
		/// </summary>
		[
			DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
			NotifyParentProperty(true),
			PersistenceMode(PersistenceMode.InnerDefaultProperty)
		]
		public DefaultButtonSettingCollection Settings {
			get {
				if (settings == null) {
					settings = new DefaultButtonSettingCollection();
					if (IsTrackingViewState) {
						((IStateManager)settings).TrackViewState();
					}
				}
				return settings;
			}
		}


		/// <summary>
		/// Overrides <see cref="Control.Visible"/> to hide it.
		/// </summary>
		[
			Browsable(false)
		]
		public override Boolean Visible {
			get { return base.Visible; }
			set { base.Visible = value; }
		}

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPreRender(System.EventArgs e) {
			base.OnPreRender(e);
			
			if ( settings == null || settings.Count == 0 ) {
				return;
			}

			if ( PropertiesValid ) {
				this.RegisterClientScript();
			} else {
				throw new HttpException("One or more settings are invalid for '" + this.ID + "'" );
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.Render"/>.
		/// </summary>
		/// <remarks>No rendering is done for this control.</remarks>
		protected override void Render(System.Web.UI.HtmlTextWriter writer) {
		}
		#endregion

		#region Client Script

		private void RegisterClientScript() {
			Page.RegisterOnSubmitStatement("DefaultButtons","if (DefaultButton_RequireOwnPostback(this) ) { return false; }; ");
			this.RegisterScriptLibrary();
			this.RegisterArray();
			this.RegisterStartup();
		}

		private void RegisterScriptLibrary() {
			ResourceManager manager = new ResourceManager( this.GetType() );
            String script = manager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true).GetString(LibraryScript);
			this.Page.RegisterClientScriptBlock(ScriptLibraryName, script );
			this.Page.GetPostBackEventReference(this);
		}

		private void RegisterArray() {
			foreach( DefaultButtonSetting setting in this.Settings ) {
				Control parent = FindControlInContainers( setting.Parent );
				Control button = FindControlInContainers( setting.Button );
				Boolean causesValidation = DetermineCausesValidation( button );
				if ( parent != null && button != null ) {
					Page.RegisterArrayDeclaration( ArrayName, @"""" + parent.ClientID + "©" + this.Page.GetPostBackEventReference(button,"") + "©" + causesValidation.ToString() + @"""" );
				}
			}
		}


		private void RegisterStartup() {
			ResourceManager manager = new ResourceManager( this.GetType() );
			String script = manager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true).GetString(StartupScript);
			this.Page.RegisterStartupScript(ScriptInitName, script );
		}

		#endregion

		#region Property Validation
		/// <summary>
		/// Ensures that the current properties are valid.
		/// </summary>
		protected virtual Boolean PropertiesValid {
			get {
				if (!(this.propertiesChecked)) {
					this.propertiesValid = this.ControlPropertiesValid();
					this.propertiesChecked = true;
				}
				return this.propertiesValid;
			}
		}
		private Boolean propertiesChecked = false;
		private Boolean propertiesValid = false;

		/// <summary>
		/// Examines the current properties to ensure their validity.
		/// </summary>
		protected virtual bool ControlPropertiesValid() {
			foreach( DefaultButtonSetting setting in this.Settings ) {
				if ( FindControlInContainers( setting.Parent ) == null || FindControlInContainers( setting.Button ) == null ) {
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Finds the given control by id.
		/// </summary>
		private Control FindControlInContainers(String controlName) {
			return this.NamingContainer.FindControl(controlName);
		}

		/// <summary>
		/// Determines if the given default button causes validation
		/// </summary>
		private Boolean DetermineCausesValidation( Control button ) {
			System.Reflection.PropertyInfo causesVal = button.GetType().GetProperty("CausesValidation");
			if ( causesVal != null ) {
				return (Boolean)causesVal.GetValue(button, null);
			} else {
				return false;
			}
		}

		#endregion

		#region Custom State Management Implementation
		/// <summary>
		/// Overrides <see cref="Control.LoadViewState"/>
		/// </summary>
		protected override void LoadViewState(object savedState) {
			object baseState = null;
			object[] myState = null;

			if (savedState != null) {
				myState = (object[])savedState;
				if (myState.Length != 2) {
					throw new ArgumentException("Invalid view state");
				}

				baseState = myState[0];
			}

			base.LoadViewState(baseState);

			if ((myState != null) && (myState[1] != null)) {
				((IStateManager)Settings).LoadViewState(myState[1]);
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.SaveViewState"/>
		/// </summary>
		protected override object SaveViewState() {
			object baseState = base.SaveViewState();
			object settingsState = null;
            
			if ((settings != null) && (settings.Count > 0)) {
				settingsState = ((IStateManager)settings).SaveViewState();
			}

			if ((baseState != null) || (settingsState != null)) {
				object[] savedState = new object[2];
				savedState[0] = baseState;
				savedState[1] = settingsState;

				return savedState;
			}

			return null;
		}

		/// <summary>
		/// Overrides <see cref="Control.TrackViewState"/>
		/// </summary>
		protected override void TrackViewState() {
			base.TrackViewState();
			if (settings != null) {
				((IStateManager)settings).TrackViewState();
			}
		}

        #endregion Custom State Management Implementation


		#region Private
		private DefaultButtonSettingCollection settings;

		private static readonly String LibraryScript = "Library Script";
		private static readonly String StartupScript = "Startup Script";
		private static readonly String ScriptLibraryName = "MetaBuilders.WebControls.DefaultButtons Library";
		private static readonly String ScriptInitName = "MetaBuilders.WebControls.DefaultButtons Init";
		private static readonly String ArrayName = "DefaultButtons";
		#endregion
	}
}
