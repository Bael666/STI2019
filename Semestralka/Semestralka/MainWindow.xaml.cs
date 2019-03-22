using Semestralka.exchange_rate_fetcher;
using sti_semestralka.exchange_rate_fetcher;
using sti_semestralka.exchange_rate_fetcher.Banks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Semestralka
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private static Dictionary<Tuple<string, DateTime>, List<MergeRates>> dictMergeRates = new Dictionary<Tuple<string, DateTime>, List<MergeRates>>();
        public MainWindow()
        {
            InitializeComponent();
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
            c2.Width = 110;
            c2.Binding = new Binding("množství");
            dataGrid.Columns.Add(c2);
            DataGridTextColumn c3 = new DataGridTextColumn();
            c3.Header = "nákup";
            c3.Width = 110;
            c3.Binding = new Binding("nákup");
            dataGrid.Columns.Add(c3);
            DataGridTextColumn c4 = new DataGridTextColumn();
            c4.Header = "prodej";
            c4.Width = 110;
            c4.Binding = new Binding("prodej");
            dataGrid.Columns.Add(c4);
            DataGridTextColumn c5 = new DataGridTextColumn();
            c5.Header = "změna";
            c5.Width = 110;
            c5.Binding = new Binding("změna");
            dataGrid.Columns.Add(c5);
            Connection.CheckingConnection();
            BankInit();
        }

        public void BankInit()
        {
            List<ABank> listBank = new List<ABank>();
            listBank.Add(new CNB());
            listBank.Add(new KB());
            listBank.Add(new RB());
            listBank.Add(new CSAS());
            listBank.Add(new CSOB());
            foreach (ABank bank in listBank)
            {
                bank.RateListsLoadAll(); //nacist ze vsech souboru
            }

            // naplneni list boxu
            lbVolba.SelectionMode = SelectionMode.Multiple;
            foreach (var rate in listBank[0].getRateLists()[0].getExchangeRates())
            {
                lbVolba.Items.Add(rate.currency);
            }

            try
            {
                Runner runner = new Runner(listBank, dictMergeRates);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void btnGraf_Click(object sender, RoutedEventArgs e)
        {

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

        private void lbVolba_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dataGrid.Items.Clear();
            foreach (Object selecteditem in lbVolba.SelectedItems)
            {
                string strItem = selecteditem as String;
                List<MergeRates> data = dictMergeRates[Tuple.Create<string, DateTime>(strItem, DateTime.Now.Date)];

                dataGrid.Items.Add(new MergeRates(strItem)); // hlavicka mena

                foreach (var item in data)
                {
                    dataGrid.Items.Add(item);
                }

                // obarveni hlavicek - nefunkcni nevim proc 
                foreach (MergeRates item in dataGrid.Items.OfType<MergeRates>())
                {
                    var row = dataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                    if (item.banka == "")
                    {
                        try
                        {
                            row.Background = Brushes.Red;
                        }
                        catch
                        {
                            //nevim proc je row nekdy null
                        }
                    }
                }
            }           
        }
    }
}
