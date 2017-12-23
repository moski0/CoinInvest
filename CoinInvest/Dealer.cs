using CoinInvest.Coin;
using CoinInvest.Trade;
using GDAXClient.Services.Accounts;
using GDAXClient.Services.Currencies.Models;
using GDAXClient.Services.Fills.Models.Responses;
using GDAXClient.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoinInvest
{
    public class Dealer
    {
        private AbstractTrade trade;
        private Client client;
        private Account accountBuy;//eur account
        private Account accountSell;// coins account
        private XElement config;
        private int precision = 8;

        private LinkedList<Guid> listRunningOrders;
        private LinkedList<Guid> listRequestedOrderIds;
        private ActualListOrder actualListOrder;

        DateTime time;
        
        ProductType productType;
        public Dealer(Client client, String currency, XElement config)
        {
            this.client = client;
            this.trade = new TradeWebSockerTicker(currency);
            this.listRequestedOrderIds = new LinkedList<Guid>();
            this.listRunningOrders = new LinkedList<Guid>();
            this.actualListOrder = new ActualListOrder();//TODO: edit for more Dealers.. file name is the same

            this.productType = Utils.GetProductType(currency);
             
        }

        private decimal GetSizeForDealEur()
        {
            decimal sizeEur = 3;//TODO: read from config
            return sizeEur;
        }



        private void SetAccounts(ProductType productType)
        {
            GDAXClient.Services.Currency currencyBuy, currencySell;

            GetCurrencies(productType, out currencyBuy, out currencySell);

            var accounts = client.GetAllAccountsSync();//TODO: get only 1 account
            
            foreach (var item in accounts)
	        {
                if (item.Currency == currencyBuy.ToString())
                {
                    this.accountBuy = item;
                }
                else if (item.Currency == currencySell.ToString())
                {
                    this.accountSell = item;
                }
	        }
        }

        private void GetCurrencies(ProductType productType,out GDAXClient.Services.Currency currencyBuy,out GDAXClient.Services.Currency currencySell)
        {
 	        switch (productType)
            {
                case ProductType.BtcEur:
                    currencyBuy = GDAXClient.Services.Currency.EUR;
                    currencySell = GDAXClient.Services.Currency.BTC;
                    break;
                default:
                    currencyBuy = GDAXClient.Services.Currency.EUR;
                    currencySell = GDAXClient.Services.Currency.LTC;
                    break;
            }
        }

        private long CalculatePrecisionNumber(int precision)
        {
            long res = 1;
            for (int i = 0; i < precision; i++)
			{
			    res*=10; 
			}
            return res;
        }

        private decimal CutSize(decimal size, decimal tradePrice)
        {
            decimal res;
            long tmp = CalculatePrecisionNumber(precision);
            decimal tmpSize = size * tmp;
            tmpSize = Math.Floor(tmpSize);
            res = tmpSize / tmp;
            return res;
        }

        private BuyCoinOrder PrepareBuyOrder(decimal sizeEur, decimal tradePrice)
        {
            decimal add = (decimal)0.003;
            decimal sizeCoin = sizeEur / tradePrice;
            sizeCoin = CutSize(sizeCoin, tradePrice);




            decimal buyPrice = tradePrice - (add * tradePrice);
            buyPrice = Math.Floor(buyPrice*100)/100;// rounding down on 2 digits

            BuyCoinOrder order = new BuyCoinOrder();
            order.BuyPrice = buyPrice;
            order.Price = buyPrice;
            order.Size = (decimal)0.01;//sizeCoin;TODO: edit accoring to min size
            return order;
        }


        private SellCoinOrder PrepareSellOrder(FillResponse buyOrder, decimal tradePrice)
        {

            decimal add = (decimal)0.005;
            decimal calcPrice = buyOrder.Price + (add * buyOrder.Price);

            //TODO: this line is possible to use only in case that we want get EURO
            #region rounding up on 2 digits
            decimal tmp = calcPrice*100;  
            tmp = Math.Floor(tmp);
            if (tmp % 10 != 0)
            {
                tmp+=1;
            }
            calcPrice = tmp/100;
            #endregion

            decimal price = (calcPrice > tradePrice) ? calcPrice : tradePrice;         

            SellCoinOrder order = new SellCoinOrder();
            order.BuyPrice = price;
            order.Price = price;
            order.Size = buyOrder.Size;
            return order;
        }


        public void Stop()
        {
            this.actualListOrder.Safe();
        }


        private Guid MakeOrder(AbstractCoinOrder order)
        {
            OrderSide type;
            if (order is BuyCoinOrder)
            {
                type = OrderSide.Buy;
            }
            else{
                type = OrderSide.Sell;
            }
            OrderResponse response = client.PlaceLimitOrderSync(type, productType, order.Size, order.Price);
            return response.Id;
        }


        private void Buy()
        {
            decimal tradePrice = this.trade.TradeStatus.Price;
            decimal sizeEur = GetSizeForDealEur();
            SetAccounts(this.productType);  
            if (IsSizeAvailable(accountBuy, sizeEur) == true)
            {
                BuyCoinOrder order = PrepareBuyOrder(sizeEur, tradePrice);
                Guid id = MakeOrder(order);
                actualListOrder.Add(id.ToString());
            }
            else
            {

            }
        }

        private void Sell()
        {
            List<OrderResponse> listOrdersRun = client.GetAllOrdersSync();
            List<String> listOrdersDone = new List<String>();
            foreach (var orderIdRequested in this.actualListOrder.GetIds())
	        {
                bool boFound = false;
                foreach (var orderRun in listOrdersRun)
                {
                    if (orderRun.Id.ToString() == orderIdRequested)
                    {
                        boFound = true;
                        break;
                    }
                }
                if (boFound == false)
                {
                    listOrdersDone.Add(orderIdRequested);//request already filled
                }

	        }

            foreach (var item in listOrdersDone)
            {
                Sell(item);
            }
        }

        private void Sell(String id)
        {
            decimal price = this.trade.TradeStatus.Price;
            FillResponse fillOrder = client.GetFillsByOrderIdSync(id);
            SellCoinOrder sellOrder = PrepareSellOrder(fillOrder, price);
            Guid sellId = MakeOrder(sellOrder);
            actualListOrder.Remove(id);
        }


        public void MakeDeal()
        {
            if ((DateTime.Now - time).TotalSeconds >30)
            {
                Buy();
                time = DateTime.Now;
            }
            Sell();
        }

        private bool IsSizeAvailable(Account account ,decimal sizeEur)
        {
            decimal diff = account.Available - sizeEur;
            return diff >0 ? true:false;
        }
    }
}
