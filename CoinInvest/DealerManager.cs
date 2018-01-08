using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CoinInvest
{
    public class DealerManager
    {

        Client client;
        bool boStop;
        object _lock;
        bool sellOnly;

        public bool SellOnly {
            get
            {
                lock (_lock)
                {
                    return sellOnly;
                }
            }
            set 
            { 
                lock (_lock)
                {
                  sellOnly  = value;
                }
            }

        
        }

        public DealerManager(Client client)
        {
            Thread thread = new Thread( e => DoWork(client));
            thread.Start();
            boStop = false;
            _lock = new Object();
            SellOnly = false;
                
        }

        private void DoWork(Client client)
        {
            System.Timers.Timer timer;


            Dealer dealer;

            DateTime time = DateTime.Now;
            //dealer = new Dealer(client, Constants.LTC_EUR, new XElement("xxx"));
            dealer = new Dealer(client, Constants.ETH_EUR, new XElement("xxx"));

            try
            {
                while (true)
                {
                    dealer.MakeDeal();

                    lock (_lock)
                    {
                        if (boStop == true)
                        {
                            break;
                        }
                        dealer.SellOnly = sellOnly;
                    }
                    Thread.Sleep(1000);
                }

            }
            catch (Exception ex)
            {
                ex = null;
            }
            finally
            {
                dealer.RequestStop();
            }
        }

        public void Stop()
        {
            lock(_lock)
            {
                this.boStop = true;
            }
        }


    }
}
