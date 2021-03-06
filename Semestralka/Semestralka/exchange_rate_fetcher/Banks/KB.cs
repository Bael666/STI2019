﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace sti_semestralka.exchange_rate_fetcher.Banks {
    class KB : ABank {
        // example url: https://api.kb.cz/openapi/v1/exchange-rates?ratesValidityDate=2019-03-14T06%3A00%3A00.000Z
        public const String BANK_NAME = "Komercni banka";
        private const String urlBase = "https://api.kb.cz/openapi/v1/exchange-rates?ratesValidityDate=";
        private const String urlEnd = "T06%3A00%3A00.000Z";

        //static void Main(string[] args) {
        //    ABank kb = new KB();
        //    Task download = kb.DownloadRateListAsync();

        //    download.Wait();

        //    kb.RateListsLoadAll();
        //    foreach (var list in kb.getRateLists()) {
        //        Console.WriteLine(list.ToString());
        //    }
        //}


        public KB() : base(BANK_NAME) {
        }

        public override async Task DownloadRateListAsync(DateTime now) {
            String date = DateTimeParser.DateToUrlKB(now);

            if (rateLists.Any(x => x.GetDate().ToString().Contains(DateTime.Now.Date.ToString())))
            {
                rateLists.Remove(rateLists.Where(x => x.GetDate().ToString().Contains(DateTime.Now.Date.ToString())).First());
            }
            RateList rateList = new RateList(now, exchangeRateListFolderPath);
            String responseData;

            using (var htttpClient = new HttpClient()) {
                
                using (var response = await htttpClient.GetAsync(urlBase + date + urlEnd).ConfigureAwait(false)) {

                    responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                }
                
            }

            
            var jsonString = responseData;

            JArray jsonData = JArray.Parse(jsonString);

            var exchangeRates = jsonData[0]["exchangeRates"];

            foreach (var rate in exchangeRates) {
                String currency = rate["currencyISO"].ToString();
                int unit = int.Parse(rate["currencyUnit"].ToString());
                float buyRate = float.Parse(rate["noncashBuy"].ToString());
                float sellRate = float.Parse(rate["noncashSell"].ToString());

                ExchangeRate exchangeRate = new ExchangeRate(currency, unit, buyRate, sellRate);
                rateList.AddExchangeRate(exchangeRate);
            }

            rateList.SaveExchangeRates();
            this.rateLists.Add(rateList);
        }

        public override void RateListsLoadWeek() {
            throw new NotImplementedException();
        }

        public override void RateListsLoadMonth() {
            throw new NotImplementedException();
        }
    }

}
