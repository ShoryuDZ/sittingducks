// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;
using System.Threading.Tasks;

namespace SittingDucks
{
	public partial class WindowController : NSWindowController
	{
		public WindowController (IntPtr handle) : base (handle)
		{
		}

        async partial void SearchButtonToolbar(NSObject sender)
        {
            var query = await SearchTool.RunSearchWindow(this.Window);
            if (query != String.Empty)
            {
                var viewController = ContentViewController as ViewController;
                viewController.Search(query);
            }
        }

        partial void RemoveFiltersButton(NSObject sender)
        {
            var viewController = ContentViewController as ViewController;
            viewController.RemoveFilters();
        }
    }
}
