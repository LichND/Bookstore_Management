using BookStore_Management.Data;
using BookStore_Management.View.TaiKhoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore_Management.ViewModel.TaiKhoan
{
    public class Sua_QLTKViewModel : BaseViewModel
    {
        public ICommand OKCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AccountData User { get; set; }
        public string ResetPassword { get; set; } = "";

        private Sua_QLTK Host { get => base.Parent as Sua_QLTK; set => base.Parent = value; }
        public Sua_QLTKViewModel()
        {
            SetParentCommand = new RelayCommand<Sua_QLTK>(p => { return Host is null; }, p => { Host = p; });

            OKCommand = new RelayCommand<object>(p => { return true; }, p => { OK(); });
            CancelCommand = new RelayCommand<object>(p => { return true; }, p => { Host.Close(); });
        }

        private void OK()
        {
            if (ResetPassword != "")
            {
                User.Password = Hash.FromString(ResetPassword);
                User.PasswordLength = ResetPassword.Length;
            }
            Host.Close();
            Message.Type = Message.MessageType.OK;
        }
    }
}
