using MaterialDesignThemes.Wpf;
using BookStore_Management;
using BookStore_Management.Data;
using BookStore_Management.View;
using BookStore_Management.ViewModel.KhoSach;
using BookStore_Management.ViewModel.TaiKhoan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Globalization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BookStore_Management.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region command
        public ICommand LoadedCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand ChangeCheckBoxCommand { get; set; }
        public ICommand LoginCommand { get; set; }
        public ICommand WindowNomalSizeCommand { get; set; }
        public ICommand ClosedCommand { get; set; }
        public ICommand MoreInfoCommand { get; set; }
        #endregion
        #region get set
        private string _Message = "";
        private string _Username = "";
        public bool IsRememberMeCheck { get; set; }
        public string Username { get => _Username; set { _Username = value; OnPropertyChange(); } }
        public string Password { get; set; } = "";
        public new string Message { get => _Message; set { _Message = ""; OnPropertyChange(); _Message = value; OnPropertyChange(); } }
        #endregion
        
        private Login Host { get => base.Parent as Login; set => base.Parent = value; }
        private MainWindow MainWindow { get; } = new MainWindow();

        public LoginViewModel()
        {
            LoadedCommand = new RelayCommand<Login>(p => { return true; }, p => { Host = p; LoadLastWork(); });
            ChangeCheckBoxCommand = new RelayCommand<Grid>(p => { return true; }, p => { ChangeCheckBox(p); });
            LoginCommand = new RelayCommand<Login>(p => { return true; }, p => { LoginSuccess(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>(p => { return true; }, p => { Password = p.Password; });
            WindowNomalSizeCommand = new RelayCommand<Window>(p => { return true; }, p => { if (p.WindowState==WindowState.Maximized) p.WindowState = WindowState.Normal; });
            ClosedCommand = new RelayCommand<object>(p => { return true; }, p => { MainWindow?.Close(); ResetCache(); SaveWork(); });
            MoreInfoCommand = new RelayCommand<object>(p => { return true; }, p => { MainWindow.WindowState = WindowState.Minimized; System.Diagnostics.Process.Start("https://github.com/LichND/Bookstore_Management"); });
        }

        private string lastwork = "";
        private static string LogFile { get; } = "./Log.ini";
        private void LoadLastWork()
        {
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "MM-dd-yyyy";
            newCulture.DateTimeFormat.DateSeparator = "-";
            System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;

            if (!File.Exists("Database.db"))
            {
                File.Copy("../../Database.db", "Database.db");
            }

            if (!File.Exists("log.ini"))
                return;

            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream("log.ini", FileMode.Open);
                LoginInfo info = formatter.Deserialize(stream) as LoginInfo;
                stream.Close();
                Username = info.Username;
                if (info.SavePass) 
                {
                    Password = info.Password;
                    Host._Password.Password = Password;
                    ChangeCheckBox(Host._CheckBox);
                }
            }
            catch { };
        }
        private void SaveWork()
        {
            LoginInfo info = new LoginInfo() { Username = Username, Password = Password, SavePass = IsRememberMeCheck };
            FileStream stream = new FileStream("log.ini", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, info);
            stream.Close();
        }

        private void ChangeCheckBox(Grid g)
        {
            try
            {
                if (IsRememberMeCheck)
                {
                    (g.Children[0] as PackIcon).Visibility = Visibility.Visible;
                    (g.Children[1] as PackIcon).Visibility = Visibility.Hidden;
                }
                else
                {
                    (g.Children[0] as PackIcon).Visibility = Visibility.Hidden;
                    (g.Children[1] as PackIcon).Visibility = Visibility.Visible;
                }
                IsRememberMeCheck = !IsRememberMeCheck;
            }
            catch { };
        }
        private void ResetCache()
        {
            (Application.Current.FindResource("MainVM") as MainViewModel)?.ResetCache();
            (Application.Current.FindResource("TaiKhoanVM") as TaiKhoanPageViewModel)?.ResetCache();
            (Application.Current.FindResource("QuanLy_TKVM") as QuanLy_TKViewModel)?.ResetCache();
            (Application.Current.FindResource("KhoSachVM") as KhoSachPageViewModel)?.ResetCache();
            (Application.Current.FindResource("ThongTin_TKVM") as ThongTin_TKViewModel)?.ResetCache();
        }
        private void LoginSuccess(Login w)
        {
            if (Username.Length == 0)
            {
                Message = "Tài khoản không được để trống!";
                return;
            }
            if (Password.Length == 0)
            {
                Message = "Mật khẩu không được để trống!";
                return;
            }
            AccountData User = AcceptAccount();
            if (User is null) // login failed
            {
                Message = "Sai tài khoản hoặc mật khẩu";
                return;
            }
            Message = "";
            if (lastwork != "" && lastwork != Username)
                ResetCache();
            w.Hide();

            (MainWindow.DataContext as MainViewModel).User = User;
            MainWindow.ShowDialog();
            User.LastLogin = DateTime.Now;
            UpdateLastLogin(User);
            switch ((MainWindow.DataContext as MainViewModel).Message.Type)
            {
                case BookStore_Management.Message.MessageType.Logout:
                    w._Password.Password = "";
                    ResetCache();
                    break;
                case BookStore_Management.Message.MessageType.Quit:
                case BookStore_Management.Message.MessageType.None:
                    w.Close();
                    return;
                case BookStore_Management.Message.MessageType.Back:
                    lastwork = Username;
                    break;
                default:
                    break;
            }
            w.Show();
        }
        private void UpdateLastLogin(AccountData User)
        {
            SQLiteDataAccess.Update("TaiKhoan", "LastLogin", User.LastLogin.ToString(), User.ID);
        }
        private AccountData AcceptAccount()
        {
            string Query = $"SELECT * FROM TaiKhoan WHERE Username = \"{Username}\" AND Password = \"{Hash.FromString(Password)}\"";
            return SQLiteDataAccess<AccountData>.Select1(Query);
        }

        [System.Serializable]
        private class LoginInfo
        {
            public string Username { get; set; } = "";
            public string Password { get; set; } = "";
            public bool SavePass { get; set; } = false;
        }
    }
}
