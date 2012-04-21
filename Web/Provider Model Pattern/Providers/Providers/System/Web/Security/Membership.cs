using System;

namespace System.Web.Security {

	public class Membership {
		public static bool Validate (string username, string password) {
			MembershipProvider provider = MembershipProvider.Instance();

			return provider.Validate(username, password);
		}

		public static void CreateUser (string username, string password) { 
			MembershipProvider provider = MembershipProvider.Instance();

			provider.CreateUser(username, password);
		}
	}
}
