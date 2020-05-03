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
        public static List<string> AccountType { get; private set; }
        public static List<string> SexType { get; private set; }
        public static List<string> NickName { get; private set; }
        public static List<string> Categories { get; private set; }
        public static List<string> Books { get; private set; }
        public static ObservableCollection<BookData> AllBooks { get; private set; }
        public static List<string> Customers { get; private set; }
        public static ObservableCollection<CustomerData> AllCustomers { get; private set; }
        public static ObservableCollection<BillData> AllBill { get; private set; }

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
        public static void Load()
        {
            MyBackgroundWorker worker = new MyBackgroundWorker();
            worker.DoWork += LoadWorker_DoWork;
            worker.RunWorkerAsync();
        }

        private static void LoadWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            AccountType = SQLiteDataAccess<string>.Select("SELECT Translate FROM LoaiTaiKhoan");
            SexType = SQLiteDataAccess<string>.Select("SELECT Type FROM GioiTinh");
            NickName = SQLiteDataAccess<string>.Select("SELECT Nickname FROM TaiKhoan");
            Categories = SQLiteDataAccess<string>.Select("SELECT Tag FROM TheLoai");
            Books = SQLiteDataAccess<string>.Select("SELECT BookName FROM Book");
            AllBooks = SQLiteDataAccess<BookData>.Select("SELECT * FROM Book").ToObservableCollection();
            foreach (var item in AllBooks)
                item.Categoties = SQLiteDataAccess<string>.Select("SELECT TAG FROM CTTHELOAI JOIN THELOAI ON CTTHELOAI.IDTheLoai=THELOAI.ID WHERE CTTheLoai.IDBook=" + item.ID);
            Customers = SQLiteDataAccess<string>.Select("SELECT Name FROM KhachHang");
            AllCustomers = SQLiteDataAccess<CustomerData>.Select("SELECT * FROM KhachHang").ToObservableCollection();
            AllBill = SQLiteDataAccess<BillData>.Select("SELECT * FROM HoaDon").ToObservableCollection();
            foreach (var item in AllBill)
                item.Datas = SQLiteDataAccess<BillInfo>.Select("SELECT * FROM CTHoaDon WHERE IDHoaDon=" + item.ID);
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