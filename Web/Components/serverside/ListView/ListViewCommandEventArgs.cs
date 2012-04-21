// ListViewCommandEventArgs.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace MSPress.WebControls {

    public class ListViewCommandEventArgs : CommandEventArgs {
    
        private ListViewItem _item;
        private object _commandSource;
        private ListViewCommandType _commandType;

        public ListViewCommandEventArgs(ListViewItem item, object commandSource, CommandEventArgs originalArgs) : base(originalArgs) {
            _item = item;
            _commandSource = commandSource;

            string cmdName = originalArgs.CommandName;
            if (String.Compare(cmdName, ListView.SelectCommandName, true, CultureInfo.InvariantCulture) == 0) {
                _commandType = ListViewCommandType.Select;
            }
            else if (String.Compare(cmdName, ListView.EditCommandName, true, CultureInfo.InvariantCulture) == 0) {
                _commandType = ListViewCommandType.Edit;
            }
            else if (String.Compare(cmdName, ListView.UpdateCommandName, true, CultureInfo.InvariantCulture) == 0) {
                _commandType = ListViewCommandType.Update;
            }
            else if (String.Compare(cmdName, ListView.CancelEditCommandName, true, CultureInfo.InvariantCulture) == 0) {
                _commandType = ListViewCommandType.CancelEdit;
            }
            else if (String.Compare(cmdName, ListView.DeleteCommandName, true, CultureInfo.InvariantCulture) == 0) {
                _commandType = ListViewCommandType.Delete;
            }
            else {
                _commandType = ListViewCommandType.Custom;
            }
        }

        public object CommandSource {
            get {
                return _commandSource;
            }
        }

        public ListViewCommandType CommandType {
            get {
                return _commandType;
            }
        }
        
        public ListViewItem Item {
            get {
                return _item;
            }
        }
    }

    public delegate void ListViewCommandEventHandler(object sender, ListViewCommandEventArgs e);
}
