﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Semestralka
{
    public class Connection
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool CheckConnection()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }

        public static void CheckingConnection()
        {
            Thread t = new Thread(() => OnTick());
            t.Start();
        }

        private static void OnTick()
        {
            // Repeat this loop until cancelled.
            while (true)
            {
                Application.Current.Dispatcher.Invoke(new Action(() => {

                    MainWindow win = (MainWindow)Application.Current.MainWindow;
                    win.lbStatusConnect.Content = (Connection.CheckConnection()) ? "Online" : "Offline";
                    win.lbStatusConnect.Foreground = win.lbStatusConnect.Content.Equals("Online") ? Brushes.Green : Brushes.Red;

                    if(win.lbStatusConnect.Content.Equals("Online"))
                    {
                        Task check = Version.GetVersionFromServer();
                        check.Wait();

                        if (!Version.versionServer.Equals(Version.versionLocal))
                        {
                            win.tbVerze.Text = "Dostupná nová verze: " + Version.versionServer + "\n" + Version.versionLink;
                            win.tbVerze.Foreground = Brushes.Red;
                        }
                        else
                        {
                            win.tbVerze.Text = "Verze je aktuální";
                            win.tbVerze.Foreground = Brushes.Green;
                        }            
                    }
                        
                }));
                // Wait to repeat again.
                Thread.Sleep(10000);
            }
        }

    }
}
