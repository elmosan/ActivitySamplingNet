using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using Application = System.Windows.Application;

namespace ActivitySampling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Notifier _notifier = null;
        private LogFile _logFile = null;

        public MainWindow()
        {
            InitializeComponent();
            _logFile = new LogFile();
        }

        public void Notify()
        {
            _notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.TopRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            var title = "ActivitySampling";
            var message = "Test Messsage";

            var options = new MessageOptions { NotificationClickAction = n =>
            {
                n.Close();
                _notifier.ShowSuccess("clicked!");
            }};

            //notifier.ShowInformation(message, options);
            //notifier.ShowSuccess(message);
            //notifier.ShowWarning(message);
            //notifier.ShowError(message);
            
            NotifyIcon ballon = new NotifyIcon();
            ballon.Text = message;
            ballon.BalloonTipText = message;
            ballon.BalloonTipTitle = title;
            ballon.Visible = true;
            ballon.Icon = new Icon("ActivityMonitor.ico");
            ballon.ShowBalloonTip(1000, title, message, ToolTipIcon.Info);
            ballon.BalloonTipClicked += (sender, args) => { };
        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
            Notify();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _notifier.Dispose();
        }

        private void BtnLog_OnClick(object sender, RoutedEventArgs e)
        {
            LstOutput.Items.Insert(0, $"{DateTime.Now}: {TxtActivity.Text}");
            _logFile.Write(TxtActivity.Text);
        }
    }
}
