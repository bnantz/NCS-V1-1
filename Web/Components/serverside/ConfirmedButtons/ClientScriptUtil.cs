using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web;
using System.Resources;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Provides clientscript services for the ConfirmedButton controls.
	/// </summary>
	/// <remarks>
	/// This suite of controls uses client-side script to perform its behavior.
	/// By default, the controls emit the script library directly into the page.
	/// However, by placing the script file in a folder and setting a value in web.config,
	/// the script file can be referenced instead. This will save server bandwidth,
	/// as the browser should cache the file seperately.
	/// </remarks>
	/// <example>
	/// The following example demonstrates how to set the script library path via web.config.
	/// <code>
	/// <![CDATA[
	/// <configSections>
	///    <sectionGroup name="metaBuilders.webControls">
	///       <section name="confirmationScript"
	///          type="System.Configuration.NameValueSectionHandler,system, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, Custom=null" />
	///    </sectionGroup>
	/// </configSections>
	/// 
	/// <metaBuilders.webControls>
	///    <confirmationScript>
	///       <add key="ReferenceLibraryUrl" value="/MetaBuilders_WebControls_client/Confirmation/2_0/WebUIConfirmation.js" />
	///    </confirmationScript>
	/// </metaBuilders.webControls>
	/// ]]>
	/// </code>
	/// </example>
	public sealed class ClientScriptUtil {
		private ClientScriptUtil() {}

		/// <summary>
		/// Gets a boolean value indicating whether an external script library should be used,
		/// instead of the library being emitted as a whole.
		/// </summary>
		private static bool UseReferenceLibrary {
			get {
				return ( ReferenceLibraryUrl.Length > 0 );
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
		///       <section name="confirmationScript"
		///          type="System.Configuration.NameValueSectionHandler,system, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, Custom=null" />
		///    </sectionGroup>
		/// </configSections>
		/// 
		/// <metaBuilders.webControls>
		///    <confirmationScript>
		///       <add key="ReferenceLibraryUrl" value="/MetaBuilders_WebControls_client/Confirmation/2_0/WebUIConfirmation.js" />
		///    </confirmationScript>
		/// </metaBuilders.webControls>
		/// ]]>
		/// </code>
		/// </example>
		private static String ReferenceLibraryUrl {
			get {
				NameValueCollection config = ConfigurationSettings.GetConfig("metaBuilders.webControls/confirmationScript") as NameValueCollection;
				if( config != null ) {
					if ( config["ReferenceLibraryUrl"]  != null ) {
						return config["ReferenceLibraryUrl"];
					}
				}
				return String.Empty;
			}
		}

		/// <summary>
		/// The library script for the confirmed button controls.
		/// </summary>
		public static String LibraryScript {
			get {
				if( UseReferenceLibrary ) {
					return "<script language=\"javascript\" src=\"" + ReferenceLibraryUrl + "\"></script>";
				} else {
					ResourceManager manager = new ResourceManager( typeof(ClientScriptUtil) );
					return manager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true).GetString("LibraryScript");
				}
			}
		}

		/// <summary>
		/// Determines if the current request should use the use the full script, or a simpler script for downlevel browsers.
		/// </summary>
		public static Boolean RenderUplevel {
			get {
				if (HttpContext.Current == null || HttpContext.Current.Request == null) {
					return false;
				}
				HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
			
				// always render uplevel if it would interfere with validation
				if (browser.MSDomVersion.Major >= 4) { 
					return ( browser.EcmaScriptVersion.CompareTo(new Version(1, 2)) < 0 ) == false;
				}

				// w3c dom compliancy should be enough
				if ( browser.W3CDomVersion.Major > 0 ) { 
					return true;
				}

				// mozilla is good too
				if ( browser.Browser.ToLower() == "netscape" && browser.MajorVersion >= 5 ) {
					return true;
				}
			
				return false;
			}
		}

		/// <summary>
		/// The startup script for the confirmed button controls.
		/// </summary>
		public static String StartupScript {
			get {
				ResourceManager manager = new ResourceManager( typeof(ClientScriptUtil) );
				return manager.GetResourceSet(System.Globalization.CultureInfo.CurrentCulture, true, true).GetString("StartupScript");
			}
		}

		/// <summary>
		/// The name of the Library Script
		/// </summary>
		public static String LibraryScriptName {
			get { return "MetaBuilders.WebControls.ConfirmedButtons Library"; }
		}

		/// <summary>
		/// The name of the Startup Script
		/// </summary>
		public static String StartupScriptName {
			get { return "MetaBuilders.WebControls.ConfirmedButtons Startup"; }
		}

		/// <summary>
		/// The name of the page array
		/// </summary>
		public static String ArrayName {
			get { return "Page_Confirmations"; }
		}

	}
}
