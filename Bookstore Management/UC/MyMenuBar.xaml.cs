using BookStore_Management.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStore_Management.UC
{
    /// <summary>
    /// Interaction logic for MyMenuBar.xaml
    /// </summary>
    [ContentProperty(nameof(Children))]
    public partial class MyMenuBar : UserControl
    {
        public int SelectedContentIndex { get; private set; }
        public double ButtonWidth { get => _Slider.ActualWidth; set => _Slider.Width = value; } 
        public UIElementCollection Children
        {
            get => _Menu.Children;
            set
            {
                foreach (var items in value)
                    _Menu.Children.Add(items as Button);
            }
        }

        private Brush SelectedColor { get; } = new SolidColorBrush(Color.FromRgb(24, 160, 251));

        public MyMenuBar()
        {
            InitializeComponent();
            #region animation
            Story.Children.Add(ThicknessAnimation);
            Storyboard.SetTargetName(ThicknessAnimation, _Slider.Name);
            Storyboard.SetTargetProperty(ThicknessAnimation, new PropertyPath(Border.MarginProperty));
            #endregion
        }
        #region animation
        private ThicknessAnimation ThicknessAnimation { get; } = new ThicknessAnimation() { Duration = new Duration(TimeSpan.FromMilliseconds(200)) };
        private Storyboard Story { get; } = new Storyboard();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (_Menu.Children[SelectedContentIndex] as Button).Foreground = Brushes.Black;
            (sender as Button).Foreground = SelectedColor;
            SelectedContentIndex = _Menu.Children.IndexOf(sender as Button);
            ThicknessAnimation.To = new Thickness(SelectedContentIndex * ButtonWidth, 0, 0, 0);
            Story.Begin(this);
        }
        #endregion
        private void MyMenuBar_Loaded(object sender, RoutedEventArgs e)
        {
            (_Menu.Children[SelectedContentIndex] as Button).Foreground = SelectedColor;
        }
        public void SelectTo(int index)
        {
            Button_Click(_Menu.Children[index], null);
        }
    }
}
