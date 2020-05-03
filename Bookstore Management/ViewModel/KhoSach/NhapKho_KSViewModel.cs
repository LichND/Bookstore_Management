using BookStore_Management.Data;
using BookStore_Management.View.KhoSach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management.ViewModel.KhoSach
{
    public class NhapKho_KSViewModel : Interface.DataGridBaseViewModel<NhapKhoData>
    {
        private ThongTinChiTiet_NKViewModel EditWindowsVM { get; } = new ThongTinChiTiet_NKViewModel();
        private NhapKho_KS Host { get => base.Parent as NhapKho_KS; set => base.Parent = value; }
        private AccountData User { get; set; }
        public NhapKho_KSViewModel()
        {
            SetParentCommand = new RelayCommand<NhapKho_KS>(p => { return Host is null; }, p => { Host = p; LoadDataBase("NhapKho"); });
        }
        protected override NhapKhoData BeforeLoadItem(NhapKhoData src)
        {
            src.Datas = SQLiteDataAccess<CTNhapKhoData>.Select("SELECT * FROM CTNhapKho WHERE IDNhapKho=" + src.ID).ToObservableCollection();
            return base.BeforeLoadItem(src);
        }
        protected override void Edit(object p)
        {
            ThongTinChiTiet_NK EditWindows = new ThongTinChiTiet_NK();
            EditWindowsVM.Item = SelectedItem.Clone();
            EditWindowsVM.ResetCache();
            EditWindowsVM.Host = EditWindows;

            EditWindows.DataContext = EditWindowsVM;
            EditWindows.ShowDialog();
            if (EditWindowsVM.Message.Type == Message.MessageType.OK)
            {
                if (!EditWindowsVM.Item.Datas.IsCompare(SelectedItem.Datas))
                {
                    foreach (var item in SelectedItem.Datas)
                        item.book.Inventory -= item.Number;
                    SQLiteDataAccess.Delete("DELETE FROM CTNhapKho WHERE IDNhapKho=" + SelectedItem.ID);
                    foreach (var item in EditWindowsVM.Item.Datas)
                    {
                        item.book.Inventory += item.Number;
                        SQLiteDataAccess.Insert("CTNhapKho", item);
                        SQLiteDataAccess.Update("Book", "Inventory", item.book.Inventory, item.book.ID);
                    }
                    SQLiteDataAccess.Update("NhapKho", "Money", (long)EditWindowsVM.Item.Money, SelectedItem.ID);
                }
                if (SelectedItem.Note != EditWindowsVM.Item.Note)
                    SQLiteDataAccess.Update("NhapKho", "Note", EditWindowsVM.Item.Note, SelectedItem.ID);
                if (SelectedItem.Supplier != EditWindowsVM.Item.Supplier)
                    SQLiteDataAccess.Update("NhapKho", "Supplier", EditWindowsVM.Item.Supplier, SelectedItem.ID);
                Datas[Datas.IndexOf(SelectedItem)] = EditWindowsVM.Item.Clone();
            }
        }
        protected override void Add(object p)
        {
            NewItem.IDManage = MainViewModel.GetCurentUser.ID;
            SQLiteDataAccess.Insert("NhapKho", NewItem);
            SelectedItem = NewItem;
            base.Add(p);
            EditCommand.Execute(null);
        }
        protected override void Delete(object p)
        {
            foreach (var item in SelectedItem.Datas)
                item.book.Inventory -= item.Number;
            SQLiteDataAccess.Delete("DELETE FROM CTNhapKho WHERE IDNhapKho" + SelectedItem.ID);
            SQLiteDataAccess.Delete("NhapKho", SelectedItem.ID);
            base.Delete(p);
        }
    }
}
