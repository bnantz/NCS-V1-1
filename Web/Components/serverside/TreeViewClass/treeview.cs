using System;
using System.ComponentModel; 
using System.Web.UI; 
using System.ComponentModel.Design; 
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design; 
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Xml;
using System.Security.Permissions;

namespace Cornerstone 
{
   public class TreeViewItemCollection : System.Collections.CollectionBase 
   { 
      public static bool IsNumeric(object value)
      {
         try 
         { 
            double i = Convert.ToDouble(value.ToString());
            return true; 
         } 
         catch (FormatException) 
         { 
            return false;
         }
      }

      public void Add(TreeViewItem item) 
      { 
         List.Add(item); 
      } 

      public TreeViewItem Add() 
      { 
         TreeViewItem Itm = new TreeViewItem("", "", "", ""); 
         List.Add(Itm); 
         return Itm; 
      } 

      public TreeViewItem Item
      { 
         get 
         { 
            return ((TreeViewItem)List); 
         } 
      } 

      public void Dispose() 
      { 
         foreach( TreeViewItem i in this) 
         { 
            i.Items.Dispose(); 
         } 
         this.Clear(); 
      } 

      public TreeViewItem GetNode(int nodeId) 
      { 
         int CurNodeId = 1; 
         return GetNode(nodeId, ref CurNodeId); 
      } 

      public TreeViewItem GetNode(int nodeId, ref int curNodeId) 
      { 
         foreach (TreeViewItem Node in this) 
         { 
            if (nodeId == curNodeId) 
            { 
               return Node; 
            } 
            curNodeId += 1; 
            if (Node.Open) 
            { 
               TreeViewItem res = Node.Items.GetNode(nodeId, ref curNodeId); 
               if (res != null) 
               { 
                  return res; 
               } 
            } 
         } 
         return null; 
      } 

      public int Render(ref WebControl owner, ref TreeView treeView) 
      { 
         return Render(ref owner, ref treeView, 0);
      } 

      public int Render(ref WebControl owner, ref TreeView treeView, int level) 
      { 
         int CurrentNodeId = 0; 
         return Render(ref owner, ref treeView, level, ref CurrentNodeId, "");
      } 

      public int Render(ref WebControl owner, ref TreeView treeView, int level, ref int currentNodeId) 
      { 
         return Render(ref owner, ref treeView, level, ref currentNodeId, "");
      } 

      public int Render(ref WebControl owner, ref TreeView treeView, int level, ref int currentNodeId, string parentBorder) 
      { 
         int CurrentIndex = 1; 
         foreach (TreeViewItem Node in this) 
         { 
            bool bLastItem = (CurrentIndex == this.Count); 
            Node.Render(ref owner, ref treeView, level + 1, ref currentNodeId, bLastItem, parentBorder); 
            CurrentIndex += 1; 
         } 

         return CurrentIndex;
      } 
      public enum ChangeState 
      { 
         CSUnselect = 0, 
         CSSelect = 1, 
         CSOpen = 2, 
         CSClose = 3, 
      } 

      public void SelectNone() 
      { 
         SetStateRecursive(ChangeState.CSUnselect); 
      } 

      public void OpenAll() 
      { 
         SetStateRecursive(ChangeState.CSOpen); 
      } 

      public void CloseAll() 
      { 
         SetStateRecursive(ChangeState.CSClose); 
      } 

      public void SelectAll() 
      { 
         SetStateRecursive(ChangeState.CSSelect); 
      } 

      public void SetStateRecursive(ChangeState newState) 
      { 
         foreach (TreeViewItem Node in this) 
         { 
            if (newState == ChangeState.CSUnselect) 
            { 
               Node.Selected = false; 
            } 
            else if (newState == ChangeState.CSClose) 
            { 
               if (Node.Items.Count > 0) 
               { 
                  Node.Open = false; 
               } 
            } 
            else if (newState == ChangeState.CSSelect) 
            { 
               Node.Selected = true; 
            } 
            else if (newState == ChangeState.CSOpen) 
            { 
               if (Node.Items.Count > 0) 
               { 
                  Node.Open = true; 
               } 
            } 
            Node.Items.SetStateRecursive(newState); 
         } 
      } 

      public void UnSelectAllVisible() 
      { 
         foreach (TreeViewItem Node in this) 
         { 
            Node.Selected = false; 
            if (Node.Open) 
            { 
               Node.Items.UnSelectAllVisible(); 
            } 
         } 
      } 

