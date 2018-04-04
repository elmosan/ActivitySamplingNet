using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ActivitySampling
{
    public class NotifyTimer : IDisposable
    {
        private readonly Timer _timer;
        //private readonly BallonNotifier _ballonNotifier;

        public NotifyTimer(int intervall)
        {
            _timer = new Timer();
            //_ballonNotifier = new BallonNotifier();

            _timer.Interval = intervall;
            _timer.Elapsed += TimerOnElapsed;
            //_ballonNotifier.OnBalloonTipClicked += BallonNotifierOnOnBalloonTipClicked;
            _timer.Start();
        }

        //public event EventHandler OnBallonClicked;
        public event EventHandler OnTimerElapsed;

        public void Dispose()
        {
            _timer.Stop();
            _timer.Elapsed -= TimerOnElapsed;
            //_ballonNotifier.OnBalloonTipClicked -= BallonNotifierOnOnBalloonTipClicked;
        }

        public void SetIntervall(int intervall)
        {
            _timer.Stop();
            _timer.Interval = intervall;
            _timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            //_timer.Stop();
            //_ballonNotifier.Notify("Current Activity", "What are you doing?");

            OnTimerElapsed?.Invoke(this, null);
        }

        //private void BallonNotifierOnOnBalloonTipClicked(object o, EventArgs eventArgs)
        //{
        //    OnBallonClicked?.Invoke(this, null);
        //}
    }
}
