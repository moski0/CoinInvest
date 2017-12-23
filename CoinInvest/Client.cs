
using GDAXClient.Authentication;
using GDAXClient.Services.Accounts;
using GDAXClient.Services.Fills.Models.Responses;
using GDAXClient.Services.Orders;
using GDAXClient.Services.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinInvest
{
    public class Client : GDAXClient.GDAXClient
    {
        public Client(Authenticator authenticator, bool sandBox = false):base(authenticator, sandBox)
        {

        }


        public IEnumerable<Account> GetAllAccountsSync()
        {
            IEnumerable<Account> res = null;
            try
            {
                res = Task.Run(async () => { return await this.AccountsService.GetAllAccountsAsync(); }).Result;
            }
            catch (Exception ex)
            {
                ex = null;
            }
            return res;
        }

        public OrderResponse PlaceLimitOrderSync(OrderSide side, ProductType productId, decimal size, decimal price)
        {
            OrderResponse res = null;
            try
            {
                res = Task.Run(async () => { return await this.OrdersService.PlaceLimitOrderAsync(side, productId, size, price); }).Result;

            }
            catch (Exception ex)
            {
                ex = null;

            }
            return res;
        }

        public ProductTicker GetProductTickerSync(ProductType productPair)
        {
            ProductTicker res = null;
            try
            {
                res = Task.Run(async () => { return await this.ProductsService.GetProductTickerAsync(productPair); }).Result;
            }
            catch (Exception ex)
            {
                ex = null;

            }
            return res;

        }


        public List<OrderResponse> GetAllOrdersSync()
        {
            List<OrderResponse> res = null;
            
            try
            {
                IList<IList<OrderResponse>> tmp = null;
                tmp = Task.Run(async () => { return await this.OrdersService.GetAllOrdersAsync(); }).Result;
                res = new List<OrderResponse>();
                foreach (var item in tmp)
                {
                    foreach (var order in item)
                    {
                        res.Add(order);
                    }
                }
            }
            catch (Exception ex)
            {
                ex = null;

            }
            return res;

        }




        public FillResponse GetFillsByOrderIdSync(String id)
        {
            FillResponse res = null;

            try
            {
                var tmp = Task.Run(async () => { return await this.FillsService.GetFillsByOrderIdAsync(id); }).Result;
                int count = 0;
                foreach (var item in tmp)
                {
                    foreach (var order in item)
                    {
                        count++;
                        res = order;
                    }
                }
                if (count != 1)
                {
                    //throw new Exception("Count 'FillResponse' is not correct");
                }
              
            }
            catch (Exception ex)
            {
                ex = null;

            }
            return res;
        }

        public ProductStats GetProductStatsSync(ProductType productType)
        {
            ProductStats res = null;
            try
            {
                res = Task.Run(async () => { return await this.ProductsService.GetProductStatsAsync(productType); }).Result;
            }
            catch (Exception ex)
            {
                ex = null;

            }
            return res;


            
        }
    }
}
