using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;

namespace MetaBuilders.WebControls
{
	/// <summary>
	/// One mapping between a control and its default button.
	/// </summary>
	[
		ControlBuilder(typeof(DefaultButtonSettingControlBuilder)),
		TypeConverter(typeof(ExpandableObjectConverter))
	]
	public class DefaultButtonSetting : IStateManager
	{
		/// <summary>
		/// Creates a new instance of the DefaultButtonSetting class.
		/// </summary>
		public DefaultButtonSetting()
		{
		}

		/// <summary>
		/// Gets or sets the parent input control in the relationship.
		/// </summary>
		[
		NotifyParentProperty(true),
		]
		public String Parent {
			get {
				string savedState = (string)(ViewState["Parent"]);
				return (savedState == null)? String.Empty : savedState;
			}
			set {
				ViewState["Parent"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the default button in the relationship.
		/// </summary>
		[
		NotifyParentProperty(true),
		]
		public String Button {
			get {
				string savedState = (string)(ViewState["Button"]);
				return (savedState == null)? String.Empty : savedState;
			}
			set {
				ViewState["Button"] = value;
			}
		}

		/// <summary>
		/// Gets the ViewState for this setting.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		protected StateBag ViewState {
			get {
				if (_viewState == null) { 
					_viewState = new StateBag(false);
					if (_isTrackingViewState) ((IStateManager)_viewState).TrackViewState();
				}
				return _viewState;
			}
		}

		internal void SetDirty() {
			if (_viewState != null) {
				ICollection Keys = _viewState.Keys;
				foreach (string key in Keys) {
					_viewState.SetItemDirty(key, true);
				}
			}
		}


		#region IStatemanager implementation
		bool IStateManager.IsTrackingViewState {
			get {
				return _isTrackingViewState;
			}
		}

		void  IStateManager.LoadViewState(object savedState) {
			if (savedState != null) {
				((IStateManager)ViewState).LoadViewState(savedState);
			}
		}

		object IStateManager.SaveViewState() {  
			if (_viewState != null) {
				return ((IStateManager)_viewState).SaveViewState();
			}
			return null;
		}

		void  IStateManager.TrackViewState() {
			_isTrackingViewState = true;
			if (_viewState != null) {
				((IStateManager)_viewState).TrackViewState();
			}
		}
        #endregion 


		private bool _isTrackingViewState;
		private StateBag _viewState;



	}
}
