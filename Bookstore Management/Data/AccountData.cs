using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BookStore_Management.Data
{
    public class AccountData: INotifyPropertyChanged, ICloneable
    {
        public int ID { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public int PasswordLength { get; set; }
        public int IDType { get; set; }
        public string NickName { get=>_NickName; set { _NickName = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NickName")); } } 
        public string Sex { get => _Sex; set { _Sex = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Sex")); } } 
        public string Phone { get => _Phone; set { _Phone = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Phone")); } }
        public string Address { get => _Address; set { _Address = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Address")); } }
        public DateTime LastLogin { get; set; } = default(DateTime);
        public BitmapImage Avatar { get => _Avatar; set { _Avatar = value; _AvatarEncode = ImageEncoder.Image2StringHex(value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Avatar")); } }
        public string AvatarEncode { get => _AvatarEncode; set { _AvatarEncode = value; _Avatar = ImageEncoder.StringHex2Image(value); } }

        public string AccountType { get => LogicData.AccountType[IDType]; set { IDType = LogicData.AccountType.IndexOf(value); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AccountType")); } }

        private string _NickName = "";
        private string _Sex = LogicData.SexType?[0];
        private string _Phone = "";
        private string _Address = "";
        private string _AvatarEncode = "";
        private BitmapImage _Avatar;

        public event PropertyChangedEventHandler PropertyChanged;

        public object Clone()
        {
            AccountData ret = new AccountData()
            {
                ID = ID,
                Username = string.Copy(Username),
                Password = string.Copy(Password),
                PasswordLength = PasswordLength,
                IDType = IDType,
                NickName = string.Copy(NickName),
                Sex = string.Copy(Sex),
                Phone = string.Copy(Phone),
                Address = string.Copy(Address),
                LastLogin = LastLogin,
                AvatarEncode = string.Copy(AvatarEncode)
            };
            return ret;
        }

        public override string ToString()
        {
            return "(" + ID + ",\"" + Username + "\",\"" + Password + "\"," + PasswordLength + ",\"" + IDType + "\",NULL,NULL,NULL,NULL,NULL,NULL)";
        }
    }
}
