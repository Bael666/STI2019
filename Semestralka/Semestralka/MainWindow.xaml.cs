using Semestralka.exchange_rate_fetcher;
using sti_semestralka.exchange_rate_fetcher;
using sti_semestralka.exchange_rate_fetcher.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.IO;

namespace Semestralka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private static Dictionary<Tuple<string, DateTime>, List<MergeRates>> dictMergeRates = new Dictionary<Tuple<string, DateTime>, List<MergeRates>>();
        List<ABank> listBank;
        Graph graph;
        int minusDaysYesterday = -1;

        public MainWindow()
        {
            InitializeComponent();
            //Title = "Verze aplikace: " + Version.versionLocal;
            Title = "Rysaeva, Pilař, Pecina";
            btnRefresh.Visibility = Visibility.Hidden;
            lbVerzeLocal.Content = "Verze aplikace: " + Version.versionLocal;
            DataGridTextColumn c1 = new DataGridTextColumn();
            c1.Header = "banka";
            c1.Binding = new Binding("banka");
            c1.Width = 110;
            dataGrid.Columns.Add(c1);
            //DataGridTextColumn c2 = new DataGridTextColumn();
            //c2.Header = "měna";
            //c2.Width = 110;
            //c2.Binding = new Binding("měna");
            //dataGrid.Columns.Add(c2);
            DataGridTextColumn c2 = new DataGridTextColumn();
            c2.Header = "množství";
            c2.Width = 70;
            c2.Binding = new Binding("množství");
            dataGrid.Columns.Add(c2);
            DataGridTextColumn c3 = new DataGridTextColumn();
            c3.Header = "nákup";
            c3.Width = 90;
            c3.Binding = new Binding("nákup");
            dataGrid.Columns.Add(c3);
            DataGridTextColumn c4 = new DataGridTextColumn();
            c4.Header = "prodej";
            c4.Width = 90;
            c4.Binding = new Binding("prodej");
            dataGrid.Columns.Add(c4);
            //DataGridTextColumn c5 = new DataGridTextColumn();
            //c5.Header = "změna";
            //c5.Width = 110;
            //c5.Binding = new Binding("změna");
            //dataGrid.Columns.Add(c5);
            DataGridTextColumn c6 = new DataGridTextColumn();
            c6.Header = "doporučení";
            c6.Width = 90;
            c6.Binding = new Binding("doporučení");
            dataGrid.Columns.Add(c6);

            Connection.CheckingConnection();
            BankInit();
            Thread.Sleep(20000);
        }
        public void BankInit()
        {
            listBank = new List<ABank>();
            listBank.Add(new CNB());
            listBank.Add(new KB());
            listBank.Add(new RB());
            listBank.Add(new CSAS());
            listBank.Add(new CSOB());
            for (int i = 1; i <= 7; i++) {
                foreach (ABank bank in listBank) {
                    try {
                        Task download2 = bank.DownloadRateListAsync(DateTime.Now.AddDays(-i));
                        download2.Wait();
                    } catch (Exception e) {
                        // download failed
                    }
                    bank.RateListsLoadAll(); //nacist ze vsech souboru
                }
            }

            // naplneni list boxu
            lbVolba.SelectionMode = SelectionMode.Multiple;

            //spusti se pred stazenim, hazi exception
            //foreach (var rate in listBank[0].getRateLists()[0].getExchangeRates()) {
            //    lbVolba.Items.Add(rate.currency);
            //}

            String[] currencies = {"AUD", "BRL", "BGN", "CNY", "DKK", "EUR", "PHP", "HKD", "HRK", "INR", "IDR", "ISK", "ILS", "JPY", "ZAR", "CAD", "KRW",
                "HUF", "MYR", "MXN", "XDR", "NOK", "NZD", "PLN", "RON", "RUB", "SGD", "SEK", "CHF", "THB", "TRY", "USD", "GBP" };

            foreach (var currency in currencies)
            {
                lbVolba.Items.Add(currency);
            }


            try
            {
                Runner runner = new Runner(listBank, dictMergeRates);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            loadCurrencies();
        }
        

        public void graphUpdate(Boolean updateOnly) {
            if (dataGrid.Items.Count == 0) {
                if (!updateOnly) {
                    System.Windows.MessageBox.Show("No data available");
                    return;
                }
                if (graph != null && graph.IsDisposed) { return; } else if (graph != null) { graph.Close(); return; }
            }

            /*
            DateTime[] week = new DateTime[] { DateTime.Today.AddDays(-6), DateTime.Today.AddDays(-5), DateTime.Today.AddDays(-4),
                DateTime.Today.AddDays(-3), DateTime.Today.AddDays(-2), DateTime.Today.AddDays(-1), DateTime.Today};
                */

            DateTime[] month = new DateTime[30];

            for (int i = 0; i < month.Length; i++) {
                month[i] = DateTime.Today.AddDays(minusDaysYesterday + 1 - i);
            }
            Array.Reverse(month);

            DateTime[] dates = month;


            var currencyList = new List<String>();
            var datesList = new List<DateTime[]>();
            var bankDataSellList = new List<List<List<double>>>();
            var bankDataBuyList = new List<List<List<double>>>();

            foreach (Object selecteditem in lbVolba.SelectedItems) {
                string currency = selecteditem as String;
                List<double> csasData = new List<double>();
                List<double> csobData = new List<double>();
                List<double> kbData = new List<double>();
                List<double> rbData = new List<double>();
                List<List<double>> bankDataSell = new List<List<double>> { kbData, rbData, csasData, csobData };
                List<List<double>> bankDataBuy = new List<List<double>> { new List<double>(), new List<double>(), new List<double>(), new List<double>() };

                for (int d = 0; d < dates.Length; d++) {
                    var date = dates[d];

                    var cnb = listBank[0];

                    var rateLists = cnb.getRateLists();
                    ExchangeRate exchangeRate_cnb = null;

                    foreach (RateList rateList in rateLists) {
                        if (rateList.GetDate().Date == date.Date) {
                            foreach (ExchangeRate er in rateList.getExchangeRates()) {
                                if (er.currency == currency) {
                                    exchangeRate_cnb = er;
                                }
                            }
                        }
                    }

                    for (int i = 1; i < listBank.Count; i++) {
                        var bankRates = listBank[i].getRateLists();
                        ExchangeRate exchangeRate_other = null;

                        foreach (RateList rateList in bankRates) {
                            if (rateList.GetDate().Date == date.Date) {
                                foreach (ExchangeRate er in rateList.getExchangeRates()) {
                                    if (er.currency == currency) {
                                        exchangeRate_other = er;
                                    }
                                }
                            }
                        }

                        double sellDifference = 0;
                        double buyDifference = 0;

                        if (exchangeRate_other != null && exchangeRate_cnb != null) {
                            buyDifference = (double)exchangeRate_cnb.buyRate - (double)exchangeRate_other.buyRate;
                            sellDifference = (double)exchangeRate_other.sellRate - (double)exchangeRate_cnb.sellRate;
                        }

                        bankDataSell[i - 1].Add(sellDifference);
                        bankDataBuy[i - 1].Add(buyDifference);
                    }
                }
                currencyList.Add(currency);
                datesList.Add(dates);
                bankDataSellList.Add(bankDataSell);
                bankDataBuyList.Add(bankDataBuy);
            }
            //System.Drawing.Point topLeft = new System.Drawing.Point();

            if (updateOnly) {
                if (graph != null && !graph.IsDisposed) {
                    graph.refreshGraphs(currencyList, datesList, bankDataSellList, bankDataBuyList);
                }
            } else {
                if (graph == null || graph.IsDisposed) {
                    graph = new Graph(currencyList, datesList, bankDataSellList, bankDataBuyList);
                    graph.Show();
                } else if (graph != null) {
                    //topLeft = graph.Location;
                    graph.refreshGraphs(currencyList, datesList, bankDataSellList, bankDataBuyList);
                }
                graph.Activate();
            }

            //graph.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            //graph.Location = topLeft;
            
        }

        private void btnGraf_Click(object sender, RoutedEventArgs e)
        {
            graphUpdate(false);
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if (dataGrid.Items.Count == 0)
            {
                System.Windows.MessageBox.Show("No data available");
            }
            else
            {
                using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
                {
                    //dialog.SelectedPath = settingshandler.getStorageTB();
                    System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                    if (result.ToString() == "OK")
                    {
                        Save.ExportFilesToExcel((dataGrid.Items.OfType<MergeRates>() != null) ? dataGrid.Items.OfType<MergeRates>().ToList() : null, dialog.SelectedPath); //dataGrid.SelectedItems.Cast<GitFile>().ToList()
                        System.Windows.MessageBox.Show("Saved");
                    }
                }
            }
        }

        private void lbVolba_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            updateGrid();
            graphUpdate(true);
            saveCurrencies();
        }

        public void updateGrid() {
            foreach (ABank bank in listBank) {
                bank.RateListsLoadAll(); //nacist ze vsech souboru
            }
            HelperAutomation.TransformIntoDict(listBank, dictMergeRates);
            dataGrid.Items.Clear();

            // find first change between cnb and set yesterday to it
            minusDaysYesterday = -1;
            List<RateList> cnbRateLists;
            foreach (ABank bank in listBank) {
                if (bank.name.Equals("CNB")) {
                    cnbRateLists = bank.getRateLists();
                    cnbRateLists.Sort();
                    try {
                        for (int i = 1; i <= 7; i++) {
                            var different = false;
                            var ratesToday = cnbRateLists[0].getExchangeRates();
                            var ratesYesterday = cnbRateLists[i].getExchangeRates();
                            ratesToday.Sort();
                            ratesYesterday.Sort();

                            for (int j = 0; j < ratesToday.Count; j++) {
                                var rateToday = ratesToday[j];
                                var rateYesterday = ratesYesterday[j];

                                if (rateToday.buyRate != rateYesterday.buyRate) {
                                    minusDaysYesterday = -i;
                                    different = true;
                                    break;
                                }
                            }

                            if (different == true) {
                                break;
                            }
                        }
                    } catch (Exception e) { }

                    break;

                }
            }

            foreach (Object selecteditem in lbVolba.SelectedItems) {
                string strItem = selecteditem as String;
                List<MergeRates> dataToday = new List<MergeRates>();
                List<MergeRates> dataYesterday = new List<MergeRates>();
                try {
                    dataYesterday = dictMergeRates[Tuple.Create<string, DateTime>(strItem, DateTime.Now.Date.AddDays(minusDaysYesterday))];
                    dataToday = dictMergeRates[Tuple.Create<string, DateTime>(strItem, DateTime.Now.Date)];
                } catch (KeyNotFoundException error) {
                    dataToday = new List<MergeRates>();
                }

                //List<double> differenceSell = new List<double>();
                //List<double> differenceBuy = new List<double>();
                //int iSell = 0;
                //int iBuy = 0;
                //double bestSell = 0;
                //double bestBuy = 100000;
                //for (int i = 0; i < dataYesterday.Count; i++) {
                //    if (Double.Parse(dataToday[i].nákup) < bestBuy) {
                //        bestBuy = Double.Parse(dataToday[i].nákup);
                //        iBuy = i;
                //    }
                //    if (Double.Parse(dataToday[i].prodej) > bestSell) {
                //        bestSell = Double.Parse(dataToday[i].prodej);
                //        iSell = i;
                //    }
                //    differenceBuy.Add(Double.Parse(dataYesterday[i].nákup) - Double.Parse(dataToday[i].nákup));
                //    differenceSell.Add(Double.Parse(dataYesterday[i].prodej) - Double.Parse(dataToday[i].prodej));
                //}

                //if (differenceBuy[iBuy] > 0 && !dataToday[iBuy].banka.Equals("CNB")) {
                //    dataToday[iBuy].doporučení = "nakup";
                //}
                //if (differenceBuy[iSell] < 0 && !dataToday[iSell].banka.Equals("CNB")) {
                //    dataToday[iSell].doporučení = "prodej";
                //}
                var hlavicka = new MergeRates(strItem);
                dataGrid.Items.Add(hlavicka); // hlavicka mena
                dataGrid.Columns[dataGrid.Columns.Count - 1].MinWidth = 120;

                float cnb_zmena = 0;
                try {
                    cnb_zmena = float.Parse(dataToday.SingleOrDefault(cnb => cnb.banka.ToUpper().Equals("CNB")).nákup)
                        - float.Parse(dataYesterday.SingleOrDefault(cnb => cnb.banka.ToUpper().Equals("CNB")).nákup);
                } catch (NullReferenceException err) {
                    // chybi vcerejsi data
                    hlavicka.doporučení = "Chybi vcerejsi data";
                }

                String doporuceni = "";
                String doporuceniBezeZmeny = "Cena se nezmenila";
                int chosen = -1;

                if (cnb_zmena > 0) {
                    doporuceni = "Prodavej";
                    float highestBuyingBank = 0;
                    for (int i = 0; i < dataToday.Count; i++) {
                        var item = dataToday[i];
                        if (!item.banka.ToUpper().Equals("CNB")) {
                            var buyingBank = float.Parse(item.nákup);

                            if (buyingBank > highestBuyingBank) {
                                highestBuyingBank = buyingBank;
                                chosen = i;
                            }
                        }
                    }
                } else if (cnb_zmena < 0) {
                    doporuceni = "Nakupuj";
                    float lowestSellingBank = float.MaxValue;
                    for (int i = 0; i < dataToday.Count; i++) {
                        var item = dataToday[i];
                        if (!item.banka.ToUpper().Equals("CNB")) {
                            var sellingBank = float.Parse(item.prodej);

                            if (sellingBank < lowestSellingBank) {
                                lowestSellingBank = sellingBank;
                                chosen = i;
                            }
                        }
                    }
                }

                if (chosen >= 0) {
                    dataToday[chosen].doporučení = doporuceni;
                } else {
                    if (doporuceni.Equals("")) {
                        hlavicka.doporučení = doporuceniBezeZmeny;
                    }
                }

                foreach (var item in dataToday) {
                    dataGrid.Items.Add(item);
                }

                //// obarveni hlavicek - nefunkcni nevim proc 
                //foreach (MergeRates item in dataGrid.Items.OfType<MergeRates>())
                //{
                //    var row = dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                //    if (item.banka == "")
                //    {
                //        try
                //        {
                //            row.Background = Brushes.Red;
                //        }
                //        catch
                //        {
                //            //nevim proc je row nekdy null
                //        }
                //    }
                //}
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            updateGrid();
            graphUpdate(true);
        }

        private void Analyza_Click_1(object sender, RoutedEventArgs e) {
            Analyza analyza = new Analyza();
            analyza.Show();
        }


        private void saveCurrencies() {
            try {
                var filePath = Path.Combine("Bank", "session.txt");

                string toSave = "";

                foreach (Object selecteditem in lbVolba.SelectedItems) {
                    string currency = selecteditem as String;
                    toSave += currency + Environment.NewLine;
                }

                using (StreamWriter outputFile = new StreamWriter(filePath)) {
                    outputFile.Write(toSave);
                }
            } catch (Exception e) { }
        }

        private void loadCurrencies() {
            try {
                var filePath = Path.Combine("Bank", "session.txt");
                string currency;
                using (StreamReader inputFile = new StreamReader(filePath)) {
                    while ((currency = inputFile.ReadLine()) != null) {
                        var index = 0;
                        for (int i = 0; i < lbVolba.Items.Count; i++) {
                            if (lbVolba.Items[i].Equals(currency)) {
                                index = i;
                                break;
                            }
                        }
                        var item = lbVolba.Items[index];
                        lbVolba.SelectedItems.Add(item);
                    }
                }
            } catch (Exception e) { }
        }

    }
}
