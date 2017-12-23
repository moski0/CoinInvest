using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinInvest.Trade.Models
{
    public class TradeStatus : ITradeStatus
    {
        public decimal Price { get; set; }
        public decimal Best_Bid { get; set; }
        public decimal Best_Ask { get; set; }


        public long Sequence { get; set; }

        //public long Trade_Id { get; set; }
        //public double Size { get; set; }
        //public double Volume { get; set; }
        //public DateTime Time { get; set; }
    }
}
