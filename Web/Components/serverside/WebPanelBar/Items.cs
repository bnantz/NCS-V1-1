using System;

namespace Cornerstone.WebPanelBar
{
	/// <summary>
	/// WebPanelBar Items
	/// </summary>
	public class Items : System.Collections.CollectionBase
	{
		
		public Items()
		{
		}
				
		/// <summary>
		/// Add a Item to WebPanelBar Items
		/// </summary>
		public void Add()
		{
			List.Add(new  Item());
		}

		public  Item this[int Index]
		{
			get
			{
				return ( Item)List[Index];
			}
		}

	}
}
