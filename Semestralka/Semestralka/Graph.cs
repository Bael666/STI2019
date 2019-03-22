using sti_semestralka.exchange_rate_fetcher;
using sti_semestralka.exchange_rate_fetcher.Banks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Semestralka {
    public partial class Graph : Form {
        

        public Graph(String title, DateTime[] dates, List<List<double>> bankDataSell, List<List<double>> bankDataBuy) {
            InitializeComponent();
            init(title, dates, bankDataSell, bankDataBuy);
            this.Text = "Vývoj ceny vůči České národní bance";
            
        }

        private void init(String title, DateTime[] dates, List<List<double>> bankDataSell, List<List<double>> bankDataBuy) {
            for (int b = 0; b < 2; b++) {
                List<List<double>> bankData;
                Chart chart;
                String[] chartTitle = { title + " - Prodej", title + " - Nákup" };

                if (b == 0) {
                    bankData = bankDataSell;
                    chart = chart1;
                } else {
                    bankData = bankDataBuy;
                    chart = chart2;
                }

                chart.Titles.Add(chartTitle[b]);


                chart.Series.Clear();
                                    
                String[] bankNames = { "Komerční banka", "Raiffeisenbank", "Česká spořitelna", "Československá obchodní banka"};
                Color[] colors = { System.Drawing.Color.Blue, System.Drawing.Color.Green, System.Drawing.Color.Red, System.Drawing.Color.Black};

            
                
                    
                    for (int i = 0; i < bankData.Count; i++) {
                    var data = bankData[i];
                    var name = bankNames[i];

                    Series series = new Series {
                        Name = name,
                        Color = colors[i],
                        IsVisibleInLegend = true,
                        IsXValueIndexed = true,
                        ChartType = SeriesChartType.Column
                    };

                    for (int j = 0; j < dates.Length; j++) {
                        series.Points.AddXY(dates[j], data[j]);
                    }


                    chart.Series.Add(series);
                    

                }
                chart.Invalidate();
            }
            
        }
    }
}
