using JayLib.WPF.BasicClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace DRJ
{
    public class MainWindowViewModel : NotificationObject
    {

        #region floating Save button demo


        private bool _isSaving;
        public bool IsSaving
        {
            get { return _isSaving; }
            private set { this.PropertyUpdate(ref _isSaving, value, () => IsSaving); }
        }

        private bool _isSaveComplete;
        public bool IsSaveComplete
        {
            get { return _isSaveComplete; }
            private set { this.PropertyUpdate(ref _isSaveComplete, value, () => IsSaveComplete); }
        }

        private double _saveProgress;
        public double SaveProgress
        {
            get { return _saveProgress; }
            private set { this.PropertyUpdate(ref _saveProgress, value, () => SaveProgress); }
        }

        #endregion

        public ICommand SaveCommand { get; }

        public MainWindowViewModel()
        {
            //just some demo code for the SAVE button
            SaveCommand = new RelayCommand(SaveExecute);
        }

        private void SaveExecute()
        {
            if (IsSaveComplete == true)
            {
                IsSaveComplete = false;
                return;
            }

            if (SaveProgress != 0) return;

            var started = DateTime.Now;
            IsSaving = true;

            new DispatcherTimer(
                TimeSpan.FromMilliseconds(50),
                DispatcherPriority.Normal,
                new EventHandler((o, e) =>
                {
                    var totalDuration = started.AddSeconds(3).Ticks - started.Ticks;
                    var currentProgress = DateTime.Now.Ticks - started.Ticks;
                    var currentProgressPercent = 100.0 / totalDuration * currentProgress;

                    SaveProgress = currentProgressPercent;

                    if (SaveProgress >= 100)
                    {
                        IsSaveComplete = true;
                        IsSaving = false;
                        SaveProgress = 0;
                        ((DispatcherTimer)o).Stop();
                    }

                }), Dispatcher.CurrentDispatcher);
        }
    }
}
