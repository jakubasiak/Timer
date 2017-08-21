using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;

namespace Timer.Model
{
    class IntervalTimer : INotifyPropertyChanged
    {
        public IntervalTimer()
        {
            Work = 60;
            Rest = 60;
            Loop = 1;
            IsPaused = false;
            TimerSpanes = new List<TimeSpan>();
        }
        private bool isCounting = false;
        bool _isPaused;
        public bool IsPaused
        {
            get
            {
                return _isPaused;
            }
            set
            {
                _isPaused = value;
                OnPropertyChanged("IsPaused");
            }

        }
        bool isStoped = false;

        private string _timerCounterDown;
        public string TimerCounterDown
        {
            get
            {
                return _timerCounterDown;
            }
            set
            {
                _timerCounterDown = value;
                OnPropertyChanged("TimerCounterDown");
            }

        }
        private int _progressBarMaxValue =1;
        public int ProgressBarMaxValue
        {
            get
            {
                return _progressBarMaxValue;
            }
            set
            {
                _progressBarMaxValue = value;
                OnPropertyChanged("ProgressBarMaxValue");
            }

        }
        private int _curentProgressBarValue;
        public int CurentProgressBarValue
        {
            get
            {
                return _curentProgressBarValue;
            }
            set
            {
                _curentProgressBarValue = value;
                OnPropertyChanged("CurentProgressBarValue");
            }

        }
        private int _work;
        public int Work
        {
            get
            {
                return _work;
            }
            set
            {
                _work = value;
                OnPropertyChanged("Work");
            }

        }
        private int _rest;
        public int Rest
        {
            get
            {
                return _rest;
            }
            set
            {
                _rest = value;
                OnPropertyChanged("Rest");
            }

        }
        private int _loop;
        public int Loop
        {
            get
            {
                return _loop;
            }
            set
            {
                _loop = value;
                WorkSequenceCounter = value;
                RestSequenceCounter = value;
                OnPropertyChanged("Loop");
            }

        }

        private int _workSequenceCounter;
        public int WorkSequenceCounter
        {
            get
            {
                return _workSequenceCounter;
            }
            set
            {
                _workSequenceCounter = value;
                OnPropertyChanged("WorkSequenceCounter");
            }

        }
        private int _restSequenceCounter;
        public int RestSequenceCounter
        {
            get
            {
                return _restSequenceCounter;
            }
            set
            {
                _restSequenceCounter = value;
                OnPropertyChanged("RestSequenceCounter");
            }

        }
        //Dodanie polecenia przez właściwość
        private ICommand _startTimerCommand;
        public ICommand StartTimerCommand
        {
            get
            {
                if (_startTimerCommand == null)
                    _startTimerCommand = new StartTimerCommand(this);

                return _startTimerCommand;
            }
        }
        private ICommand _pauseTimerCommand;
        public ICommand PauseTimerCommand
        {
            get
            {
                if (_pauseTimerCommand == null)
                    _pauseTimerCommand = new PauseTimerCommand(this);

                return _pauseTimerCommand;
            }
        }
        private ICommand _stopTimerCommand;
        public ICommand StopTimerCommand
        {
            get
            {
                if (_stopTimerCommand == null)
                    _stopTimerCommand = new StopTimerCommand(this);

                return _stopTimerCommand;
            }
        }

        public List<TimeSpan> TimerSpanes { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }


        //Metody
        public void Start()
        {
            if (!isCounting)
            {
                isCounting = true;
                CreateCycles();

                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    CountingDown();
                    RemoveCountedFromTimeSpanes();
                    isCounting = false;
                }).Start();

            }
        }
        public void Pause()
        {
            if (isCounting)
                if (!IsPaused)
                    IsPaused = true;
                else
                    IsPaused = false;
        }
        public void Stop()
        {
            isStoped = true;
        }

        private void RemoveCountedFromTimeSpanes()
        {
            foreach (TimeSpan timeSpan in TimerSpanes.Where(x => x.IsActive).ToList<TimeSpan>())
            {
                TimerSpanes.Remove(timeSpan);
            }
        }

        private void CountingDown()
        {
            foreach (TimeSpan timeSpan in TimerSpanes)
            {
                ProgressBarMaxValue = timeSpan.Period;
                if (timeSpan.IsActive == true)
                {
                    for (int i = timeSpan.Period; i > 0; i--)
                    {
                        if(isStoped)
                        {
                            ClearAppOnStoping();

                            return;
                        }
                        while (_isPaused)
                        {
                            if (isStoped)
                            {
                                ClearAppOnStoping();
                                return;
                            }
                            else
                            {
                                Thread.Sleep(250);
                            }
                        }
                        timeSpan.CountDown();
                        TimerCounterDown = timeSpan.Period + " second";
                        CurentProgressBarValue = ProgressBarMaxValue - timeSpan.Period;
                        Thread.Sleep(1000);
                        if(timeSpan.Period==0)
                            SystemSounds.Beep.Play();
                    }

                    timeSpan.IsActive = false;
                }
                UpdateSequenceCounters();
            }
            ClearAppOnStoping();
        }

        private void ClearAppOnStoping()
        {
            _isPaused = false;
            isStoped = false;
            CurentProgressBarValue = 0;
            ProgressBarMaxValue = 1;
            TimerCounterDown = "";
            TimerSpanes.Clear();
            WorkSequenceCounter = Loop;
            RestSequenceCounter = Loop;
        }

        private void UpdateSequenceCounters()
        {
            WorkSequenceCounter = TimerSpanes.Where(x => x.IsActive).Where(x => x.SpaneType == SpaneType.WORK).Count();
            RestSequenceCounter = TimerSpanes.Where(x => x.IsActive).Where(x => x.SpaneType == SpaneType.REST).Count();
        }

        private void CreateCycles()
        {
            for (int i = 0; i < Loop; i++)
            {
                TimeSpan w = new TimeSpan() { Period = Work, SpaneType = SpaneType.WORK, IsActive = true };
                TimeSpan r = new TimeSpan() { Period = Rest, SpaneType = SpaneType.REST, IsActive = true };
                TimerSpanes.Add(w);
                TimerSpanes.Add(r);
            }
        }
    }
}
