﻿
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace sti_semestralka.exchange_rate_fetcher
{
    abstract class ABank
    {
        protected String name;
        protected ArrayList rateLists;
        protected String folderPath;
        protected String exchangeRateListFolderPath;
        // BASE_PATH -> misto ze ktereho budou moci aktualizovane verze brat stare data
        private String BASE_PATH = "";
        private const String BANK_FOLDER = "Bank";
        private const String EXCHANGE_RATE_FOLDER = "ExchangeRates";

        public ABank(String name) {
            this.name = name;
            this.rateLists = new ArrayList();
            BASE_PATH = Directory.GetCurrentDirectory();
            this.folderPath = Path.Combine(BANK_FOLDER, name);
            this.exchangeRateListFolderPath = Path.Combine(folderPath, EXCHANGE_RATE_FOLDER);
        }

        //abstract public void DownloadRateLists(DateTime from, DateTime to);

        abstract public Task DownloadRateListAsync();

        public void RateListsLoadAll() {
            rateLists = new ArrayList();
            String[] fileNames = Directory.GetFiles(this.exchangeRateListFolderPath);
            String date;
            foreach (String fileName in fileNames) {
                date = Path.GetFileNameWithoutExtension(fileName);
                RateList rateList = new RateList(DateTimeParser.FileNameToDate(date), exchangeRateListFolderPath);
                rateLists.Add(rateList);
                rateList.LoadExchangeRates();
            }
        }

        abstract public void RateListsLoadWeek();

        abstract public void RateListsLoadMonth();

        public ArrayList getRateLists() {
            return rateLists;
        }

    }
}