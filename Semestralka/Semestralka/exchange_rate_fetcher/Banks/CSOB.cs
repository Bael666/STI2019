using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace sti_semestralka.exchange_rate_fetcher.Banks {
    class CSOB : ABank {
        // example url : https://www.csob.cz/portal/lide/kurzovni-listek/-/date/2019-02-28/kurzovni-listek.xml
        private const String BANK_NAME = "CSOB";
        private const String urlBase = "https://www.csob.cz/portal/lide/kurzovni-listek/-/date/";
        private const String urlEnd = "/kurzovni-listek.xml";

        public CSOB() : base(BANK_NAME) {
        }

        public static void Main(String[] args) {
            ABank csob = new CSOB();

            Task task = csob.DownloadRateListAsync();

            task.Wait();

            csob.RateListsLoadAll();
            foreach (var list in csob.getRateLists()) {
                Console.WriteLine(list.ToString());
            }
        }

        public override async Task DownloadRateListAsync() {
            String date = DateTimeParser.DateToUrlCSOB(DateTime.Now);

            RateList rateList = new RateList(DateTime.Now, exchangeRateListFolderPath);
            String responseData;
            using (var htttpClient = new HttpClient()) {
                using (var response = await htttpClient.GetAsync(urlBase + date + urlEnd)) {

                    responseData = await response.Content.ReadAsStringAsync();

                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseData);

                XmlNodeList countryNodes = xmlDoc.DocumentElement.SelectNodes("/ExchangeRate/Country");

                foreach (XmlNode country in countryNodes) {
                    var currency = country.Attributes["ID"].Value;
                    var unit = country.Attributes["quota"].Value;
                    XmlNode FXcashless = country.SelectSingleNode("FXcashless");
                    var buyRate = FXcashless.Attributes["Buy"].Value;
                    var sellRate = FXcashless.Attributes["Sale"].Value;

                    var exchangeRate = new ExchangeRate(currency, int.Parse(unit), float.Parse(buyRate), float.Parse(sellRate));

                    rateList.AddExchangeRate(exchangeRate);
                }

                rateList.SaveExchangeRates();
                this.rateLists.Add(rateList);

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
