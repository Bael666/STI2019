using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace sti_semestralka.exchange_rate_fetcher.Banks {
    class CSAS : ABank {
        // 
        // {api vraci pouze stary soubor pro developerske ucely, api produkce vyzaduje schvaleni bankou:
        // https://webapi.developers.erstegroup.com/api/csas/sandbox/v2/rates/exchangerates/?fromDate=2015-03-22&toDate=2015-03-23}
        // 
        //example: https://www.porovnej24.cz/kurzy/ceska-sporitelna/devizy/14.03.2019
        //
        public const String BANK_NAME = "Ceska sporitelna";
        private const String urlBase = "https://www.porovnej24.cz/kurzy/ceska-sporitelna/devizy/";
        private const String urlEnd = "";

        public CSAS() : base(BANK_NAME) {
        }

        //public static void Main(String[] args) {
        //    ABank csas = new CSAS();

        //    Task task = csas.DownloadRateListAsync();

        //    task.Wait();
            
        //    csas.RateListsLoadAll();
        //    foreach (var list in csas.getRateLists()) {
        //        Console.WriteLine(list.ToString());
        //    }
        //}

        public override async Task DownloadRateListAsync() {
            String date = DateTimeParser.DateToUrlCSAS(DateTime.Now);
            String responseData;

            if (rateLists.Any(x => x.GetDate().ToString().Contains(DateTime.Now.Date.ToString())))
            {
                rateLists.Remove(rateLists.Where(x => x.GetDate().ToString().Contains(DateTime.Now.Date.ToString())).First());
            }
            RateList rateList = new RateList(DateTime.Now, exchangeRateListFolderPath);

            using (var httpClient = new HttpClient { }) {

                using (var response = await httpClient.GetAsync(urlBase + date + urlEnd).ConfigureAwait(false)) {

                    responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }

            }

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(responseData);

            var dataSourceNodes = doc.DocumentNode.SelectNodes("//tr[@class='datasource']");            
            
            foreach (var dataSource in dataSourceNodes) {
                String currency = dataSource.SelectSingleNode("td[@class='center code']").InnerText;
                int unit = int.Parse(dataSource.SelectSingleNode("td[@class='right unit bigrightpad']").InnerText);
                float buyRate = float.Parse(dataSource.SelectSingleNode("td[@class='right buy']").InnerText);
                float sellRate = float.Parse(dataSource.SelectSingleNode("td[@class='right sell']").InnerText);

                ExchangeRate exchangeRate = new ExchangeRate(currency, unit, buyRate, sellRate);

                rateList.AddExchangeRate(exchangeRate);
            }

            rateList.SaveExchangeRates();
            rateLists.Add(rateList);
        }

        public override void RateListsLoadMonth() {
            throw new NotImplementedException();
        }

        public override void RateListsLoadWeek() {
            throw new NotImplementedException();
        }
    }
}
