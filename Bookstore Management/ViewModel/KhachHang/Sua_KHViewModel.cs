using BookStore_Management.Data;
using BookStore_Management.View.KhachHang;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore_Management.ViewModel.KhachHang
{
    public class Sua_KHViewModel : BaseViewModel
    {
        public ICommand OKCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private CustomerData _Customer;
        public CustomerData Customer { get => _Customer; set { _Customer = value; OnPropertyChange(); } }

        private Sua_KH Host { get => base.Parent as Sua_KH; set => base.Parent = value; }
        public Sua_KHViewModel()
        {
            SetParentCommand = new RelayCommand<Sua_KH>(p => { return Host is null; }, p => { Host = p; });
            OKCommand = new RelayCommand<object>(p => { return true; }, p => { Host.Close(); Message.Type = Message.MessageType.OK; });
            CancelCommand = new RelayCommand<object>(p => { return true; }, p => { Host.Close(); Message.Type = Message.MessageType.Quit; });
        }
    }
}
