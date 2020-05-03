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
    /// Interaction logic for WarningPasswordBox.xaml
    /// </summary>
    public partial class WarningPasswordBox : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(WarningPasswordBox), new PropertyMetadata("", new PropertyChangedCallback(OnPasswordChanged)));
        public static readonly DependencyProperty WarningProperty = DependencyProperty.Register("Warning", typeof(string), typeof(WarningPasswordBox), new PropertyMetadata("", new PropertyChangedCallback(OnWarningChanged)));

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as WarningPasswordBox)?.OnTextChanged(e);
        private static void OnWarningChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as WarningPasswordBox)?.OnWarningChanged(e);

        private void OnTextChanged(DependencyPropertyChangedEventArgs e) => Password = e.NewValue.ToString();
        private void OnWarningChanged(DependencyPropertyChangedEventArgs e) => _Warning.Text = e.NewValue.ToString();

        public string Password { get => (string)GetValue(PasswordProperty); set { SetValue(PasswordProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password")); } }
        public string Warning { get => (string)GetValue(WarningProperty); set { SetValue(WarningProperty, value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Warning")); } }

        public WarningPasswordBox()
        {
            InitializeComponent();
        }
        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = _PasswordBox.Password;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
