using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management.Data
{
    public class CustomerData : ViewModel.BaseNotifyPropertyChanged
    {
        private Money _Spent = (Money)0;
        public int ID { get; set; } = LogicData.NextID("KhachHang");
        public string Name { get; set; } = "";
        public string Sex { get; set; } = LogicData.SexType?[0];
        public string Phone { get; set; } = "";
        public string Address { get; set; } = "";
        public Money Spent { get => _Spent; set { _Spent = value; OnPropertyChange(); } }

        public string Contact
        {
            get
            {
                if (StringIsNull(Phone))
                {
                    if (StringIsNull(Address))
                        return "N/A";
                    else
                        return Address;
                }
                else
                {
                    if (StringIsNull(Address))
                        return Phone;
                    else
                        return Phone + "; " + Address;
                }
            }
        }

        private bool StringIsNull(string srt) => srt == "" || srt is null;

        public override string ToString()
        {
            return "(" + ID + ",\"" + Name + "\",\"" + Sex + "\",\"" + Phone + "\",\"" + Address + "\"," + Spent.ToLong() + ')';
        }

        public CustomerData Clone()
        {
            return new CustomerData() { ID = ID, Name = string.Copy(Name), Sex = string.Copy(Sex), Phone = string.Copy(Phone), Address = string.Copy(Address), Spent = new Money(Spent.ToLong()) };
        }
    }
}
