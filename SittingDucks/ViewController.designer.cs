// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace SittingDucks
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTextField labelCounter { get; set; }

		[Action ("counterButton:")]
		partial void counterButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (labelCounter != null) {
				labelCounter.Dispose ();
				labelCounter = null;
			}
		}
	}
}
