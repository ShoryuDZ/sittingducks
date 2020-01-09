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
		AppKit.NSTextField accountField { get; set; }

		[Outlet]
		AppKit.NSTextField accountHolder { get; set; }

		[Outlet]
		AppKit.NSSecureTextField passwordField { get; set; }

		[Outlet]
		AppKit.NSTextField passwordHolder { get; set; }

		[Outlet]
		AppKit.NSTextField websiteField { get; set; }

		[Outlet]
		AppKit.NSTextField websiteHolder { get; set; }

		[Action ("newAccountButton:")]
		partial void newAccountButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (websiteField != null) {
				websiteField.Dispose ();
				websiteField = null;
			}

			if (accountField != null) {
				accountField.Dispose ();
				accountField = null;
			}

			if (passwordField != null) {
				passwordField.Dispose ();
				passwordField = null;
			}

			if (websiteHolder != null) {
				websiteHolder.Dispose ();
				websiteHolder = null;
			}

			if (accountHolder != null) {
				accountHolder.Dispose ();
				accountHolder = null;
			}

			if (passwordHolder != null) {
				passwordHolder.Dispose ();
				passwordHolder = null;
			}
		}
	}
}
