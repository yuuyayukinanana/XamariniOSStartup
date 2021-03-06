﻿using Foundation;
using System;
using UIKit;
using System.Collections.Generic;



namespace Phoneword_iOS
{
    public partial class ViewController : UIViewController
    {

        string translatedNumber = "";

        public List<string> PhoneNumbers { get; set; }


        public ViewController(IntPtr handle) : base(handle)
        {
            PhoneNumbers = new List<string>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.


            //変換ボタン
            TranslateButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                //値の変換
                translatedNumber = PhoneTranslator.ToNumber(PhoneNumberText.Text);

                //キーボードを閉じる
                PhoneNumberText.ResignFirstResponder();

                //テキストが入っている場合はボタンを有効化
                if (translatedNumber == "")
                {
                    //ボタンのテキストを変更
                    CallButton.SetTitle("Call", UIControlState.Normal);
                    CallButton.Enabled = false;
                }
                else
                {
                    CallButton.SetTitle($"Call{translatedNumber}", UIControlState.Normal);
                    CallButton.Enabled = true;
                }
            };

            CallButton.TouchUpInside += (object sender, EventArgs e) =>
            {


                //リストに電話番号を追加
                PhoneNumbers.Add(translatedNumber);

                // Use URL handler with tel: prefix to invoke Apple's Phone app...
                var url = new NSUrl("tel:" + translatedNumber);

                // ...otherwise show an alert dialog
                if (!UIApplication.SharedApplication.OpenUrl(url))
                {
                    var alert = UIAlertController.Create("Not supported", "Scheme 'tel:' is not supported on this device", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("Ok", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
            };

            //セグエ無し遷移（viewControllerにストーリーボードIDの設定が必要）
            CallHistoryButton.TouchUpInside += (object sender, EventArgs e) => {
                // 対象のコントローラーを設定（InstantiateViewControllerの引数はストーリボードID）
                CallHistoryController callHistory = this.Storyboard.InstantiateViewController("CallHistoryController") as CallHistoryController;
                if (callHistory != null)
                {
                    callHistory.PhoneNumbers = PhoneNumbers;
                    this.NavigationController.PushViewController(callHistory, true);
                }
            };
        }




        //履歴に遷移するときにデータを渡す処理をオーバーライド
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            //セグエ対象のコントローラーを決める（キャストする）
            var callHistoryController = segue.DestinationViewController as CallHistoryController;

            //処理が落ちないようにnullチェック
            if (callHistoryController != null)
            {
                //リストを履歴のコントローラーに渡す
                callHistoryController.PhoneNumbers = PhoneNumbers;
            }
        }


        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}