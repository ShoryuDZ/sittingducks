using System;
using System.Collections.Generic;
using AppKit;

namespace SittingDucks
{
    public class RecordTableDataSource : NSTableViewDataSource
    {
        public RecordTableDataSource()
        {
        }

        public List<Record> Records = new List<Record>();

        public override nint GetRowCount(NSTableView tableView)
        {
            return Records.Count;
        }
    }
}
