using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinInvest.Trade.Models
{
    public interface ITradeStatus
    {
        decimal Price { get; }
        decimal Best_Bid { get; }
        decimal Best_Ask { get; }
        long Sequence { get; }
    }
}
