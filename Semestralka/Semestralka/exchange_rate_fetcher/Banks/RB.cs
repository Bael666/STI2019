using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace sti_semestralka.exchange_rate_fetcher.Banks {
    class RB : ABank {
        // XML example url: https://www.rb.cz/informacni-servis/kurzovni-listek?p_p_id=CurrencyRate_WAR_CurrencyRateportlet_INSTANCE_24de2a2f&
        //              p_p_lifecycle=2&p_p_state=normal&p_p_mode=view&p_p_resource_id=downloadXml&p_p_cacheability=cacheLevelPage&p_p_col_id=
        //              _DynamicNestedPortlet_INSTANCE_026c60a5__column-1-1&p_p_col_count=1&
        //              _CurrencyRate_WAR_CurrencyRateportlet_INSTANCE_24de2a2f_date=2019-03-28
        //
        // url for xml alone is not working -> tracks session or smth ->
        //
        // https://www.rb.cz/informacni-servis/kurzovni-listek?date=2019-03-04
        // and then get url from button and fetch xml from there
        //
        private const String BANK_NAME = "RB";
        private const String urlBase = "https://www.rb.cz/informacni-servis/kurzovni-listek?date=";
        private const String urlBaseXML = "https://www.rb.cz/informacni-servis/kurzovni-listek";
        private const String urlEnd = "";

        static void Main(string[] args) {
            ABank rb = new RB();
            Task download = rb.DownloadRateListAsync();

            download.Wait();

            rb.RateListsLoadAll();
            foreach (var list in rb.getRateLists()) {
                Console.WriteLine(list.ToString());
            }
        }

        public RB() : base(BANK_NAME) {
        }

        public override async Task DownloadRateListAsync() {
            String date = DateTimeParser.DateToUrlRB(DateTime.Now);

            RateList rateList = new RateList(DateTime.Now, exchangeRateListFolderPath);
            String responseData;
            
            using (var htttpClient = new HttpClient()) {
                
                using (var response = await htttpClient.GetAsync(urlBase + date)) {

                    responseData = await response.Content.ReadAsStringAsync();

                }

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(responseData);
                var nodes = doc.DocumentNode.SelectNodes("//a");
                var linkToXML = "";
                foreach (var node in nodes) {
                    try {
                        var href = node.Attributes["href"].Value;
                        if (href.Contains("downloadXml")) {
                            linkToXML = href;
                            break;
                        }
                    } catch (NullReferenceException e) {
                        // <a> tag without href
                    }
                }

                // if linkToXml is empty, no url found

                String xml;
                using (var response = await htttpClient.GetAsync(urlBaseXML + linkToXML)) {

                    xml = await response.Content.ReadAsStringAsync();

                }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                var buyNode = xmlDoc.DocumentElement.SelectSingleNode("/exchange_rates/exchange_rate[@type='XML_RATE_TYPE_EBNK_PURCHASE_DEVIZA']");
                var saleNode = xmlDoc.DocumentElement.SelectSingleNode("/exchange_rates/exchange_rate[@type='XML_RATE_TYPE_EBNK_SALE_DEVIZA']");

                foreach (XmlNode currencyNode in buyNode.ChildNodes) {
                    var currency = currencyNode.Attributes["name"].Value;
                    var unit = currencyNode.Attributes["quota"].Value;
                    var buyRate = currencyNode.Attributes["rate"].Value;
                    var sellRate = saleNode.SelectSingleNode("currency[@name='" + currency + "']/@rate").Value;

                    var exchangeRate = new ExchangeRate(currency, int.Parse(unit), float.Parse(buyRate), float.Parse(sellRate));

                    rateList.AddExchangeRate(exchangeRate);
                }

                rateList.SaveExchangeRates();
                rateLists.Add(rateList);

            }

        }

        public override void RateListsLoadWeek() {
            throw new NotImplementedException();
        }

        public override void RateListsLoadMonth() {
            throw new NotImplementedException();
        }
    }
}
