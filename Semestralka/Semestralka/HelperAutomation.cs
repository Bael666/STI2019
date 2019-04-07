using Semestralka.exchange_rate_fetcher;
using sti_semestralka.exchange_rate_fetcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semestralka
{
    public class HelperAutomation
    {
        public static void TransformIntoDict(List<ABank> listBank, Dictionary<Tuple<string, DateTime>, List<MergeRates>> dictMergeRates)
        {
            // naplneni dictionary podle men
            foreach (ABank listItem in listBank) // banky
            {
                var rateList = listItem.getRateLists();
                foreach (var rateItem in rateList) // pro kazdy datum
                {
                    foreach (var rate in rateItem.getExchangeRates()) // mena
                    {
                        // transformace
                        MergeRates mr = new MergeRates(listItem.name, rate);
                        string tmp = rateItem.GetDate().ToString();
                        if (rateItem.GetDate().ToString().Contains("07.04.2019") && mr.měna.Equals("AUD"))
                        {
                            { }
                        }

                        if (!dictMergeRates.ContainsKey(new Tuple<string, DateTime>(rate.currency, rateItem.GetDate())))
                        {
                            dictMergeRates.Add(new Tuple<string, DateTime>(rate.currency, rateItem.GetDate()), new List<MergeRates> { mr });
                        }
                        else
                        {
                            dictMergeRates[new Tuple<string, DateTime>(rate.currency, rateItem.GetDate())].Add(mr);
                        }
                    }
                }
            }
        }

        public static void CountDifference(Dictionary<Tuple<string, DateTime>, List<MergeRates>> dictMergeRates)
        {
            //var yesterdayCollection = dictMergeRates.Where(x => x.Key.Item2 == DateTime.Now.AddDays(-1));
            //var listValuesYesterday = yesterdayCollection.Select(x => x.Value).First();
            var todayCollection = dictMergeRates.Where(x => x.Key.Item2 == DateTime.Now.Date);
            var listValuesToday = todayCollection.Select(x => x.Value).ToList();

            foreach(var mena in listValuesToday)
            {
                var cnb = mena[0];
                float a = float.Parse(cnb.prodej);
                for (int i = 1; i < mena.Count(); i++)
                {
                    float b = float.Parse(mena[i].prodej);
                    mena[i].změna = (((b - a) * 100) / a).ToString();
                }
            }     
        }

    }
}
