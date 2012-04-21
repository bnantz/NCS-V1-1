using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Xml;

namespace Cornerstone.WebPanelBar
{
	/// <summary>
	/// WebPanelBar
	/// </summary>
	[DefaultProperty("ButtonBackgroundColor"), 
	ToolboxData("<{0}:WebPanelBar runat=server></{0}:WebPanelBar>")]
	public class WebPanelBarNav : System.Web.UI.WebControls.WebControl
	{
		private Color _ButtonBackgroundColor;
		private Uri _UpImageUrl;
		private Uri _DownImageUrl;
		private Uri _LeftBackgroundCornerImageUrl;
		private Uri _RightBackgroundCornerImageUrl;
		private Unit _Width;
		private  Groups _Groups;
		private string _FlyoutImagePath;
		private Uri _FlyoutHtcUrl;
		
		public WebPanelBarNav() : base()
		{
			_Groups = new  Groups();			
		}

		/// <summary>
		/// Set and Get group title background color
		/// </summary>
		[Bindable(false), Category("WebPanelBar Appearance"), Description("Set and Get group title background color")] 
		public Color ButtonBackgroundColor 
		{
			get
			{
				return _ButtonBackgroundColor;
			}

			set
			{
				_ButtonBackgroundColor = value;
			}
		}

		/// <summary>
		/// Set and Get up image URL
		/// </summary>
		[Category("WebPanelBar Appearance"), DefaultValue(""), Description("Set and Get up image URL")] 
		public Uri UpImageUrl
		{
			get
			{
				return this._UpImageUrl;
			}
			set
			{
				this._UpImageUrl = value;
			}
		}

		/// <summary>
		/// Set and Get down image URL
		/// </summary>
		[Bindable(false), Category("WebPanelBar Appearance"), DefaultValue(""), Description("Set and Get down image URL")] 
		public Uri DownImageUrl
		{
			get
			{
				return this._DownImageUrl;
			}
			set
			{
				this._DownImageUrl = value;
			}
		}

		/// <summary>
		/// Set and Get group left background image URL
		/// </summary>
		[Bindable(false), Category("WebPanelBar Appearance"), DefaultValue(""), Description("Set and Get group left background image URL")] 
		public Uri LeftBackgroundCornerImageUrl
		{
			get
			{
				return this._LeftBackgroundCornerImageUrl;
			}
			set
			{
				this._LeftBackgroundCornerImageUrl = value;
			}
		}

		/// <summary>
		/// Set and Get group right background image URL
		/// </summary>		
		[Bindable(false), Category("WebPanelBar Appearance"), DefaultValue(""), Description("Set and Get group right background image URL")] 
		public Uri RightBackgroundCornerImageUrl
		{
			get
			{
				return this._RightBackgroundCornerImageUrl;
			}
			set
			{
				this._RightBackgroundCornerImageUrl = value;
			}
		}

		/// <summary>
		/// Set and Get PanelBar width
		/// </summary>
		[Bindable(false), Category("WebPanelBar Appearance"), DefaultValue("150px"), Description("Set and Get PanelBar width")] 
		public override Unit Width
		{
			get
			{
				return this._Width;
			}
			set
			{
				this._Width = value;
			}
		}

		/// <summary>
		/// Set and Get WebPanelBar groups.
		/// </summary>
		[Description("Get WebPanelBar groups.")]
		public  Groups Groups
		{
			get
			{
				return this._Groups;
			}
		}

		/// <summary>
		/// Set and Get Flyout Menu images file path.
		/// </summary>
		[Category("Flyout Menu Setting"), Description("Set and Get Flyout Menu images file path.")]
		public string FlyoutImagePath
		{
			get
			{
				return this._FlyoutImagePath;
			}
			set
			{
				this._FlyoutImagePath = value;
			}
		}

		/// <summary>
		/// Set and Get Flyout htc file URL.
		/// </summary>
		[Category("Flyout Menu Setting"), Description("Set and Get Flyout htc file URL.")]
		public Uri FlyoutHtcUrl
		{
			get
			{
				return this._FlyoutHtcUrl;
			}
			set
			{
				this._FlyoutHtcUrl = value;
			}
		}

		

