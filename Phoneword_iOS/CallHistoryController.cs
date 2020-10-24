using Foundation;
using System;
using UIKit;
using System.Collections.Generic;


namespace Phoneword_iOS
{
    public partial class CallHistoryController : UITableViewController
    {

        public List<string> PhoneNumbers { get; set; }

        static NSString callhistoryCellId = new NSString("callHistoryCell");
            
        public CallHistoryController (IntPtr handle) : base (handle)
        {
            TableView.RegisterClassForCellReuse(typeof(UITableViewCell),callhistoryCellId);
            TableView.Source = new CallhistoryDataSource(this);
            PhoneNumbers = new List<string>();
        }

        class CallhistoryDataSource : UITableViewSource
        {
            CallHistoryController controller;

            public CallhistoryDataSource(CallHistoryController controller)
            {
                this.controller = controller;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return controller.PhoneNumbers.Count;
            }

            public override UITableViewCell GetCell(UITableView tableview, NSIndexPath indexpath)
            {
                var cell = tableview.DequeueReusableCell(CallHistoryController.callhistoryCellId);

                int row = indexpath.Row;
                cell.TextLabel.Text = controller.PhoneNumbers[row];
                return cell;
            }
        }
    }
}