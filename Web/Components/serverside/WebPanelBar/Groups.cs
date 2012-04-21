using System;

namespace Cornerstone.WebPanelBar
{
	/// <summary>
	/// WebPanelBar Groups
	/// </summary>
	public class Groups : System.Collections.CollectionBase
	{
		public Groups()
		{
			
		}

		/// <summary>
		/// Add a Group to WebPanelBar Groups
		/// </summary>
		public void Add()
		{
			List.Add(new  Group());			
		}		

		public  Group this[int index]
		{			
			get 
			{ 
				if(index >= Count || index < 0)
				{
					return null;
				}
				return ( Group) List[index];
			}
		}
	}
}
