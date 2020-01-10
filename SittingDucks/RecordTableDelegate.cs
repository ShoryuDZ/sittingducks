using System;
using AppKit;

namespace SittingDucks
{
    public class RecordTableDelegate : NSTableViewDelegate
    {
        public RecordTableDelegate(RecordTableDataSource dataSource)
        {
            this.DataSource = dataSource;
        }

        private const string CellIdentifier = "RecordCell";

        private RecordTableDataSource DataSource;

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data
            NSTextField view = (NSTextField)tableView.MakeView(CellIdentifier, this);
            if (view == null)
            {
                view = new NSTextField();
                view.Identifier = CellIdentifier;
                view.BackgroundColor = NSColor.Clear;
                view.Bordered = false;
                view.Selectable = false;
                view.Editable = false;
            }

            // Setup view based on the column selected
            switch (tableColumn.Title)
            {
                case "Website":
                    view.StringValue = DataSource.Records[(int)row].Website;
                    break;
                case "Account":
                    view.StringValue = DataSource.Records[(int)row].AccountName;
                    break;
                case "Password":
                    view.StringValue = DataSource.Records[(int)row].Password;
                    break;
            }

            return view;
        }
    }
}
