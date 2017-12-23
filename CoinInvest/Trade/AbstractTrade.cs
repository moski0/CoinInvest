using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinInvest.Trade
{
    public abstract class AbstractTrade:ITrade
    {

        
        public AbstractTrade(String currency)
        {
            Currency = currency;   
        }

        protected string Currency { get; private set; }

        
        

        //public abstract void Reset();
        //public abstract void Start();
        public abstract void Close();

        
        public Models.ITradeStatus TradeStatus
        {
            get
            {
                return getTradeStatus();
            }
        }
        protected abstract Models.ITradeStatus getTradeStatus();
        
        

       
        
    }
}
