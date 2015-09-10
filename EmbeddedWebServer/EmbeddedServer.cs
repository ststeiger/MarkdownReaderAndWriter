
namespace EmbeddedWebServer
{



    public class EmbeddedServer
    {


        public static int GetRandomUnusedPort()
        {
            System.Net.Sockets.TcpListener listener = new System.Net.Sockets.TcpListener(System.Net.IPAddress.Any, 0);
            listener.Start();
            int port = ((System.Net.IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        } // End Function GetRandomUnusedPort 



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

            System.Collections.Generic.List<PlatformInfo.BrowserInfo> bi = PlatformInfo.DistroInfo.GetPreferableBrowser(); 
            string url = "\"" + "http://127.0.0.1:" + this.m_Port.ToString() + "/Index.htm\"";

            if (bi.Count > 0)
            {
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                psi.FileName =bi[0].Path;
                psi.Arguments = url;
                
                System.Diagnostics.Process.Start(psi);
                return;
            }

            System.Diagnostics.Process.Start(url);
        } // End Sub OpenBrowser 


        public void Stop()
        {
            m_ApplicationServer.Stop();
        }


    } // End Class EmbeddedServer 


} // End Namespace EmbeddedWebServer 
