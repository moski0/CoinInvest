﻿using GDAXClient.Authentication;
using GDAXClient.Products;
using GDAXClient.Services.Accounts;
using GDAXClient.Services.CoinbaseAccounts;
using GDAXClient.Services.Currencies;
using GDAXClient.Services.Deposits;
using GDAXClient.Services.Fills;
using GDAXClient.Services.Orders;
using GDAXClient.Services.Payments;
using GDAXClient.Services.WithdrawalsService;
using GDAXClient.Utilities;

namespace GDAXClient
{
    public class GDAXClient
    {
        private readonly Authenticator authenticator;

        public GDAXClient(Authenticator authenticator, bool sandBox = false)
        {
            this.authenticator = authenticator;

            var httpClient = new HttpClient.HttpClient();
            var clock = new Clock();
            var httpRequestMessageService = new Services.HttpRequest.HttpRequestMessageService(clock, sandBox);

            AccountsService = new AccountsService(httpClient, httpRequestMessageService, authenticator);
            CoinbaseAccountsService = new CoinbaseAccountsService(httpClient, httpRequestMessageService, authenticator);
            OrdersService = new OrdersService(httpClient, httpRequestMessageService, authenticator);
            PaymentsService = new PaymentsService(httpClient, httpRequestMessageService, authenticator);
            WithdrawalsService = new WithdrawalsService(httpClient, httpRequestMessageService, authenticator);
            DepositsService = new DepositsService(httpClient, httpRequestMessageService, authenticator);
            ProductsService = new ProductsService(httpClient, httpRequestMessageService, authenticator);
            CurrenciesService = new CurrenciesService(httpClient, httpRequestMessageService, authenticator);
            FillsService = new FillsService(httpClient, httpRequestMessageService, authenticator);
        }

        public AccountsService AccountsService { get; set; }

        public CoinbaseAccountsService CoinbaseAccountsService { get; set; }

        public OrdersService OrdersService { get; set; }

        public PaymentsService PaymentsService { get; set; }

        public WithdrawalsService WithdrawalsService { get; set; }

        public DepositsService DepositsService { get; set; }

        public ProductsService ProductsService { get; set; }

        public CurrenciesService CurrenciesService { get; set; }

        public FillsService FillsService { get; set; }
    }
}
