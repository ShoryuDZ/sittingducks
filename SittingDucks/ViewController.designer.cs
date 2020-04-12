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
		AppKit.NSButton CancelSearch { get; set; }

		[Outlet]
		AppKit.NSButtonCell NewAccountButton { get; set; }

		[Outlet]
		AppKit.NSSecureTextField passwordField { get; set; }

		[Outlet]
		AppKit.NSTableView recordTable { get; set; }

		[Outlet]
		AppKit.NSButton SearchButton { get; set; }

		[Outlet]
		AppKit.NSSearchField SearchField { get; set; }

		[Outlet]
		AppKit.NSTextField websiteField { get; set; }

		[Action ("EnterSearch:")]
		partial void EnterSearch (Foundation.NSObject sender);

		[Action ("GeneratePasswordButton:")]
		partial void GeneratePasswordButton (Foundation.NSObject sender);

		[Action ("newAccountButton:")]
		partial void newAccountButton (Foundation.NSObject sender);

		[Action ("searchButton:")]
		partial void searchButton (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (SearchField != null) {
				SearchField.Dispose ();
				SearchField = null;
			}

			if (accountField != null) {
				accountField.Dispose ();
				accountField = null;
			}

			if (NewAccountButton != null) {
				NewAccountButton.Dispose ();
				NewAccountButton = null;
			}

			if (passwordField != null) {
				passwordField.Dispose ();
				passwordField = null;
			}

			if (recordTable != null) {
				recordTable.Dispose ();
				recordTable = null;
			}

			if (SearchButton != null) {
				SearchButton.Dispose ();
				SearchButton = null;
			}

			if (websiteField != null) {
				websiteField.Dispose ();
				websiteField = null;
			}

			if (CancelSearch != null) {
				CancelSearch.Dispose ();
				CancelSearch = null;
			}
		}
	}
}