		/// <summary> 
		/// out put html
		/// </summary>
		/// <param name="output"> HTML Writer </param>
		protected override void Render(HtmlTextWriter output)
		{
			
			StringBuilder sb = new StringBuilder();
			sb.Append("\n<!--/////////////////////////////////////////////////////////////////////////-->\n");
			sb.Append("<!--///                   WebPanelBar for ASP.Net                        ////-->\n");
			sb.Append("<!--///                   v2.0.0.2 write by yanhao                         ////-->\n");
			sb.Append("<!--/////////////////////////////////////////////////////////////////////////-->\n");
			sb.Append("<style>\n");
			sb.Append("  .ListNugget { margin-top: 12px; font-size: 10px; }\n");
			sb.Append("  .ListNuggetBody { padding-top: 2px;  padding-left: 4px; padding-bottom: 2px; border: " + RGBToString(this.ButtonBackgroundColor) + " 1px solid; font-family: ËÎÌו; background-color: #f1f1f1; width: " + this.Width + "; }\n");
			sb.Append("  .ListNuggetHeader {background-color: " + RGBToString(this.ButtonBackgroundColor) + "; color: White; cursor: hand;}\n");
			sb.Append("  .ListNuggetTitleCellBlue {background-image: url(" + this.LeftBackgroundCornerImageUrl + "); background-position: left top; background-repeat: no-repeat; padding-left: 8px; padding-top: 2px; padding-bottom: 2px;}\n");
			sb.Append("  .ListNuggetHeaderClosed { background-color: #CCCCCC; color: Black; cursor: hand;}\n");
			sb.Append("  .ListNuggetButtonCellBlue { background-image: url(" + this.RightBackgroundCornerImageUrl + "); background-position: right top; background-repeat: no-repeat; text-align: right; cursor: hand;}\n");
			sb.Append("  .ListNuggetButton	{ margin-right: 4px; margin-left: 2px; margin-top: 2px; margin-bottom: 2px; }\n");
			sb.Append("  .ListNuggetUpButton {}\n");			sb.Append("  .ListNuggetDownButton	{display: none; }\n");
			sb.Append("  /* FLYOUTMENU - START */\n");
			sb.Append("	.flyoutMenu \n");
			sb.Append("	{\n");
			sb.Append("		background-color: #F2F2F2;\n");
			sb.Append("		width: 98%;\n");
			sb.Append("		behavior: url(" + this.FlyoutHtcUrl + "); \n");
			sb.Append("	}\n");
			
			sb.Append("	.UpLevelFlyout \n");
			sb.Append("	{\n");
			sb.Append("		margin-right: 8px;\n");
			sb.Append("	}\n");
			sb.Append("	.UpLevelFlyout .flyoutMenu \n");
			sb.Append("	{\n");
			sb.Append("		margin-right: -8px;margin-left: -2px; \n");
			sb.Append("	}\n");

			sb.Append("	.DownLevelFlyout .flyoutMenu \n");
			sb.Append("	{\n");
			sb.Append("		margin-right: 0px;\n");
			sb.Append("	}\n");


			sb.Append("	.flyoutMenu UL \n");
			sb.Append("	{\n");
			sb.Append("		padding-left: 0px;\n");
			sb.Append("		\n");
			sb.Append("	}\n");
			sb.Append("	.flyoutMenu LI \n");
			sb.Append("	{\n");
			sb.Append("		margin-left: 0px;\n");
			sb.Append("		padding-top: 3px;\n");
			sb.Append("		padding-bottom: 3px;\n");
			sb.Append("	}\n");
			sb.Append("	.flyoutMenu A:visited { color: Purple; }\n");


			sb.Append("	.flyoutHeading {\n");
			sb.Append("		background-color: #F2F2F2;\n");
			sb.Append("		font-size: 12px;\n");
			sb.Append("		font-weight: bold;\n");
			sb.Append("		padding-top: 2px;\n");
			sb.Append("		padding-bottom: 4px;\n");
			sb.Append("		padding-left: 2px;\n");
			sb.Append("		cursor: default;\n");
			sb.Append("	}\n");
			sb.Append("	.flyoutLink  {\n");
			sb.Append("		border-color: #F2F2F2;\n");
			sb.Append("		border-width: 1px;\n");
			sb.Append("		border-style: solid;\n");
			sb.Append("		font-size: 12px;\n");
			sb.Append("		padding-left: 2px;\n");
			sb.Append("		padding-right: 12px;\n");
			sb.Append("		padding-top: 3px;\n");
			sb.Append("		cursor: hand;\n");
			sb.Append("	}\n");

			sb.Append("	.flyoutLink A { margin-right: 14px; }\n");

			sb.Append("	.flyoutMenu TD.flyoutLink  \n");
			sb.Append("	{\n");
			sb.Append("		font-size: 12px;\n");
			sb.Append("		padding-top: 3px; padding-left: 0px;\n");
			sb.Append("		padding-bottom: 3px; padding-right: 0px;\n");
			sb.Append("		cursor: hand;\n");
			sb.Append("	}\n");

			sb.Append("	TD.flyoutLink a, \n");
			sb.Append("	TD.flyoutLink a:hover,\n");
			sb.Append("	TD.flyoutLink a:visited,\n");
			sb.Append("	TD.flyoutLink a:active \n");
			sb.Append("	{\n");
			sb.Append("		color: black;\n");
			sb.Append("		font-size: 12px;\n");
			sb.Append("		text-decoration: none;\n");
			sb.Append("	}	\n");

			sb.Append("	TD.flyoutSubLink a,\n");
			sb.Append("	TD.flyoutSubLink a:hover,\n");
			sb.Append("	TD.flyoutSubLink a:visited,\n");
			sb.Append("	TD.flyoutSubLink a:active \n");
			sb.Append("	{\n");
			sb.Append("		font-size: 12px;\n");
			sb.Append("		color: black;\n");
			sb.Append("		text-decoration: none;\n");
			sb.Append("	}\n");

			sb.Append("	.flyoutSubHeading {\n");
			sb.Append("		font-size: 12px;\n");
			sb.Append("		background-color: #F2F2F2;\n");
			sb.Append("		font-weight: bold;\n");
			sb.Append("		padding-top: 2px;\n");
			sb.Append("		padding-bottom: 4px;\n");
			sb.Append("		padding-left: 7px;\n");
			sb.Append("		cursor: default;\n");
			sb.Append("	}\n");

			sb.Append("	.flyoutSubLink {\n");
			sb.Append("		border-color: #F2F2F2;\n");
			sb.Append("		border-width: 1px;\n");
			sb.Append("		border-style: solid;\n");
			sb.Append("		padding: 2px 15px 4px 12px;\n");
			sb.Append("		cursor: hand;\n");
			sb.Append("	}\n");
			sb.Append("	/* FLYOUTMENU - END */\n");
			sb.Append("</style>\n");

			sb.Append("<script language='JavaScript'>");
			sb.Append("function PartWrapperToggle(elementName) {\n");
			sb.Append("	var HeaderElement = null;\n");
			sb.Append("	var BodyElement = null; \n");
			sb.Append("	if(document.getElementsByName) {\n");
			sb.Append("		HeaderElement = document.getElementsByName(elementName+\"Header\");\n");
			sb.Append("		BodyElement = document.getElementsByName(elementName+\"Body\");\n");
			sb.Append("		UpImage = document.getElementsByName(elementName+\"Up\");\n");
			sb.Append("		DownImage = document.getElementsByName(elementName+\"Down\");\n");
			sb.Append("		if(BodyElement) {	\n");
			sb.Append("			if(BodyElement[0].style.display == \"none\") {\n");
			sb.Append("				BodyElement[0].style.display = \"block\";\n");
			sb.Append("				HeaderElement[0].className = \"ListNuggetHeader\";\n");
			sb.Append("				DownImage[0].style.display = \"none\";\n");
			sb.Append("				UpImage[0].style.display = \"block\";\n");
			sb.Append("			} else {\n");
			sb.Append("				BodyElement[0].style.display = \"none\";\n");
			sb.Append("				HeaderElement[0].className = \"ListNuggetHeaderClosed\";\n");
			sb.Append("				UpImage[0].style.display = \"none\";\n");
			sb.Append("				DownImage[0].style.display = \"block\";\n");
			sb.Append("			}\n");
			sb.Append("		}	\n");
			sb.Append("	}\n");
			sb.Append("	window.event.cancelBubble = true;\n");
			sb.Append("	return false;\n");
			sb.Append("}\n");
			sb.Append("</script>\n");
			
			int i = 0;
			int j = 0;
			if (this.Groups.Count != 0)
			{
				foreach ( Group gr in this.Groups)
				{
					sb.Append("<div class='ListNugget'>");
					sb.Append("<Table id=\"GRP" + i.ToString() + "Header\" width='" + this.Width + "' class='ListNuggetHeader' border='0' cellspacing='0' cellpadding='0'>\n");
					sb.Append("	<tr>\n");
					sb.Append("	  <td class='ListNuggetTitleCellBlue' onclick=\"PartWrapperToggle('GRP" + i.ToString() + "');\">\n");
					sb.Append("			<b>" + gr.Caption + "</b>\n");
					sb.Append("   </td>\n");
					sb.Append("	  <td class='ListNuggetButtonCellBlue' onclick=\"PartWrapperToggle('GRP" + i.ToString() + "');\">\n");
					sb.Append("     <div class='ListNuggetButton'>\n");
					sb.Append("       <img class='ListNuggetUpButton' id=\"GRP" + i.ToString() + "Up\" src='" + this.UpImageUrl + "' align=\"right\" BORDER=\"0\" ALT=\"hide option\">\n");
					sb.Append("       <img class='ListNuggetDownButton' id=\"GRP" + i.ToString() + "Down\" src='" + this.DownImageUrl + "' align=\"right\" BORDER=\"0\" ALT=\"option\">\n");
					sb.Append("     </div>");
					sb.Append("   </td>\n");
					sb.Append("  </tr>\n");
					sb.Append("</Table>\n");
				
					sb.Append("<div id=\"GRP" + i.ToString() + "Body\" class='ListNuggetBody'>\n");
				
					switch (gr.WebPanelBarGroupType)
					{
						case  WebPanelBarGroupType.Normal:

							sb.Append("   <Table  BORDER=\"0\" CELLSPACING=\"0\" CELLPADDING=\"3\" width=\"" + Convert.ToString(this.Width.Value - 10) + "\">\n");

							foreach ( Item it in this.Groups[i].Items)
							{
								sb.Append("<tr>\n");
								sb.Append("  <td>\n");
								sb.Append("		<a href=\"" + it.Url + "\">" + it.Caption + "</a>\n");
								sb.Append("  </td>\n");
								sb.Append("</tr>\n");
							}

							sb.Append("   </Table>\n");
							sb.Append("</div>\n");
							sb.Append("</div>\n");
							break;

						case  WebPanelBarGroupType.FlyOut:							

							sb.Append(" <script language=jscript>\n");
							sb.Append("         var IMAGES = \"" + this.FlyoutImagePath + "\";\n");
							sb.Append("			var menudata = \"" + BuildMenuXMLDataCache().Replace("\"", "'")  + "\";\n");
							sb.Append(" </script>\n");
							
							sb.Append("   <Table class=\"flyoutMenu\" BORDER=\"0\" CELLSPACING=\"1\" CELLPADDING=\"0\" width=\"" + Convert.ToString(this.Width.Value - 10) + "\" >\n");
							sb.Append("<tr>\n");
							sb.Append("  <td>\n");
							sb.Append("     <table width=\"100%\" cellpadding=\"0\" cellspacing=\"1\" border=\"0\">");

							j = 0;
							foreach ( Item it in this.Groups[i].Items)
							{
								switch (it.WebPanelBarItemType)
								{
									case  WebPanelBarItemType.FlyoutLink:
										sb.Append("		<tr>\n");
										sb.Append("			<td class=\"flyoutLink\" handle=\"GPR" + i.ToString() + j.ToString() + "\">\n");
										sb.Append("				<a href=\"" + it.Url + "\">" + it.Caption + "</a>\n");
										sb.Append("			 </td>\n");
										sb.Append("		</tr>\n");
										break;

									case  WebPanelBarItemType.FlyoutNormalLink:

										sb.Append("		<tr>\n");
										sb.Append("			<td class=\"flyoutLink\" handle=\"\">\n");
										sb.Append("				<a href=\"" + it.Url + "\">" + it.Caption + "</a>\n");
										sb.Append("			 </td>\n");
										sb.Append("		</tr>\n");
										break;
								}
								
								j++;
							}
							sb.Append("    </table>");
							sb.Append("  </td>\n");
							sb.Append("</tr>\n");
							sb.Append("</Table>\n");
							sb.Append("</div>\n");
							sb.Append("</div>\n");
							
							break;
					}
					i++;
										
				}
				sb.Append("<!--///                   end of WebPanelBar                               ////-->\n");
			
				output.Write(sb.ToString());
			}
		}

