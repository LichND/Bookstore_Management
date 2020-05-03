using BookStore_Management.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookStore_Management.Interface
{
    public interface IChangeData
    {
        ICommand AddCommand { get; set; }
        ICommand EditCommand { get; set; }
        ICommand DeleteCommand { get; set; }
        ICommand ClearCommand { get; set; }
    }

    [System.Serializable]
    public class DataGridBaseViewModel<T>: BaseViewModel, IChangeData
    {
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ClearCommand { get; set; }

        private ObservableCollection<T> _Datas = new ObservableCollection<T>();
        public ObservableCollection<T> Datas { get => _Datas; protected set { _Datas = value; OnPropertyChange(); } }

        private T _SelectedItem = default(T);
        private int _SelectedIndex = -1;
        private T _NewItem = (T)Activator.CreateInstance(typeof(T));
        public T SelectedItem { get => _SelectedItem; set { _SelectedItem = value; OnPropertyChange(); } }
        public int SelectedIndex { get => _SelectedIndex; set { _SelectedIndex = value; OnPropertyChange(); } }
        public T NewItem { get => _NewItem; set { _NewItem = value; OnPropertyChange(); } }

        protected DataGridBaseViewModel()
        {
            AddCommand = new RelayCommand<object>(p => { return CanAdd(p); }, p => { Add(p); });
            EditCommand = new RelayCommand<object>(p => { return CanEdit(p); }, p => { Edit(p); });
            DeleteCommand = new RelayCommand<object>(p => { return CanDelete(p); }, p => { Delete(p); });
            ClearCommand = new RelayCommand<object>(p => { return true; }, p => { Clear(p); });
        }
        #region FirstLoad
        public string Query { get; set; }
        protected virtual void LoadDataBase(string table = "")
        {
            MyBackgroundWorker worker = new MyBackgroundWorker();
            if (Query is null || Query == "")
                Query = "SELECT * FROM " + table;
            worker.DoWork += Worker_DoWork;
            worker.UpdateUI += Worker_UpdateUI;
            worker.RunWorkerAsync();
        }
        protected virtual T BeforeLoadItem(T src) => src;
        protected virtual void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<T> data = SQLiteDataAccess<T>.Select(Query);
            if (data is null)
                throw new Exception("Lỗi load data");
            for (int i = 0; i < data.Count; i++)
            {
                (sender as BackgroundWorker).ReportProgress(0, BeforeLoadItem(data[i]));
                if ((sender as BackgroundWorker).CancellationPending)
                    return;
                System.Threading.Thread.Sleep(66);
            }
        }
        protected virtual void Worker_UpdateUI(object sender, ProgressChangedEventArgs e)
        {
            Datas.Add((T)e.UserState);
        }
        #endregion
        protected virtual bool CanAdd(object p) => true;
        protected virtual void Add(object p) { Datas.Add(NewItem); Clear(null); }

        protected virtual bool CanEdit(object p) => SelectedIndex >= 0;
        protected virtual void Edit(object p) { }

        protected virtual bool CanDelete(object p) => SelectedIndex >= 0;
        protected virtual void Delete(object p) { Datas.Remove(SelectedItem); SelectedIndex = -1; }

        protected virtual void Clear(object p) => NewItem = (T)Activator.CreateInstance(typeof(T));
    }
}
