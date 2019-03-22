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
    }
}
