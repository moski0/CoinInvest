using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinInvest.Coin
{
    public abstract class AbstractCoinOrder
    {
        public decimal BuyPrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal Size { get; set; }
        public decimal Price { get; set; }
        public string Currency { get; set; }
        public decimal Fee { get; set; }
        public decimal Status { get; set; }
        public long TradeId { get; set; }
        public DateTime Time { get; set; }
    }
}
