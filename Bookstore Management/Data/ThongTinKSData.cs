using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore_Management.Data
{
    public class ThongTinKSData
    {
        public string ID { get; set; }
        public string BookName { get; set; }
        public string MainGenre { get; set; }
        public long Sold { get; set; }
        public long Inventory { get; set; }
        public Money DefCost { get; set; }
        public Money Cost { get; set; }
    }
}
