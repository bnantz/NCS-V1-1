using System;
using System.Web.UI.Design;

namespace MetaBuilders.WebControls {
	/// <summary>
	/// The Designer for the <see cref="FirstFocus"/> control.
	/// </summary>
	public class FirstFocusDesigner : System.Web.UI.Design.ControlDesigner {
		
		/// <summary>
		/// Overridden to create a placeholder.
		/// </summary>
		/// <returns></returns>
		public override string GetDesignTimeHtml() {
			return this.CreatePlaceHolderDesignTimeHtml();
		}

		/// <summary>
		/// Overridden to disable resizing.
		/// </summary>
		public override bool AllowResize {
			get {
				return false;
			}
		}

	}
}
