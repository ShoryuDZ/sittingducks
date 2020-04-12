using System;
using AppKit;
using CoreGraphics;

namespace SittingDucks
{
    public static class SearchTool
    {
        public static string RunSearch()
        {
            var searchQuery = new NSSearchField(new CGRect(0, 0, 300, 20));

            var searchAlert = new NSAlert()
            {
                AlertStyle = NSAlertStyle.Informational,
                MessageText = "Enter Search Query",
            };
            searchAlert.AddButton("Enter");
            searchAlert.AddButton("Cancel");
            searchAlert.AccessoryView = searchQuery;
            searchAlert.Layout();
            var result = searchAlert.RunModal();

            searchAlert.Dispose();

            return result == 1000 ? searchQuery.StringValue : String.Empty;
        }
    }
}
