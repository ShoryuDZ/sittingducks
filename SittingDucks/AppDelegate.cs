using System;
using System.IO;
using AppKit;
using Foundation;

namespace SittingDucks
{
    [Register("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate()
        {
        }

        partial void HelpButton(NSObject sender)
        {
            string applicationDirectory = Directory.GetCurrentDirectory();
            string myFile = Path.Combine(applicationDirectory, "help.html");
            string helpFile = "file:///" + myFile;

            System.Diagnostics.Process.Start(helpFile);
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            // Insert code here to initialize your application
            // APPDELEGATE DFL not required
        }

        public override void WillTerminate(NSNotification notification)
        {
            // Insert code here to tear down your application
        }
    }
}
