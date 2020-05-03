using BookStore_Management.Data;
using BookStore_Management.View;
using BookStore_Management.View.KhoSach;
using BookStore_Management.View.TaiKhoan;
using BookStore_Management.ViewModel.TaiKhoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BookStore_Management.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        #region command
        public ICommand LogoutCommand { get; set; }
        public ICommand InfoCommand { get; set; }

        public ICommand NhaSachShowCommand { get; set; }
        public ICommand LichSuBanShowCommand { get; set; }
        public ICommand KhoSachShowCommand { get; set; }
        public ICommand NhanSuShowCommand { get; set; }
        public ICommand KhachHangShowCommand { get; set; }
        public ICommand TaiKhoanShowCommand { get; set; }
        #endregion
        #region get set
        private static AccountData _User = null;
        private int _SeletedMenuItem = 0;
        public AccountData User { get => _User; set { _User = value; OnPropertyChange(); } }
        public static AccountData GetCurentUser { get => _User; }
        public int SeletedMenuItem { get => _SeletedMenuItem; set { _SeletedMenuItem = value; OnPropertyChange(); } }
        #endregion
        private MainWindow Host { get => base.Parent as MainWindow; set => base.Parent = value; }

        #region navigation
        private Page _ContentZone;
        public Page ContentZone { get => _ContentZone; set { _ContentZone = value; OnPropertyChange(); } }

        private NhaSachPage NhaSachPage { get; } = new NhaSachPage(); 
        private LichSuBanPage LichSuBanPage { get; set; }
        private KhoSachPage KhoSachPage { get; set; }
        private KhachHangPage KhachHangPage { get; set; }
        private TaiKhoanPage TaiKhoanPage { get; set; }
        #endregion
        public MainViewModel()
        {
            ContentZone = NhaSachPage;

            SetParentCommand = new RelayCommand<MainWindow>(p => { return Host is null; }, p => { Host = p; LogicData.Load(); });
            InfoCommand = new RelayCommand<object>(p => { return true; }, p => { ShowInfoTK(); });
            LogoutCommand = new RelayCommand<FrameworkElement>(p => { return true; }, p => { Message = new Message(Message.MessageType.Logout); Host.Hide(); });

            NhaSachShowCommand = new RelayCommand<object>(p => { return true; }, p => { ContentZone = NhaSachPage; });
            LichSuBanShowCommand = new RelayCommand<object>(p => { return true; }, p => { if (LichSuBanPage is null) LichSuBanPage = new LichSuBanPage(); ContentZone = LichSuBanPage; });
            KhoSachShowCommand = new RelayCommand<object>(p => { return User.IDType == 0 || User.IDType == 2; }, p => { if (KhoSachPage is null) KhoSachPage = new KhoSachPage(); ContentZone = KhoSachPage; });
            KhachHangShowCommand = new RelayCommand<object>(p => { return User.IDType != 2; }, p => { if (KhachHangPage is null) KhachHangPage = new KhachHangPage(); ContentZone = KhachHangPage; });
            TaiKhoanShowCommand = new RelayCommand<object>(p => { return true; }, p => { if (TaiKhoanPage is null) TaiKhoanPage = new TaiKhoanPage(); ContentZone = TaiKhoanPage; });
        }

        public void ResetCache()
        {
            NhaSachShowCommand?.Execute(null);
            Host?._uc.SelectTo(0);
        }

        private void ShowInfoTK()
        {
            Button Tk = Host._uc.Children[4] as Button;
            Tk?.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            Tk?.Command.Execute(null);

            Tk = TaiKhoanPage._uc.Children[0] as Button;
            Tk?.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            Tk?.Command.Execute(null);
        }
    }
}
