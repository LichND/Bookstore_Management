using BookStore_Management.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookStore_Management.UC
{
    /// <summary>
    /// Interaction logic for MyMessageBox.xaml
    /// </summary>
    public partial class MyMessageBox : Window
    {
        private MyMessageBoxViewModel ViewModel { get; set; }
        public new string Title { get => (_Title.DataContext as ControlBarViewModel).Title; set => (_Title.DataContext as ControlBarViewModel).Title = value; }
        public bool IsHaveOK { get => ViewModel.IsHaveOK; set => ViewModel.IsHaveOK = value; }
        public bool IsHaveCancel { get => ViewModel.IsHaveCancel; set => ViewModel.IsHaveCancel = value; }

        public MyMessageBox()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new MyMessageBoxViewModel() { Host = this };
        }

        public static Message Show(string content, string title = "Thông báo", bool isHaveCancel = true, bool isHaveOK = true)
        {
            MyMessageBox messageBox = new MyMessageBox() { Title = title};
            messageBox.ViewModel.Content = content;
            messageBox.ViewModel.IsHaveCancel = isHaveCancel;
            messageBox.ViewModel.IsHaveOK = isHaveOK;
            messageBox.ShowDialog();
            return messageBox.ViewModel.Message;
        }
    }
}
