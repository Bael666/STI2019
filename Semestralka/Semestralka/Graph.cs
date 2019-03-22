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
        

        public Graph(String title, DateTime[] dates, List<List<double>> bankData) {
            InitializeComponent();
            init(title, dates, bankData);
            
        }

        private void init(String title, DateTime[] dates, List<List<double>> bankData) {
            Chart chart = chart1;
            chart.Series.Clear();
            chart.Titles.Add(title);
                                    
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
