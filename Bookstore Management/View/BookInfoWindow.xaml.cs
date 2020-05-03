using MaterialDesignThemes.Wpf;
using BookStore_Management.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStore_Management.View
{
    /// <summary>
    /// Interaction logic for BookInfoWindow.xaml
    /// </summary>
    public partial class BookInfoWindow : Window, INotifyPropertyChanged
    {
        public BookInfoWindowViewModel BookInfoWM { get; set; }
        private bool _IsHaveHistory = true;
        public bool IsHaveHistory { get => _IsHaveHistory; set { _IsHaveHistory = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsHaveHistory")); } }
        public BookInfoWindow()
        {
            InitializeComponent();
            this.DataContext = BookInfoWM = new BookInfoWindowViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
