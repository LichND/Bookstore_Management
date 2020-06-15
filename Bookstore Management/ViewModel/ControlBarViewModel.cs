using BookStore_Management.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BookStore_Management.ViewModel
{
    public class ControlBarViewModel : BaseViewModel
    {
        #region commands
        public ICommand BackWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MoveWindowCommand { get; set; }
        #endregion
        public bool IsHaveBack { get; set; } = false;
        public bool IsHaveMaximize { get; set; } = true;
        public bool IsHaveMinimize { get; set; } = true;
        public string Title { get; set; } = "Tiêu đề mặc định";
        public ControlBarViewModel()
        {
            BackWindowCommand = new RelayCommand<FrameworkElement>(p => { return true; }, p => 
            {
                Window w = GetWindow(p);
                (w.DataContext as BaseViewModel).Message = new Message(Message.MessageType.Back);
                w.Hide();
            });
            MinimizeWindowCommand = new RelayCommand<FrameworkElement>(p => { return true; }, p => 
            {
                Window w = GetWindow(p);
                if (w != null)
                    w.WindowState = WindowState.Minimized;
            });
            MaximizeWindowCommand = new RelayCommand<FrameworkElement>(p => { return true; }, p =>
            {
                Window w = GetWindow(p) as Window;
                if (w.WindowState == WindowState.Maximized)
                    w.WindowState = WindowState.Normal;
                else w.WindowState = WindowState.Maximized;
            });
            CloseWindowCommand = new RelayCommand<FrameworkElement>(p => { return true; }, p => 
            {
                Window w = GetWindow(p);
                (w.DataContext as BaseViewModel).Message = new Message(Message.MessageType.Quit);
                w.Close();
            });
            MoveWindowCommand = new RelayCommand<FrameworkElement>(p => { return true; }, p => { (GetWindow(p) as Window)?.DragMove(); });
        }
    }
}