using CoinInvest.Trade.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoinInvest.Trade
{
    public class TradeWebSockerTicker : AbstractTradeWebSocket
    {

        public TradeWebSockerTicker(String currency)
            : base(currency)
        {
            Reset();
        }

        

        protected override string GetRequest()
        {
            String tmp = "\""+this.Currency+"\""+"";
         

            string json = "{\"type\": \"subscribe\",\"channels\": [{ \"name\": \"ticker\", \"product_ids\": ["+tmp+"] }]}";
            return json;
        }

        protected override void ProcessResponse(string jsonString)
        {
            var obj = JsonConvert.DeserializeObject<TradeStatus>(jsonString);
            if (obj.Sequence != 0 )
            { 
                Status = obj;
            }
            else
            {
                int a = 1;
            }
        }

        
    }
}
