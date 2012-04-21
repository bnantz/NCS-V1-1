// DatePickerDesigner.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using MSPress.WebControls;

namespace MSPress.WebControls.Design {

    public class DatePickerDesigner : ControlDesigner {
        private static string ClientFilePathPrefix;

        private string GetClientFilePath(string fileName) {
            if (ClientFilePathPrefix == null) {
                // Use the config setting to determine where the client files are located.
                // Client files are located in the aspnet_client v-root, and then distributed
                // into sub-folders by assembly name and assembly version.
            
                string location = null;
                IDictionary configData = (IDictionary)ConfigurationSettings.GetConfig("system.web/webControls");
                if (configData != null) {
                    location = (string)configData["clientScriptsLocation"];
                }

                if (location == null) {
                    location = String.Empty;
                }
                else if (location.IndexOf("{0}") >= 0) {
                    AssemblyName assemblyName = Component.GetType().Assembly.GetName();

                    string assembly = assemblyName.Name.Replace('.', '_').ToLower();
                    string version =  assemblyName.Version.ToString().Replace('.', '_');

                    location = String.Format(location, assembly, version);
                }

                string wwwroot = Path.Combine(Directory.GetDirectoryRoot(Environment.GetFolderPath(Environment.SpecialFolder.System)),
                                              "inetpub\\wwwroot");
                ClientFilePathPrefix = wwwroot + location.Replace('/', '\\');
            }
            return ClientFilePathPrefix + fileName;
        }

        public override string GetDesignTimeHtml() {
            // The designer can assume knowledge of the runtime control, since they
            // go hand in hand. Therefore the DatePicker designer can index into the
            // Controls collection.

            ControlCollection childControls = ((DatePicker)Component).Controls;
            Debug.Assert(childControls.Count == 3);
            Debug.Assert(childControls[1] is Image);
            Debug.Assert(childControls[2] is RegularExpressionValidator);

            // Changing the URL of the picker image to the file location is specific
            // to the designer; thus, it is done here.

            Image pickerImage = (Image)childControls[1];
            pickerImage.ImageUrl = "file:///" + GetClientFilePath("Picker.gif");

            // The validator should not show up in design view, thus for design-time
            // scenarios, its visibility is set to false.

            RegularExpressionValidator validator = (RegularExpressionValidator)childControls[2];
            validator.Visible = false;

            return base.GetDesignTimeHtml();
        }

        public override void Initialize(IComponent component) {
            // Check correct usage of the designer - in other words, that it has been associated with
            // the right type of runtime control, so that all the assumptions a designer makes
            // will hold.
            if (!(component is DatePicker)) {
                throw new ArgumentException("Component must be a DatePicker", "component");
            }
            base.Initialize(component);
        }
    }
}
