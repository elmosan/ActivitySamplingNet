using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Clipboard = System.Windows.Clipboard;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;


namespace ActivitySampling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NotifyTimer  _notifier = null;
        private readonly LogFile _logFile = null;
        private readonly NotifyIcon ballon = null;

        public MainWindow()
        {
            InitializeComponent();
            _notifier = new NotifyTimer(15 * 60 * 1000);
            _logFile = new LogFile();
            _logFile.Init();

            ReadLog();

            //_notifier.OnBallonClicked += NotifierTimerOnBallonClicked;
            _notifier.OnTimerElapsed += NotifierOnOnTimerElapsed;

            ballon = new NotifyIcon
            {
                Text = "ActivitySampler",
                BalloonTipText = "ActivitySampler",
                BalloonTipTitle = "ActivitySampler",
                Visible = true,
                Icon = new Icon("ActivityMonitor.ico")
            };
            ballon.BalloonTipClicked += BallonOnBalloonTipClicked;
            //ballon.BalloonTipClosed += BallonNotifierOnBalloonTipClosed;
            LstOutput.MouseDoubleClick += LstOutputOnMouseDoubleClick;
            LstOutput.KeyUp += LstOutputOnKeyUp;
        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _notifier.Dispose();
        }

        private void BtnLog_OnClick(object sender, RoutedEventArgs e)
        {
            WriteLog();
        }

        private void ReadLog()
        {
            var logs = _logFile.Load();

            foreach (var item in logs)
            {
                LstOutput.Items.Add(item);
            }
        }

        private void WriteLog()
        {
            var msg = $"{DateTime.Now}: {TxtActivity.Text}";
            LstOutput.Items.Insert(0, msg);
            _logFile.Write(msg);
        }
        
        private void NotifierOnOnTimerElapsed(object sender, EventArgs eventArgs)
        {
            ballon.ShowBalloonTip(3000, "Current Activity", "What are you doing?", ToolTipIcon.Info);
        }

        private void BallonOnBalloonTipClicked(object sender, EventArgs eventArgs)
        {
            WriteLog();
        }

        private void TxtIntervall_OnLostFocus(object sender, RoutedEventArgs e)
        {
            _notifier.SetIntervall(int.Parse(TxtIntervall.Text));
        }

        private void LstOutputOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var text = LstOutput.SelectedItem.ToString().Substring(21);
            TxtActivity.Text = text;
        }

        private void LstOutputOnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;
            var text = LstOutput.SelectedItem.ToString().Substring(21);

            if (key == Key.C && keyEventArgs.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {
                Clipboard.SetText(text);
                TxtActivity.Text = text;
            }

        }
    }
}
