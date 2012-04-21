using System;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Button configurations used by the <see cref="DialogMessageBox"/> control.
	/// </summary>
	public enum DialogMessageBoxButton {
		/// <summary>
		/// Displays an OK button
		/// </summary>
		OK = 0,
		/// <summary>
		/// Displays OK and Cancel buttons
		/// </summary>
		OKCancel = 1,
		/// <summary>
		/// Displays Retry and Cancel buttons
		/// </summary>
		RetryCancel = 2,
		/// <summary>
		/// Displays Yes and No buttons
		/// </summary>
		YesNo = 3,
		/// <summary>
		/// Displays Yes, No, and Cancel buttons
		/// </summary>
		YesNoCancel = 4
	}
}
