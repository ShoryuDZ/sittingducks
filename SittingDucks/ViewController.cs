using System;

using AppKit;
using Foundation;
using System.Linq;
using Mono.Data.Sqlite;

namespace SittingDucks
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public RecordTableDataSource DataSource { get; set; }

        public NSTextField[] NSTextFields { get; set; }

        private SqliteConnection DatabaseConnection = null;
        private SqliteConnection _conn;

        public nint? indexToEdit = null;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            DatabaseConnection = SqliteManager.GetDatabaseConnection();

            Authenticator.Initialise(DatabaseConnection, _conn);

            DataSource = new RecordTableDataSource(DatabaseConnection);
            NSTextFields = new NSTextField[] { websiteField, accountField, passwordField };

            PushView();
        }

        partial void newAccountButton(NSObject sender)
        {
            if (websiteField.StringValue != string.Empty && accountField.StringValue != string.Empty && PasswordGenerator.IsSecure(passwordField.StringValue))
            {
                AddNewAccount(websiteField.StringValue, accountField.StringValue, passwordField.StringValue, indexToEdit);
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
                        AddNewAccount(websiteField.StringValue, accountField.StringValue, passwordField.StringValue, indexToEdit);
                    }
                }
            }
        }

        partial void searchButton(NSObject sender)
        {
            var query = SearchTool.RunSearch();
        }

        void AddNewAccount(string website, string account, string password, nint? index = null)
        {
            DataSource.AddRecord(new Record(website, account, password, DatabaseConnection), index);
            indexToEdit = null;

            PushView();
        }

        partial void GeneratePasswordButton(NSObject sender)
        {
            passwordField.StringValue = PasswordGenerator.GeneratePassword();
        }

        public void PushView()
        {
            recordTable.DataSource = DataSource;
            recordTable.Delegate = new RecordTableDelegate(DataSource, this);

            recordTable.ReloadData();

            foreach (var textField in NSTextFields)
            {
                textField.StringValue = string.Empty;
                textField.BackgroundColor = NSColor.Clear;
            }

            NewAccountButton.Title = "Create New Record";
        }

        public void RefillRecord(Record record, nint index)
        {
            NewAccountButton.Title = "Edit Record";
            websiteField.StringValue = record.Website;
            accountField.StringValue = record.AccountName;
            passwordField.StringValue = record.Password;

            indexToEdit = index;
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

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            // Get access to database
            DatabaseConnection = SqliteManager.GetDatabaseConnection();
        }
    }
}
