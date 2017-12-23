using CoinInvest.Trade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinInvest.Trade
{
    public interface ITrade
    {
        ITradeStatus TradeStatus { get; }
    }
}
