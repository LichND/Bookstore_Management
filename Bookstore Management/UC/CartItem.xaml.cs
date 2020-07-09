using BookStore_Management.Data;
using BookStore_Management.ViewModel;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStore_Management.UC
{
    /// <summary>
    /// Interaction logic for CartItem.xaml
    /// </summary>
    public partial class CartItem : UserControl
    {
        public CartItemViewModel CartItemVM { get; set; }
        public CartItem(BookData book)
        {
            InitializeComponent();
            DataContext = CartItemVM = new CartItemViewModel(book) { Host = this };
        }

        public event EventHandler<CartItemEventArgs> DeleteThis;
        public event EventHandler<MoneyEventArgs> MoneyChanged;
        public event EventHandler<MoneyEventArgs> UnknownChanged;

        public void CartItemVM_NoItem()
        {
            DeleteThis?.Invoke(this, new CartItemEventArgs(CartItemVM));
        }
        public void CartItemVM_MoneyChanged(Money money)
        {
            MoneyChanged?.Invoke(this, new MoneyEventArgs(money));
        }
    }

    public class CartItemViewModel : BaseNotifyPropertyChanged
    {
        #region command
        public ICommand AddCommand { get; set; }
        public ICommand MinusCommand { get; set; }
        #endregion
        private long _Number;
        private BookData _Book;
        public CartItem Host { get; set; }
        public BookData Book { get => _Book; set { _Book = value; OnPropertyChange(); } }
        public long Number
        { 
            get => _Number; 
            set
            {
                if (value <= 0)
                {
                    Host?.CartItemVM_MoneyChanged(Book.Cost * (1 - Number));
                    if (value == 0)
                        Host?.CartItemVM_NoItem();
                    else
                        value = 1;
                }
                else
                {
                    if (value > _Book.Inventory)
                        value = _Book.Inventory;
                    Host?.CartItemVM_MoneyChanged(Book.Cost * (value - Number));
                }
                    
                _Number = value;
                OnPropertyChange(); 
            } 
        }
        public CartItemViewModel(BookData book)
        {
            Book = book;
            Number = 1;
            AddCommand = new RelayCommand<object>(p => { return _Book.Inventory > _Number; }, p => { Number++; });
            MinusCommand = new RelayCommand<object>(p => { return true; }, p => { Number--; });
        }
    }

    public class CartItemEventArgs
    {
        public CartItemViewModel CartItem { get; set; }
        public CartItemEventArgs(CartItemViewModel src)
        {
            CartItem = src;
        }
    }
}