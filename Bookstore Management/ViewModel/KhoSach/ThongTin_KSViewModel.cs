using MaterialDesignThemes.Wpf;
using BookStore_Management.Data;
using BookStore_Management.Interface;
using BookStore_Management.View.KhoSach;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using BookStore_Management.UC;

namespace BookStore_Management.ViewModel.KhoSach
{
    public class ThongTin_KSViewModel : DataGridBaseViewModel<BookData>
    {
        private ThemThongTin_KS AddBook { get; set; }
        private ThemThongTin_KSViewModel AddBookVM { get; } = new ThemThongTin_KSViewModel();
        private ThemThongTin_KS EditBook { get; set; }
        private ThemThongTin_KSViewModel EditBookVM { get; } = new ThemThongTin_KSViewModel();
        private ThongTin_KS Host { get => base.Parent as ThongTin_KS; set => base.Parent = value; }

        public ThongTin_KSViewModel()
        {
            SetParentCommand = new RelayCommand<ThongTin_KS>(p => { return Host is null; }, p => { Host = p; Datas = LogicData.AllBooks; });
            EditBookVM.ResetCommand = new RelayCommand<object>(p => { return true; }, p => { EditBook.Close(); });
        }

        public void ResetCache()
        {
            AddBook?.Close();
            EditBook?.Close();
        }
        protected override void Add(object p)
        {
            if (AddBook is null)
            {
                AddBookVM.book = new BookData() { ID = LogicData.NextID("Book") };
                AddBookVM.Host = AddBook = new ThemThongTin_KS();
                AddBook.DataContext = AddBookVM;
            }
            AddBook.ShowDialog();
            switch (AddBookVM.Message.Type)
            {
                case Message.MessageType.Quit:
                case Message.MessageType.None:
                    AddBook = null;
                    break;
                case Message.MessageType.OK:
                    SQLiteDataAccess.Insert("Book", AddBookVM.book);
                    foreach (var item in AddBookVM.book.Categoties)
                        SQLiteDataAccess.Insert("INSERT INTO CTTheLoai VALUES(" + AddBookVM.book.ID + ',' + LogicData.Categories.IndexOf(item) + ')');
                    Datas.Add(AddBookVM.book);
                    LogicData.Books.Add(AddBookVM.book.BookName);
                    AddBook = null;
                    MyMessageBox.Show("Thêm sách thành công", "Thông báo", false);
                    break;
                default:
                    break;
            }
        }
        protected override void Delete(object p)
        {
            SQLiteDataAccess.Delete("DELETE FROM CTNhapKho WHERE IDBook=" + SelectedItem.ID);
            SQLiteDataAccess.Delete("DELETE FROM CTXuatKho WHERE IDBook=" + SelectedItem.ID);
            SQLiteDataAccess.Delete("DELETE FROM CTTheLoai WHERE IDBook=" + SelectedItem.ID);
            SQLiteDataAccess.Delete("Book", SelectedItem.ID);
            int index = LogicData.AllBooks.IndexOf(SelectedItem);
            LogicData.Books.RemoveAt(index);
            //LogicData.AllBooks.RemoveAt(index);
            base.Delete(p);
            MyMessageBox.Show("Xóa thành công", "Thông báo", false);
        }
        protected override void Edit(object p)
        {
            EditBookVM.book = SelectedItem.Clone();
            EditBookVM.Host = EditBook = new ThemThongTin_KS();
            EditBook.DataContext = EditBookVM;
            EditBook._Reset_Cancel.Content = "Hủy";
            foreach (var item in EditBookVM.book.Categoties)
                EditBook._Categories.Children.Add(new Chip() { Content = item });
            EditBook.ShowDialog();
            if (EditBookVM.Message.Type == Message.MessageType.OK) 
            {
                if (EditBookVM.book.ImageEncode != SelectedItem.ImageEncode) 
                    SQLiteDataAccess.Update("Book", "ImageEncode", EditBookVM.book.ImageEncode, EditBookVM.book.ID);
                if (EditBookVM.book.BookName != SelectedItem.BookName) 
                    SQLiteDataAccess.Update("Book", "BookName", EditBookVM.book.BookName, EditBookVM.book.ID);
                if (EditBookVM.book.Author != SelectedItem.Author)
                    SQLiteDataAccess.Update("Book", "Author", EditBookVM.book.Author, EditBookVM.book.ID);
                if (EditBookVM.book.Year != SelectedItem.Year)
                    SQLiteDataAccess.Update("Book", "Year", EditBookVM.book.Year, EditBookVM.book.ID);
                if (!EditBookVM.book.Categoties.IsCompare(SelectedItem.Categoties)) 
                {
                    SQLiteDataAccess.Delete("DELETE FROM CTTheLoai WHERE IDBook=" + SelectedItem.ID);
                    foreach (var item in EditBookVM.book.Categoties)
                        SQLiteDataAccess.Insert("INSERT INTO CTTheLoai VALUES(" + EditBookVM.book.ID + ',' + LogicData.Categories.IndexOf(item) + ')');
                }
                if (EditBookVM.book.Cost != SelectedItem.Cost)
                    SQLiteDataAccess.Update("Book", "Cost", EditBookVM.book.Cost.ToLong(), EditBookVM.book.ID);
                if (EditBookVM.book.DefCost != SelectedItem.DefCost)
                    SQLiteDataAccess.Update("Book", "DefCost", EditBookVM.book.DefCost.ToLong(), EditBookVM.book.ID);
                if (EditBookVM.book.PublishingCompany != SelectedItem.PublishingCompany)
                    SQLiteDataAccess.Update("Book", "PublishingCompany", EditBookVM.book.PublishingCompany, EditBookVM.book.ID);
                if (EditBookVM.book.Summary != SelectedItem.Summary)
                    SQLiteDataAccess.Update("Book", "Summary", EditBookVM.book.Summary, EditBookVM.book.ID);
                Datas[Datas.IndexOf(SelectedItem)] = EditBookVM.book;
                MyMessageBox.Show("Sửa thành công", "Thông báo", false);
            }
        }

        public static void DeleteBook(BookData book)
        {
            SQLiteDataAccess.Delete("DELETE FROM CTNhapKho WHERE IDBook=" + book.ID);
            SQLiteDataAccess.Delete("DELETE FROM CTXuatKho WHERE IDBook=" + book.ID);
            SQLiteDataAccess.Delete("DELETE FROM CTTheLoai WHERE IDBook=" + book.ID);
            SQLiteDataAccess.Delete("Book", book.ID);
            int index = LogicData.AllBooks.IndexOf(book.BookName.FindBook());
            LogicData.Books.RemoveAt(index);
            LogicData.AllBooks.RemoveAt(index);
            
        }
    }
}
