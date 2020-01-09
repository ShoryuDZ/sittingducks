using System;

using AppKit;
using Foundation;

namespace SittingDucks
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        int buttonClickCounter;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            websiteHolder.StringValue = string.Empty;
            accountHolder.StringValue = string.Empty;
            passwordHolder.StringValue = string.Empty;
            // Do any additional setup after loading the view.
        }

        partial void newAccountButton(NSObject sender)
        {
            websiteHolder.StringValue += websiteField.StringValue + '\n';
            accountHolder.StringValue += accountField.StringValue + '\n';
            passwordHolder.StringValue += passwordField.StringValue + '\n';
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
