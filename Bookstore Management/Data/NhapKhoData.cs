using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management.Data
{
    public class NhapKhoData : ViewModel.BaseNotifyPropertyChanged
    {
        private Money _Money = new Money(0);
        public int ID { get; set; } = LogicData.NextID("NhapKho");
        public int IDManage { get; set; }
        public string NameManage { get => LogicData.NickName[IDManage]; set => IDManage = LogicData.NickName.IndexOf(value); }
        public string Supplier { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string Note { get; set; } = "";
        public Money Money { get => _Money; set { _Money = value; OnPropertyChange(); } }
        public ObservableCollection<CTNhapKhoData> Datas { get; set; } = new ObservableCollection<CTNhapKhoData>();
        public override string ToString() => "(" + ID + "," + IDManage + ",\"" + Supplier + "\",\"" + DateTime.ToString() + "\",\"" + Note + "\"," + (long)Money + ")";
        public NhapKhoData Clone() => new NhapKhoData()
        {
            ID = ID,
            IDManage = IDManage,
            NameManage = NameManage.SafeCopy(),
            Supplier = Supplier.SafeCopy(),
            DateTime = DateTime,
            Note = Note.SafeCopy(),
            Money = Money.Clone(),
            Datas = Datas.SafeClone()
        };
    }

    public class CTNhapKhoData : INotifyPropertyChanged
    {
        private int _Number = 0;
        private Money _Price = new Money(0);
        public int IDNhapKho { get; set; }
        public int IDBook { get => book.ID; set { book = value.FindBook(); } }
        public BookData book { get; set; }
        public string BookName { get => book?.BookName; set { book = value.FindBook(); } }
        public int Number { get => _Number; set { _Number = value; Price = value * book.DefCost; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Number")); } }
        public Money Price { get => _Price; set { Money old = value - _Price; _Price = value; PropertyChanged?.Invoke(old, new PropertyChangedEventArgs("UpdatePrice")); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Price")); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public CTNhapKhoData() { }
        public CTNhapKhoData(CTNhapKhoData data)
        {
            IDNhapKho = data.IDNhapKho;
            book = data.book;
            Number = data._Number;
        }
        public override string ToString() => "(" + IDNhapKho + "," + IDBook + "," + Number + ")";
    }
}
