using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using sti_semestralka.exchange_rate_fetcher;
using Semestralka.exchange_rate_fetcher;

namespace Semestralka
{
    public class Runner
    {
        public CancellationTokenSource cts = new CancellationTokenSource();

        public Runner(List<ABank> listBank, Dictionary<Tuple<string, DateTime>, List<MergeRates>> dictMergeRates)
        {

            var dueTime = TimeSpan.FromSeconds(1);
            var interval = TimeSpan.FromSeconds(3600);

            //Task.Run(() => CheckingRepositoryPeriodicAsync(OnTick, dueTime, interval, cts.Token, getter).Wait());
            CheckingRepositoryPeriodicAsync(dueTime, interval, cts.Token, listBank, dictMergeRates);//CancellationToken.None
        }

        // The `onTick` method will be called periodically unless cancelled.
        private static async Task CheckingRepositoryPeriodicAsync(TimeSpan dueTime, TimeSpan interval, CancellationToken token, List<ABank> listBank, Dictionary<Tuple<string, DateTime>, List<MergeRates>> dictMergeRates)
        {
            // Initial wait time before we begin the periodic loop.
            if (dueTime > TimeSpan.Zero)
                await Task.Delay(dueTime, token).ConfigureAwait(false);

            // Repeat this loop until cancelled.
            while (!token.IsCancellationRequested)
            {
                // Call our onTick function.
                //onTick?.Invoke(getter, currentDateTime);

                Task task = Task.Factory.StartNew(() => OnTick(listBank, dictMergeRates));

                //task.Wait(TimeSpan.FromMinutes(1));
                //if (!task.IsCompleted)
                //{
                //    Application.Current.Dispatcher.Invoke(new Action(() =>
                //    {
                //        MainWindow win = (MainWindow)Application.Current.MainWindow;
                //        win.lb_status.Content = "failed";
                //        win.lb_status_connect.Content = (Connection.CheckConnection()) ? "Online" : "Offline";
                //    }));
                //}

                // Wait to repeat again.
                if (interval > TimeSpan.Zero)
                    await Task.Delay(interval, token).ConfigureAwait(false);
            }
        }

        private static void OnTick(List<ABank> listBank, Dictionary<Tuple<string, DateTime>, List<MergeRates>> dictMergeRates)
        {
            //Task.Run(() => CheckingRepositoryPeriodicAsync(OnTick, dueTime, interval, cts.Token, getter).Wait());
            //CancellationToken.None
            //CheckingConnectionAsync(OnTick, dueTime, interval, CancellationToken.None);

            // stazeni poslednich dat
            foreach (ABank bank in listBank)
            {
                Task download = bank.DownloadRateListAsync();
                download.Wait();
            }

            HelperAutomation.TransformIntoDict(listBank, dictMergeRates);
            HelperAutomation.CountDifference(dictMergeRates);
        }
    }
}
