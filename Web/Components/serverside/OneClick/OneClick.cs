using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.ComponentModel;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// The OneClick control allows a page developer to determine if the user has submitted a form more than once
	/// without waiting for the first submision to complete.
	/// </summary>
	/// <remarks>
	/// <p>The common use for this control is to check if a user has clicked a button more than once,
	/// while the page was still processing the first click event. This can be a problem if the click
	/// handler causes a database write to occur, or some similar process that is problematic to
	/// have happen twice.</p>
	/// <p>
	/// The OneClick control is designed to allow the same form to be subitted twice, 
	/// as long as the original request is allowed to complete.
	/// </p>
	/// </remarks>
	/// <example>
	/// The following example shows a OneClick control used on a page.
	/// <code>
	/// &lt;%@ Register tagprefix="mboc" namespace="MetaBuilders.WebControls" assembly="MetaBuilders.WebControls.OneClick" %&gt;
	/// &lt;script runat="server" language="c#" &gt;
	///	 protected void Button1_Click( Object sender, EventArgs e ) {
	///		if ( OneClick1.IsValid ) {
	///			// Code here is important to happen only once, and might take substantial amounts of time to complete
	///			// For example purposes, the thread will be put to sleep for 3 seconds.
	///			System.Threading.Thread.Sleep( TimeSpan.FromSeconds(3) );
	///		}
	/// }
	/// &lt;/script&gt;
	/// &lt;html&gt;&lt;body&gt;&lt;form runat="server" &gt;
	///		&lt;mboc:OneClick runat="server" Id="OneClick1" /&gt;
	///		&lt;asp:Button runat="server" OnClick="Button1_Click" Text="Do Something Important" /&gt;
	/// &lt;/form&gt;&lt;/body&gt;&lt;/html&gt;
	/// </code>
	/// </example>
	[
	Designer(typeof(OneClickDesigner)),
	ParseChildren(true),
	PersistChildren(false),
	]
	public class OneClick : System.Web.UI.Control {

		/// <summary>
		/// Gets a value indicating whether the user has only submitted the form once per form life cycle.
		/// </summary>
		/// <value>
		/// true if the user only submitted the form once, false if more submitals were received.
		/// </value>
		[
		Description("Gets a value indicating whether the user has only submitted the form once per form life cycle."),
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
		]
		public Boolean IsValid {
			get {
				return isValid;
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.OnInit"/>.
		/// </summary>
		protected override void OnInit(EventArgs e) {
			HttpContext context = HttpContext.Current;
			if ( context != null ) {
				oneClickID = context.Request.Form.Get(OneClick.IDkey);

				this.isValid = ( oneClickID == null || context.Cache.Get(oneClickID) == null );
				if ( this.isValid && oneClickID != null ) {
					context.Cache.Insert(oneClickID,true,null,DateTime.Now.AddSeconds(context.Server.ScriptTimeout),TimeSpan.Zero);
				}
			}

			base.OnInit (e);
		}

		/// <summary>
		/// Overrides <see cref="Control.CreateControlCollection"/>.
		/// </summary>
		protected override ControlCollection CreateControlCollection() {
			return new EmptyControlCollection(this);
		}

		/// <summary>
		/// Overrides <see cref="Control.OnPreRender"/>.
		/// </summary>
		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender (e);

			if ( this.Page != null ) {
				String newID = Guid.NewGuid().ToString();
				this.Page.RegisterHiddenField(OneClick.IDkey,newID);
				this.Page.Trace.Write("OneClick ID",newID);
			}
		}

		/// <summary>
		/// Overrides <see cref="Control.Render"/>.
		/// </summary>
		protected override void Render(HtmlTextWriter writer) {
			// No Rendering
		}


		private static readonly String IDkey = "__MetaBuilders_WebControls_OneClickID";
		private String oneClickID;
		private Boolean isValid;

	}
}
