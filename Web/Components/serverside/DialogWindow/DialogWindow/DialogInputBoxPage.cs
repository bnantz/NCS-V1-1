using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// This is the page which provides the UI for the <see cref="DialogInputBox"/>.
	/// </summary>
	internal class DialogInputBoxPage : DialogPage {
		
		/// <summary>
		/// Overrides <see cref="Control.OnInit"/>
		/// </summary>
		protected override void OnInit(EventArgs e) {
			InitializeComponent();
			base.OnInit (e);
		}

		private Label prompt;
		private TextBox entry;
		private Button ok;
		private Button cancel;
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

			theForm.Controls.Add(new LiteralControl("<table width='100%' border='0'><tr><td class='textCell'>"));

			prompt = new Label();
			prompt.ID = "Prompt";
			prompt.Text = HttpContext.Current.Request.QueryString["prompt"];
			theForm.Controls.Add(prompt);

			theForm.Controls.Add(new LiteralControl("</td></tr><tr><td>"));

			entry = new TextBox();
			entry.ID = "Entry";
			entry.Style["width"] = "100%";
			entry.Text = HttpContext.Current.Request.QueryString["defaulttext"];
			theForm.Controls.Add(entry);

			theForm.Controls.Add(new LiteralControl("</td></tr><tr><td valign='bottom' align='right'>"));

			ok = new Button();
			ok.ID = "OK";
			ok.Text = "   OK   ";
			ok.Click += new EventHandler(ok_Click);
			theForm.Controls.Add(ok);
			
			theForm.Controls.Add(new LiteralControl("&nbsp;"));
			
			cancel = new Button();
			cancel.ID = "Cancel";
			cancel.Text = "Cancel";
			cancel.Click += new EventHandler(cancel_Click);
			theForm.Controls.Add(cancel);

			theForm.Controls.Add(new LiteralControl("</td></tr></table>"));

			this.Controls.Add(new LiteralControl("</body></html>"));
		}

		private void ok_Click(object sender, EventArgs e) {
			String result = entry.Text;
			this.Close(result);
		}

		private void cancel_Click(object sender, EventArgs e) {
			this.Close();
		}
	}
}
