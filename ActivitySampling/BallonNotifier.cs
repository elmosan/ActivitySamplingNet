using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;

namespace ActivitySampling
{
    public class BallonNotifier : IDisposable
    {
        //private Notifier _toastNotifier;

        public void Notify()
        {
            //_toastNotifier = new Notifier(cfg =>
            //{
            //    cfg.PositionProvider = new WindowPositionProvider(
            //        parentWindow: Application.Current.MainWindow,
            //        corner: Corner.TopRight,
            //        offsetX: 10,
            //        offsetY: 10);

            //    cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
            //        notificationLifetime: TimeSpan.FromSeconds(3),
            //        maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            //    cfg.Dispatcher = Application.Current.Dispatcher;
            //});

            //var title = "ActivitySampling";
            //var message = "Test Messsage";

            //var options = new MessageOptions
            //{
            //    NotificationClickAction = n =>
            //    {
            //        n.Close();
            //        _toastNotifier.ShowSuccess("clicked!");
            //    }
            //};

            //_toastNotifier.ShowInformation(message, options);
            //_toastNotifier.ShowSuccess(message);
            //_toastNotifier.ShowWarning(message);
            //_toastNotifier.ShowError(message);
        }

        public void Notify(string title, string message)
        {
            Thread.Sleep(4000);

            NotifyIcon ballon = new NotifyIcon
            {
                Text = message,
                BalloonTipText = message,
                BalloonTipTitle = title,
                Visible = true,
                Icon = new Icon("ActivityMonitor.ico")
            };
            ballon.BalloonTipClicked += BallonNotifierOnBalloonTipClicked;
            //ballon.BalloonTipClosed += BallonNotifierOnBalloonTipClosed;
            ballon.ShowBalloonTip(3000, title, message, ToolTipIcon.Info);
        }

        public event EventHandler OnBalloonTipClicked;

        public void Dispose()
        {
            //_toastNotifier.Dispose();
            //_ballon.BalloonTipClicked -= BallonNotifierOnBalloonTipClicked;
        }

        private void BallonNotifierOnBalloonTipClicked(object o, EventArgs eventArgs)
        {
            OnBalloonTipClicked?.Invoke(this, null);
        }

        private void BallonNotifierOnBalloonTipClosed(object o, EventArgs eventArgs)
        {
            //_ballon.BalloonTipClicked -= BallonNotifierOnBalloonTipClicked;
        }
    }
}