      public void Remove(TreeViewItem item) 
      { 
         this.InnerList.Remove(item); 
      } 
   } 
   [ParseChildren(true, "Items"), PersistChildren(false)] 
   public class TreeViewItem 
   { 
      public enum SelectionMode 
      { 
         None = 0, 
         CheckBox = 1, 
         RadioButton = 2, 
      } 
      private TreeViewItemCollection _Items = new TreeViewItemCollection(); 
      private string _Text; 
      private string _Link; 
      private string _Image; 
      private string _TargetFrame; 
      private string _OnClick; 
      private string _Value; 
      private bool _Open = false; 
      private bool _Selected = false; 
      private SelectionMode _Selector = SelectionMode.None; 
      private bool _LoadOnExpand = false; 
      //private int _NodeId; 

      [Bindable(true)] 
      public bool LoadOnExpand 
      { 
         get 
         { 
            return _LoadOnExpand; 
         } 
         set 
         { 
            _LoadOnExpand = value; 
         } 
      } 

      [Bindable(true)] 
      public bool Selected 
      { 
         get 
         { 
            return _Selected; 
         } 
         set 
         { 
            _Selected = value; 
         } 
      } 

      [Bindable(true)] 
      public string Text 
      { 
         get 
         { 
            return _Text; 
         } 
         set 
         { 
            _Text = value; 
         } 
      } 

      [Bindable(true)] 
      public string Link 
      { 
         get 
         { 
            return _Link; 
         } 
         set 
         { 
            _Link = value; 
         } 
      } 

      [Bindable(true)] 
      public string TargetFrame 
      { 
         get 
         { 
            return _TargetFrame; 
         } 
         set 
         { 
            _TargetFrame = value; 
         } 
      } 

      [Bindable(true)] 
      public string OnClick 
      { 
         get 
         { 
            return _OnClick; 
         } 
         set 
         { 
            _OnClick = value; 
         } 
      } 

      [Bindable(true)] 
      public string Value 
      { 
         get 
         { 
            return _Value; 
         } 
         set 
         { 
            _Value = value; 
         } 
      } 

      [Bindable(true)] 
      public bool Open 
      { 
         get 
         { 
            return _Open; 
         } 
         set 
         { 
            _Open = value; 
         } 
      } 

      [Bindable(true)] 
      public SelectionMode Selector 
      { 
         get 
         { 
            return _Selector; 
         } 
         set 
         { 
            _Selector = value; 
         } 
      } 

      [Bindable(true)] 
      public string Image 
      { 
         get 
         { 
            return _Image; 
         } 
         set 
         { 
            _Image = value; 
         } 
      } 

      [PersistenceMode(PersistenceMode.InnerDefaultProperty), NotifyParentProperty(true), Bindable(false)] 
      public TreeViewItemCollection Items 
      { 
         get 
         { 
            return _Items; 
         } 
      } 

      public TreeViewItem() 
      { 
      } 

      public TreeViewItem(string Text, string Image, string Link, string TargetFrame) 
      { 
         this.Text = Text; 
         this.Image = Image; 
         this.TargetFrame = TargetFrame; 
         this.Link = Link; 
      } 

      public void RenderExpandImage(System.Web.UI.WebControls.WebControl owner, ref TreeView treeView, bool lastItem, int nodeId) 
      { 
         HtmlImage ExpandImage = new HtmlImage(); 
         ExpandImage.Src = "node.gif"; 
         if (!Open) 
         { 
            if ((LoadOnExpand | Items.Count > 0)) 
            { 
               ExpandImage.Src = "closed-" + ExpandImage.Src; 
               ExpandImage.Attributes.Add("onClick", treeView.GetPostBackClientEvent("O" + nodeId.ToString())); 
            } 
         } 
         else 
         { 
            ExpandImage.Src = "open-" + ExpandImage.Src; 
            ExpandImage.Attributes.Add("onClick", treeView.GetPostBackClientEvent("O" + nodeId.ToString())); 
         } 
         if (lastItem) 
         { 
            ExpandImage.Src = "end-" + ExpandImage.Src; 
         } 
         ExpandImage.Src = treeView.ImagesFolder + ExpandImage.Src; 
         owner.Controls.Add(ExpandImage); 
      } 

      private object DecToHex(int nNumber, int nDigits) 
      { 
         string res = Convert.ToString(nNumber, 16); 
         while ((res.Length < nDigits)) 
         { 
            res = "0" + res; 
         } 
         return res; 
      } 

