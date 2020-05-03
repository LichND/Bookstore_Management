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
    public class Update_TKViewModel : BaseViewModel
    {
        #region command
        public ICommand OKCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion
        private AccountData _User;
        private string _WarningNickname = "";
        private string _WarningPhone = "";
        private string _WarningAddress = "";

        public AccountData User { get=>_User; set { _User = value; OnPropertyChange(); } }
        public string WarningNickname { get => _WarningNickname; set { _WarningNickname = value; OnPropertyChange(); } }
        public string WarningPhone { get => _WarningPhone; set { _WarningPhone = value; OnPropertyChange(); } }
        public string WarningAddress { get => _WarningAddress; set { _WarningAddress = value; OnPropertyChange(); } }

        private Update_TK Host { get => base.Parent as Update_TK; set => base.Parent = value; }
        public Update_TKViewModel()
        {
            SetParentCommand = new RelayCommand<Update_TK>(p => { return Host is null; }, p => { Host = p; });

            OKCommand = new RelayCommand<object>(p => { return true; }, p => { OK(); });
            CancelCommand = new RelayCommand<object>(p => { return true; }, p => { Host.Close(); });
        }

        private void OK()
        {
            if (User.NickName == "") 
            {
                WarningNickname = "";
                WarningNickname = "Nhập tên người dùng!";
                return;
            }
            if (User.Phone == "")
            {
                WarningPhone = "";
                WarningPhone = "Nhập số điện thoại!";
                return;
            }
            if (User.Address == "")
            {
                WarningAddress = "";
                WarningAddress = "Nhập địa chỉ!";
                return;
            }
            Host.Close();
            Message.Type = Message.MessageType.OK;
        }
    }
}
