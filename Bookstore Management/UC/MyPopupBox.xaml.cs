using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStore_Management.UC
{
    /// <summary>
    /// Interaction logic for MyPopupBox.xaml
    /// </summary>

    [ContentProperty(nameof(Children))]
    public partial class MyPopupBox : UserControl
    {
        public static double DeltaAngle { get; set; } = 5.625d;
        public UIElementCollection Children
        {
            get => _Children.Children;
            set
            {
                foreach (var items in value)
                    _Children.Children.Add(items as UIElement);
            }
        }
        public bool IsOpen { get => _PopupBox.IsPopupOpen; set => _PopupBox.IsPopupOpen = value; }
        private double Angle { get; set; } = 0;
        private BackgroundWorker worker = new BackgroundWorker() { WorkerSupportsCancellation = true, WorkerReportsProgress = true };
        private bool ReplayWorker { get; set; } = false;
        public MyPopupBox()
        {
            InitializeComponent();
            
            worker.DoWork += RotatePopup_Work;
            worker.ProgressChanged += Change_Popup_Angle;
            worker.RunWorkerCompleted += Change_Angle_Completed;
        }
        private void RotatePopup_Work(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            while (Angle <= 45 && Angle >= 0)
            {
                Angle += DeltaAngle;
                if (Angle > 45)
                {
                    Angle = 45;
                    return;
                }
                if (Angle < 0)
                {
                    Angle = 0;
                    return;
                }
                if (worker.CancellationPending)
                    return;
                worker.ReportProgress((int)Angle);
                Thread.Sleep(33);
            }
        }
        private void Change_Popup_Angle(object sender, ProgressChangedEventArgs e)
        {
            _Angle.Angle = Angle;
        }
        private void Change_Angle_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            DeltaAngle = -DeltaAngle;
            if (ReplayWorker)
            {
                worker.RunWorkerAsync();
                ReplayWorker = false;
            }
        }
        private void PopupBox_IsPopupOpenChange(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy)
            {
                worker.CancelAsync();
                ReplayWorker = true;
            }
            else
                worker.RunWorkerAsync();
        }
    }
}
