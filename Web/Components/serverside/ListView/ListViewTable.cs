// ListViewTable.cs
// Developing Microsoft ASP.NET Server Controls and Components
// Copyright © 2002, Nikhil Kothari and Vandana Datye
//

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MSPress.WebControls {

    [
    ToolboxItem(false)
    ]
    public class ListViewTable : Table {
        private int _columns;
        
        public ListViewTable() {
            _columns = 1;
            CellSpacing = 4;
            CellPadding = 0;
            GridLines = GridLines.None;
            BorderWidth = 0;
            Width = Unit.Percentage(100);
            Height = Unit.Percentage(100);
        }

        public int Columns {
            get {
                return _columns;
            }
            set {
                if (value < 1) {
                    throw new ArgumentOutOfRangeException("value");
                }
                _columns = value;
            }
        }

        protected override void RenderContents(HtmlTextWriter writer) {
            if (Rows.Count != 1) {
                return;
            }
            TableCellCollection cells = Rows[0].Cells;
            int cellsRendered = 0;
            bool endTagRequired = false;

            foreach (TableCell cell in cells) {
                if (cellsRendered == 0) {
                    writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                    endTagRequired = true;
                }

                if (cell.Visible) {
                    cell.RenderControl(writer);
                    cellsRendered++;
                }

                if (cellsRendered == _columns) {
                    writer.RenderEndTag();
                    endTagRequired = false;
                    cellsRendered = 0;
                }
            }

            if (endTagRequired) {
                writer.RenderEndTag();
            }
        }
    }
}
