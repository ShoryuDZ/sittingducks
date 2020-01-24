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
                AddNewAccount(websiteField.StringValue, accountField.StringValue, passwordField.StringValue);
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
                    var alert = new NSAlert()
                    {
                        AlertStyle = NSAlertStyle.Informational,
                        InformativeText = "Your password is insecure. Secure passwords contain at least 2 of each: special character, number, lowercase character, uppercase character.",
                        MessageText = "Insecure Password",
                    };
                    alert.AddButton("OK");
                    alert.AddButton("Ignore");
                    var result = alert.RunModal();

                    if (result == 1000)
                    {
                        passwordField.BackgroundColor = NSColor.Yellow;
                    }
                    if (result == 1001)
                    {
                        AddNewAccount(websiteField.StringValue, accountField.StringValue, passwordField.StringValue);
                    }
                }
            }
        }

        void AddNewAccount(string website, string account, string password)
        {
            DataSource.Records.Add(new Record(website, account, password));

            recordTable.DataSource = DataSource;
            recordTable.Delegate = new RecordTableDelegate(DataSource);

            recordTable.ReloadData();

            foreach (var textField in NSTextFields)
            {
                textField.StringValue = string.Empty;
                textField.BackgroundColor = NSColor.Clear;
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
