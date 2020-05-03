using MaterialDesignThemes.Wpf;
using BookStore_Management.Data;
using BookStore_Management.UC;
using BookStore_Management.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore_Management.ViewModel
{
    public class NhaSachPageViewModel : BaseViewModel
    {
        #region command
        public ICommand SearchCommand { get; set; }
        public ICommand ChangedCheckBox { get; set; }
        public ICommand AddBillCommand { get; set; }
        #endregion
        #region Support function
        private List<string> advCategories { get; } = new List<string>();
        private List<string> advYear { get; } = new List<string>();
        private void AddCheckBox(StackPanel where, string what, string text)
        {
            CheckBox c = new CheckBox();
            c.Content = text;
            c.Command = ChangedCheckBox;
            c.CommandParameter = c;
            for (int i = 0; i < where.Children.Count; i = i + 2)
                if ((where.Children[i] as Label).Content.ToString() == what)
                {
                    c.Tag = i + 1;
                    (where.Children[i + 1] as WrapPanel).Children.Add(c);
                    return;
                }
            where.Children.Add(new Label() { Content = what });
            where.Children.Add(new WrapPanel());
            c.Tag = where.Children.Count - 1;
            (where.Children[where.Children.Count - 1] as WrapPanel).Children.Add(c);
        }
        private bool ChangeCheckBox(string what, bool value)
        {
            WrapPanel wrap;
            CheckBox check;
            for (int i = 1; i < Host._AdvSearch.Children.Count; i += 2)
            {
                wrap = Host._AdvSearch.Children[i] as WrapPanel;
                for (int j = 0; j < wrap.Children.Count; j++)
                {
                    check = wrap.Children[j] as CheckBox;
                    if (check.Content.ToString() == what)
                    {
                        check.IsChecked = value;
                        if (i == 1)
                            if (value)
                                advCategories.Add(what);
                            else
                                advCategories.RemoveAt(advCategories.IndexOf(what));
                        else if (i == 3) 
                            if (value)
                            advYear.Add(what);
                        else
                            advYear.RemoveAt(advCategories.IndexOf(what));
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
        #region UI binding
        private string _Search = "";
        private int _CartCount = 0;
        private Money _Money = (Money)0;
        public string Search { get => _Search; set { _Search = value; OnPropertyChange(); LoadBook(); } }
        public int CartCount { get => _CartCount; set { _CartCount = value; OnPropertyChange(); } }
        public Money Money { get => _Money; set { _Money = value; OnPropertyChange(); } }
        #endregion
        public ObservableCollection<BookTemplateViewModel> BookDatas { get; } = new ObservableCollection<BookTemplateViewModel>();
        public int SelectedBook { get; set; } = -1;
        public NhaSachPage Host { get => base.Parent as NhaSachPage; set => base.Parent = value; }
        public NhaSachPageViewModel()
        {
            SetParentCommand = new RelayCommand<NhaSachPage>(p => { return true; }, p => { if (Host is null) { Host = p; LoadContentAdvanceMenu(p._AdvSearch); } LoadBook(); });
            SearchCommand = new RelayCommand<object>(p => { return Search.Length > 0; }, p => { LoadBook(); });
            ChangedCheckBox = new RelayCommand<CheckBox>(p => { return true; }, p => 
            {
                if ((int)p.Tag == 1)
                    if ((bool)p.IsChecked)
                        advCategories.Add(p.Content.ToString());
                    else advCategories.RemoveAt(advCategories.IndexOf(p.Content.ToString()));
                else
                    if ((bool)p.IsChecked)
                    advYear.Add(p.Content.ToString());
                else advYear.RemoveAt(advYear.IndexOf(p.Content.ToString()));
                LoadBook();
            });
            AddBillCommand = new RelayCommand<object>(p => { return true; }, p => 
            {
                View.KhachHang.ThemSua_KH themSua_KH = new View.KhachHang.ThemSua_KH();
                KhachHang.ThemSua_KHViewModel themSua_KHVM = new KhachHang.ThemSua_KHViewModel() { Host = themSua_KH };
                themSua_KH.DataContext = themSua_KHVM;
                themSua_KH.ShowDialog();
                if (themSua_KHVM.Message.Type == Message.MessageType.OK) 
                {
                    BillData bill = new BillData() { IDNhanVien = MainViewModel.GetCurentUser.ID, IDKhachHang = themSua_KHVM.Customer.ID, Money = _Money };
                    if (themSua_KHVM.NameCustomer != themSua_KHVM.Customer.Name)
                    {
                        themSua_KHVM.Customer.Spent = _Money;
                        themSua_KHVM.Customer.Name = themSua_KHVM.NameCustomer;
                        LogicData.AllCustomers.Add(themSua_KHVM.Customer);
                        LogicData.Customers.Add(themSua_KHVM.NameCustomer);
                        SQLiteDataAccess.Insert("KhachHang", themSua_KHVM.Customer);
                    }
                    else
                        themSua_KHVM.Customer.Spent += _Money;
                    foreach (var item in Host._Bill.Children)
                    {
                        CartItem tmp = item as CartItem;
                        BillInfo info = new BillInfo() { IDBook = tmp.CartItemVM.Book.ID, IDHoaDon = bill.ID, Number = tmp.CartItemVM.Number };
                        bill.Datas.Add(info);
                        SQLiteDataAccess.Insert("CTHoaDon", info);
                        // reduce book
                        tmp.CartItemVM.Book.Inventory -= tmp.CartItemVM.Number;
                        tmp.CartItemVM.Book.Sold += tmp.CartItemVM.Number;
                        SQLiteDataAccess.Update("Book", "Inventory", tmp.CartItemVM.Book.Inventory, tmp.CartItemVM.Book.ID);
                        SQLiteDataAccess.Update("Book", "Sold", tmp.CartItemVM.Book.Sold, tmp.CartItemVM.Book.ID);
                    }
                    SQLiteDataAccess.Update("KhachHang", "Spent", (long)themSua_KHVM.Customer.Spent, themSua_KHVM.Customer.ID);
                    LogicData.AllBill.Add(bill);
                    SQLiteDataAccess.Insert("HoaDon", bill);
                    
                    Host._Bill.Children.Clear();
                    CartCount = 0;
                    Money = (Money)0;
                }
                Host._popup.IsPopupOpen = false;
                LoadBook();
            });
            worker.DoWork += FindBook;
            worker.UpdateUI += ShowBook;
        }

        private void LoadContentAdvanceMenu(StackPanel w)
        {
            List<string> categogies = SQLiteDataAccess<string>.Select("SELECT Tag FROM TheLoai ORDER BY Tag");
            foreach (var item in categogies)
                AddCheckBox(w, "Thể loại", item);
            List<int> year = SQLiteDataAccess<int>.Select("SELECT Year FROM Book ORDER BY Year");
            foreach (var item in year)
                AddCheckBox(w, "Năm", item.ToString());
        }
        public void SearchBook(Message message)
        {
            foreach (var item in advCategories)
                ChangeCheckBox(item, false);
            foreach (var item in advYear)
                ChangeCheckBox(item, false);
            
            if (ChangeCheckBox((string)message.Sender, true))
            {
                if (Search.Length > 0)
                    Search = "";
                LoadBook();
            }
            else
                Search = (string)message.Sender;    
        }

        private MyBackgroundWorker worker = new MyBackgroundWorker();
        private void ShowBook(object sender, ProgressChangedEventArgs e)
        {
            BookDatas.Add(new BookTemplateViewModel() { data = e.UserState as BookData, Host = this });
        }
        private void FindBook(object sender, DoWorkEventArgs e)
        {
            string Query = "SELECT * FROM Book";
            if (Search != "")
                Query += " WHERE BookName LIKE '%" + Search + "%'";
            List<BookData> books = SQLiteDataAccess<BookData>.Select(Query);
            if (books is null)
                return;
            BackgroundWorker tmp = sender as BackgroundWorker;
            foreach (var item in books)
            {
                if (tmp.CancellationPending)
                    return;
                item.Categoties = SQLiteDataAccess<string>.Select("SELECT TheLoai.Tag FROM CTTheLoai JOIN TheLoai on CTTheLoai.IDTheLoai=TheLoai.ID WHERE CTTheLoai.IDBook=" + item.ID);
                bool accept = true;
                for (int i = 0; i < advCategories.Count; i++)
                    if (item.Categoties.IndexOf(advCategories[i]) < 0)
                    {
                        accept = false;
                        break;
                    }
                if (!accept)
                    continue;
                if (advYear.Count > 0 && advYear.IndexOf(item.Year.ToString()) < 0)
                    continue;
                tmp.ReportProgress(0, item);
                System.Threading.Thread.Sleep(200);
            }
        }
        private void LoadBook()
        {
            SelectedBook = -1;
            BookDatas.Clear();
            worker.RunWorkerAsync();
        }
        #region cart
        public void Add2Cart(BookData book)
        {
            foreach (var item in Host._Bill.Children)
            {
                CartItem tmp = item as CartItem;
                if (tmp.CartItemVM.Book.ID == book.ID)
                {
                    if (tmp.CartItemVM.AddCommand.CanExecute(null))
                        tmp.CartItemVM.AddCommand.Execute(null);
                    return;
                }
            }
            CartItem NewItem = new CartItem(book.ID.FindBook());
            NewItem.DeleteThis += DeleteThisItem;
            NewItem.MoneyChanged += MoneyChangedThisItem;
            Host._Bill.Children.Add(NewItem);
            CartCount++;
            Money += book.Cost;
        }
        private void DeleteThisItem(object sender, CartItemEventArgs e)
        {
            Host._Bill.Children.Remove(sender as CartItem);
            CartCount--;
        }
        private void MoneyChangedThisItem(object sender, MoneyEventArgs e)
        {
            Money += e.Money;
        }
        #endregion
    }
}
