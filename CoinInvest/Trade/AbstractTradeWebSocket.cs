using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoinInvest.Trade
{
    public abstract class AbstractTradeWebSocket:AbstractTrade
    {
        protected ClientWebSocket socket;
        private object _lock = new Object();
        private object _lockInit = new Object();
        private bool boClose = false;
        private Thread thread;
        private Models.ITradeStatus tradeStatus;
        private bool boFirstRun;

        




        public AbstractTradeWebSocket(String currency):base(currency)
        {
            //Reset();
        }

        protected abstract string GetRequest();


        public void Reset()
        {
            boFirstRun = true;
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }

            socket = new ClientWebSocket();
            Task task = socket.ConnectAsync(new Uri("wss://ws-feed.gdax.com"), CancellationToken.None);
            task.Wait();
            thread = new Thread(
                delegate(object obj)
                {
                    byte[] recBytes = new byte[1024];
                    try
                    {
                        while (true)
                        {
                            ArraySegment<byte> t = new ArraySegment<byte>(recBytes);
                            Task<WebSocketReceiveResult> receiveAsync = socket.ReceiveAsync(t, CancellationToken.None);
                            receiveAsync.Wait();
                            string jsonString = Encoding.UTF8.GetString(recBytes);
                            //Console.Out.WriteLine("jsonString = {0}", jsonString);

                            //SetText(jsonString);
                            ProcessResponse(jsonString);

                            recBytes = new byte[1024];

                            lock (_lock)
                            {
                                if (boClose == true)
                                {
                                    break;
                                }
                            }
                            if (boFirstRun == true)
                            {
                                boFirstRun = false;
                                lock (_lockInit)
                                {
                                    Monitor.Pulse(_lockInit);
                                }
                            }

                            Thread.Sleep(1000);
                        }
                    }
                    catch (Exception)
                    {

                        Reset();
                    }
                

                });
            thread.Start();
            


            //string json = "{\"product_ids\":[\"btc-usd\"],\"type\":\"subscribe\"}";

            //string json = "{\"type\": \"subscribe\",\"channels\": [{ \"name\": \"ticker\", \"product_ids\": [\"LTC-EUR\"] }]}";
            string json = GetRequest();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            ArraySegment<byte> subscriptionMessageBuffer = new ArraySegment<byte>(bytes);
            socket.SendAsync(subscriptionMessageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);
            lock (_lockInit)
            {
                Monitor.Wait(_lockInit);
            }
        }

        protected abstract void ProcessResponse(string jsonString);

        //public override void Start()
        //{
        //    //Reset();
        //}

        public override void Close()
        {
            lock (_lock)
            {
                boClose = true;
            }
        }


        protected Models.ITradeStatus Status
        {
            get
            {
                lock (_lock)
                {
                    return tradeStatus;
                }
            }

            set
            {
                lock (_lock)
                {
                    tradeStatus = value;
                }
            }
        }

        protected override Models.ITradeStatus getTradeStatus()
        {
            return Status;
        }



    }
}
