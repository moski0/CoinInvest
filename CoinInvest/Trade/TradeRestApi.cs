using CoinInvest.Trade.Models;
using GDAXClient.Services.Orders;
using GDAXClient.Services.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinInvest.Trade
{
    public class TradeRestApi:AbstractTrade
    {
        private Client client;
        public TradeRestApi(Client client, String currency)
            : base(currency)
        {
            this.client = client;
        }


        

        public override void Close()
        {
        }

        protected override Models.ITradeStatus getTradeStatus()
        {
            return GetStatus();
        }

        private Models.ITradeStatus GetStatus()
        {
            ProductType productType = Utils.GetProductType(this.Currency);
            ProductTicker ticker = client.GetProductTickerSync(productType);
            
            TradeStatus  status = new TradeStatus();
            status.Price = ticker.Price;
            return status;
        }
    }
}
