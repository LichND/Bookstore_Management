using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management.Data
{
    public class BillData
    {
        public int ID { get; set; } = LogicData.NextID("HoaDon");
        public int IDKhachHang { get; set; }
        public int IDNhanVien { get; set; }
        public List<BillInfo> Datas { get; set; } = new List<BillInfo>();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public Money Money { get; set; } = (Money)0;
        public override string ToString() => "(" + ID + "," + IDKhachHang + "," + IDNhanVien + "," + (long)Money + ",\"" + DateTime + "\")";

        public string Customer { get => SQLiteDataAccess<string>.Select1("SELECT Name FROM KhachHang WHERE ID=" + IDKhachHang); }
        public string Employees { get => SQLiteDataAccess<string>.Select1("SELECT Nickname FROM TaiKhoan WHERE ID=" + IDNhanVien); }
    }

    public class BillInfo
    {
        private BookData _Book;
        private int _IDBook;
        public int IDBook { get => _IDBook; set { _IDBook = value; _Book = value.FindBook(); } }
        public int IDHoaDon { get; set; }
        public long Number { get; set; }
        public string Money { get => (Number * _Book.Cost).ToString(); }
        public override string ToString() => "(" + IDHoaDon + "," + IDBook + "," + Number + ")";

        public string BookName { get => _Book?.BookName; }
        public string Cost { get => _Book?.Cost.ToString(); }
        public string Price { get => (_Book.Cost * Number).ToString(); }
    }
}
