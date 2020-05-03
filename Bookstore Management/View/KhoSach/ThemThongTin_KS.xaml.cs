using BookStore_Management.ViewModel.KhoSach;
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

namespace BookStore_Management.View.KhoSach
{
    /// <summary>
    /// Interaction logic for ThemThongTin_KS.xaml
    /// </summary>
    public partial class ThemThongTin_KS : Window
    {
        public ThemThongTin_KS()
        {
            InitializeComponent();
        }
        private void AddChip_DropDownOpened(object sender, EventArgs e)
        {
            (sender as ComboBox).IsDropDownOpen = false;
        }
    }
}
