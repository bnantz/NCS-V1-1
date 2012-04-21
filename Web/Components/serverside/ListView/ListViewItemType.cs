// ListViewItemType.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;

namespace MSPress.WebControls {

    [Flags]
    public enum ListViewItemType {
        Header = 1,
        Footer = 2,
        Item = 4,
        EditItem = 8,
        SelectedItem = 16
    }
}
