using System;

using System.Diagnostics;

namespace Linux
{
    public class dpkg
    {
        public dpkg()
        {
        }


        public static bool hasDPKG()
        {
            if(System.IO.File.Exists("/usr/bin/dpkg"))
                return true;

            return false;
        }


        static bool isPackageInstalled(string packageName)
        {
            Process process = new Process();
            process.StartInfo.FileName = "dpkg";
            process.StartInfo.Arguments = "-s \"" + packageName + "\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.WaitForExit();
            int result = process.ExitCode;

            if (result == 0)
                return true;

            return false;
        }


        static string getExecutable(string packageName)
        {
            Process process = new Process();
            process.StartInfo.FileName = "dpkg";
            process.StartInfo.Arguments = "-L \"" + packageName + "\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if(output != null)
                output = output.Replace("\r", "\n");
            string[] lines = output.Split(new char[]{ '\n' }, StringSplitOptions.RemoveEmptyEntries);

            string executable = null;

            foreach (string line in lines)
            {
                if (line.IndexOf("/bin/") != -1)
                {
                    executable = line;
                    break;
                }
            }

            return executable;
        }


        public class BrowserInfo
        {
            public string Name;
            public string Path;
            public int MainSort;

        }

        public static System.Collections.Generic.List<BrowserInfo> getInstalledBrowsers()
        {
            System.Collections.Generic.List<BrowserInfo> ls = new System.Collections.Generic.List<BrowserInfo>();
            System.Collections.Generic.List<string> packageList = getPossibleBrowsers();

            foreach (string packageName in packageList)
            {
                if (isPackageInstalled(packageName))
                {
                    ls.Add(new BrowserInfo(){ 
                         Name = packageName
                        ,Path = getExecutable(packageName)
                    });

                }
            }

            return ls;
        }


        public static System.Collections.Generic.List<string> getPossibleBrowsers()
        {
            return searchPackages("www-browser");
        }


        public static System.Collections.Generic.List<string> searchPackages(string categoryName) 
        {
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();

            Process process = new Process();
            process.StartInfo.FileName = "apt-cache";
            process.StartInfo.Arguments = "search \"" + categoryName + "\"";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            //* Read the output (or the error)
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if(output != null)
                output = output.Replace("\r", "\n");

            string[] lines = output.Split(new char[]{ '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                int pos = line.IndexOf(" ");
                if (pos < 0)
                    continue;

                string packageName = line.Substring(0, pos);
                ls.Add(packageName);
            }

            return ls;
        }




    }
}

