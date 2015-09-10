using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Diagnostics;


namespace MarkDownReader
{
    

    static class Program
    {


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // isPackageInstalled();
            List<Linux.dpkg.BrowserInfo> ls = Linux.dpkg.getInstalledBrowsers();
            System.Console.WriteLine(ls);

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MarkDownReader());
        }
    }
}
