using System;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Collections.Specialized;

namespace System.Web.Security {

	public class MembershipConfiguration {
		string defaultProvider;
		Hashtable providers = new Hashtable();

		public static MembershipConfiguration GetConfig() {
			return (MembershipConfiguration) ConfigurationSettings.GetConfig("system.web/membership");
		}  

		public void LoadValuesFromConfigurationXml(XmlNode node) {
			XmlAttributeCollection attributeCollection = node.Attributes;

			// Get the default provider
			defaultProvider = attributeCollection["defaultProvider"].Value;

			// Read child nodes
			foreach (XmlNode child in node.ChildNodes) {
				if (child.Name == "providers")
					GetProviders(child);
			}
		}

		void GetProviders(XmlNode node) {
			foreach (XmlNode provider in node.ChildNodes) {
				switch (provider.Name) {
					case "add" :
						providers.Add(provider.Attributes["name"].Value, new Provider(provider.Attributes) );
						break;

					case "remove" :
						providers.Remove(provider.Attributes["name"].Value);
						break;

					case "clear" :
						providers.Clear();
						break;
				}
			}
		}

		// Properties
		//
		public string DefaultProvider { get { return defaultProvider; } }
		public Hashtable Providers { get { return providers; } } 

	}

	public class Provider {
		string name;
		string providerType;
		NameValueCollection providerAttributes = new NameValueCollection();

		public Provider (XmlAttributeCollection attributes) {

			// Set the name of the provider
			//
			name = attributes["name"].Value;

			// Set the type of the provider
			//
			providerType = attributes["type"].Value;

			// Store all the attributes in the attributes bucket
			//
			foreach (XmlAttribute attribute in attributes) {

				if ( (attribute.Name != "name") && (attribute.Name != "type") )
					providerAttributes.Add(attribute.Name, attribute.Value);

			}

		}

		public string Name {
			get {
				return name;
			}
		}

		public string Type {
			get {
				return providerType;
			}
		}

		public NameValueCollection Attributes {
			get {
				return providerAttributes;
			}
		}

	}
}
