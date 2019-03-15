using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sti_semestralka.exchange_rate_fetcher
{
    class ExchangeRate
    {
        private String currency;
        private int unit;
        private float buyRate;
        private float sellRate;

        public ExchangeRate(String currency, int unit, float buyRate, float sellRate) {
            setCurrency(currency);
            this.unit = unit;
            this.buyRate = buyRate;
            this.sellRate = sellRate;
        }

        public String getCurrency() {
            return currency;
        }

        private void setCurrency(String currency) {
            this.currency = currency.Trim(' ', '\n', '\r', '\t').ToUpper();
        }

        public override string ToString()
        {
            return currency + ";" + unit + ";" + buyRate + ";" + sellRate;
        }
    }
}
