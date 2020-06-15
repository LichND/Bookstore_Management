using BookStore_Management.UC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore_Management.ViewModel
{
    public class MyMessageBoxViewModel : BaseViewModel
    {
        public ICommand OKCommand { get; set; } = null;
        public ICommand CancelCommand { get; set; } = null;

        public MyMessageBox Host { get; set; } = null;
        public bool IsHaveOK { get; set; } = true;
        public bool IsHaveCancel { get; set; } = true;

        private string _Content = "";
        public string Content { get => _Content; set { _Content = value; OnPropertyChange(); } }

        public MyMessageBoxViewModel()
        {
            if (IsHaveOK)
                OKCommand = new RelayCommand<object>(p => { return true; }, p => { Message.Type = Message.MessageType.OK; Host.Close(); });
            if (IsHaveCancel)
                CancelCommand = new RelayCommand<object>(p => { return true; }, p => { Message.Type = Message.MessageType.Quit; Host.Close(); });
        }
    }
}
