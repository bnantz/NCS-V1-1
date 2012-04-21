using System;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// The signature for a method handling the <see cref="DialogWindowBase.DialogClosed"/> event.
	/// </summary>
	public delegate void DialogResultEventHandler( Object sender, DialogResultEventArgs e );
	
	/// <summary>
	/// The <see cref="EventArgs"/> for the <see cref="DialogWindowBase.DialogClosed"/> event.
	/// </summary>
	public class DialogResultEventArgs : EventArgs {
		
		/// <summary>
		/// Instnatiates a <see cref="DialogResultEventArgs"/> with the given results.
		/// </summary>
		/// <param name="results"></param>
		public DialogResultEventArgs(String results) {
			this.results = results;
		}

		/// <summary>
		/// Gets the results for the event.
		/// </summary>
		public String Results {
			get {
				return results;
			}
		}
		private String results;
	}

}
