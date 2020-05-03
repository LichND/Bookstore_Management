using BookStore_Management.Data;
using BookStore_Management.View.TaiKhoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace BookStore_Management.ViewModel.TaiKhoan
{
    public class TaiKhoanPageViewModel : BaseViewModel
    {
        #region Command
        public ICommand ThongTinShowCommand { get; set; }
        public ICommand QuanLyShowCommand { get; set; }
        #endregion
        #region navigation
        private Page _ContentZone;
        public Page ContentZone { get => _ContentZone; set { _ContentZone = value; OnPropertyChange(); } }

        private ThongTin_TK ThongTin { get; } = new ThongTin_TK();
        private QuanLy_TK QuanLy { get; set; }
        #endregion
        
        private TaiKhoanPage Host { get => base.Parent as TaiKhoanPage; set => base.Parent = value; }
        public TaiKhoanPageViewModel()
        {
            ContentZone = ThongTin;
            SetParentCommand = new RelayCommand<TaiKhoanPage>(p => { return Host is null; }, p => { Host = p; });
            ThongTinShowCommand = new RelayCommand<object>(p => { return true; }, p => { ContentZone = ThongTin; });
            QuanLyShowCommand = new RelayCommand<object>(p => { return (Application.Current.TryFindResource("MainVM") as MainViewModel).User.IDType < 2; }, p => { if (QuanLy is null) QuanLy = new QuanLy_TK(); ContentZone = QuanLy; });
        }

        public void ResetCache()
        {
            ThongTinShowCommand?.Execute(null);
            Host?._uc.SelectTo(0);
        }
    }
}
