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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStore_Management.UC
{
    /// <summary>
    /// Interaction logic for ControlBar.xaml
    /// </summary>
    [ContentProperty(nameof(Children))]
    public partial class ControlBar : UserControl
    {
        public bool IsHaveBack { get => ViewModel.IsHaveBack; set => ViewModel.IsHaveBack = value; }
        public bool IsHaveMaximize { get => ViewModel.IsHaveMaximize; set => ViewModel.IsHaveMaximize = value; }
        public string Title { get => ViewModel?.Title; set => ViewModel.Title = value; }
        public Message Message { get => ViewModel?.Message; set => ViewModel.Message = value; }
        public UIElementCollection Children
        {
            get => _ContentChild.Children;
            set
            {
                foreach (var items in value)
                    _ContentChild.Children.Add(items as Button);
            }
        }
        private ControlBarViewModel ViewModel { get; set; }
        public ControlBar()
        {
            InitializeComponent();
            this.DataContext = ViewModel = new ControlBarViewModel();
            FocusManager.SetFocusedElement(this, this._ContentChild);
        }
    }
}
