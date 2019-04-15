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


        List<String> title;
        List<DateTime[]> dates;
        List<List<List<double>>> bankDataSell;
        List<List<List<double>>> bankDataBuy;
        Boolean week = true;
        FlowLayoutPanel panel;

        public Graph(List<String> title, List<DateTime[]> dates, List<List<List<double>>> bankDataSell, List<List<List<double>>> bankDataBuy) {
            InitializeComponent();
            this.Text = "Vývoj ceny vůči České národní bance";
            this.panel = flowLayoutPanel1;
            this.title = title;
            this.dates = dates;
            this.bankDataSell = bankDataSell;
            this.bankDataBuy = bankDataBuy;

            refreshGraphs();
            
        }

        private void refreshGraphs() {
            foreach (Control chart in panel.Controls) {
                panel.Controls.Remove(chart);
                chart.Dispose();
            }

            panel.Controls.Clear();

            for (int i = 0; i < title.Count; i++) {

                addGraph(title[i], dates[i], bankDataSell[i], bankDataBuy[i]);

            }

            Refresh();

        }

        private void addGraph(String title, DateTime[] dates, List<List<double>> bankDataSell, List<List<double>> bankDataBuy) {
            for (int b = 0; b < 2; b++) {
                List<List<double>> bankData;

                Chart chart = new Chart {
                    Width = panel.Width - 50,
                    BackColor = Color.LightGray
                };

                ChartArea area = new ChartArea();
                area.Name = "ChrtArea";

                chart.ChartAreas.Add(area);

                Legend legend = new Legend();
                legend.Name = "Legend";

                chart.Legends.Add(legend);


                String[] chartTitle = { title + " - Prodej", title + " - Nákup" };

                if (b == 0) {
                    bankData = bankDataSell;
                } else {
                    bankData = bankDataBuy;
                }

                Title chartingTitle = chart.Titles.Add(chartTitle[b]);
                chartingTitle.Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
                
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

                    int j = 0;
                    if (week) {
                        j = dates.Length - 7;
                    }
                    for (; j < dates.Length; j++) {
                        series.Points.AddXY(dates[j], data[j]);
                        Console.WriteLine("{2} == date: {0}, data: {1}", dates[j], data[j], name);
                    }


                    chart.Series.Add(series);
                    

                }
                
                chart.Invalidate();
                panel.Controls.Add(chart);

            }
            
        }

        private void buttonViewType_Click(object sender, EventArgs e) {
            String weekTitle = "Zobrazit týden";
            String monthTitle = "Zobrazit měsíc";

            if (!week) {
                ((Button)sender).Text = monthTitle;
                week = true;
            } else {
                ((Button)sender).Text = weekTitle;
                week = false;
            }
            refreshGraphs();
        }
    }
}