      private string ColToHex(Color cColor) 
      { 
         int r = cColor.ToArgb() & 255; 
         int g = cColor.ToArgb() >> 8 & 255; 
         int b = cColor.ToArgb() >> 16 & 255; 
         return ("#" + DecToHex(r, 2) + DecToHex(g, 2) + DecToHex(b, 2)); 
      } 

      private HtmlAnchor CreateLink(TreeView treeView) 
      { 
         HtmlAnchor a = new HtmlAnchor(); 
         a.HRef = Link; 
         a.Attributes.Add("style", "text-decoration:none;color:" + ColToHex(treeView.ForeColor)); 
         if (TargetFrame.Length > 0) 
         { 
            a.Target = TargetFrame; 
         } 
         else 
         { 
            a.Target = treeView.TargetFrame; 
         } 
         return a; 
      } 

      public void Render(ref System.Web.UI.WebControls.WebControl owner, ref TreeView treeView, int level, ref int currentNodeId, bool lastItem, string parentBorder) 
      { 
         currentNodeId += 1; 
         Table NodeTable = new Table(); 
         TableRow NodeRow = new TableRow(); 
         TableCell TextCell = new TableCell(); 
         NodeTable.CellPadding = 0; 
         NodeTable.CellSpacing = 0; 
         if (parentBorder.Length > 0) 
         { 
            TableCell ParentBorderCell = new TableCell(); 
            Label ParentBorderLabel = new Label(); 
            ParentBorderLabel.Text = parentBorder; 
            ParentBorderCell.Controls.Add(ParentBorderLabel); 
            NodeRow.Controls.Add(ParentBorderCell); 
         } 
         if (lastItem) 
         { 
            parentBorder += "<img src='" + treeView.ImagesFolder + "blank-node.gif'>"; 
         } 
         else 
         { 
            parentBorder += "<img src='" + treeView.ImagesFolder + "edge-node.gif'>"; 
         } 
         TableCell ExpandImageCell = new TableCell(); 
         RenderExpandImage(ExpandImageCell, ref treeView, lastItem, currentNodeId); 
         NodeRow.Controls.Add(ExpandImageCell); 
         if (this.Selector != SelectionMode.None) 
         { 
            TableCell SelectorCell = new TableCell(); 
            if (this.Selector == SelectionMode.CheckBox) 
            { 
               HtmlInputCheckBox CBox = new HtmlInputCheckBox(); 
               CBox.Style.Add("height", "14"); 
               CBox.Style.Add("width", "14"); 
               CBox.ID = treeView.UniqueID + "_selected"; 
               CBox.Checked = this.Selected; 
               CBox.Value = currentNodeId.ToString(); 
               SelectorCell.Controls.Add(CBox); 
            } 
            else if (this.Selector == SelectionMode.RadioButton) 
            { 
               HtmlInputRadioButton RButton = new HtmlInputRadioButton(); 
               RButton.Style.Add("height", "15"); 
               RButton.Style.Add("width", "15"); 
               RButton.ID = treeView.UniqueID + "_selected"; 
               RButton.Checked = this.Selected; 
               RButton.Value = currentNodeId.ToString(); 
               SelectorCell.Controls.Add(RButton); 
            } 
            NodeRow.Controls.Add(SelectorCell); 
         } 
         if (Image.Length > 0) 
         { 
            TableCell ImageCell = new TableCell(); 
            TableCell SpaceCell = new TableCell(); 
            System.Web.UI.HtmlControls.HtmlImage NodeImage = new System.Web.UI.HtmlControls.HtmlImage(); 
            NodeImage.Src = Image; 
            NodeImage.Border = 0; 
            NodeImage.Attributes.Add("align", "absmiddle"); 
            Label SpaceLabel = new Label(); 
            SpaceLabel.Text = "&nbsp;"; 
            SpaceCell.Controls.Add(SpaceLabel); 
            if (Link.Length > 0) 
            { 
               HtmlAnchor a = CreateLink(treeView); 
               a.Controls.Add(NodeImage); 
               ImageCell.Controls.Add(a); 
            } 
            else 
            { 
               ImageCell.Controls.Add(NodeImage); 
               ImageCell.Attributes.Add("onClick", treeView.GetPostBackClientEvent("C" + currentNodeId.ToString())); 
            } 
            NodeRow.Controls.Add(ImageCell); 
            NodeRow.Controls.Add(SpaceCell); 
         } 
         Label TextLabel = new Label(); 
         TextLabel.Text = Text; 
         TextCell.HorizontalAlign = HorizontalAlign.Left; 
         TextCell.Wrap = false; 
         if (Link.Length  > 0) 
         { 
            HtmlAnchor a = CreateLink(treeView); 
            a.Controls.Add(TextLabel); 
            TextCell.Controls.Add(a); 
         } 
         else 
         { 
            TextLabel.Style.Add("cursor", "hand"); 
            TextCell.Controls.Add(TextLabel); 
            TextCell.Attributes.Add( "onClick", treeView.GetPostBackClientEvent("C" + currentNodeId.ToString()) ); 
         } 
         NodeRow.Controls.Add(TextCell); 
         NodeTable.Controls.Add(NodeRow); 
         owner.Controls.Add(NodeTable); 
         if (this.Items.Count > 0 & this.Open) 
         { 
            Items.Render(ref owner, ref treeView, level + 1, ref currentNodeId, parentBorder); 
         } 
      } 
   } 

