using System;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// The designer for DialogWindow controls.
	/// </summary>
	public class DialogDesigner : System.Web.UI.Design.ControlDesigner {
		
		/// <summary>
		/// Overrides <see cref="System.Web.UI.Design.ControlDesigner.GetDesignTimeHtml"/>.
		/// </summary>
		public override string GetDesignTimeHtml() {
			return this.CreatePlaceHolderDesignTimeHtml();
		}

	}
}
