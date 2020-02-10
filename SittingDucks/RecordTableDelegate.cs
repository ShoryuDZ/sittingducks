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

        private RecordTableDataSource DataSource;
        private ViewController Controller;

        public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data
            NSTableCellView view = (NSTableCellView)tableView.MakeView(tableColumn.Title, this);

            view = new NSTableCellView();
            var showPassword = DataSource.Records[(int)row].ShowPassword;

            // Configure the view
            view.Identifier = tableColumn.Title;

            // Take action based on title
            switch (tableColumn.Title)
            {
                case "Website/Service":
                case "Account Name":
                    view.TextField = new NSTextField(new CGRect(0, 0, 400, 16));
                    ConfigureTextField(view, row);
                    break;
                case "Password":
                    view.TextField = showPassword ? new NSTextField(new CGRect(0, 0, 400, 16)) : new NSSecureTextField(new CGRect(0, 0, 400, 16));
                    ConfigureTextField(view, row);
                    break;
                case "Actions":
                    // Create new button
                    var removeButton = new NSButton(new CGRect(0, 0, 60, 16));
                    removeButton.Tag = row;
                    removeButton.Bordered = false;
                    removeButton.Cell = new NSButtonCell { BackgroundColor = NSColor.DarkGray, Title = "Remove" };

                    // Wireup events
                    removeButton.Activated += (sender, e) =>
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

                    view.AddSubview(removeButton);

                    var showButton = new NSButton(new CGRect(70, 0, 60, 16));
                    showButton.Tag = row;
                    showButton.Cell = new NSButtonCell { BackgroundColor = NSColor.DarkGray, Title = showPassword ? "Hide" : "Show" };

                    showButton.Activated += (sender, e) =>
                    {
                        var btn = sender as NSButton;
                        var selectedRecord = DataSource.Records[(int)btn.Tag];

                        foreach (var record in DataSource.Records)
                        {
                            record.ShowPassword = false;
                        }

                        selectedRecord.ShowPassword = showPassword ? false : true;
                        Controller.PushView();
                    };

                    view.AddSubview(showButton);
                    break;
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
                case "Actions":
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

            // Tag view
            view.TextField.Tag = row;
        }
    }
}
