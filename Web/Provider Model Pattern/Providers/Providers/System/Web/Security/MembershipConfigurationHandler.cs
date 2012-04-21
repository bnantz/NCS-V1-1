using System;
using System.Configuration;
using System.Xml;


namespace System.Web.Security {

	/// <summary>
	/// Summary description for MembershipConfigurationHandler.
	/// </summary>
	public class MembershipConfigurationHandler : IConfigurationSectionHandler {
		public virtual object Create(Object parent, Object context, XmlNode node) {
			MembershipConfiguration config = new MembershipConfiguration();
			config.LoadValuesFromConfigurationXml(node);
			return config;
		}
	}

}
