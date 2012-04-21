using System;
using System.Reflection;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// The designer for the dialog opener controls.
	/// </summary>
	public class OpenerDesigner : System.Web.UI.Design.ControlDesigner {
		
		/// <summary>
		/// Overrides <see cref="System.Web.UI.Design.ControlDesigner.GetDesignTimeHtml"/>.
		/// </summary>
		public override string GetDesignTimeHtml() {
			PropertyInfo textProp = this.Component.GetType().GetProperty("Text");
			Boolean resetToNothing = false;
			if ( textProp != null ) {
				if ( textProp.GetValue(this.Component,null).ToString().Length == 0 ) {
					resetToNothing = true;
					PropertyInfo IDProp = this.Component.GetType().GetProperty("ID");
					String currentID = IDProp.GetValue(this.Component,null) as String;
					if ( currentID != null ) {
						textProp.SetValue(this.Component,"[" + currentID + "]",null);
					} else {
						textProp.SetValue(this.Component,"[Text]",null);
					}
				}
			}

			String result = base.GetDesignTimeHtml();

			if ( resetToNothing ) {
				textProp.SetValue(this.Component,"",null);
			}

			return result;
		}

	}
}
