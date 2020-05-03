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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookStore_Management.UC
{
    /// <summary>
    /// Interaction logic for WarningTextBox.xaml
    /// </summary>
    //[BindableAttribute(true)]
    public partial class WarningTextBox : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(WarningTextBox), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnTextChanged)));
        public static readonly DependencyProperty WarningProperty = DependencyProperty.Register("Warning", typeof(string), typeof(WarningTextBox), new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnWarningChanged)));

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as WarningTextBox)?.OnTextChanged(e);
        private static void OnWarningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as WarningTextBox)?.OnWarningChanged(e);

        private void OnTextChanged(DependencyPropertyChangedEventArgs e) => _TextBox.Text = e.NewValue.ToString();
        private void OnWarningChanged(DependencyPropertyChangedEventArgs e) => _Warning.Text = e.NewValue.ToString();

        public string Text { get => (string)GetValue(TextProperty); set { SetValue(TextProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text")); } }
        public string Warning { get => (string)GetValue(WarningProperty); set { SetValue(WarningProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Warning")); } }

        public WarningTextBox()
        {
            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
