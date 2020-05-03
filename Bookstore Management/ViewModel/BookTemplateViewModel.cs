using MaterialDesignThemes.Wpf;
using BookStore_Management.Data;
using BookStore_Management.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore_Management.ViewModel
{
    public class BookTemplateViewModel : BaseViewModel
    {
        #region command
        public ICommand MoreDetailsCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand FlipCommand { get; set; }
        #endregion
        public BookData data { get; set; }
        private bool _IsFlipped = false;
        public bool IsFlipped { get => _IsFlipped; set { _IsFlipped = value; OnPropertyChange(); } }
        private BookInfoWindow BookInfoWindow { get; set; }
        public NhaSachPageViewModel Host { get => Parent as NhaSachPageViewModel; set => Parent = value; }

        public BookTemplateViewModel()
        {
            MoreDetailsCommand = new RelayCommand<object>(p => { return true; }, p => { MoreDatails(); });
            DeleteCommand = new RelayCommand<object>(p => { return MainViewModel.GetCurentUser.IDType == 0 || MainViewModel.GetCurentUser.IDType == 2; }, p => { KhoSach.ThongTin_KSViewModel.DeleteBook(data); Host.BookDatas.Remove(this); });
            AddToCartCommand = new RelayCommand<object>(p => { return data.Inventory > 0; }, p => { Host.Add2Cart(data); if (Host.SelectedBook >= 0) FlipCommand.Execute(null); });
            FlipCommand = new RelayCommand<object>(p => { return true; }, p => 
            {
                if (IsFlipped)
                {
                    IsFlipped = false;
                    Host.SelectedBook = -1;
                }
                else
                {
                    if (Host.SelectedBook >= 0)
                        Host.BookDatas[Host.SelectedBook].IsFlipped = false;
                    IsFlipped = true;
                    Host.SelectedBook = Host.BookDatas.IndexOf(this);
                }
            });
        }

        private void MoreDatails()
        {
            IsFlipped = false;
            if (BookInfoWindow == null)
            {
                BookInfoWindow = new BookInfoWindow() { IsHaveHistory = false };
                BookInfoWindow.BookInfoWM.data = data;
            }
            BookInfoWindow.ShowDialog();
            switch (BookInfoWindow.BookInfoWM.Message.Type)
            {
                case Message.MessageType.Quit:
                case Message.MessageType.None:
                    BookInfoWindow = null;
                    break;
                case Message.MessageType.Search:
                    Host.SearchBook(BookInfoWindow.BookInfoWM.Message);
                    BookInfoWindow = null;
                    break;
                default:
                    break;
            }
        }
    }
}
