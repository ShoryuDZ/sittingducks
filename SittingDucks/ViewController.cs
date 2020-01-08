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

            labelCounter.StringValue = "Not clicked";

            buttonClickCounter = 0;
            // Do any additional setup after loading the view.
        }

        partial void counterButton(NSObject sender)
        {
            buttonClickCounter++;
            labelCounter.StringValue = string.Format("Number of clicks: {0}", buttonClickCounter);
            MainClass.doSomeThing();
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
