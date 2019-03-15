using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sti_semestralka.exchange_rate_fetcher {
    static class DateTimeParser {

        public static void Main(String[] args) {

            var dateString = DateTime.Now.ToUniversalTime();
            
            Console.WriteLine(dateString);
            
        }

        public static string DateToFileName(DateTime dateTime) {
            var dateString = dateTime.ToString("MMddyyyy");
            return dateString + ".txt";
        }

        public static DateTime FileNameToDate(String fileName) {
            var month = int.Parse(fileName.Substring(0, 2));
            var day = int.Parse(fileName.Substring(2, 2));
            var year = int.Parse(fileName.Substring(4, 4));
            DateTime dateTime = new DateTime(year, month, day);
            return dateTime;
        }

        public static String DateToUrlKB(DateTime dateTime) {
            string dateString = dateTime.ToString("yyyy-MM-dd");
            return dateString;
        }

        public static String DateToUrlCNB(DateTime dateTime) {
            var dateString = dateTime.ToString("dd.MM.yyyy");
            return dateString;
        }

        public static String DateToUrlCSOB(DateTime dateTime) {
            var dateString = dateTime.ToString("yyyy-MM-dd");
            return dateString;
        }

        public static String DateToUrlCSAS(DateTime dateTime) {
            var dateString = dateTime.ToString("dd.MM.yyyy");
            return dateString;
        }

        public static String DateToUrlRB(DateTime dateTime) {
            var dateString = dateTime.ToString("yyyy-MM-dd");
            return dateString;
        }


    }
}
