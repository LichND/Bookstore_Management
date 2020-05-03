using BookStore_Management.View.KhoSach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore_Management.ViewModel.KhoSach
{
    public class KhoSachPageViewModel : BaseViewModel
    {
        public ICommand ThongTinShowCommand { get; set; }
        public ICommand NhapKhoShowCommand { get; set; }
        public ICommand XuatKhoShowCommand { get; set; }


        private ThongTin_KS ThongTin { get; } = new ThongTin_KS();
        private NhapKho_KS NhapKho { get; set; }
        private XuatKho_KS XuatKho { get; set; }

        private Page _ContentZone;
        public Page ContentZone { get => _ContentZone; set { _ContentZone = value; OnPropertyChange(); } }

        private KhoSachPage Host { get => base.Parent as KhoSachPage; set => base.Parent = value; }
        public KhoSachPageViewModel()
        {
            ContentZone = ThongTin;
            SetParentCommand = new RelayCommand<KhoSachPage>(p => { return Host is null; }, p => { Host = p; });
            ThongTinShowCommand = new RelayCommand<object>(p => { return true; }, p => { ContentZone = ThongTin; });
            NhapKhoShowCommand = new RelayCommand<object>(p => { return true; }, p => { if (NhapKho is null) NhapKho = new NhapKho_KS(); ContentZone = NhapKho; });
            XuatKhoShowCommand = new RelayCommand<object>(p => { return true; }, p => { if (XuatKho is null) XuatKho = new XuatKho_KS(); ContentZone = XuatKho; });
        }
        public void ResetCache()
        {
            (ThongTin?.DataContext as ThongTin_KSViewModel)?.ResetCache();
            ThongTinShowCommand?.Execute(null);
            Host?._uc.SelectTo(0);
        }
    }
}
