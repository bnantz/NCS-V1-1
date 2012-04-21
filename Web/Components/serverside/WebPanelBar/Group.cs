
using System;
using System.ComponentModel;

namespace Cornerstone.WebPanelBar
{
	/// <summary>
	/// WebPanelBar Group
	/// </summary>
	public class Group
	{
		private string _Caption;
		private int _DividerHeight;
		private string _DividerColor;
		private  Items _Items;
		private  WebPanelBarGroupType _WebPanelBarGroupType;
		
		public Group()
		{
			_Items = new  Items();			
		}		

		/// <summary>
		/// Set and Get WebPanelBar Group Caption
		/// </summary>
		[Category("WebPanelBar"), Description("Set and Get WebPanelBar Group Caption")]
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
		/// it no use set in order to support old code
		/// </summary>
		public int DividerHeight
		{
			get
			{
				return this._DividerHeight;
			}
			set 
			{
				this._DividerHeight = value;
			}
		}

		/// <summary>
		/// it no use set in order to support old code
		/// </summary>
		public string DividerColor
		{
			get
			{
				return this._DividerColor;
			}
			set 
			{
				this._DividerColor = value;
			}
		}

		public  Items Items
		{
			get
			{
				return this._Items;
			}
		}		


		/// <summary>
		/// Set and Get WebPanelBar Group Type
		/// </summary>
		[Category("WebPanelBar"), Description("Set and Get WebPanelBar Group Type")]
		public  WebPanelBarGroupType WebPanelBarGroupType
		{
			get
			{
				return this._WebPanelBarGroupType;
			}
			set
			{
				this._WebPanelBarGroupType = value;
			}
		}
		
	}
}
