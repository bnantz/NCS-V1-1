using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// This is the page which provides the UI for the <see cref="DialogMessageBox"/>.
	/// </summary>
	internal class DialogMessageBoxPage : DialogPage {
		
		/// <summary>
		/// Overrides <see cref="Control.OnInit"/>
		/// </summary>
		protected override void OnInit(EventArgs e) {
			InitializeComponent();
			base.OnInit (e);
		}

		private Image icon;
		private Label prompt;
		private Button ok;
		private Button cancel;
		private Button yes;
		private Button no;
		private Button retry;
		private System.Web.UI.HtmlControls.HtmlForm theForm;

		/// <summary>
		/// Creates the page's control tree via code instead of parsing an aspx.
		/// </summary>
		private void InitializeComponent() {

			this.Controls.Add(new LiteralControl("<html><head>"));
			
			String title = HttpContext.Current.Request.QueryString["title"];
			if ( title != null && title.Length > 0 ) {
				this.Controls.Add( new LiteralControl("<title>" + title + "</title>"));
			}

			this.Controls.Add( new LiteralControl("<style>\r\n") );

			Boolean supportsSystemColors = true;
			HttpBrowserCapabilities browser = this.Request.Browser;
			if ( browser.Browser.ToUpper() == "NETSCAPE" && browser.MajorVersion < 5 ) {
				supportsSystemColors = false;
			}
			if ( browser.Browser.ToUpper() == "IE" && browser.MajorVersion < 4 ) {
				supportsSystemColors = false;
			}
			if ( browser.Browser.ToUpper() == "OPERA" && browser.MajorVersion < 5 ) {
				supportsSystemColors = false;
			}
			if ( supportsSystemColors ) {
				this.Controls.Add( new LiteralControl(@"
body {
	background-color: ButtonFace;
}
"));
			}

			if ( browser.Browser.ToUpper() == "IE" && browser.MajorVersion > 3 ) { 
this.Controls.Add( new LiteralControl( @"
table {
	height:100%;
}
td.textCell {
	height:100%;
}
"));
			}

			this.Controls.Add(new LiteralControl("\r\n</style></head><body style='overflow:auto;border-width:0px;'>"));


			theForm = new System.Web.UI.HtmlControls.HtmlForm();
			theForm.ID = "TheForm";
			this.Controls.Add(theForm);

			theForm.Controls.Add(new LiteralControl("<table width='100%' border='0'><tr><td valign='middle' class='textCell'>"));

			icon = new Image();
			icon.ID = "Icon";
			icon.ImageAlign = ImageAlign.Middle;
			String appPath = this.Request.ApplicationPath;
			if ( !appPath.EndsWith("/") ) {
				appPath += "/";
			}
			icon.ImageUrl =  appPath + DialogHandlerFactory.HandlerName + "?" + DialogHandlerFactory.HandlerNameKey + "=DialogImage&image=" + HttpContext.Current.Request.QueryString["icon"].ToLower();
			icon.Visible = HttpContext.Current.Request.QueryString["icon"].ToLower() != "none";
			theForm.Controls.Add(icon);

			prompt = new Label();
			prompt.ID = "Prompt";
			prompt.Text = HttpContext.Current.Request.QueryString["Prompt"];
			theForm.Controls.Add(prompt);

			theForm.Controls.Add(new LiteralControl("</td></tr><tr><td valign='bottom' align='center'>"));

			ok = new Button();
			ok.ID = "OK";
			ok.CommandName = "OK";
			ok.Text = "   OK   ";
			ok.Click += new EventHandler(button_Click);
			theForm.Controls.Add(ok);
			
			theForm.Controls.Add(new LiteralControl(" "));

			retry= new Button();
			retry.ID = "Retry";
			retry.CommandName = "Retry";
			retry.Text = " Retry ";
			retry.Click += new EventHandler(button_Click);
			theForm.Controls.Add(retry);
			
			theForm.Controls.Add(new LiteralControl(" "));

			yes = new Button();
			yes.ID = "Yes";
			yes.CommandName = "Yes";
			yes.Text = "  Yes   ";
			yes.Click += new EventHandler(button_Click);
			theForm.Controls.Add(yes);
			
			theForm.Controls.Add(new LiteralControl(" "));

			no = new Button();
			no.ID = "No";
			no.CommandName = "No";
			no.Text = "   No   ";
			no.Click += new EventHandler(button_Click);
			theForm.Controls.Add(no);
			
			theForm.Controls.Add(new LiteralControl(" "));
			
			cancel = new Button();
			cancel.ID = "Cancel";
			cancel.CommandName = "Cancel";
			cancel.Text = "Cancel";
			cancel.Click += new EventHandler(button_Click);
			theForm.Controls.Add(cancel);

			theForm.Controls.Add(new LiteralControl("</td></tr></table>"));

			this.Controls.Add(new LiteralControl("</body></html>"));
		}

		protected override void OnPreRender(EventArgs e) {
			base.OnPreRender (e);
			String buttons = HttpContext.Current.Request.QueryString["buttons"];
			this.ok.Visible = ( buttons.IndexOf("OK") != -1 );
			this.retry.Visible = ( buttons.IndexOf("Retry") != -1 );
			this.yes.Visible = ( buttons.IndexOf("Yes") != -1 );
			this.no.Visible = ( buttons.IndexOf("No") != -1 );
			this.cancel.Visible = ( buttons.IndexOf("Cancel") != -1 );

		}


		private void button_Click(object sender, EventArgs e) {
			Button b = sender as Button;
			this.Close(b.CommandName);
		}

	}
}
