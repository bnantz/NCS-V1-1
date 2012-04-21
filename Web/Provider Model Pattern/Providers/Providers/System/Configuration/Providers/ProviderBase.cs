using System;
using System.Collections.Specialized;

namespace System.Configuration.Providers {

	public abstract class ProviderBase {
		public abstract void Initialize (string name, NameValueCollection configValue);
		public abstract string Name { get; }
	}

}
