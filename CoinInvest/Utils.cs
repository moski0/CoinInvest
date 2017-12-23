using GDAXClient.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinInvest
{
    public static class Utils
    {
        public static ProductType GetProductType(String currency)
        {
            ProductType res = ProductType.BtcEur;
            switch (currency)
            {
                case Constants.BTC_EUR:
                    res = ProductType.BtcEur;
                    break;
                case Constants.LTC_EUR:
                    res = ProductType.LtcEur;
                    break;
            }
            return res;
        }
    }
}
