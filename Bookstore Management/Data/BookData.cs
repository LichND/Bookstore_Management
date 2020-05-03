using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BookStore_Management.Data
{
    public class BookData : ViewModel.BaseNotifyPropertyChanged
    {
        private Money _Cost;
        private Money _DefCost;
        private long _Sold = 0;
        private long _Inventory = 0;
        private double _Ratting = 0;

        public int ID { get; set; }
        public string ImageEncode { get; set; }
        public BitmapImage Image
        {
            get
            {
                if (_Image is null)
                    _Image = ImageEncoder.StringHex2Image(ImageEncode);
                return _Image;
            }
            set
            {
                _Image = value; ImageEncode = ImageEncoder.Image2StringHex(value); OnPropertyChange();
            }
        }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public List<string> Categoties { get; set; } = new List<string>();
        public Money Cost { get => _Cost; set { _Cost = value; OnPropertyChange(); } }
        public Money DefCost { get => _DefCost; set { _DefCost = value; OnPropertyChange(); } }
        public long Sold { get => _Sold; set { _Sold = value; _Ratting = RatingCaculate(); OnPropertyChange(); } }
        public long Inventory { get => _Inventory; set { _Inventory = value; _Ratting = RatingCaculate(); OnPropertyChange(); } }
        public string Summary { get; set; }
        public string PublishingCompany { get; set; }

        private BitmapImage _Image;

        #region get only for UI
        public string AuthorYear { get => Author + " - " + Year; }
        public string MainGenre { get => Categoties.Count > 0 ? Categoties[0] : "N/A"; }
        public string SRatting { get => string.Format("{0:0.0}", _Ratting); }
        public int Ratting { get => (int)Math.Round(_Ratting); }
        #endregion
        public BookData Clone() => new BookData()
        {
            ID = ID,
            ImageEncode = ImageEncode.SafeCopy(),
            BookName = BookName.SafeCopy(),
            Author = Author.SafeCopy(),
            Year = Year,
            Categoties = Categoties.ToList(),
            Cost = Cost.Clone(),
            DefCost = DefCost.Clone(),
            Sold = Sold,
            Inventory = Inventory,
            Summary = Summary.SafeCopy(),
            PublishingCompany = PublishingCompany.SafeCopy()
        };
        public override string ToString() => "(" + ID + ",\"" + ImageEncode + "\",\"" + BookName + "\",\"" + Author + "\"," + Year + ",\"" + MainGenre + "\"," + Cost.ToLong() + ',' + DefCost.ToLong() + ',' + Sold + ',' + Inventory + ',' + Ratting + ",\"" + Summary + "\",\"" + PublishingCompany + "\")";

        private double RatingCaculate()
        {
            long tmp = _Sold + _Inventory;
            double ret = tmp == 0 ? 0 : 5.0 * _Sold / tmp;
            return ret > 5 ? 5 : ret;
        }
    }
}
