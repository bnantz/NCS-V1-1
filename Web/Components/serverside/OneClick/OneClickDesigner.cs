using System;
using System.Web.UI.Design;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Simple designer for <see cref="OneClick"/> which shows a placeholder.
	/// </summary>
	public class OneClickDesigner : ControlDesigner {
		
		/// <summary>
		/// Overridden to use <see cref="ControlDesigner.CreatePlaceHolderDesignTimeHtml"/>.
		/// </summary>
		/// <returns></returns>
		public override string GetDesignTimeHtml() {
			return this.CreatePlaceHolderDesignTimeHtml();
		}

	}
}
