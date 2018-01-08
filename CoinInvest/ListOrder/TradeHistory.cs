using GDAXClient.Services.Fills.Models.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoinInvest.ListOrder
{
    public class TradeHistory
    {
        private struct TradeData
        {
            public string BuyId;
            public decimal BuyPrice;
            public DateTime BuyTime;
            public decimal buyFee;

            public string SellId;
            public decimal SellPrice;
            public DateTime SellTime;

            public decimal Profit;
            public TimeSpan TradeTime;
        }

        Dictionary<String,TradeData> dic;
        String path;

        public TradeHistory(String path)
        {
            dic = new Dictionary<string, TradeData>();
            this.path = path;
        }

        public void AddBuy(String sellId, FillResponse buyOrder)
        {
            var tmp  = new TradeData();

            tmp.BuyId = buyOrder.Order_id.ToString();
            tmp.BuyTime = buyOrder.Created_at;
            tmp.BuyPrice = buyOrder.Price;
            tmp.buyFee = buyOrder.Fee;

            tmp.SellId = sellId;
            dic.Add(sellId, tmp);
        }

        public void AddSell(FillResponse sellOrder)
        {
            try
            {
                TradeData tmp;
                if (dic.TryGetValue(sellOrder.Order_id.ToString(), out tmp) == true)
                {
                    tmp.SellTime = DateTime.Now;
                    tmp.SellPrice = sellOrder.Price;

                    tmp.Profit = tmp.SellPrice - tmp.BuyPrice - tmp.buyFee;
                    tmp.TradeTime = tmp.BuyTime - tmp.SellTime;
                }
            }
            catch (Exception ex)
            {
                ex = null;
            }
        }


        public void Load()
        {
            try
            {
                if (File.Exists(path) == true)
                {
                    XElement root = XElement.Load(path);
                    foreach (var item in root.Elements())
                    {
                        TradeData data = new TradeData();
                        data.BuyId = item.Attribute(XmlConstants.BuyId).Value;
                        Decimal.TryParse(item.Attribute(XmlConstants.BuyPrice).Value, out data.BuyPrice);
                        DateTime.TryParse(item.Attribute(XmlConstants.BuyTime).Value, out data.BuyTime);


                        data.SellId = item.Attribute(XmlConstants.SellId).Value;
                        if (item.Attribute(XmlConstants.SellPrice).Value != null)
                        {
                            Decimal.TryParse(item.Attribute(XmlConstants.SellPrice).Value, out data.SellPrice);
                        }

                        if (item.Attribute(XmlConstants.SellTime).Value != null)
                        {
                            DateTime.TryParse(item.Attribute(XmlConstants.SellTime).Value, out data.SellTime);
                        }

                        if (item.Attribute(XmlConstants.Profit).Value != null)
                        {
                            Decimal.TryParse(item.Attribute(XmlConstants.Profit).Value, out data.Profit);
                        }

                        if (item.Attribute(XmlConstants.TradeTime).Value != null)
                        {
                            TimeSpan.TryParse(item.Attribute(XmlConstants.TradeTime).Value, out data.TradeTime);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex = null;
            }

        }


        public void Save()
        {

            XElement root = null;
            if (File.Exists(path) == true)
            {
                root = XElement.Load(path);
            }
            else
            {
                root = new XElement(XmlConstants.TradeHistory);
            }

            foreach (var item in dic)
            {
                XElement el = new XElement(XmlConstants.Trade);

                el.Add(new XAttribute(XmlConstants.BuyId, item.Value.BuyId));
                el.Add(new XAttribute(XmlConstants.BuyPrice, item.Value.BuyPrice));
                el.Add(new XAttribute(XmlConstants.BuyTime, item.Value.BuyTime));

                el.Add(new XAttribute(XmlConstants.SellId, item.Value.SellId));
                el.Add(new XAttribute(XmlConstants.SellPrice, item.Value.SellPrice));
                el.Add(new XAttribute(XmlConstants.SellTime, item.Value.SellTime));

                el.Add(new XAttribute(XmlConstants.TradeTime, item.Value.TradeTime));
                el.Add(new XAttribute(XmlConstants.Profit, item.Value.Profit));

                root.Add(el);
            }
            root.Save(path);

        }




    }
}
