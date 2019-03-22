using sti_semestralka.exchange_rate_fetcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestralka.exchange_rate_fetcher
{
    public class MergeRates
    {
        public string banka { get; set; }
        public string měna { get; set; }
        public string množství { get; set; }
        public string nákup { get; set; }
        public string prodej { get; set; }
        public string změna { get; set; } // proti predeslemu datu

        public MergeRates()
        {
            this.banka = "";
            this.měna = "";
            this.množství = "";
            this.nákup = "";
            this.prodej = "";
            this.změna = "";
        }
        public MergeRates(string banka, string měna, string množství, string nákup, string prodej)
        {
            this.banka = banka;
            this.měna = měna;
            this.množství = množství;
            this.nákup = nákup;
            this.prodej = prodej;
            this.změna = "0";
        }

        public MergeRates(string bank, ExchangeRate exchangeRate)
        {
            this.banka = bank;
            this.měna = exchangeRate.currency;
            this.množství = exchangeRate.unit.ToString();
            this.nákup = exchangeRate.buyRate.ToString();
            this.prodej = exchangeRate.sellRate.ToString();
            this.změna = "0";
        }
    }
}
