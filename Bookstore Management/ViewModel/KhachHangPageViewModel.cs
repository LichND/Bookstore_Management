using BookStore_Management.Data;
using BookStore_Management.View;
using BookStore_Management.View.KhachHang;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management.ViewModel
{
    public class KhachHangPageViewModel : Interface.DataGridBaseViewModel<CustomerData>
    {
        private KhachHangPage Host { get => base.Parent as KhachHangPage; set => base.Parent = value; }
        public KhachHangPageViewModel()
        {
            SetParentCommand = new RelayCommand<KhachHangPage>(p => { return Host is null; }, p => { Host = p; Datas = LogicData.AllCustomers; });
        }

        protected override void Add(object p)
        {
            base.Add(p);
            SQLiteDataAccess.Insert("KhachHang", NewItem);
            Host._uc.IsOpen = false;
        }
        protected override void Edit(object p)
        {
            KhachHang.Sua_KHViewModel SuaVM = new KhachHang.Sua_KHViewModel() { Customer = SelectedItem.Clone() };
            Sua_KH EditWindows = new Sua_KH() { DataContext = SuaVM };
            EditWindows.ShowDialog();
            if (SuaVM.Message.Type == Message.MessageType.OK)
            {
                if (SuaVM.Customer.Name != SelectedItem.Name)
                    SQLiteDataAccess.Update("KhachHang", "Name", SuaVM.Customer.Name, SelectedItem.ID);
                if (SuaVM.Customer.Sex != SelectedItem.Sex)
                    SQLiteDataAccess.Update("KhachHang", "Sex", SuaVM.Customer.Sex, SelectedItem.ID);
                if (SuaVM.Customer.Phone != SelectedItem.Phone)
                    SQLiteDataAccess.Update("KhachHang", "Phone", SuaVM.Customer.Phone, SelectedItem.ID);
                if (SuaVM.Customer.Address != SelectedItem.Address)
                    SQLiteDataAccess.Update("KhachHang", "Address", SuaVM.Customer.Address, SelectedItem.ID);

                Datas[Datas.IndexOf(SelectedItem)] = SuaVM.Customer;
            }
        }
        protected override void Delete(object p)
        {
            SQLiteDataAccess.Delete("KhachHang", SelectedItem.ID);
            base.Delete(p);
        }
    }
}
