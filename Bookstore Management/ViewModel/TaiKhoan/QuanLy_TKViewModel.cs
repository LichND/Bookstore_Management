using BookStore_Management.Data;
using BookStore_Management.Interface;
using BookStore_Management.View.TaiKhoan;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore_Management.ViewModel.TaiKhoan
{
    public class QuanLy_TKViewModel : DataGridBaseViewModel<AccountData>
    {
        public string _WarningUsername = "";
        public string _WarningPassword = "";
        public string _WarningRePassword = "";
        
        public string Password { get; set; } = "";
        public string RePassword { get; set; } = "";
        public string WarningUsername { get => _WarningUsername; set { _WarningUsername = value; OnPropertyChange(); } }
        public string WarningPassword { get => _WarningPassword; set { _WarningPassword = value; OnPropertyChange(); } }
        public string WarningRePassword { get => _WarningRePassword; set { _WarningRePassword = value; OnPropertyChange(); } }

        public List<string> AccountType { get; } = new List<string>();
        public AccountData CurentUser { get; set; }

        private QuanLy_TK Host { get => base.Parent as QuanLy_TK; set => base.Parent = value; }
        public QuanLy_TKViewModel()
        {
            SetParentCommand = new RelayCommand<QuanLy_TK>(p => { return Host is null; }, p => { Host = p; LoadDataBase("TaiKhoan"); LoadLogic(); });
        }
        
        private void LoadLogic()
        {
            CurentUser = (Application.Current.FindResource("MainVM") as MainViewModel).User;
            for (int i = CurentUser.IDType + 1; i < LogicData.AccountType.Count; i++) 
                AccountType.Add(LogicData.AccountType[i]);
            NewItem.AccountType = AccountType.Last();
        }

        public void ResetCache()
        {
            Clear(null);
            Datas.Clear();
            AccountType.Clear();
            Host = null;
        }

        protected override void Worker_UpdateUI(object sender, ProgressChangedEventArgs e)
        {
            base.Worker_UpdateUI(sender, e);
            if ((e.UserState as AccountData).ID == CurentUser.ID)
                Datas[Datas.Count - 1] = CurentUser;
        }

        protected override bool CanEdit(object p) => base.CanEdit(p) && (CurentUser.IDType <= SelectedItem.IDType);
        protected override bool CanDelete(object p) => base.CanDelete(p) && (CurentUser.IDType < SelectedItem.IDType);

        protected override void Add(object p)
        {
            if (NewItem.Username == "")
            {
                WarningUsername = "";
                WarningUsername = "Tài khoản không được để trống";
                return;
            }
            if (SQLiteDataAccess.IsHaveData("SELECT Username FROM TaiKhoan WHERE Username=\"" + NewItem.Username + '\"'))
            {
                WarningUsername = "";
                WarningUsername = "Tài khoản đã được sử dụng";
                return;
            }
            if (Password =="")
            {
                WarningPassword = "";
                WarningPassword = "Mật khẩu không được để trống";
                return;
            }
            if (RePassword!=Password)
            {
                WarningRePassword = "";
                WarningRePassword = "Xác nhận mật khẩu không chính xác";
                return;
            }
            // them tk
            NewItem.ID = LogicData.NextID("TaiKhoan");
            NewItem.PasswordLength = Password.Length;
            NewItem.Password = Hash.FromString(Password);
            SQLiteDataAccess.Insert("TaiKhoan", NewItem);
            base.Add(null);
            NewItem.AccountType = AccountType.Last();
            Host._uc.IsOpen = false;
        }
        protected override void Edit(object p)
        {
            int index = Datas.IndexOf(SelectedItem);
            Sua_QLTKViewModel SuaVM = new Sua_QLTKViewModel() { User = SelectedItem.Clone() as AccountData };
            Sua_QLTK sua = new Sua_QLTK() { DataContext = SuaVM };
            sua.ShowDialog();
            if (SuaVM.Message.Type == Message.MessageType.OK) 
            {
                if (SuaVM.User.Password != SelectedItem.Password && SuaVM.ResetPassword != "")
                    SQLiteDataAccess.Update("TaiKhoan", "Password", SuaVM.User.Password, SuaVM.User.ID);
                if (SuaVM.User.IDType != SelectedItem.IDType)
                    SQLiteDataAccess.Update("TaiKhoan", "IDType", SuaVM.User.IDType.ToString(), SuaVM.User.ID);
                
                Datas[index] = (sua.DataContext as Sua_QLTKViewModel).User;
            }
        }
        protected override void Delete(object p)
        {
            SQLiteDataAccess.Delete("TaiKhoan", SelectedItem.ID);
            base.Delete(p);
        }
    }
}
