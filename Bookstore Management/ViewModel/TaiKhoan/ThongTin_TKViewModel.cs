using Microsoft.Win32;
using BookStore_Management.Data;
using BookStore_Management.View;
using BookStore_Management.View.TaiKhoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BookStore_Management.ViewModel.TaiKhoan
{
    public class ThongTin_TKViewModel : BaseViewModel
    {
        #region Command
        public ICommand SetAccountCommand { get; set; }
        public ICommand UpdateAvatarCommand { get; set; }
        public ICommand UpdateCommand { get; set; }
        #endregion
        private AccountData _User;
        public AccountData User { get => _User; set { _User = value; OnPropertyChange(); } }
        public ThongTin_TKViewModel()
        {
            SetAccountCommand = new RelayCommand<object>(p => { return User is null; }, p => { User = MainViewModel.GetCurentUser; });
            UpdateAvatarCommand = new RelayCommand<object>(p => { return true; }, p => { AddPicture(); });
            UpdateCommand = new RelayCommand<object>(p => { return true; }, p => { Update(); });
        }

        private void AddPicture()
        {
            OpenFileDialog open = new OpenFileDialog() { Multiselect = false, Filter = "Image files (*.jpg, *.png)|*.jpg;*.png|All files (*.*)|*.*" };
            if (open.ShowDialog() == true) 
            {
                User.Avatar = new BitmapImage(new Uri(open.FileName));
                SQLiteDataAccess.Update("TaiKhoan", "AvatarEncode", User.AvatarEncode, User.ID);
            }
        }
        private void Update()
        {
            Update_TKViewModel SuaVM = new Update_TKViewModel() { User = User.Clone() as AccountData };
            Update_TK sua_TK = new Update_TK() { DataContext = SuaVM };
            sua_TK.ShowDialog();
            if (SuaVM.Message.Type == Message.MessageType.OK)
            {
                if (User.NickName != SuaVM.User.NickName)
                {
                    User.NickName = SuaVM.User.NickName;
                    SQLiteDataAccess.Update("TaiKhoan", "NickName", SuaVM.User.NickName, SuaVM.User.ID);
                }
                if (User.Sex != SuaVM.User.Sex)
                {
                    User.Sex = SuaVM.User.Sex;
                    SQLiteDataAccess.Update("TaiKhoan", "Sex", SuaVM.User.Sex, SuaVM.User.ID);
                }
                if (User.Phone != SuaVM.User.Phone)
                {
                    User.Phone = SuaVM.User.Phone;
                    SQLiteDataAccess.Update("TaiKhoan", "Phone", SuaVM.User.Phone, SuaVM.User.ID);
                }
                if (User.Address != SuaVM.User.Address)
                {
                    User.Address = SuaVM.User.Address;
                    SQLiteDataAccess.Update("TaiKhoan", "Address", SuaVM.User.Address, SuaVM.User.ID);
                }
            }
        }

        public void ResetCache()
        {
            User = null;
        }
    }
}
