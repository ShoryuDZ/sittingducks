﻿using System;
using AppKit;
using CoreGraphics;
using System.Linq;
using System.Collections.Generic;

namespace SittingDucks
{
    public static class SearchTool
    {
        public static string RunSearchWindow()
        {
            var searchQuery = new NSSearchField(new CGRect(0, 0, 300, 20));

            var searchAlert = new NSAlert()
            {
                AlertStyle = NSAlertStyle.Informational,
                MessageText = "Enter Search Query"
            };

            var subissionButton = searchAlert.AddButton("Enter");

            searchAlert.AddButton("Cancel");
            searchAlert.AccessoryView = searchQuery;
            searchAlert.Layout();

            subissionButton.Enabled = false;
            searchQuery.Changed += (sender, e) =>
            {
                subissionButton.Enabled = searchQuery.StringValue != String.Empty;
            };

            var result = searchAlert.RunModal();


            searchAlert.Dispose();

            return result == 1000 ? searchQuery.StringValue : String.Empty;
        }

        public static (RecordTableDataSource, bool) SearchSource(RecordTableDataSource dataSource, string query)
        {
            var searchData = new RecordTableDataSource(dataSource.Records.Where(x => x.Website.Contains(query, StringComparison.InvariantCultureIgnoreCase) || x.AccountName.Contains(query, StringComparison.InvariantCultureIgnoreCase)).ToList());

            if (searchData.Records.Count == 0)
            {
                var searchAlert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Warning,
                    InformativeText = "No search results!"
                };

                searchAlert.RunModal();

                return (dataSource, false);
            }

            return (searchData, true);
        }
    }
}
