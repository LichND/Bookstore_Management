using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management
{
    public class MyBackgroundWorker
    {
        public bool RefreshAble { get; set; } = true;
        private BackgroundWorker Worker { get; } = new BackgroundWorker() { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
        private bool Restart { get; set; } = false;

        public MyBackgroundWorker()
        {
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        public event DoWorkEventHandler DoWork { add => Worker.DoWork += value; remove => Worker.DoWork -= value; }
        public event ProgressChangedEventHandler UpdateUI { add => Worker.ProgressChanged += value; remove => Worker.ProgressChanged -= value; }
        public event RunWorkerCompletedEventHandler RunWorkerCompleted { add => Worker.RunWorkerCompleted += value; remove => Worker.RunWorkerCompleted -= value; }

        public void RunWorkerAsync()
        {
            if (RefreshAble && Worker.IsBusy)
            {
                Restart = true;
                Worker.CancelAsync();
            }
            else
                Worker.RunWorkerAsync();
        }
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (RefreshAble && Restart)
            {
                Restart = false;
                Worker.RunWorkerAsync();
            }
        }
    }
}
