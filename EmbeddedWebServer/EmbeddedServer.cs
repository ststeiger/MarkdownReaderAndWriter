
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
        }


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
            System.Diagnostics.Process.Start("http://127.0.0.1:" + this.m_Port.ToString());
        }


        public void Stop()
        {
            m_ApplicationServer.Stop();
        }


    }


}
