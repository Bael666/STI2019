using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace sti_semestralka.exchange_rate_fetcher.Banks {
    class CNB : ABank {
        // example url: https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt?date=02.03.2019
        private const String BANK_NAME = "CNB";
        private const String urlBase = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt?date=";
        private const String urlEnd = "";

        public CNB() : base(BANK_NAME) {
        }

        public static void Main(String[] args) {
            ABank cnb = new CNB();

            Task task = cnb.DownloadRateListAsync();

            task.Wait();

            cnb.RateListsLoadAll();
            foreach (var list in cnb.getRateLists()) {
                Console.WriteLine(list.ToString());
            }
        }

        public override async Task DownloadRateListAsync() {
            String date = DateTimeParser.DateToUrlCNB(DateTime.Now);

            RateList rateList = new RateList(DateTime.Now, exchangeRateListFolderPath);
            String responseData;
            using (var htttpClient = new HttpClient()) {
                using (var response = await htttpClient.GetAsync(urlBase + date)) {

                    responseData = await response.Content.ReadAsStringAsync();

                }

                int newLineIndex1 = responseData.IndexOf('\n');
                var headerDate = responseData.Substring(0, newLineIndex1);
                var textData = responseData.Substring(newLineIndex1 + 1);
                int newLineIndex2 = textData.IndexOf('\n');
                var headerAttributes = textData.Substring(0, newLineIndex2);

                textData = textData.Substring(newLineIndex2 + 1).Trim('\n');

                String[] textDataArray = textData.Split('\n');
                foreach (var line in textDataArray) {
                    var exchangeRateData = line.Split('|');
                    var exchangeRate = new ExchangeRate(exchangeRateData[3], int.Parse(exchangeRateData[2]), float.Parse(exchangeRateData[4]), float.Parse(exchangeRateData[4]));
                    rateList.AddExchangeRate(exchangeRate);
                }

                rateList.SaveExchangeRates();
                rateLists.Add(rateList);

            }
        }

        public override void RateListsLoadMonth() {
            throw new NotImplementedException();
        }

        public override void RateListsLoadWeek() {
            throw new NotImplementedException();
        }
    }
}
