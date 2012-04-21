using System;
using System.Web;
using System.Reflection;

namespace MetaBuilders.WebControls {

	internal class DialogHandlerFactory : System.Web.IHttpHandlerFactory {
		
		#region IHttpHandlerFactory Members

		public void ReleaseHandler(IHttpHandler handler) {
		}

		public IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated) {
			switch( context.Request.QueryString[HandlerNameKey] ) {
				case "DialogInputBoxPage":
					return new DialogInputBoxPage();
				case "DialogMessageBoxPage":
					return new DialogMessageBoxPage();
				case "DialogImage":
					return new DialogImageHandler();
			}
			return null;
		}

		#endregion

		/// <summary>
		/// Ensures that the IHttpHandlerFactory required for built in dialogs to function, is registered for this application.
		/// </summary>
		internal static void EnsureHandlerFactory() {
			if ( !IsRegistered ) {
				throw new ApplicationException("The '" + HandlerName + "' IHttpHandlerFactory must be registered with web.config.");
			}
		}

		
		private static Boolean IsRegistered {
			get {
				HttpContext context = HttpContext.Current;
				if ( context == null ) {
					return DetermineIsRegistered();
				}
				String cacheKey = "IHttpHandlerFactory Installed " + HandlerName;
				if ( context.Cache[cacheKey] == null ) {
					context.Cache.Insert(cacheKey, DetermineIsRegistered());
				}
				return (Boolean)context.Cache[cacheKey];
			}
		}

		private static Boolean DetermineIsRegistered() {
			Object handlerMap = System.Web.HttpContext.GetAppConfig("system.web/httpHandlers");
			if ( handlerMap == null ) {
				return false;
			}

			MethodInfo findMapping = handlerMap.GetType().GetMethod("FindMapping", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic );
			if ( findMapping == null ) {
				return false;
			}

			Object handler = findMapping.Invoke(handlerMap, new Object[] { "GET", HandlerName } );
			if ( handler == null ) {
				return false;
			}

			PropertyInfo handlerPathProperty = handler.GetType().GetProperty("Path", BindingFlags.NonPublic | BindingFlags.Instance );
			if ( handlerPathProperty == null ) {
				return false;
			}

			String handlerPath = handlerPathProperty.GetValue(handler,null) as String;
			if ( handlerPath == null || handlerPath != HandlerName ) {
				return false;
			}

			return true;
		}



		internal static String HandlerName {
			get {
				return "MetaBuilders_DialogWindow.axd";
			}
		}

		internal static String HandlerNameKey {
			get {
				return "MetaBuilders_Dialog";
			}
		}
	}
}
