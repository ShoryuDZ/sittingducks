using System;
using AppKit;
using CoreGraphics;

namespace SittingDucks
{
    public class RecordTableDelegate : NSTableViewDelegate
    {
        public RecordTableDelegate(RecordTableDataSource dataSource, ViewController controller)
        {
            this.DataSource = dataSource;
            this.Controller = controller;
        }

        private const string CellIdentifier = "RecordCell";

        private RecordTableDataSource DataSource;
        private ViewController Controller;

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data
            NSTableCellView view = (NSTableCellView)tableView.MakeView(tableColumn.Title, this);
            if (view == null)
            {
                view = new NSTableCellView();

                // Configure the view
                view.Identifier = tableColumn.Title;

                // Take action based on title
                switch (tableColumn.Title)
                {
                    case "Website/Service":
                    case "Account Name":
                    case "Password":
                        view.TextField = new NSTextField(new CGRect(0, 0, 400, 16));
                        ConfigureTextField(view, row);
                        break;
                    case "Delete":
                        // Create new button
                        var button = new NSButton(new CGRect(0, 0, 20, 16));
                        button.Tag = row;
                        button.Bordered = false;
                        button.Cell = new NSButtonCell { BackgroundColor = NSColor.DarkGray, Title = "-" };

                        // Wireup events
                        button.Activated += (sender, e) =>
                        {
                            // Get button and product
                            var btn = sender as NSButton;
                            var record = DataSource.Records[(int)btn.Tag];
                            var connection = SqliteManager.GetDatabaseConnection();

                            // Configure alert
                            var alert = new NSAlert()
                            {
                                AlertStyle = NSAlertStyle.Informational,
                                InformativeText = $"Are you sure you want to delete data for {record.Website}? This operation cannot be undone.",
                                MessageText = $"Delete data for {record.Website}?",
                            };
                            alert.AddButton("Cancel");
                            alert.AddButton("Delete");
                            alert.BeginSheetForResponse(Controller.View.Window, (result) =>
                            {
                                // Should we delete the requested row?
                                if (result == 1001)
                                {
                                    // Remove the given row from the dataset
                                    DataSource.RemoveRecord(record, connection);
                                    Controller.PushView();
                                }
                            });
                        };

                        // Add to view
                        view.AddSubview(button);
                        break;
                }

            }

            // Setup view based on the column selected
            switch (tableColumn.Title)
            {
                case "Website/Service":
                    view.TextField.StringValue = DataSource.Records[(int)row].Website;
                    break;
                case "Account Name":
                    view.TextField.StringValue = DataSource.Records[(int)row].AccountName;
                    break;
                case "Password":
                    view.TextField.StringValue = DataSource.Records[(int)row].Password;
                    break;
                case "Delete":
                    foreach (NSView subview in view.Subviews)
                    {
                        var btn = subview as NSButton;
                        if (btn != null)
                        {
                            btn.Tag = row;
                        }
                    }
                    break;
            }

            return view;
        }

        private void ConfigureTextField(NSTableCellView view, nint row)
        {
            // Add to view
            view.TextField.AutoresizingMask = NSViewResizingMask.WidthSizable;
            view.AddSubview(view.TextField);

            // Configure
            view.TextField.BackgroundColor = NSColor.Clear;
            view.TextField.Bordered = false;
            view.TextField.Selectable = true;
            view.TextField.Editable = false;

            // Wireup events
            view.TextField.EditingEnded += (sender, e) => {

                // Take action based on type
                switch (view.Identifier)
                {
                    case "Website/Service":
                        DataSource.Records[(int)row].Website = view.TextField.StringValue;
                        break;
                    case "Account Name":
                        DataSource.Records[(int)row].AccountName = view.TextField.StringValue;
                        break;
                    case "Password":
                        DataSource.Records[(int)row].Password = view.TextField.StringValue;
                        break;
                }
            };

            // Tag view
            view.TextField.Tag = row;
        }
    }
}
