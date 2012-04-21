using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Design;
using System.Web.UI.Design.WebControls;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// The designer for the <see cref="ComboBox"/>.
	/// </summary>
	public class ComboBoxDesigner: ListControlDesigner {

		/// <summary>
		/// Overrides <see cref="ListControlDesigner.GetDesignTimeHtml"/>.
		/// </summary>
		public override String GetDesignTimeHtml() {
			try {
				combo.TextControl.Visible = false;
				combo.TextControl.BackColor = combo.BackColor;
				combo.ContainerControl.Visible = false;

				Int32 oldSelectedIndex = -1;
				if ( combo.Text.Length > 0 ) {
					oldSelectedIndex = combo.SelectedIndex;
					combo.Items.Insert( 0, new ListItem( combo.Text, combo.Text ) );
					combo.SelectedIndex = 0;
				}
				String result = base.GetDesignTimeHtml();
				if ( combo.Text.Length  > 0 ) {
					combo.Items.RemoveAt(0);
					combo.SelectedIndex = oldSelectedIndex;
				}
				
				combo.TextControl.Visible = true;
				combo.ContainerControl.Visible = true;
				return result;
			} catch ( Exception  ex ) {
				return this.GetErrorDesignTimeHtml(ex);
			}
		}

		/// <summary>
		/// Overrides <see cref="ListControlDesigner.Initialize"/>.
		/// </summary>
		public override void Initialize(System.ComponentModel.IComponent component) {
			base.Initialize( component );
			this.combo = (ComboBox)component;
		}

			/// <summary>
		/// The ComboBox for this designer.
		/// </summary>
		private ComboBox combo;
	}
}
