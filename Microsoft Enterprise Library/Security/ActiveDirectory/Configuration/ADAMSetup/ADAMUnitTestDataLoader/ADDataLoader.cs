//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.DirectoryServices;

namespace ADAMUnitTestDataLoader
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class ADDataLoader
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			DirectoryEntry objADAM = null;   
			DirectoryEntry objRoles = null;
			DirectoryEntry objGroup = null;
			DirectoryEntry mgr;
			DirectoryEntry objUser;
			string strPath;

			Console.WriteLine("Enter the binding string (server:port/ADpartition)");
			Console.WriteLine("or press enter for the default:");
			Console.WriteLine("    localhost:389/OU=EntLibPartition,O=EntLib,C=US");
			strPath = Console.ReadLine();
			if (strPath.Length == 0)
			{
				strPath = "LDAP://localhost:389/OU=EntLibPartition,O=EntLib,C=US";
			}
			else
			{
				strPath = "LDAP://" + strPath;
			}

			//Bind to ADAM
			try
			{
				objADAM = new DirectoryEntry(strPath);
				objADAM.RefreshCache();
				Console.WriteLine("Bind successful.");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error:   Bind failed.");
				Console.WriteLine("         {0}", e.Message);
				Console.ReadLine();
				return;
			}

			//Create user testuser
			try
			{
				objGroup = objADAM.Children.Find("CN=Users,CN=Roles");
				objUser = objADAM.Children.Add("CN=testuser", "user");
				objUser.Properties["displayName"].Add("TestUserDisplayName");
				objUser.Properties["userPrincipalName"].Add("TestUser@Fabrikam.Us");
				objUser.CommitChanges();
				Console.WriteLine("User testuser created.");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error:   Create User testuser failed.");
				Console.WriteLine("         {0}", e.Message);
				Console.ReadLine();
				return;
			}

			//Adding a new group called Admins
			try
			{
				objRoles = objADAM.Children.Find("CN=Roles");
				mgr = objRoles.Children.Add("CN=Admins","group");
				mgr.CommitChanges();
				Console.WriteLine("Group Admins created.");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error:   Create group Admins failed.");
				Console.WriteLine("         {0}", e.Message);
				Console.ReadLine();
				return;
			}

			//Adding a new group called Managers
			try
			{
				mgr = objRoles.Children.Add("CN=Managers","group");
				mgr.CommitChanges();
				Console.WriteLine("Group Managers created.");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error:   Create group Managers failed.");
				Console.WriteLine("         {0}", e.Message);
				Console.ReadLine();
				return;
			}

			//Just checking to see the group Users exists in Roles
			try
			{
				objGroup = objADAM.Children.Find("CN=Users,CN=Roles");
				Console.WriteLine("Group Users created.");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error:   Could not find group Users.");
				Console.WriteLine("         {0}", e.Message);
				Console.ReadLine();
				return;
			}

			//Add testuser to Manager group/role
			try
			{
				objGroup = objADAM.Children.Find("CN=Managers,CN=Roles");
				objUser = objADAM.Children.Find("CN=testuser");

				objGroup.Properties["member"].Add(objUser.Properties["distinguishedName"].Value);
				objGroup.CommitChanges();
				Console.WriteLine("Testuser added to Manager group.");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error:   Adding testuser to Manager group failed.");
				Console.WriteLine("         {0}", e.Message);
				Console.ReadLine();
				return;
			}

			//Add testuser to Users group/role.
			try
			{
				objGroup = objADAM.Children.Find("CN=Users,CN=Roles");
				objUser = objADAM.Children.Find("CN=testuser");

				objGroup.Properties["member"].Add(objUser.Properties["distinguishedName"].Value);
				objGroup.CommitChanges();
				Console.WriteLine("Testuser added to Users group.");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error:   Adding testuser to Users group failed.");
				Console.WriteLine("         {0}", e.Message);
				Console.ReadLine();
				return;
			}

			//Create a second user with no group memberships
			try
			{
				objUser = objADAM.Children.Add("CN=bogususer", "user");
				objUser.Properties["displayName"].Add("BogusUserDisplayName");
				objUser.Properties["userPrincipalName"].Add("BogusUser@Fabrikam.Us");
				objUser.CommitChanges();
				Console.WriteLine("User bogususer created.");
			}
			catch (Exception e)
			{
				Console.WriteLine("Error:   Create User bogususer failed.");
				Console.WriteLine("         {0}", e.Message);
				return;
			}

			//Enumerate group memberships for a user.
			try
			{
				objGroup = objADAM.Children.Find("CN=Users,CN=Roles");
				objUser = objADAM.Children.Find("CN=testuser");
				object groups = objUser.Invoke("Groups");
				string groupNames = "";
				foreach ( object group in (IEnumerable)groups)   
				{
					// Get the Directory Entry.
					DirectoryEntry groupEntry  = new DirectoryEntry(group);
					groupNames += groupEntry.Name + " "; 
				}
				Console.WriteLine("testuser is in " + groupNames);
			}
			catch (Exception e)
			{
				Console.WriteLine("Error:   Enumerating group membership.");
				Console.WriteLine("         {0}", e.Message);
				Console.ReadLine();
				return;
			}

			Console.WriteLine("Press return to end...");
			Console.ReadLine();
			return;
		}
	}
}
