using System;

using AppKit;
using Foundation;
using System.Linq;

namespace SittingDucks
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public RecordTableDataSource DataSource { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            recordTable.TableColumns().ElementAt(0).HeaderCell.StringValue = "Website";
            recordTable.TableColumns().ElementAt(1).HeaderCell.StringValue = "Account";
            recordTable.TableColumns().ElementAt(2).HeaderCell.StringValue = "Password";

            DataSource = new RecordTableDataSource();
        }

        partial void newAccountButton(NSObject sender)
        {
            DataSource.Records.Add(new Record(websiteField.StringValue, accountField.StringValue, passwordField.StringValue));

            recordTable.DataSource = DataSource;
            recordTable.Delegate = new RecordTableDelegate(DataSource);

            recordTable.ReloadData();

            websiteField.StringValue = String.Empty;
            accountField.StringValue = String.Empty;
            passwordField.StringValue = String.Empty;
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
