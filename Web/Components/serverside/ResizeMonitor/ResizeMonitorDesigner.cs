using System;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Provides the Designer for the ResizeMonitor control.
	/// </summary>
	public class ResizeMonitorDesigner : System.Web.UI.Design.ControlDesigner {
		/// <summary>
		/// Creates a new instance of the ResizeMontiorDesigner.
		/// </summary>
		public ResizeMonitorDesigner() : base() {}

		/// <summary>
		/// Always creates a PlaceHolder style representation.
		/// </summary>
		/// <returns></returns>
		public override string GetDesignTimeHtml() {
			return this.CreatePlaceHolderDesignTimeHtml();
		}

		/// <summary>
		/// Always returns false.
		/// </summary>
		public override bool AllowResize {
			get {
				return false;
			}
		}

		
	}
}
