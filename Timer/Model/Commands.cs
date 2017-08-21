using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Timer.Model
{
    class StartTimerCommand : ICommand
    {
        private readonly IntervalTimer intervalTimer;
        public StartTimerCommand(IntervalTimer intervalTimer)
        {
            if (intervalTimer == null)
                throw new ArgumentNullException("intervalTimer");
            this.intervalTimer = intervalTimer;
        }


        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            intervalTimer.Start();
        }
    }

    class PauseTimerCommand : ICommand
    {
        private readonly IntervalTimer intervalTimer;
        public PauseTimerCommand(IntervalTimer intervalTimer)
        {
            if (intervalTimer == null)
                throw new ArgumentNullException("intervalTimer");
            this.intervalTimer = intervalTimer;
        }


        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            intervalTimer.Pause();
        }
    }

    class StopTimerCommand : ICommand
    {
        private readonly IntervalTimer intervalTimer;
        public StopTimerCommand(IntervalTimer intervalTimer)
        {
            if (intervalTimer == null)
                throw new ArgumentNullException("intervalTimer");
            this.intervalTimer = intervalTimer;
        }


        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            intervalTimer.Stop();
        }
    }
}
