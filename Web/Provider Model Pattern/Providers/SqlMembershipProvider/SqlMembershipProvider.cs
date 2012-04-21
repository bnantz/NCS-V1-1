using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Web.Security;

namespace NothinButAspNet {

	public class SqlMembershipProvider : MembershipProvider {

		public SqlMembershipProvider (string connectionString) {
		}

		#region Membership specific behaviors
		public override void CreateUser(string username, string password) {

			// connect to sql server (or other database) here and create
			// the user

		}

		public override bool Validate(string username, string password) {

			if (username == "Demo" && password == "password")
				return true;

			return false;
		}
		#endregion

		#region Provider specific behaviors
		public override void Initialize(string name, NameValueCollection configValue) {

		}

		public override string Name {
			get {
				return null;
			}
		}
		#endregion

	}
}
