using CoinInvest.Trade;
using GDAXClient.Services.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CoinInvest
{
    public partial class Form1 : Form
    {



        System.Timers.Timer timer;
        AbstractTrade trade;
        AbstractTrade trade2;

        //Dealer dealer;
        DealerManager manager;
        public Form1()
        {
            InitializeComponent();

        }


       

        void timerElapsed(object sender, ElapsedEventArgs e)
        {
            if (trade.TradeStatus != null)
            {
                string price = trade.TradeStatus.Price.ToString();
                string price2 = trade2.TradeStatus.Price.ToString();
                SetText(price, trade.TradeStatus.Sequence, price2);
                
            }    
        }



        private IEnumerable<Account> aaa(GDAXClient.GDAXClient client)
        {
            var result = Task.Run(async () => { return await client.AccountsService.GetAllAccountsAsync(); }).Result;
            return result;
        }

        private async void  button1_Click(object sender, EventArgs e)
        {
            try
            {
               GDAXClient.Authentication.Authenticator aut = new GDAXClient.Authentication.Authenticator(Secret.apiKey, Secret.secret, Secret.phrase);

                Client client = new Client(aut);
                manager = new DealerManager(client);


                button1.Text = "Running";
                //trade2 = new TradeRestApi(client, Constants.LTC_EUR);
                //trade = new TradeWebSockerTicker(Constants.LTC_EUR);

                //DateTime time = DateTime.Now;
                //dealer = new Dealer(client, Constants.LTC_EUR, new XElement("xxx"));

            }
            catch (Exception ex)
            {
                ex = null;
            }
            finally
            {
            }
            
        }


       
        delegate void StringArgReturningVoidDelegate(string text, long number, string text2);  
        private void SetText(string text, long number, string text2)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.textBox1.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetText);
                this.Invoke(d, new object[] { text , number, text2});
            }
            else
            {//number.ToString()+
                this.textBox1.Text = "#\r\n " + text + "   "+ "    "+ text2 +" \r\n";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                manager.Stop();
            }
            catch (Exception ex)
            {

                ex = null;
            }
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            manager.SellOnly = checkBox1.Checked;
        }  

        

        //public void xxx()
        //{
        //    ClientWebSocket socket = new ClientWebSocket();
        //    Task task = socket.ConnectAsync(new Uri("wss://ws-feed.gdax.com"), CancellationToken.None);
        //    task.Wait();
        //    Thread readThread = new Thread(
        //        delegate(object obj)
        //        {
        //            byte[] recBytes = new byte[1024];
        //            int count = 6;
        //            while (count != 0)
        //            {
        //                ArraySegment<byte> t = new ArraySegment<byte>(recBytes);
        //                Task<WebSocketReceiveResult> receiveAsync = socket.ReceiveAsync(t, CancellationToken.None);
        //                receiveAsync.Wait();
        //                string jsonString = Encoding.UTF8.GetString(recBytes);
        //                //Console.Out.WriteLine("jsonString = {0}", jsonString);

        //                SetText(jsonString);

                
        //                recBytes = new byte[1024];
        //                count--;
        //                Thread.Sleep(200);
        //            }

        //        });
        //    readThread.Start();
        //    //string json = "{\"product_ids\":[\"btc-usd\"],\"type\":\"subscribe\"}";
        //    string json = "{\"type\": \"subscribe\",\"channels\": [{ \"name\": \"ticker\", \"product_ids\": [\"LTC-EUR\"] }]}";
        //    byte[] bytes = Encoding.UTF8.GetBytes(json);
        //    ArraySegment<byte> subscriptionMessageBuffer = new ArraySegment<byte>(bytes);
        //    socket.SendAsync(subscriptionMessageBuffer, WebSocketMessageType.Text, true, CancellationToken.None);

        //}




    }
}
