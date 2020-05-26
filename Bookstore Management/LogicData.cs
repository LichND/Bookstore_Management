using BookStore_Management.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management
{
    public class LogicData
    {
        private static List<string> _AccountType = null;
        private static List<string> _SexType = null;
        private static List<string> _NickName = null;
        private static List<string> _Categories = null;
        private static List<string> _Books = null;
        private static ObservableCollection<BookData> _AllBooks = null;
        private static List<string> _Customers = null;
        private static ObservableCollection<CustomerData> _AllCustomers = null;
        private static ObservableCollection<BillData> _AllBill = null;
        public static List<string> AccountType
        {
            get
            {
                if (_AccountType is null)
                    _AccountType = SQLiteDataAccess<string>.Select("SELECT Translate FROM LoaiTaiKhoan");
                return _AccountType;
            }
        }
        public static List<string> SexType
        {
            get
            {
                if (_SexType is null)
                    SQLiteDataAccess<string>.Select("SELECT Type FROM GioiTinh");
                return _SexType;
            }
        }
        public static List<string> NickName
        {
            get
            {
                if (_NickName is null)
                    _NickName = SQLiteDataAccess<string>.Select("SELECT Nickname FROM TaiKhoan");
                return _NickName;
            }
        }
        public static List<string> Categories
        {
            get
            {
                if (_Categories is null)
                    _Categories = SQLiteDataAccess<string>.Select("SELECT Tag FROM TheLoai");
                return _Categories;
            }
        }
        public static List<string> Books
        {
            get
            {
                if (_Books is null)
                    _Books = SQLiteDataAccess<string>.Select("SELECT BookName FROM Book");
                return _Books;
            }
        }
        public static ObservableCollection<BookData> AllBooks
        {
            get
            {
                if (_AllBooks is null)
                {
                    _AllBooks = SQLiteDataAccess<BookData>.Select("SELECT * FROM Book").ToObservableCollection();
                    foreach (BookData item in _AllBooks)
                        item.Categoties = SQLiteDataAccess<string>.Select($"SELECT TAG FROM CTTHELOAI JOIN THELOAI ON CTTHELOAI.IDTheLoai=THELOAI.ID WHERE CTTheLoai.IDBook={item.ID}");
                }
                return _AllBooks;
            }
        }
        public static List<string> Customers
        {
            get
            {
                if (_Customers is null)
                    _Customers = SQLiteDataAccess<string>.Select("SELECT Name FROM KhachHang");
                return _Customers;
            }
        }
        public static ObservableCollection<CustomerData> AllCustomers
        {
            get
            {
                if (_AllCustomers is null)
                    _AllCustomers = SQLiteDataAccess<CustomerData>.Select("SELECT * FROM KhachHang").ToObservableCollection();
                return _AllCustomers;
            }
        }
        public static ObservableCollection<BillData> AllBill
        {
            get
            {
                if (_AllBill is null)
                {
                    _AllBill = SQLiteDataAccess<BillData>.Select("SELECT * FROM HoaDon").ToObservableCollection();
                    foreach (var item in AllBill)
                        item.Datas = SQLiteDataAccess<BillInfo>.Select("SELECT * FROM CTHoaDon WHERE IDHoaDon=" + item.ID);
                }
                return _AllBill;
            }
        }

        public static async void LoadDataAsync()
        {
            await Task.WhenAll(new Task[]
            {
                new Task(() => { var tmp = AccountType; }),
                new Task(() => { var tmp = SexType; }),
                new Task(() => { var tmp = NickName; }),
                new Task(() => { var tmp = Categories; }),
                new Task(() => { var tmp = Books; }),
                new Task(() => { var tmp = AllBooks; }),
                new Task(() => { var tmp = Customers; }),
                new Task(() => { var tmp = AllCustomers; }),
                new Task(() => { var tmp = AllBill; }),
            });
        }


        public static int NextID(string table)
        {
            var tmp = SQLiteDataAccess<int>.Select("SELECT ID FROM " + table + " ORDER BY ID DESC LIMIT 1");
            return tmp.Count == 0 ? 0 : tmp[0] + 1;
        }

        public static string NullValue(string src)
        {
            if (src is null || src == "")
                return "NULL";
            else
                return '\"' + src + '\"';
        }
    }
    public static class ExtensionMethodClass
    {
        public static bool IsCompare<T>(this List<T> source, List<T> What)
        {
            var firstNotSecond = source.Except(What).ToList();
            var secondNotFirst = What.Except(source).ToList();
            return !firstNotSecond.Any() && !secondNotFirst.Any();
        }
        public static bool IsCompare<T>(this ObservableCollection<T> source, ObservableCollection<T> What) => source.ToList().IsCompare(What.ToList());
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> src)
        {
            if (src is null)
                return null;
            ObservableCollection<T> ret = new ObservableCollection<T>();
            foreach (var item in src)
                ret.Add(item);
            return ret;
        }
        public static ObservableCollection<T> SafeClone<T>(this ObservableCollection<T> src)
        {
            if (src is null)
                return null;
            ObservableCollection<T> ret = new ObservableCollection<T>();
            foreach (var item in src)
                ret.Add((T)Activator.CreateInstance(typeof(T), item));
            return ret;
        }
        public static string SafeCopy(this string src) => src is null ? null : string.Copy(src);
        public static BookData FindBook(this string BookName)
        {
            int index = LogicData.Books.IndexOf(BookName);
            return index < 0 ? null : LogicData.AllBooks?[index];
        }
        public static BookData FindBook(this int IDBook)
        {
            foreach (var item in LogicData.AllBooks)
                if (item.ID == IDBook)
                    return item;
            return null;
        }
    }
}