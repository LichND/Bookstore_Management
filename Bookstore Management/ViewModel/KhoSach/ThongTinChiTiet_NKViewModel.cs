using BookStore_Management.View.KhoSach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookStore_Management.ViewModel.KhoSach
{
    public class ThongTinChiTiet_NKViewModel : Interface.DataGridBaseViewModel<Data.CTNhapKhoData>
    {
        public ICommand OKCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public Data.NhapKhoData Item { get; set; }

        public ThongTinChiTiet_NK Host { get; set; }
        public ThongTinChiTiet_NKViewModel()
        {
            OKCommand = new RelayCommand<object>(p => { return true; }, p => { Host.Close(); Host = null; Message.Type = Message.MessageType.OK; });
            CancelCommand = new RelayCommand<object>(p => { return true; }, p => { Host.Close(); Host = null; Message.Type = Message.MessageType.Quit; });
        }

        private void UpdatePrice(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UpdatePrice")
                Item.Money += sender as Money;
        }
        protected override bool CanAdd(object p) => !(NewItem.book is null);
        protected override void Add(object p)
        {
            NewItem.PropertyChanged += UpdatePrice;
            Item.Money += NewItem.Price;
            Host._uc.IsOpen = false;
            NewItem.IDNhapKho = Item.ID;
            base.Add(p);
        }
        protected override void Delete(object p)
        {
            Item.Money -= SelectedItem.Price;
            base.Delete(p);
        }

        public void ResetCache()
        {
            if (!Datas.Equals(Item.Datas))
            {
                Datas.Clear();
                Datas = Item.Datas;
                Item.Money = (Money)0;
                foreach (var item in Datas)
                {
                    item.PropertyChanged += UpdatePrice;
                    Item.Money += item.Price;
                }
            }
        }
    }
}
