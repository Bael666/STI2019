using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Semestralka
{
    public class Version
    {
        public static string versionLocal = "1.0.3";
        public static string versionServer = "";
        public static string versionLink = "";
        public static async Task GetVersionFromServer()
        {
            string url = "https://github.com/Bael666/STI2019/blob/master/README.md";
            
            String responseData;
            using (var htttpClient = new HttpClient())
            {
                using (var response = await htttpClient.GetAsync(url).ConfigureAwait(false))
                {

                    responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                }

                Regex regex = new Regex(@"Aktualni verze: (.*)<");
                Match match = regex.Match(responseData);
                if (match.Success)
                {
                    versionServer = match.Groups[1].Value;
                }

                regex = new Regex("Link: <a href=\"(.*)\" rel");
                match = regex.Match(responseData);
                if (match.Success)
                {
                    versionLink = match.Groups[1].Value;
                }
            }
        }
    }
}
