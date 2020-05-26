using BookStore_Management.Data;
using BookStore_Management.View.KhoSach;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management.ViewModel.KhoSach
{
    public class XuatKho_KSViewModel : Interface.DataGridBaseViewModel<XuatKhoData>
    {
        private ThongTinChiTiet_XKViewModel EditWindowsVM { get; } = new ThongTinChiTiet_XKViewModel();
        private XuatKho_KS Host { get => base.Parent as XuatKho_KS; set => base.Parent = value; }
        private AccountData User { get; set; }
        public XuatKho_KSViewModel()
        {
            SetParentCommand = new RelayCommand<XuatKho_KS>(p => { return Host is null; }, p => { Host = p; LoadDataBase("XuatKho"); });
        }
        protected override XuatKhoData BeforeLoadItem(XuatKhoData src)
        {
            src.Datas = SQLiteDataAccess<CTXuatKhoData>.Select("SELECT * FROM CTXuatKho WHERE IDXuatKho=" + src.ID).ToObservableCollection();
            return base.BeforeLoadItem(src);
        }
        protected override void Edit(object p)
        {
            ThongTinChiTiet_XK EditWindows = new ThongTinChiTiet_XK();
            EditWindowsVM.Item = SelectedItem.Clone();
            EditWindowsVM.ResetCache();
            EditWindowsVM.Host = EditWindows;
            
            EditWindows.DataContext = EditWindowsVM;
            EditWindows.ShowDialog();
            if ((EditWindows.DataContext as BaseViewModel)?.Message.Type == Message.MessageType.OK)
            {
                if (!EditWindowsVM.Item.Datas.IsCompare(SelectedItem.Datas))
                {
                    foreach (var item in SelectedItem.Datas)
                    {
                        item.book.Inventory += item.Number;
                        item.book.Sold -= item.Number;
                    }
                    SQLiteDataAccess.Delete("DELETE FROM CTXuatKho WHERE IDXuatKho=" + SelectedItem.ID);
                    foreach (var item in EditWindowsVM.Item.Datas)
                    {
                        item.book.Inventory -= item.Number;
                        item.book.Sold += item.Number;
                        SQLiteDataAccess.Insert("CTXuatKho", item);
                        SQLiteDataAccess.Update("Book", "Inventory", item.book.Inventory, item.book.ID);
                        SQLiteDataAccess.Update("Book", "Sold", item.book.Sold, item.book.ID);
                    }
                    SQLiteDataAccess.Update("XuatKho", "Money", (long)EditWindowsVM.Item.Money, SelectedItem.ID);
                }
                if (SelectedItem.Note != EditWindowsVM.Item.Note)
                    SQLiteDataAccess.Update("XuatKho", "Note", EditWindowsVM.Item.Note, SelectedItem.ID);
                Datas[Datas.IndexOf(SelectedItem)] = EditWindowsVM.Item.Clone();
            }
        }
        protected override void Add(object p)
        {
            NewItem.IDManage = (System.Threading.Thread.CurrentPrincipal.Identity as AccountData).ID;
            SQLiteDataAccess.Insert("XuatKho", NewItem);
            SelectedItem = NewItem;
            base.Add(p);
            EditCommand.Execute(null);
        }
        protected override void Delete(object p)
        {
            foreach (var item in SelectedItem.Datas)
            {
                item.book.Inventory += item.Number;
                item.book.Sold -= item.Number;
                SQLiteDataAccess.Update("Book", "Inventory", item.book.Inventory, item.book.ID);
                SQLiteDataAccess.Update("Book", "Sold", item.book.Sold, item.book.ID);
            }
            SQLiteDataAccess.Delete("DELETE FROM CTXuatKho WHERE IDXuatKho=" + SelectedItem.ID);
            SQLiteDataAccess.Delete("XuatKho", SelectedItem.ID);
            base.Delete(p);
        }
    }
}
