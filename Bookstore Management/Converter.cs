using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BookStore_Management
{
    public class Int2Bool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value > 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public class NotBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool)value;
        }
    }
    public class Bool2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
            return DependencyProperty.UnsetValue;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                if ((Visibility)value == Visibility.Visible)
                    return true;
                return false;
            }
            return DependencyProperty.UnsetValue;
        }
    }
    public class Count2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
                return (int)value > 0 ? Visibility.Hidden : Visibility.Visible;
            if (value is long)
                return (long)value > 0 ? Visibility.Hidden : Visibility.Visible;
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public class Null2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public class Increase1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return 1;
                else return 2;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                if ((int)value == 1)
                    return true;
                else
                    return false;
            }
            return DependencyProperty.UnsetValue;
        }
    }
    public class Heigh2With : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * 9 / 16;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 9 * 16;
        }
    }
    public class Zero2Null : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && (int)value == 0)
                return null;
            if (value is long && (long)value == 0)
                return null;
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int ret;
            if (int.TryParse(value as string, out ret))
                return ret;
            return 0;
        }
    }
    public class Index2Thickness : IValueConverter
    {
        public static long Offset { get; set; } = 125;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Thickness(Offset * (int)value, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Thickness)value).Left / Offset;
        }
    }
    public class Money2String : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value as Money)?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Money money = new Money(value as string);
            if (money is null)
                return (Money)0;
            return money;
        }
    }
    public class double2long : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (long)value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public class IndentWrapConverter : IValueConverter
    {
        public static int Index { get; set; } = 4;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (((string)value)?.Length > 0) 
            {
                string tmp = value as string;
                return "   " + tmp.Replace("\n", "\n   ");
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType == typeof(string))
            {
                string s = (string)value;
                string res = "";
                int i = 0;
                while (i < s.Length)
                {
                    while (s[i] == ' ')
                    {
                        i++;
                        if (i >= s.Length)
                            break;
                    }
                    while (s[i] != '\r')
                    {
                        res += s[i];
                        i++;
                        if (i >= s.Length)
                            break;
                    }
                    i += 2;
                    if (i >= s.Length)
                        break;
                    res += '\n';
                }
                return res;
            }
            return value;
        }
    }
    public class NumberWithSignConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((long)value == 0)
                return "± 0";
            else if ((long)value > 0)
                return "+ " + value.ToString();
            else
                return "− " + (-(long)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long ret;
            string tmp = value as string;
            if (tmp[0] == '±')
                long.TryParse(tmp.Remove(0, 2), out ret);
            else if (tmp[0] == '+')
                long.TryParse(tmp.Remove(0, 2), out ret);
            else
            {
                long.TryParse(tmp.Remove(0, 2), out ret);
                ret = -ret;
            }
            return ret;
        }
    }
    public class int2Star : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string tmp = "";
                for (int i = 0; i < (int)value; i++)
                    tmp += '*';
                return tmp;
            }
            catch { return DependencyProperty.UnsetValue; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    public class CheckDateNull : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                if ((DateTime)value == default(DateTime))
                    return "N/A";
                else return value.ToString();
            }
            return "N/A";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((string)value == "N/A")
                return default(DateTime);
            DateTime ret;
            if (DateTime.TryParse(value.ToString(), out ret))
                return ret;
            return default(DateTime);
        }
    }
}
