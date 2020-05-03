using MaterialDesignThemes.Wpf;
using BookStore_Management.Data;
using BookStore_Management.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore_Management.ViewModel
{
    public class BookInfoWindowViewModel : BaseViewModel
    {
        #region command
        public ICommand ChipClickCommand { get; set; }
        public ICommand WindowNomalSizeCommand { get; set; }
        #endregion
        public List<BookHistoryData> History { get; } = new List<BookHistoryData>();
        public BookData data { get; set; }
        private BookInfoWindow Host { get => base.Parent as BookInfoWindow; set => base.Parent = value; }

        public BookInfoWindowViewModel()
        {
            ChipClickCommand = new RelayCommand<Chip>(p => { return true; }, p => { ChipClick(p); });
            WindowNomalSizeCommand = new RelayCommand<Window>(p => { return true; }, p => { if (p.WindowState == WindowState.Maximized) p.WindowState = WindowState.Normal; });
            SetParentCommand = new RelayCommand<BookInfoWindow>(p => { return base.Parent is null; }, p => { Host = p; AddCategories(); });
        }

        private void AddCategories()
        {
            foreach (var item in data.Categoties)
                Host._Categoties.Children.Add(new Chip() { Content = item });
        }
        private void ChipClick(Chip c)
        {
            Message = new Message() { Sender = c.Content, Type = Message.MessageType.Search };
            Host.Close();
        }
    }
}