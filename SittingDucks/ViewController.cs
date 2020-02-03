using System;

using AppKit;
using Foundation;
using System.Linq;
using System.Data;
using System.IO;
using Mono.Data.Sqlite;
using System.Threading.Tasks;

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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            recordTable.TableColumns().ElementAt(0).HeaderCell.StringValue = "Website";
            recordTable.TableColumns().ElementAt(1).HeaderCell.StringValue = "Account";
            recordTable.TableColumns().ElementAt(2).HeaderCell.StringValue = "Password";

            DataSource = new RecordTableDataSource();

            NSTextFields = new NSTextField[] { websiteField, accountField, passwordField };

            DatabaseConnection = GetDatabaseConnection();
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
            DataSource.Records.Add(new Record(website, account, password, DatabaseConnection));

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

        private SqliteConnection GetDatabaseConnection()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string folder = Path.Combine(appData, "SittingDucks");
            string database = Path.Combine(folder, "database.db3");

            // Create the database if it doesn't already exist
            bool exists = File.Exists(database);
            if (!exists)
            {
                DirectoryInfo directoryInfo = Directory.CreateDirectory(folder);
                SqliteConnection.CreateFile(database);
            }

            // Create connection to the database
            var conn = new SqliteConnection("Data Source=" + database);

            // Set the structure of the database
            if (!exists)
            {
                var commands = new[] {"CREATE TABLE Data (ID TEXT, Website TEXT, Account TEXT, Password TEXT)"};
                conn.Open();
                foreach (var cmd in commands)
                {
                    using (var c = conn.CreateCommand())
                    {
                        c.CommandText = cmd;
                        c.CommandType = CommandType.Text;
                        c.ExecuteNonQuery();
                    }
                }
                conn.Close();
            }

            return conn;
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
            DatabaseConnection = GetDatabaseConnection();
        }
    }
}