   [ParseChildren(true, "Items"), PersistChildren(false), PermissionSet(SecurityAction.LinkDemand, XML="<PermissionSet class=\"System.Security.PermissionSet\"\r\n               version=\"1\">\r\n   <IPermission class=\"System.Web.AspNetHostingPermission, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\r\n                version=\"1\"\r\n                Level=\"Minimal\"/>\r\n</PermissionSet>\r\n"), PermissionSet(SecurityAction.InheritanceDemand, XML="<PermissionSet class=\"System.Security.PermissionSet\"\r\n               version=\"1\">\r\n   <IPermission class=\"System.Web.AspNetHostingPermission, System, Version=1.0.5000.0, Culture=neutral, PublicKeyToken=b77a5c561934e089\"\r\n                version=\"1\"\r\n                Level=\"Minimal\"/>\r\n</PermissionSet>\r\n")]
   public class TreeView : System.Web.UI.WebControls.WebControl, IPostBackEventHandler, IPostBackDataHandler 
   { 
      private const string _ImagesFolder = "images/"; 

      public string ImagesFolder{get{return _ImagesFolder;}}
      
      public delegate void OnNodeClickEventHandler(ref TreeViewItem item);


      public delegate void OnNodeCloseEventHandler(ref TreeViewItem item);


      public delegate void OnNodeLoadOnDemandEventHandler(ref TreeViewItem item);


      public delegate void OnNodeOpenEventHandler(ref TreeViewItem item);


      public static bool IsNumeric(object value)
      {
         try 
         { 
            double i = Convert.ToDouble(value.ToString());
            return true; 
         } 
         catch (FormatException) 
         { 
            return false;
         }
      }

      public bool LoadPostData(string postDataKey, NameValueCollection postCollection) 
      { 
         LoadSelection(postCollection[postDataKey + "_selected"]); 
         return true; 
      } 

      private void LoadSelection(string Str) 
      { 
         string[] Selected = Str.Split(','); 
         Items.UnSelectAllVisible(); 
         foreach (string Item in Selected) 
         { 
            try 
            { 
               this.Items.GetNode(System.Convert.ToInt32(Item)).Selected = true; 
            } 
            catch (Exception ex) 
            { 
               Debug.WriteLine(ex.ToString());
            } 
         } 
      } 

      public void RaisePostBackEvent(string eventArgument) 
      { 
         string NodeId = eventArgument.Substring(2); 
         if (IsNumeric(NodeId)) 
         { 
            if (eventArgument.Substring(1, 1) == "C") 
            { 
               if (OnNodeClick != null) 
               { 
                  TreeViewItem Node = Items.GetNode(System.Convert.ToInt32(NodeId));
                  OnNodeClick(ref Node); 
               } 
            } 
            else if (eventArgument.Substring(1, 1) == "O") 
            { 
               TreeViewItem Node = Items.GetNode(System.Convert.ToInt32(NodeId)); 
               Node.Open = !Node.Open; 
               if (Node.Open) 
               { 
                  if (OnNodeOpen != null) 
                  { 
                     OnNodeOpen(ref Node); 
                  } 
                  if (Node.LoadOnExpand) 
                  { 
                     if (OnNodeLoadOnDemand != null) 
                     { 
                        OnNodeLoadOnDemand(ref Node); 
                     } 
                  } 
               } 
               else 
               { 
                  if (OnNodeClose != null) 
                  { 
                     OnNodeClose(ref Node); 
                  } 
               } 
            } 
         } 
      } 