		private string RGBToString(Color clrRGB)
		{
			return "RGB(" + clrRGB.R + ", " + clrRGB.G + ", " + clrRGB.B + ")";
		}

		private string BuildMenuXMLDataCache()
		{
			int i = 0;
			int j = 0;
			int k = 0;
			System.Xml.XmlDocument xd = new XmlDocument();
			if (this.Groups.Count != 0)
			{
				xd.LoadXml("<menu xmlns:fhns='urn:schemas-microsoft-com/PSS/FHXML2' xmlns:msxsl='urn:schemas-microsoft-com:xslt' xmlns:user='urn:user-scripts'><menugroup></menugroup></menu>");

				i = 0;
				foreach ( Group gr in this.Groups)
				{
					j = 0;						
					foreach ( Item it in gr.Items)
					{
						if (it.Group != null)
						{
							
							XmlElement  xnSubMenu = xd.CreateElement("submenu");
							XmlAttribute xabHandle = xd.CreateAttribute("handle");
							xabHandle.Value = "GPR" + i.ToString() + j.ToString();
							XmlAttribute xabHref = xd.CreateAttribute("href");
							xabHref.Value = "";
							XmlAttribute xabLabel = xd.CreateAttribute("label");
							xabLabel.Value = it.Group.Caption;
							xnSubMenu.Attributes.Append(xabHandle);
							xnSubMenu.Attributes.Append(xabHref);
							xnSubMenu.Attributes.Append(xabLabel);

							xd.DocumentElement.ChildNodes[0].AppendChild(xnSubMenu);

							k = 0;
							foreach ( Item subit in it.Group.Items)
							{
								switch (subit.WebPanelBarItemType)
								{
									case  WebPanelBarItemType.FlyoutSubLink:

										XmlElement  xnItemMenu = xd.CreateElement("item");
										XmlAttribute xabItemHref = xd.CreateAttribute("href");
										xabItemHref.Value = subit.Url.ToString();
										XmlAttribute xabItemLabel = xd.CreateAttribute("label");
										xabItemLabel.Value = subit.Caption;
										xnItemMenu.Attributes.Append(xabItemHref);
										xnItemMenu.Attributes.Append(xabItemLabel);
										xd.DocumentElement.ChildNodes[0].LastChild.AppendChild(xnItemMenu);
										break;

									case  WebPanelBarItemType.FlyoutHeading:
										
										xnItemMenu = xd.CreateElement("heading");
										xabItemLabel = xd.CreateAttribute("label");
										xabItemLabel.Value = subit.Caption;
										xnItemMenu.Attributes.Append(xabItemLabel);
										xd.DocumentElement.ChildNodes[0].LastChild.AppendChild(xnItemMenu);
										break;

									case  WebPanelBarItemType.FlyoutSeparator:

										xnItemMenu = xd.CreateElement("separator");
										xd.DocumentElement.ChildNodes[0].LastChild.AppendChild(xnItemMenu);
										break;
									
								}
								k++;
							}							
						}
						j++;											
					}
					i++;
				}
				return xd.InnerXml;			
			}
			else
			{
				return "";
			}
		}
	}

#region Enumerations

	public enum WebPanelBarGroupType
	{
		/// <summary>
		/// Normal hyperlink 
		/// </summary>
		Normal, 
		/// <summary>
		/// Flyout menu, support submenu and menu scroll.
		/// </summary>
		FlyOut
	}

	public enum WebPanelBarItemType
	{
		/// <summary>
		/// Normal link
		/// </summary>
		NormalLink,
		/// <summary>
		/// Flyoutlink has submenu
		/// </summary>
		FlyoutLink, 
		/// <summary>
		/// FlyoutNormalLink no submenu
		/// </summary>
		FlyoutNormalLink,
		/// <summary>
		/// FlyoutSubLink
		/// </summary>
		FlyoutSubLink,
		/// <summary>
		/// Submenu Separator
		/// </summary>
		FlyoutSeparator, 
		/// <summary>
		/// Submenu Head
		/// </summary>
		FlyoutHeading
	}

#endregion

	
}
