using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Application = System.Windows.Application;
using Clipboard = System.Windows.Clipboard;
using ContextMenu = System.Windows.Forms.ContextMenu;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MenuItem = System.Windows.Forms.MenuItem;


namespace ActivitySampling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly NotifyTimer  _notifier;
        private readonly LogFile _logFile;
        private readonly NotifyIcon _trayIcon;
        private string _activity;
        private int _intervall = 15 * 60 * 1000;

        public MainWindow()
        {
            InitializeComponent();
            
            _notifier = new NotifyTimer(_intervall);
            _logFile = new LogFile();
            _logFile.Init();

            _logFile.Write("Start Logging");

            ReadLog();

            //_notifier.OnBallonClicked += NotifierTimerOnBallonClicked;
            _notifier.OnTimerElapsed += NotifierOnOnTimerElapsed;

            var menuItems = new List<MenuItem>();
            MenuItem menuItemExit = new MenuItem("Exit");
            menuItemExit.Click += MenuItemOnClick;
            menuItems.Add(menuItemExit);
            ContextMenu contextMenu = new ContextMenu(menuItems.ToArray());

            var test = Resources;
            _trayIcon = new NotifyIcon
            {
                Text = "ActivitySampler",
                BalloonTipText = "ActivitySampler",
                BalloonTipTitle = "ActivitySampler",
                Visible = true,
                Icon = new Icon("ActivityMonitor.ico"),
                ContextMenu = contextMenu
                
            };
            _trayIcon.BalloonTipClicked += TrayIconOnBalloonTipClicked;
            //ballon.BalloonTipClosed += BallonNotifierOnBalloonTipClosed;
            TxtActivity.KeyUp += TxtActivityOnKeyUp;
            TxtActivity.TextChanged += TxtActivityTextChanged;
            LstOutput.MouseDoubleClick += LstOutputOnMouseDoubleClick;
            LstOutput.KeyUp += LstOutputOnKeyUp;
        }

        private void MenuItemOnClick(object sender, EventArgs eventArgs)
        {
            var menuItem = (MenuItem)sender;

            switch (menuItem?.Text)
            {
                case "Exit":
                    Application.Current.Shutdown();
                    break;

                default:
                    throw new ArgumentException();
            }
        }

        private void MainWindow_OnActivated(object sender, EventArgs e)
        {
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _notifier.Dispose();
            _logFile.Write("Stop Logging");
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
            var msg = _logFile.Write(TxtActivity.Text);
            LstOutput.Items.Insert(0, msg);
        }
        
        private void NotifierOnOnTimerElapsed(object sender, EventArgs eventArgs)
        {
            var activity = string.IsNullOrEmpty(_activity) ? "" : $"\n\nLast activity: {_activity}";
            _trayIcon.ShowBalloonTip(8000, "Current Activity", $"What are you doing? {activity}", ToolTipIcon.Info);
        }

        private void TrayIconOnBalloonTipClicked(object sender, EventArgs eventArgs)
        {
            WriteLog();
        }

        private void BtnRestart_OnClick(object sender, RoutedEventArgs e)
        {
            _intervall = int.Parse(TxtIntervall.Text) * 60 * 1000;
            _notifier.SetIntervall(_intervall);
        }

        private void LstOutputOnMouseDoubleClick(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var entry = LstOutput.SelectedItem as LogEntry;
            TxtActivity.Text = entry?.Message;
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

        private void TxtActivityOnKeyUp(object sender, KeyEventArgs keyEventArgs)
        {
            var key = keyEventArgs.Key;

            if (key == Key.Enter)
            {
                WriteLog();
            }
        }

        private void TxtActivityTextChanged(object sender, TextChangedEventArgs e)
        {
            _activity = TxtActivity.Text;
        }
    }
}
