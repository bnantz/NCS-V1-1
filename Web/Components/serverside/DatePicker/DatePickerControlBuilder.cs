// DatePickerControlBuilder.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.Diagnostics;
using System.Web.UI;

namespace MSPress.WebControls {

    public class DatePickerControlBuilder : ControlBuilder {

        public override bool AllowWhitespaceLiterals() {
            return false;
        }
    }
}
