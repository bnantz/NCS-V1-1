
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Web.UI;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// The collection of settings for the <see cref="DefaultButtons.Settings"/> property.
	/// </summary>
	[
	Editor(typeof(System.ComponentModel.Design.CollectionEditor), typeof(UITypeEditor))
	]
	public sealed class DefaultButtonSettingCollection : IList, IStateManager {

		private ArrayList settings;
		private bool _isTrackingViewState;
		private bool _saveAll;
    
		internal DefaultButtonSettingCollection() {
			settings = new ArrayList();
		}
        
		/// <summary>
		/// The indexer for this collection.
		/// </summary>
		public DefaultButtonSetting this[int index] {
			get {
				return (DefaultButtonSetting)settings[index];
			}
		}


		// Privately implement those members of IList that take or 
		// return the object type and expose equivalent public members
		// that take or return a DefaultButtonSetting instance instead. Also
		// implement privately those members of IList that are not meaninful
		// to expose in the public object model of DefaultButtonSettingCollection.
		object IList.this[int index] {
			get {
				return settings[index];
			}
			set {
				settings[index] = (DefaultButtonSetting)value;
			}
		}
    
		/// <summary>
		/// Adds a setting to the collection
		/// </summary>
		public int Add(DefaultButtonSetting item) {
			if (item == null) {
				throw new ArgumentNullException("item");
			}

			settings.Add(item);
			if (_isTrackingViewState) {
				((IStateManager)item).TrackViewState();
				item.SetDirty();
			}

			return settings.Count - 1;
		}
        
		/// <summary>
		/// Clears the collection.
		/// </summary>
		public void Clear() {
			settings.Clear();
			if (_isTrackingViewState) {
				_saveAll = true;
			}
		}
            
		/// <summary>
		/// Determines if the collection contains the given setting.
		/// </summary>
		public bool Contains(DefaultButtonSetting item) {
			if (item == null) {
				return false;
			}
			return settings.Contains(item);
		}

		/// <summary>
		/// Gets the index of the given setting.
		/// </summary>
		public int IndexOf(DefaultButtonSetting item) {
			if (item == null) {
				throw new ArgumentNullException("item");
			}
			return settings.IndexOf(item);
		}

		/// <summary>
		/// Adds the given setting to the collection at the given index
		/// </summary>
		public void Insert(int index, DefaultButtonSetting item) {
			if (item == null) {
				throw new ArgumentNullException("item");
			}

			settings.Insert(index,item);
			if (_isTrackingViewState) {
				((IStateManager)item).TrackViewState();
				_saveAll = true;
			}
		}

		/// <summary>
		/// Removes the setting at the given setting
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index) {
			settings.RemoveAt(index);
			if (_isTrackingViewState) {
				_saveAll = true;
			}
		}
    
		/// <summary>
		/// Removes the given setting from the collection
		/// </summary>
		/// <param name="item"></param>
		public void Remove(DefaultButtonSetting item) {
			if (item == null) {
				throw new ArgumentNullException("item");
			}

			int index = IndexOf(item);
			if (index >= 0) {
				RemoveAt(index);
			}
		}

        #region IEnumerable Implementation
		/// <summary>
		/// The enumerator for this collection
		/// </summary>
		/// <returns></returns>
		public IEnumerator GetEnumerator() {
			return settings.GetEnumerator();
		}
        #endregion IEnumerable Implementation

        #region ICollection Implementation
		/// <summary>
		/// Gets the number of settings in the collection.
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public int Count {
			get {
				return settings.Count;
			}
		}

		/// <summary>
		/// copies the settings to an array
		/// </summary>
		public void CopyTo(Array array, int index) {
			settings.CopyTo(array,index);
		}

		/// <summary>
		/// returns false
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public bool IsSynchronized {
			get {
				return false;
			}
		}

		/// <summary>
		/// Gets the SyncRoot
		/// </summary>
		[
		Browsable(false),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)
		]
		public object SyncRoot {
			get {
				return this;
			}
		}
        #endregion ICollection Implementation

        #region IList Implementation
		bool IList.IsFixedSize {
			get {
				return false;
			}
		}

		bool IList.IsReadOnly {
			get {
				return false;
			}
		}

		int IList.Add(object item) {
			if (item == null) {
				throw new ArgumentNullException("item");
			}
			if (!(item is DefaultButtonSetting)) {
				throw new ArgumentException("item must be a DefaultButtonSetting");
			}

			return Add((DefaultButtonSetting)item);
		}

		void IList.Clear() {
			Clear();
		}

		bool IList.Contains(object item) {
			return Contains(item as DefaultButtonSetting);
		}

		int IList.IndexOf(object item) {
			if (item == null) {
				throw new ArgumentNullException("item");
			}
			if (!(item is DefaultButtonSetting)) {
				throw new ArgumentException("item must be a DefaultButtonSetting");
			}

			return IndexOf((DefaultButtonSetting)item);
		}

		void IList.Insert(int index, object item) {
			if (item == null) {
				throw new ArgumentNullException("item");
			}
			if (!(item is DefaultButtonSetting)) {
				throw new ArgumentException("item must be a DefaultButtonSetting");
			}

			Insert(index, (DefaultButtonSetting)item);
		}

		void IList.Remove(object item) {
			if (item == null) {
				throw new ArgumentNullException("item");
			}
			if (!(item is DefaultButtonSetting)) {
				throw new ArgumentException("item must be a DefaultButtonSetting");
			}

			Remove((DefaultButtonSetting)item);
		}

		void IList.RemoveAt(int index) {
			RemoveAt(index);
		}
        #endregion IList Implementation
       
        #region IStateManager Implementation
		bool IStateManager.IsTrackingViewState {
			get {
				return _isTrackingViewState;
			}
		}

		void IStateManager.LoadViewState(object savedState) {
			if (savedState == null) {
				return;
			}

			if (savedState is ArrayList) {
				// All items were saved.
				// Create new DefaultButtonSettings collection using view state.
				_saveAll = true;
				ArrayList states = (ArrayList) savedState;

				settings = new ArrayList(states.Count);
				for (int i = 0; i < states.Count; i++) {
					DefaultButtonSetting setting = new DefaultButtonSetting();
					Add(setting);
					((IStateManager)setting).LoadViewState(states[i]);
				}
			}
			else {
				// Load modified items.
				Pair p = (Pair) savedState;
				ArrayList indices = (ArrayList)p.First;
				ArrayList states = (ArrayList)p.Second; 

				for (int i = 0 ;  i < indices.Count; i++) {
					int index = (int)indices[i];
					if (index < this.Count) {
						((IStateManager)settings[index]).LoadViewState(states[i]);
					}
					else {
						DefaultButtonSetting setting = new DefaultButtonSetting();
						Add(setting);
						((IStateManager)setting).LoadViewState(states[i]);
					}
				}
			}
		}

		void IStateManager.TrackViewState() {
			_isTrackingViewState = true;
			foreach (DefaultButtonSetting setting in settings) {
				((IStateManager)setting).TrackViewState();
			}
		}

		object IStateManager.SaveViewState() {
			if (_saveAll == true) { 
				// Save all items.
				ArrayList states = new ArrayList(Count);
				for (int i = 0; i < Count; i++) {
					DefaultButtonSetting setting = (DefaultButtonSetting)settings[i];
					setting.SetDirty();
					states.Add(((IStateManager)setting).SaveViewState());
				}
				if (states.Count > 0) {
					return states;
				}
				else {
					return null;
				}
			}
			else { 
				// Save only the dirty items.
				ArrayList indices = new ArrayList();
				ArrayList states = new ArrayList();
             
				for (int i = 0; i < Count; i++) {
					DefaultButtonSetting setting = (DefaultButtonSetting)settings[i];
					object state = ((IStateManager)setting).SaveViewState();
					if (state != null) {
						states.Add(state);
						indices.Add(i);
					}
				}

				if (indices.Count > 0) {
					return new Pair(indices, states);
				}

				return null;
			}
		}                   
        #endregion IStateManager Implementation
	}
}
