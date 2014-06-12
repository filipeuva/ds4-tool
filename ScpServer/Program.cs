﻿using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Threading;

namespace ScpServer 
{
    static class Program 
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() 
        {
            Process.GetCurrentProcess().PriorityBoostEnabled = true;
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            Application.Run(new ScpForm());

            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            

            
                rk.SetValue("DS4 to XInput Mapper", Application.ExecutablePath.ToString());
            //rk.DeleteValue("DS4 to XInput Mapper",false);//           TODO: or use as fixme to remove  

        }
    }
}
