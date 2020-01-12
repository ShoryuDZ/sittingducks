using System;

using AppKit;
using Foundation;
using System.Linq;
using System.Collections.Generic;

namespace SittingDucks
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public RecordTableDataSource DataSource { get; set; }

        public NSTextField[] NSTextFields { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            recordTable.TableColumns().ElementAt(0).HeaderCell.StringValue = "Website";
            recordTable.TableColumns().ElementAt(1).HeaderCell.StringValue = "Account";
            recordTable.TableColumns().ElementAt(2).HeaderCell.StringValue = "Password";

            DataSource = new RecordTableDataSource();

            NSTextFields = new NSTextField[] { websiteField, accountField, passwordField };
        }

        partial void newAccountButton(NSObject sender)
        {
            if (websiteField.StringValue != string.Empty && accountField.StringValue != string.Empty && PasswordGenerator.IsSecure(passwordField.StringValue))
            {
                DataSource.Records.Add(new Record(websiteField.StringValue, accountField.StringValue, passwordField.StringValue));

                recordTable.DataSource = DataSource;
                recordTable.Delegate = new RecordTableDelegate(DataSource);

                recordTable.ReloadData();

                foreach (var textField in NSTextFields)
                {
                    textField.StringValue = string.Empty;
                    textField.BackgroundColor = NSColor.Clear;
                }
            }

            else
            {
                foreach (var textField in NSTextFields)
                {
                    if (textField.StringValue == string.Empty)
                    {
                        textField.BackgroundColor = NSColor.Red;
                    }
                    else
                    {
                        textField.BackgroundColor = NSColor.Clear;
                    }
                }

                if (passwordField.StringValue != String.Empty && !PasswordGenerator.IsSecure(passwordField.StringValue))
                {
                    passwordField.BackgroundColor = NSColor.Yellow;
                }
            }
        }

        partial void GeneratePasswordButton(NSObject sender)
        {
            passwordField.StringValue = PasswordGenerator.GeneratePassword();
        }

        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }
    }
}