      public void RaisePostDataChangedEvent() 
      { 
      } 
   
      private TreeViewItemCollection _Items = new TreeViewItemCollection(); 
   
      public event OnNodeClickEventHandler OnNodeClick;
      public event OnNodeCloseEventHandler OnNodeClose;
      public event OnNodeLoadOnDemandEventHandler OnNodeLoadOnDemand;
      public event OnNodeOpenEventHandler OnNodeOpen;

      public string EncodeXml() 
      { 
         XmlDataDocument myXMLobj = new XmlDataDocument(); 
         XmlElement myXML = myXMLobj.CreateElement("items"); 
         EncodeNodes(Items, myXML, myXMLobj); 
         return myXML.OuterXml; 
      } 

      public void DecodeXml(string xml) 
      { 
         XmlDataDocument myXml = new XmlDataDocument(); 
         Items.Clear(); 
         myXml.LoadXml(xml); 
         DecodeXML(myXml); 
      } 

      void DecodeXML(XmlDataDocument myXML) 
      { 
         DecodeItems(ref _Items, (XmlElement) myXML.ChildNodes[0]); 
      } 

      private void EncodeNodes(TreeViewItemCollection NColl, XmlElement myXML, XmlDataDocument myXMLobj) 
      { 
         foreach (TreeViewItem Itm in NColl) 
         { 
            XmlElement nx = myXMLobj.CreateElement("item"); 
            nx.SetAttribute("Text", Itm.Text); 
            nx.SetAttribute("Image", Itm.Image); 
            nx.SetAttribute("Link", Itm.Link); 
            nx.SetAttribute("TargetFrame", Itm.TargetFrame); 
            nx.SetAttribute("Open", Itm.Open.ToString()); 
            nx.SetAttribute("value", Itm.Value); 
            nx.SetAttribute("Selector", Itm.Selector.ToString()); 
            nx.SetAttribute("Selected", Itm.Selected.ToString()); 
            nx.SetAttribute("OnClick", Itm.OnClick); 
            nx.SetAttribute("LoadOnExpand", Itm.LoadOnExpand.ToString()); 
            myXML.AppendChild(nx); 
            if (Itm.Items.Count > 0) 
            { 
               EncodeNodes(Itm.Items, nx, myXMLobj); 
            } 
         } 
      } 

      private void DecodeItems(ref TreeViewItemCollection NColl, XmlElement myXML) 
      {
         foreach (XmlElement Node in myXML.ChildNodes) 
         { 
            TreeViewItem tvi = NColl.Add();

            tvi.Text = Node.GetAttribute("Text"); 
            tvi.Image = Node.GetAttribute("Image"); 
            tvi.Link = Node.GetAttribute("Link"); 
            tvi.TargetFrame = Node.GetAttribute("TargetFrame"); 
            tvi.Open = Convert.ToBoolean(Node.GetAttribute("Open")); 
            tvi.Selector = (TreeViewItem.SelectionMode) Enum.Parse( typeof(TreeViewItem.SelectionMode), Node.GetAttribute("Selector") ); 
            tvi.Selected = Convert.ToBoolean(Node.GetAttribute("Selected")); 
            tvi.Value = Node.GetAttribute("value"); 
            tvi.OnClick = Node.GetAttribute("OnClick"); 
            tvi.LoadOnExpand = Convert.ToBoolean(Node.GetAttribute("LoadOnExpand")); 

            if (Node.ChildNodes.Count > 0) 
            { 
               DecodeItems(ref NColl, Node); 
            } 
         } 
      } 
      private XmlDataDocument m_xmlDataSource; 

      [PersistenceMode(PersistenceMode.InnerDefaultProperty), NotifyParentProperty(true), Category("Data")] 
      TreeViewItemCollection Items 
      { 
         get 
         { 
            return _Items; 
         } 
      } 

      public XmlDataDocument DataSource 
      { 
         get 
         { 
            return (m_xmlDataSource); 
         } 
         set 
         { 
            m_xmlDataSource = value; 
            DecodeXML(value); 
         } 
      } 

