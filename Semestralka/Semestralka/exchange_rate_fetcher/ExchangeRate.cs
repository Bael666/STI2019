﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sti_semestralka.exchange_rate_fetcher
{
    public class ExchangeRate : IComparable<ExchangeRate> {
        //private String currency;
        //private int unit;
        //private float buyRate;
        //private float sellRate;

        public string currency { get; set; }
        public int unit { get; set; }
        public float buyRate { get; set; }
        public float sellRate { get; set; }

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

        public int CompareTo(ExchangeRate other) {
            return this.currency.CompareTo(other.currency);
        }
    }
}
