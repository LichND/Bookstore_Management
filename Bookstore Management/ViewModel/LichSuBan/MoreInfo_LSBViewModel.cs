using BookStore_Management.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore_Management.ViewModel.LichSuBan
{
    public class MoreInfo_LSBViewModel : BaseViewModel
    {
        public ICommand OKCommand { get; set; }
        public BillData Bill { get; set; }
        public View.LichSuban.MoreInfo_LSB Host { get => base.Parent as View.LichSuban.MoreInfo_LSB; set => base.Parent = value; }
        public MoreInfo_LSBViewModel()
        {
            OKCommand = new RelayCommand<object>(p => { return true; }, p => { Host?.Close(); Message.Type = Message.MessageType.OK; });
        }
    }
}