      [Bindable(true)] 
      public int LineSize 
      { 
         get 
         { 
            if (this.ViewState["LineSize"] == null) 
            { 
               return 0; 
            } 
            return (int) this.ViewState["LineSize"]; 
         } 
         set 
         { 
            this.ViewState["LineSize"] = value; 
         } 
      } 

      [Bindable(true)] 
      public int LineWidth 
      { 
         get 
         { 
            if (this.ViewState["LineWidth"] == null) 
            { 
               return 0; 
            } 
            return (int) this.ViewState["LineWidth"]; 
         } 
         set 
         { 
            this.ViewState["LineWidth"] = value; 
         } 
      } 

      [Bindable(true)] 
      public int FontSize 
      { 
         get 
         { 
            if (this.ViewState["FontSize"] == null) 
            { 
               return 2; 
            } 
            return (int) this.ViewState["FontSize"]; 
         } 
         set 
         { 
            this.ViewState["FontSize"] = value; 
         } 
      } 

      [Bindable(true)] 
      public string TargetFrame 
      { 
         get 
         { 
            if (this.ViewState["TargetFrame"] == null) 
            { 
               return (""); 
            } 
            return this.ViewState["TargetFrame"].ToString(); 
         } 
         set 
         { 
            this.ViewState["TargetFrame"] = value; 
         } 
      } 

      [Bindable(true)] 
      public bool AutoPostBack 
      { 
         get 
         { 
            if (this.ViewState["AutoPostBack"] == null) 
            { 
               return true; 
            } 
            return (bool) this.ViewState["AutoPostBack"]; 
         } 
         set 
         { 
            this.ViewState["AutoPostBack"] = value; 
         } 
      } 

      [Bindable(true)] 
      public new Color ForeColor 
      { 
         get 
         { 
            if (this.ViewState["ForeColor"] == null) 
            { 
               return Color.Black; 
            } 
            if (IsNumeric(ViewState["ForeColor"])) 
            { 
               return Color.FromArgb((int)this.ViewState["ForeColor"]); 
            } 
            return (Color.Black); 
         } 
         set 
         { 
            this.ViewState["ForeColor"] = value.ToArgb(); 
         } 
      } 

      TreeView() : base() 
      { 
         this.ViewState["AutoPostBack"] = false; 
         this.ViewState["TargetFrame"] = ""; 
         this.ViewState["FontSize"] = 2; 
         this.ViewState["LineSize"] = 6; 
         this.ViewState["LineWidth"] = 1; 
         this.ViewState["ForeColor"] = Color.Black.ToArgb(); 
      } 

      public string GetPostBackClientEvent(string argument) 
      { 
         return Page.GetPostBackClientEvent(this, argument); 
      } 
      protected override void Render(HtmlTextWriter OutPut)
      {
         base.Render(OutPut);
         if (this.Items.Count > 0)
         {
            Panel panel1 = new Panel();
            panel1.Attributes.Add("style", "cursor:hand;");
            WebControl control1 = panel1;
            TreeView view1 = this;
            int num1 = 0;
            this.Items.Render(ref control1, ref view1, 0, ref num1, "");
            panel1 = (Panel) control1;
            panel1.Attributes.Add("style", this.Attributes["style"]);
            panel1.BorderColor = this.BorderColor;
            panel1.BorderWidth = this.BorderWidth;
            panel1.BorderStyle = this.BorderStyle;
            panel1.BackColor = this.BackColor;
            panel1.Width = this.Width;
            panel1.Height = this.Height;
            panel1.RenderControl(OutPut);
         }
         else
         {
            OutPut.Write("[TreeView - No items]");
         }
      }
      
      protected override void LoadViewState(object savedState) 
      { 
         base.LoadViewState(savedState); 
         if (this.ViewState["items"] != null) 
         { 
            DecodeXml(this.ViewState["items"].ToString()); 
         } 
         LoadSelection(Page.Request[UniqueID + "_selected"]); 
      } 

      protected override object SaveViewState() 
      { 
         this.ViewState["items"] = EncodeXml(); 
         return base.SaveViewState(); 
      } 

      public void SelectNone() 
      { 
         Items.SelectNone(); 
      } 

      public void OpenAll() 
      { 
         Items.OpenAll(); 
      } 

      public void CloseAll() 
      { 
         Items.CloseAll(); 
      } 

      public void SelectAll() 
      { 
         Items.SelectAll(); 
      } 

      public override void Dispose() 
      { 
         Items.Dispose(); 
         base.Dispose(); 
      } 
   }
}