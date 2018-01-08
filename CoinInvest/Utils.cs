using GDAXClient.Services.Orders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
                case Constants.ETH_EUR:
                    res = ProductType.EthEur;
                    break;
            }
            return res;
        }

        //[MethodImpl(MethodImplOptions.NoInlining)]
        //public string GetCurrentMethod()
        //{
        //    StackTrace st = new StackTrace();
        //    StackFrame sf = st.GetFrame(1);

        //    return sf.GetMethod().Name;
        //}
    }
}
