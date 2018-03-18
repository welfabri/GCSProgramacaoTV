using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using GCSProgramacaoTV.iOS.Model.Classes;
using GCSProgramacaoTV.Model.Interfaces;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(MessageiOS))]
namespace GCSProgramacaoTV.iOS.Model.Classes
{
    public class MessageiOS : IMessage
    {
        UIAlertController alert;
        NSTimer alertDelay;
        const double SHORT_DELAY = 2.0;
        const double LONG_DELAY = 3.5;

        public void LongAlert(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }
        public void ShortAlert(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                DismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        void DismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }
}