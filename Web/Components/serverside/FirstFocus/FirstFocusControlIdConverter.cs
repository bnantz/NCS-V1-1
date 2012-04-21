using System;
using System.ComponentModel;
using System.Collections;
using System.Web.UI;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// The TypeConverter for the <see cref="FirstFocus.ControlToFocus"/> property.
	/// </summary>
	public class FirstFocusControlIdConverter : StringConverter {

		/// <summary>
		/// Gets the list of controls in the container.
		/// </summary>
		/// <param name="container">The <see cref="IContainer"/> to search in for controls.</param>
		/// <returns>An array of <see cref="Control"/> ID <see cref="String"/>.</returns>
		protected virtual Object[] GetControls( IContainer container ) {
			ArrayList standardValues = new ArrayList();
			foreach( IComponent component in container.Components ) {
				Control control = component as Control;
				if ( control != null && !(control is Page) && !(control is FirstFocus) && !(control is UserControl)) {
					standardValues.Add(String.Copy(control.ID));
				}
			}
			standardValues.Sort();
			return standardValues.ToArray();
		}

		/// <summary>
		/// Overrides <see cref="TypeConverter.GetStandardValues"/>.
		/// </summary>
		/// <returns>Returns an array of <see cref="Control"/> ID <see cref="String"/>.</returns>
		public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context) {
			if (context == null || context.Container == null) {
				return null;
			}
			object[] standardValues = this.GetControls(context.Container);
			if (standardValues != null) {
				return new StandardValuesCollection(standardValues);
			}
			return null;
		}

		/// <summary>
		/// Overrides <see cref="TypeConverter.GetStandardValuesSupported"/>.
		/// </summary>
		/// <returns>Returns true.</returns>
		public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context) {
			return true;
		}

		/// <summary>
		/// Overrides <see cref="TypeConverter.GetStandardValuesExclusive"/>.
		/// </summary>
		/// <returns>Returns false.</returns>
		public override bool GetStandardValuesExclusive(System.ComponentModel.ITypeDescriptorContext context) {
			return false;
		}
	}
}
