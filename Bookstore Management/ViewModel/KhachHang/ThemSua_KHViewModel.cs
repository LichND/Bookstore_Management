using BookStore_Management.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore_Management.ViewModel.KhachHang
{
    public class ThemSua_KHViewModel : BaseViewModel
    {
        #region command
        public ICommand OKCommand { get; set; }
        public ICommand BackCommand { get; set; }
        public ICommand CanDropCommand { get; set; }
        #endregion
        private bool _IsReadOnly = true;
        private string _NameCustomer = "";
        private CustomerData _Customer;

        public bool IsReadOnly { get => _IsReadOnly; set { _IsReadOnly = value; OnPropertyChange(); } }
        public string NameCustomer { get => _NameCustomer; set { _NameCustomer = value; OnPropertyChange(); Find(); } }
        public CustomerData Customer { get => _Customer; set { _Customer = value; OnPropertyChange(); } }
        public View.KhachHang.ThemSua_KH Host { get => base.Parent as View.KhachHang.ThemSua_KH; set => base.Parent = value; }
        public ThemSua_KHViewModel()
        {
            OKCommand = new RelayCommand<object>(p => { return NameCustomer.Replace(" ","") != ""; }, p => { Host.Close(); Message.Type = Message.MessageType.OK; });
            BackCommand = new RelayCommand<object>(p => { return true; }, p => { Host.Close(); Message.Type = Message.MessageType.Back; });
            CanDropCommand = new RelayCommand<System.Windows.Controls.ComboBox>(p => { return true; }, p => { if (IsReadOnly) p.IsDropDownOpen = false; });
            worker = new MyBackgroundWorker();
            worker.DoWork += FindCustomer;
            worker.UpdateUI += UpdateUI;
        }

        #region Find available customer
        private void Find()
        {
            IsReadOnly = true;
            Customer = new CustomerData();
            worker.RunWorkerAsync();
        }
        private MyBackgroundWorker worker { get; set; }
        private void FindCustomer(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(200);
            CustomerData ret = null;
            foreach (var item in LogicData.AllCustomers)
            {
                if ((sender as BackgroundWorker).CancellationPending)
                    return;
                if (item.Name == NameCustomer)
                    ret = item;
            }
            if (ret is null)
                IsReadOnly = false;
            else
                (sender as BackgroundWorker).ReportProgress(0, ret);
        }
        private void UpdateUI(object sender, ProgressChangedEventArgs e)
        {
            Customer = e.UserState as CustomerData;
        }
        #endregion
    }
}
