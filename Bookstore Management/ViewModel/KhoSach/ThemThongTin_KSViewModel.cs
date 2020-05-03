using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using BookStore_Management.Data;
using BookStore_Management.View.KhoSach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BookStore_Management.ViewModel.KhoSach
{
    public class ThemThongTin_KSViewModel : BaseViewModel
    {
        public ICommand OKCommand { get; set; }
        public ICommand AddPictureCommand { get; set; }
        public ICommand AddChipCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand ChipDeleteClick { get; set; }

        private BookData _book;
        private string _Notify="nothing";
        public BookData book { get => _book; set { _book = value; OnPropertyChange(); } }
        public string Notify { get => _Notify; set { _Notify = value; OnPropertyChange(); } }
        public ThemThongTin_KS Host { get => base.Parent as ThemThongTin_KS; set => base.Parent = value; }
        public ThemThongTin_KSViewModel()
        {
            OKCommand = new RelayCommand<object>(p => { return CanAnd(); }, p => { Message.Type = Message.MessageType.OK; Host.Close(); });
            AddPictureCommand = new RelayCommand<object>(p => { return true; }, p => { AddPicture(); });
            AddChipCommand = new RelayCommand<ComboBox>(p => { return true; }, p => { AddChip(p); });
            ChipDeleteClick = new RelayCommand<Chip>(p => { return true; }, p => { Host._Categories.Children.Remove(p); book.Categoties.Remove(p.Content as string); });
            ResetCommand = new RelayCommand<object>(p => { return true; }, p => { Host._Categories.Children.RemoveRange(1, book.Categoties.Count); book = new BookData() { ID = LogicData.NextID("Book") }; });
        }

        private void AddPicture()
        {
            OpenFileDialog open = new OpenFileDialog() { Multiselect = false, Filter = "Image files (*.jpg, *.png)|*.jpg;*.png|All files (*.*)|*.*"};
            if (open.ShowDialog() == true) 
                book.Image = new BitmapImage(new Uri(open.FileName));
        }
        private void AddChip(ComboBox p)
        {
            if (book.Categoties.IndexOf(p.Text) >= 0 || LogicData.Categories.IndexOf(p.Text) < 0) 
            {
                p.Text = "";
                return;
            }
            Host._Categories.Children.Add(new Chip() { Content = p.Text });
            book.Categoties.Add(p.Text);
            p.Text = "";
        }
        private bool CanAnd()
        {
            if (book.ImageEncode is null)
            {
                Notify = "Chọn ảnh bìa";
                return false;
            }
            if (book.BookName is null || book.BookName == "")
            {
                Notify = "Nhập tên sách";
                return false;
            }
            if (book.Author is null || book.Author == "") 
            {
                Notify = "Nhập tên tác giả";
                return false;
            }
            if (book.PublishingCompany is null || book.PublishingCompany == "")
            {
                Notify = "Nhập tên nhà xuất bản";
                return false;
            }
            if (book.Year == default(int))
            {
                Notify = "Nhập năm xuất bản";
                return false;
            }
            if (book.Categoties.Count == 0) 
            {
                Notify = "Nhập ít nhất một thể loại";
                return false;
            }
            if (book.Summary is null || book.Summary == "")
            {
                Notify = "Hãy viết tóm tắt ngắn cho sách";
                return false;
            }
            if (book.DefCost is null || book.DefCost?.ToLong() == 0) 
            {
                Notify = "Nhập giá gốc";
                return false;
            }
            if (book.Cost is null || book.Cost?.ToLong() == 0) 
            {
                Notify = "Nhập giá bán";
                return false;
            }
            if (book.DefCost?.ToLong() > book.Cost?.ToLong())
            {
                Notify = "Giá bán phải nhỏ hơn hoặc bằng giá gốc";
                return false;
            }
            Notify = null;
            return true;
        }
    }
}
