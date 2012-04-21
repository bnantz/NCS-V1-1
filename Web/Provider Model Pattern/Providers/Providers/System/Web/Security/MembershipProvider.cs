using System;
using System.Reflection;
using System.Web.Caching;
using System.Collections.Specialized;
using System.Configuration.Providers;

namespace System.Web.Security {

	public abstract class MembershipProvider : ProviderBase {

		public static MembershipProvider Instance() {

			// Use the cache because the reflection used later is expensive
			Cache cache = HttpRuntime.Cache;
			Type type = null;
			string cacheKey = null;

			// Get the names of the providers
			MembershipConfiguration config = MembershipConfiguration.GetConfig();

			// Read the configuration specific information
			// for this provider
			Provider membershipProvider = (Provider) config.Providers[config.DefaultProvider];

			// In the cache?
			cacheKey = "Membership::" + config.DefaultProvider;
			if ( cache[cacheKey] == null ) {

				// The assembly should be in \bin or GAC, so we simply need
				// to get an instance of the type
				try {

					type = Type.GetType( membershipProvider.Type );

					// Insert the type into the cache
					Type[] paramTypes = new Type[1];
					paramTypes[0] = typeof(string);
					cache.Insert( cacheKey, type.GetConstructor(paramTypes) );

				} catch (Exception e) {
					throw new Exception("Unable to load provider", e);
				}

			}

			// Load the configuration settings
			object[] paramArray = new object[1];
			paramArray[0] = membershipProvider.Attributes["connectionString"];

			return (MembershipProvider)(  ((ConstructorInfo)cache[cacheKey]).Invoke(paramArray) );
		}

		public abstract bool Validate (string username, string password);
		public abstract void CreateUser (string username, string password);
	}
}
