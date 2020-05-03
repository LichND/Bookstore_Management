using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management
{
    public class Money : IComparable
    {
        private long _money;
        private string _smoney;
        private void SetDataFromLong(long money)
        {
            _money = money;
            _smoney = "";
            long tmp;
            int count = -1;
            while (money > 0)
            {
                count++;
                tmp = money % 10;
                money /= 10;
                if (count == 3)
                {
                    count = 0;
                    _smoney = Separactor + _smoney;
                }
                _smoney = tmp.ToString() + _smoney;
            }
        }

        public static char Separactor { get; set; } = '.';
        public static string MoneyUnit { get; set; } = "VNĐ";
        public Money(decimal money)
        {
            SetDataFromLong((long)money);
        }
        public Money(long money)
        {
            SetDataFromLong(money);
        }
        public Money(string smoney)
        {
            smoney = smoney.Replace(MoneyUnit, "");
            smoney = smoney.Replace(" ", "");
            smoney = smoney.Replace(Separactor.ToString(), "");
            long.TryParse(smoney.Replace("N/A", ""), out _money);
            SetDataFromLong(_money);
        }
        public Money(Money money)
        {
            _money = money._money;
            _smoney = money._smoney;
        }
        public long ToLong() { return _money; }
        public override string ToString() => _money > 0 ? _smoney + " " + MoneyUnit : "N/A";

        public int CompareTo(object obj)
        {
            Money tager = obj as Money;
            if (tager.ToLong() > _money)
                return 1;
            else if (tager.ToLong() < _money)
                return -1;
            else return 0;
        }

        public Money Clone() => new Money(this);

        public static explicit operator long(Money money) => money.ToLong(); 
        public static explicit operator Money(long money) => new Money(money); 
        public static explicit operator string(Money money) => money.ToString(); 
        public static explicit operator Money(string money) => new Money(money);
        public static explicit operator decimal(Money money) => money.ToLong();
        public static explicit operator Money(decimal money) => new Money(money);
        public static explicit operator int(Money money) => (int)money.ToLong();
        public static explicit operator Money(int money) => new Money(money);
        public static Money operator *(long left, Money right) => new Money(left * right._money);
        public static Money operator *(Money left, long right) => new Money(left._money * right);
        public static Money operator *(Money left, Money right) => new Money(left._money * right._money);
        public static Money operator +(long left, Money right) => new Money(left + right._money);
        public static Money operator +(Money left, long right) => new Money(left._money + right);
        public static Money operator +(Money left, Money right) => new Money(left._money + right._money);
        public static Money operator -(long left, Money right) => new Money(left - right._money);
        public static Money operator -(Money left, long right) => new Money(left._money - right);
        public static Money operator -(Money left, Money right) => new Money(left._money - right._money);
    }

    public class MoneyEventArgs
    {
        public Money Money { get; set; }
        public MoneyEventArgs(Money money)
        {
            Money = money;
        }
    }
        
}
