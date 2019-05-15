using sti_semestralka.exchange_rate_fetcher;
using sti_semestralka.exchange_rate_fetcher.Banks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.Statistics;

namespace Semestralka {
    public partial class Analyza : Form {
        DateTime[] week;
        List<ABank> banks;

        public Analyza(List<ABank> listBank) {
            InitializeComponent();

            listView.GridLines = true;
            listView.FullRowSelect = true;
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            

            init(listBank);

            listView.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
        }


        private void init(List<ABank> listBank) {
            week = new DateTime[] { DateTime.Today.AddDays(-6), DateTime.Today.AddDays(-5), DateTime.Today.AddDays(-4),
                DateTime.Today.AddDays(-3), DateTime.Today.AddDays(-2), DateTime.Today.AddDays(-1), DateTime.Today};

            //var banks = new [new CNB(), new CSAS(), new CSOB(), new KB(), new RB()];
            banks = new List<ABank>();
            banks.Add(new CNB());
            banks.Add(new KB());
            banks.Add(new RB());
            banks.Add(new CSAS());
            banks.Add(new CSOB());

            foreach (var bank in banks) {
                try {
                    var rateLists = listBank.FirstOrDefault(o => o.name.Equals(bank.name)).getRateLists();
                    rateLists.Sort();
                    for (int i = 0; i < 7; i++) {
                        bank.getRateLists().Add(rateLists[i]);
                    }
                } catch (Exception e) { // download fail
                }
            }

            // usd only
            Dictionary<String, Double[]> nakup = new Dictionary<string, double[]>();
            Dictionary<String, Double[]> prodej = new Dictionary<string, double[]>();
            
            foreach (var bank in banks) {
                Double[] nakupy = new double[7];
                Double[] prodeje = new double[7];

                List<RateList> rateLists = bank.getRateLists();
                rateLists.Sort();

                for (int i = 0; i < rateLists.Count; i++) {
                    var exchangeRates = rateLists[i].getExchangeRates();
                    foreach (var rate in exchangeRates) {
                        if (rate.currency.Equals("USD")) {
                            var usdRate = rate;
                            nakupy[i] = usdRate.buyRate;
                            prodeje[i] = usdRate.sellRate;
                            break;
                        }
                    }
                }

                nakup.Add(bank.name, nakupy);
                prodej.Add(bank.name, prodeje);
            }

            foreach (ABank bank in banks) {
                if (bank.name.Equals("CNB")) { continue; }
                var name = bank.name;
                Double[] buyRates = nakup[name];
                Double[] sellRates = prodej[name];

                var korBuy = korelace(nakup["CNB"], buyRates);
                var korSell = korelace(prodej["CNB"], sellRates);

                var rozBuy = rozptyl(nakup["CNB"], buyRates);
                var rozSell = rozptyl(prodej["CNB"], sellRates);

                var sazBuy = sazba(nakup["CNB"], buyRates);
                var sazSell = sazba(prodej["CNB"], sellRates);

                ListViewItem item = new ListViewItem(name);
                item.SubItems.Add(String.Format("{0:0.0000}", korBuy));
                item.SubItems.Add(String.Format("{0:0.0000}", korSell));
                item.SubItems.Add(String.Format("{0:0.0000}", rozBuy));
                item.SubItems.Add(String.Format("{0:0.0000}", rozSell));
                item.SubItems.Add(String.Format("{0:0.0000}%", 100*sazBuy));
                item.SubItems.Add(String.Format("{0:0.0000}%", 100*sazSell));


                listView.Items.Add(item);
                
            }

        }


        private double korelace(Double[] cnb, Double[] other) {
            var correlation = Correlation.Pearson(cnb, other);
            return correlation;
        }
        private double rozptyl(Double[] cnb, Double[] other) {
            var diff = new Double[cnb.Length];
            for (int i = 0; i < cnb.Length; i++) {
                diff[i] = cnb[i] - other[i];
            }

            return diff.StandardDeviation();
        }
        
        private double sazba(Double[] cnb, Double[] other) {
            double diff = 0;
            for (int i = 0; i < cnb.Length; i++) {
                diff += other[i]/cnb[i];
            }

            return diff/cnb.Length;
        }

    }
}
