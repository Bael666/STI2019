using System;
using System.Collections;
using System.IO;

namespace sti_semestralka.exchange_rate_fetcher {
    class RateList {

        private DateTime date;
        private ArrayList exchangeRates;
        private String filePath;

        private RateList(DateTime date) {
            this.date = date;
            this.exchangeRates = new ArrayList();
        }

        public RateList(DateTime date, String folderPath) : this(date) {
            this.filePath = Path.Combine(folderPath, DateTimeParser.DateToFileName(date));
        }

        public ArrayList getExchangeRates() {
            return exchangeRates;
        }

        public void AddExchangeRate(ExchangeRate exchangeRate) {
            exchangeRates.Add(exchangeRate);
        }

        public void SaveExchangeRates() {
            // create directory if it doesnt exist
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (StreamWriter outputFile = new StreamWriter(filePath)) {
                foreach (ExchangeRate exchangeRate in exchangeRates) {
                    outputFile.WriteLine(exchangeRate.ToString());
                }
            }
        }

        public void LoadExchangeRates() {
            this.exchangeRates = new ArrayList();

            String line;
            String[] exchangeRateData;
            using (StreamReader inputFile = new StreamReader(filePath)) {
                while ((line = inputFile.ReadLine()) != null) {
                    exchangeRateData = line.Split(';');
                    ExchangeRate exchangeRate = new ExchangeRate(exchangeRateData[0], int.Parse(exchangeRateData[1]), float.Parse(exchangeRateData[2]), float.Parse(exchangeRateData[3]));
                    exchangeRates.Add(exchangeRate);
                }
            }
        }

        public override string ToString() {
            String result = "";
            foreach (var exrate in exchangeRates) {
                result += exrate.ToString() + Environment.NewLine;
            }
            return result;
        }
    }
}
