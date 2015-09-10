
using System;

namespace MarkDownReader
{


    public partial class MarkDownReader : System.Windows.Forms.Form
    {

        private EmbeddedWebServer.EmbeddedServer m_ews;
        private System.Windows.Forms.NotifyIcon m_TrayIcon;

        private System.Windows.Forms.ContextMenuStrip TrayIconContextMenu;
        private System.Windows.Forms.ToolStripMenuItem CloseMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReopenBrowserMenuItem;



        static void UnhandledExceptionTrapper(object sender, System.UnhandledExceptionEventArgs e)
        {
            System.Console.WriteLine(e.ExceptionObject);
            System.Windows.Forms.Application.Exit();
        }


        static void ThreadExceptionEventHandlerTrapper(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            System.Console.WriteLine(e.Exception);
            System.Windows.Forms.Application.Exit();
        }


        public MarkDownReader()
        {
            System.AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
            System.Windows.Forms.Application.ThreadException += ThreadExceptionEventHandlerTrapper;


            this.m_TrayIcon = new System.Windows.Forms.NotifyIcon();
            
            string resourceName = this.GetType().Namespace + ".MarkdownServer.ico";
            using (System.IO.Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                this.m_TrayIcon.Icon = new System.Drawing.Icon(stream);
            }


            this.m_TrayIcon.BalloonTipTitle = "Markdown Server Running HERE";
            this.m_TrayIcon.BalloonTipText = "If you exit the browser, you can close the Markdown-Server here.";
            this.m_TrayIcon.Text = "MarkdownReader Server-Instance";

            this.m_TrayIcon.DoubleClick += TrayIcon_DoubleClick;


            //Optional - Add a context menu to the TrayIcon:
            this.TrayIconContextMenu = new System.Windows.Forms.ContextMenuStrip();
            this.CloseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReopenBrowserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayIconContextMenu.SuspendLayout();


            // TrayIconContextMenu
            this.TrayIconContextMenu.Items.AddRange(
                new System.Windows.Forms.ToolStripItem[] { this.ReopenBrowserMenuItem, this.CloseMenuItem }
            );
            this.TrayIconContextMenu.Name = "TrayIconContextMenu";
            this.TrayIconContextMenu.Size = new System.Drawing.Size(153, 70);

            // CloseMenuItem
            this.CloseMenuItem.Name = "CloseMenuItem";
            this.CloseMenuItem.Size = new System.Drawing.Size(152, 22);
            this.CloseMenuItem.Text = "Shutdown Markdown-Server";
            this.CloseMenuItem.Click += new System.EventHandler(this.CloseMenuItem_Click);


            this.ReopenBrowserMenuItem.Name = "CloseMenuItem";
            this.ReopenBrowserMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ReopenBrowserMenuItem.Text = "Reopen the browser";
            this.ReopenBrowserMenuItem.Click += new System.EventHandler(this.ReopenBrowserMenuItem_Click);

            TrayIconContextMenu.ResumeLayout(false);
            this.m_TrayIcon.ContextMenuStrip = TrayIconContextMenu;

            InitializeComponent();


            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            curDir = System.IO.Path.Combine(curDir, "..");
            curDir = System.IO.Path.Combine(curDir, "..");
            curDir = System.IO.Path.Combine(curDir, "..");
            curDir = System.IO.Path.Combine(curDir, "MarkDownReaderAndWriter");

            curDir = System.IO.Path.GetFullPath(curDir);

            this.m_ews = new EmbeddedWebServer.EmbeddedServer();
            this.m_ews.AddApplication(curDir);
        } // End Constructor 


        private void TrayIcon_DoubleClick(object sender, System.EventArgs e)
        {
            this.m_TrayIcon.BalloonTipTitle = "Use right-click";
            this.m_TrayIcon.BalloonTipText = "Right click and select \"Shutdown Markdown-Server\".";
            //Here you can do stuff if the tray icon is doubleclicked
            m_TrayIcon.ShowBalloonTip(10000);
        } // End Sub TrayIcon_DoubleClick 


        private void ReopenBrowserMenuItem_Click(object sender, System.EventArgs e)
        {
            this.m_ews.OpenBrowser();
        } // End Sub ReopenBrowserMenuItem_Click 


        private void CloseMenuItem_Click(object sender, System.EventArgs e)
        {
            this.m_TrayIcon.BalloonTipTitle = "Markdown-Server shutting down";
            this.m_TrayIcon.BalloonTipText = "Shutting down - stopping web-server. ";
            m_TrayIcon.ShowBalloonTip(500);
            StopServer();
            System.Threading.Thread.Sleep(2000);
            this.m_TrayIcon.BalloonTipTitle = "Markdown-Server stopped.";
            this.m_TrayIcon.BalloonTipText = "The server has stopped. ";
            m_TrayIcon.ShowBalloonTip(500);
            System.Threading.Thread.Sleep(2000);
            m_TrayIcon.Visible = false;


            //if (System.Windows.Forms.MessageBox.Show("Do you really want to close me?", "Are you sure?"
            //    , System.Windows.Forms.MessageBoxButtons.YesNo
            //    , System.Windows.Forms.MessageBoxIcon.Exclamation,
            //        System.Windows.Forms.MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            //{
            //    System.Windows.Forms.Application.Exit();
            //}
            System.Windows.Forms.Application.Exit();
        } // End Sub CloseMenuItem_Click 


        private void btnTest_Click(object sender, System.EventArgs e)
        {
            this.m_ews.Start();
            // this.Visible = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
        } // End Sub btnTest_Click 


        public void StopServer()
        {
            try
            {
                this.m_ews.Stop();
            }
            catch (System.Exception)
            {
                // Who cares
            }
        } // End Sub StopServer


        private void MarkDownReader_Closing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            StopServer();
        } // End Sub MarkDownReader_Closing 


        private void MarkDownReader_Resize(object sender, System.EventArgs e)
        {

            if (System.Windows.Forms.FormWindowState.Minimized == this.WindowState)
            {
                m_TrayIcon.Visible = true;
                m_TrayIcon.ShowBalloonTip(500);
                this.Hide();
            }
            else if (System.Windows.Forms.FormWindowState.Normal == this.WindowState)
            {
                m_TrayIcon.Visible = false;
            }
        } // End Sub MarkDownReader_Resize 


    } // End Partial Class MarkDownReader : System.Windows.Forms.Form


} // End Namespace MarkDownReader 
