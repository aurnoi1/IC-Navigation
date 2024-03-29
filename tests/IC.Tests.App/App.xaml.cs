﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace IC.Tests.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow;
            if (e != null && e.Args.Count() > 0)
            {
                string imgPath = e.Args[0];
                mainWindow = new MainWindow(imgPath);
            }
            else
            {
                 mainWindow = new MainWindow();
            }

            var workArea = SystemParameters.WorkArea;
            mainWindow.Left = workArea.Left;
            mainWindow.Top = workArea.Top;
            mainWindow.Show();
        }
    }
}
