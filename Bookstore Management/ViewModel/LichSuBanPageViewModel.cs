using BookStore_Management.Data;
using BookStore_Management.Interface;
using BookStore_Management.UC;
using BookStore_Management.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore_Management.ViewModel
{
    public class LichSuBanPageViewModel : BaseViewModel
    {
        public ICommand MoreInfoCommand { get; set; }
        public BillData SelectedItem { get; set; }
        public LichSuBanPageViewModel()
        {
            MoreInfoCommand = new RelayCommand<object>(p => { return SelectedItem != null; }, p =>
            {
                View.LichSuban.MoreInfo_LSB Info = new View.LichSuban.MoreInfo_LSB();
                LichSuBan.MoreInfo_LSBViewModel InfoVM = new LichSuBan.MoreInfo_LSBViewModel() { Host = Info, Bill = SelectedItem };
                Info.DataContext = InfoVM;
                Info.ShowDialog();
            });
        }
    }
}
