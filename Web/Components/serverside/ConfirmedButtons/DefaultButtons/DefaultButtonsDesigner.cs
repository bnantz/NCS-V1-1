using System;


namespace MetaBuilders.WebControls
{
	/// <summary>
	/// The designer for the DefaultButtons control.
	/// </summary>
	public class DefaultButtonsDesigner : System.Web.UI.Design.ControlDesigner
	{
		/// <summary>
		/// Creates a new instance of the DefaultButtonsDesigner
		/// </summary>
		public DefaultButtonsDesigner(){}

		/// <summary>
		/// Overrides <see cref="System.Web.UI.Design.ControlDesigner.GetDesignTimeHtml"/>
		/// </summary>
		public override string GetDesignTimeHtml() {
			return this.CreatePlaceHolderDesignTimeHtml();
		}


	}
}
