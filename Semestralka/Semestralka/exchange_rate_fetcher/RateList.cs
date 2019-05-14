using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace sti_semestralka.exchange_rate_fetcher {
    public class RateList : IComparable<RateList> {

        private DateTime date;
        private List<ExchangeRate> exchangeRates;
        private String filePath;

        private RateList(DateTime date) {
            this.date = date;
            this.exchangeRates = new List<ExchangeRate>();
        }

        public RateList(DateTime date, String folderPath) : this(date) {
            this.filePath = Path.Combine(folderPath, DateTimeParser.DateToFileName(date));
        }

        public List<ExchangeRate> getExchangeRates() {
            return exchangeRates;
        }

        public DateTime GetDate()
        {
            return date.Date;
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
            this.exchangeRates = new List<ExchangeRate>();

            String line;
            String[] exchangeRateData;
            try {
                using (StreamReader inputFile = new StreamReader(filePath)) {
                    while ((line = inputFile.ReadLine()) != null) {
                        exchangeRateData = line.Split(';');
                        ExchangeRate exchangeRate = new ExchangeRate(exchangeRateData[0], int.Parse(exchangeRateData[1]), float.Parse(exchangeRateData[2]), float.Parse(exchangeRateData[3]));
                        exchangeRates.Add(exchangeRate);
                    }
                }
            } catch (FileNotFoundException err) {
                // file deleted after initial load
            }
        }

        public override string ToString() {
            String result = "";
            foreach (var exrate in exchangeRates) {
                result += exrate.ToString() + Environment.NewLine;
            }
            return result;
        }

        public int CompareTo(RateList other) {
            return other.GetDate().CompareTo(this.GetDate());
        }
    }
}
