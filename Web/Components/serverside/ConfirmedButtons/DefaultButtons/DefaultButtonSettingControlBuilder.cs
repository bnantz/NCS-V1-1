using System;
using System.Web;
using System.Web.UI;

namespace MetaBuilders.WebControls
{
	/// <summary>
	/// The builder for a <see cref="DefaultButtonSetting"/>
	/// </summary>
	public class DefaultButtonSettingControlBuilder : ControlBuilder
	{
		/// <summary>
		/// Creates a new instance of the DefaultButtonSettingControlBuilder class.
		/// </summary>
		public DefaultButtonSettingControlBuilder()
		{
		}

		/// <summary>
		/// Overrides <see cref="ControlBuilder.AllowWhitespaceLiterals"/>
		/// </summary>
		/// <returns></returns>
		public override bool AllowWhitespaceLiterals() {
			return false;
		}
	}
}
