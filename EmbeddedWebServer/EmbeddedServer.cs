
namespace EmbeddedWebServer
{



    public class BrowserInfo : System.IComparable<BrowserInfo>

    {

        public string Name;
        public string Path;
        public int MainSort;

        public int CompareTo(BrowserInfo other)
        {
            if (this == null || other == null)
                return 0;

            System.Console.Write(this.Name);
            System.Console.Write(other.Name);
            int MainComp = this.MainSort.CompareTo(other.MainSort);

            if (MainComp != 0)
                return MainComp;

            return this.Name.CompareTo(other.Name);
        }
    }


    public class EmbeddedServer
    {


        public static int GetRandomUnusedPort()
        {
            System.Net.Sockets.TcpListener listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, 0);
            listener.Start();
            int port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }


        public static System.Collections.Generic.List<BrowserInfo> GetPreferableBrowser()
        {
            System.Collections.Generic.List<BrowserInfo> ls = new System.Collections.Generic.List<BrowserInfo>();
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                return ls;


            int i = 0;
            using (Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.LocalMachine)
            {
                Microsoft.Win32.RegistryKey webClientsRootKey = hklm.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
                if (webClientsRootKey != null)
                    foreach (var subKeyName in webClientsRootKey.GetSubKeyNames())
                        if (webClientsRootKey.OpenSubKey(subKeyName) != null)
                            if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell") != null)
                                if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open") != null)
                                    if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command") != null)
                                    {
                                        string commandLineUri = (string)webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command").GetValue(null);
                                        if (string.IsNullOrEmpty(commandLineUri))
                                            continue;
                                        commandLineUri = commandLineUri.Trim("\"".ToCharArray());

                                        // viewer.Executable = commandLineUri;
                                        string Name = (string)webClientsRootKey.OpenSubKey(subKeyName).GetValue(null);

                                        int sort = 9998;

                                        if (Name.IndexOf("Google") != -1) sort = 1;
                                        if (Name.IndexOf("Chromium") != -1) sort = 2;
                                        if (Name.IndexOf("Firefox") != -1) sort = 3;
                                        if (Name.IndexOf("Safari") != -1) sort = 4;
                                        if (Name.IndexOf("Explorer") != -1) sort = 9999;

                                        ls.Add(new BrowserInfo()
                                        {
                                            Name = Name
                                            ,
                                            Path = commandLineUri
                                            ,
                                            MainSort = sort
                                        });

                                        i++;
                                    }
            } // End Using 



            

            using (Microsoft.Win32.RegistryKey hklm = Microsoft.Win32.Registry.CurrentUser)
            {
                Microsoft.Win32.RegistryKey webClientsRootKey = hklm.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
                if (webClientsRootKey != null)
                    foreach (var subKeyName in webClientsRootKey.GetSubKeyNames())
                        if (webClientsRootKey.OpenSubKey(subKeyName) != null)
                            if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell") != null)
                                if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open") != null)
                                    if (webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command") != null)
                                    {
                                        string commandLineUri = (string)webClientsRootKey.OpenSubKey(subKeyName).OpenSubKey("shell").OpenSubKey("open").OpenSubKey("command").GetValue(null);
                                        if (string.IsNullOrEmpty(commandLineUri))
                                            continue;
                                        commandLineUri = commandLineUri.Trim("\"".ToCharArray());

                                        // viewer.Executable = commandLineUri;
                                        string Name = (string)webClientsRootKey.OpenSubKey(subKeyName).GetValue(null);
                                        int sort = 9998;

                                        if (Name.IndexOf("Google") != -1) sort = 1;
                                        if (Name.IndexOf("Chromium") != -1) sort = 2;
                                        if (Name.IndexOf("Firefox") != -1) sort = 3;
                                        if (Name.IndexOf("Safari") != -1) sort = 4;
                                        if (Name.IndexOf("Explorer") != -1) sort = 9999;

                                        ls.Add(new BrowserInfo()
                                        {
                                            Name = Name
                                            ,
                                            Path = commandLineUri
                                            ,
                                            MainSort = sort
                                        });

                                        i++;
                                    }
            } // End Using

            ls.Sort();
            return ls;
        } // End Function GetPreferableBrowser 



        private int m_Port;
        private System.Net.IPAddress m_IP;
        private Mono.WebServer.XSPWebSource m_WebSource;
        private Mono.WebServer.ApplicationServer m_ApplicationServer;
        private string m_ServerDirectory;


        public int Port
        {
            get { return m_Port; }
            set { m_Port = value; }
        }


        public EmbeddedServer() : this(GetRandomUnusedPort())
        { }


        public EmbeddedServer(int port)
        {
            this.m_Port = port;
            this.m_IP = System.Net.IPAddress.Parse("127.0.0.1");
            this.m_WebSource = new Mono.WebServer.XSPWebSource(this.m_IP, port);

            this.m_ServerDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            this.m_ApplicationServer = new Mono.WebServer.ApplicationServer(this.m_WebSource, m_ServerDirectory);
        }


        public void AddApplication(string ApplicationDirectory)
        {
            this.m_ApplicationServer.AddApplication("127.0.0.1", this.m_Port, "/", ApplicationDirectory);
        }


        public void Start()
        {
            m_ApplicationServer.Start(false, 0);
            this.OpenBrowser();
        }


        public void OpenBrowser()
        {
            System.Collections.Generic.List<BrowserInfo> bi = GetPreferableBrowser();
            string url = "\"" + "http://127.0.0.1:" + this.m_Port.ToString() + "\"";

            if (bi.Count > 0)
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName =bi[0].Path;
                psi.Arguments = url;
                
                System.Diagnostics.Process.Start(psi);
                return;
            }

            System.Diagnostics.Process.Start(url);
        }


        public void Stop()
        {
            m_ApplicationServer.Stop();
        }


    }


}
