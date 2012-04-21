using System;
using System.Web;
using System.Resources;
using System.Reflection;
using System.IO;

namespace MetaBuilders.WebControls {

	/// <summary>
	/// Responds to requests by outputting the messagebox icons embedded into the assembly as resources.
	/// </summary>
	public class DialogImageHandler : IHttpHandler {
		
		#region IHttpHandler Members

		/// <summary>
		/// Implements <see cref="IHttpHandler.ProcessRequest"/>.
		/// </summary>
		public void ProcessRequest(HttpContext context) {
			String imageRequested = context.Request.QueryString["image"];
			switch( imageRequested ) {
				case "exclamation":
				case "information":
				case "question":
				case "stop":
					
					Assembly asm = Assembly.GetExecutingAssembly();
					String resourceName = "MetaBuilders.WebControls." + imageRequested + ".gif";
					context.Response.ContentType = "image/gif";

					using( Stream imageStream = asm.GetManifestResourceStream(resourceName) ) {
						Byte[] imageBuffer = new byte[1024];
						Int32 bufferLength = 1;
						Stream outputStream = context.Response.OutputStream;
						while ((bufferLength > 0)) {
							bufferLength = imageStream.Read(imageBuffer, 0, 1024);
							outputStream.Write(imageBuffer, 0, bufferLength);
						}
					}
					break;
			}
			context.Response.End();
		}

		/// <summary>
		/// Implements <see cref="IHttpHandler.IsReusable"/>.
		/// </summary>
		public bool IsReusable {
			get {
				return true;
			}
		}

		#endregion
	}
}
