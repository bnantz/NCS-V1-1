using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Collections.Specialized;

namespace NCS.Web.Components.Serverside
{
	public class DropDownColorPicker : System.Web.UI.WebControls.DropDownList, IPostBackDataHandler
	{
		private bool _addedNewElements = false;

		#region EVENT NewItemsAdded
		public event EventHandler NewItemsAdded;
		#endregion

		#region PROPERTY EnableClientInsert
		public bool EnableClientInsert
		{
			get 
			{
				object o = ViewState["EnableClientInsert"];
				if (o == null)
					return false;
				return (bool) o;
			}
			set 
			{
				ViewState["EnableClientInsert"] = value;
				Attributes["Modifiable"] = value.ToString().ToLower();
				if (value)
					Style["behavior"] = "url(DropDownColorPicker.htc)"; 
			}
		}
		#endregion

		#region INTERNAL PROPERTY HiddenFieldInternal
		// *****************************************************************************
		// PROPERTY: HiddenFieldInternal
		// Gets the name of the hidden field used to carry client info
		protected string HiddenFieldInternal
		{
			get { return ClientID + "_My"; }
		}
		#endregion

		#region EVENT PreRender
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			// Register the hidden fields to communicate with the client
			Page.RegisterHiddenField(HiddenFieldInternal, "");
		}
		#endregion

		#region METHOD OnNewItemsAdded
		protected virtual void OnNewItemsAdded(EventArgs e)
		{
			if (NewItemsAdded != null)
				NewItemsAdded(this, e);
		}
		#endregion


		#region IPostBackDataHandler Members

		void IPostBackDataHandler.RaisePostDataChangedEvent()
		{
			OnSelectedIndexChanged(EventArgs.Empty);

			// Consider another custom event for added elements
			if (_addedNewElements)
				OnNewItemsAdded(EventArgs.Empty);
		}

		bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
		{
			bool returnValue = false;

			// Do as the base class does
			string selectedText = postCollection[postDataKey];
			if (selectedText != null)
			{
				ListItem item = Items.FindByText(selectedText);
				int index = Items.IndexOf(item);
				if (SelectedIndex != index)
				{
					// Change occurred
					SelectedIndex = index;
					returnValue = true;
				}
			}


			// Handle the additional hidden field
			string addedElements = postCollection[HiddenFieldInternal];
			if (addedElements != null)
			{
				string[] rgElements = addedElements.Split(','); 
				if (rgElements != null)
				{
					for (int i=0; i<rgElements.Length; i++)
					{
						string buf = rgElements[i];
						string[] rgTokens = buf.Split('|');
						string text = rgTokens[0];
						string id = rgTokens[1];
						ListItem item = new ListItem(text, id);
						Items.Add(item);
					}
					returnValue = true;
					_addedNewElements = true;
				}
			}

			return returnValue;
		}

		#endregion
	}
}
