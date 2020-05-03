using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management.Data
{
    public class XuatKhoData : ViewModel.BaseNotifyPropertyChanged
    {
        private Money _Money = new Money(0);
        public int ID { get; set; } = LogicData.NextID("XuatKho");
        public int IDManage { get; set; } 
        public string NameManage { get => LogicData.NickName[IDManage]; set => IDManage = LogicData.NickName.IndexOf(value); }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Note { get; set; } = "";
        public Money Money { get => _Money; set { _Money = value; OnPropertyChange(); } }
        public ObservableCollection<CTXuatKhoData> Datas { get; set; } = new ObservableCollection<CTXuatKhoData>();
        public override string ToString() => "(" + ID + "," + IDManage + ",\"" + DateTime.ToString() + "\",\"" + Note + "\"," + (long)Money + ")";
        public XuatKhoData Clone() => new XuatKhoData()
        {
            ID = ID,
            IDManage = IDManage,
            NameManage = NameManage.SafeCopy(),
            DateTime = DateTime,
            Note = Note.SafeCopy(),
            Money = Money.Clone(),
            Datas = Datas.SafeClone()
        };
    }
    public class CTXuatKhoData : INotifyPropertyChanged
    {
        private int _Number = 0;
        private Money _Price = new Money(0);
        private BookData _book;
        private string _InfoInvertory = "";
        public int IDXuatKho { get; set; }
        public int IDBook { get => _book.ID; set { book = value.FindBook(); } }
        public BookData book { get => _book; set { _book = value; InfoInvertory = value is null ? "Sách không tồn tại" : "Còn lại: " + book.Inventory; } }
        public string BookName { get => _book?.BookName; set { book = value.FindBook(); } }
        public int Number { get => _Number; set { _Number = value; Price = value * book.Cost; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Number")); } }
        public Money Price { get => _Price; set { Money old = value - _Price; _Price = value; PropertyChanged?.Invoke(old, new PropertyChangedEventArgs("UpdatePrice")); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price")); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public CTXuatKhoData() { }
        public CTXuatKhoData(CTXuatKhoData data)
        {
            IDXuatKho = data.IDXuatKho;
            book = data.book;
            Number = data._Number;
        }
        public string InfoInvertory { get => _InfoInvertory; set { _InfoInvertory = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("InfoInvertory")); } }
        public override string ToString() => "(" + IDXuatKho + "," + IDBook + "," + Number + ")";
    }
}
