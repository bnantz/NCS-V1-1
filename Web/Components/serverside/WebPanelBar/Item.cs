
using System;
using System.ComponentModel;

namespace Cornerstone.WebPanelBar
{
	/// <summary>
	/// WebPanelar Item
	/// </summary>
	public class Item
	{
		public Item()
		{
			
		}

		private string _Caption;
		private Uri _Url;
		private  WebPanelBarItemType _WebPanelBarItemType;
		private  Group _Group;
		
		/// <summary>
		/// Set and Get WebPanelBar Item Caption
		/// </summary>
		[Category("WebPanelBar"), Description("Set and Get WebPanelBar Item Caption")]
		public string Caption
		{
			get
			{
				return this._Caption;
			}
			set
			{
				this._Caption = value;
			}
		}

		/// <summary>
		/// Set and Get WebPanelBar Item Url
		/// </summary>
		[Category("WebPanelBar"), Description("Set and Get WebPanelBar Item Url")]
		public Uri Url
		{
			get
			{
				return this._Url;
			}
			set 
			{
				this._Url = value;
			}
		}

		/// <summary>
		/// Set and Get WebPanelBar Item Type
		/// </summary>
		[Category("WebPanelBar"), Description("Set and Get WebPanelBar Item Type")]
		public  WebPanelBarItemType WebPanelBarItemType
		{
			get
			{
				return this._WebPanelBarItemType;
			}
			set
			{
				this._WebPanelBarItemType = value;
			}
		}

		public  Group Group
		{
			get
			{
				return this._Group;
			}
			set
			{
				this._Group = value;
			}
		}

		/// <summary>
		/// Add submenu to item
		/// </summary>		
		public void AddGroup()
		{
			this.Group = new  Group();
		}

	}
}
