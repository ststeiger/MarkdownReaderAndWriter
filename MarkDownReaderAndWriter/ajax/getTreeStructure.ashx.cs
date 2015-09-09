
using System;
using System.Collections.Generic;
using System.Web;

using jQuery.Plugins.jsTree.v3;


namespace MarkDownReaderAndWriter.ajax
{


    /// <summary>
    /// Zusammenfassungsbeschreibung für TestData
    /// </summary>
    public class getTreeStructure : IHttpHandler
    {

        

        public static List<TreeItem> GetNonRootData(string dir)
        {
            List<TreeItem> ls = new List<TreeItem>();


            System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(dir);


            foreach (System.IO.DirectoryInfo di in info.GetDirectories())
            {
                TreeItem root = new TreeItem();

                root.id = System.Guid.NewGuid().ToString();
                root.text = di.Name;
                root.children = true;
                root.state = TreeItem.NodeState.closed;
                root.data = di.FullName;

                
                // Buggy & slow
                try
                {
                    root.children = false;
                    root.state = TreeItem.NodeState.leaf;

                    if ((di.Attributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
                        continue;

                        if (di.GetFiles().Length > 0 || di.GetDirectories().Length > 0)
                    {
                        root.children = true;
                        root.state = TreeItem.NodeState.closed;
                    }
                }
                catch (Exception ex)
                {
                    continue;
                    // Don't add these branches
                }
                

                // root.type = 123;
                // root.type = type_t.SO;
                // root.type = (type_t)Enum.Parse(typeof(type_t), System.Convert.ToString(dr["objtype"]));
                root.type = "Directory";
                // root.a_attr.target = "_blank";
                // root.a_attr.href = "http://127.0.0.1";

                ls.Add(root);
            }


            foreach (System.IO.FileInfo fi in info.GetFiles())
            {
                if ((fi.Attributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden)
                    continue;

                TreeItem root = new TreeItem();

                root.id = System.Guid.NewGuid().ToString();
                root.text = fi.Name;
                root.children = false;
                root.state = TreeItem.NodeState.leaf;
                root.data = fi.FullName;

                // root.type = 123;
                // root.type = type_t.SO;
                // root.type = (type_t)Enum.Parse(typeof(type_t), System.Convert.ToString(dr["objtype"]));
                root.type = "file";

                root.a_attr.target = "ifrmContent";
                // string uri = System.Web.HttpUtility.UrlEncode( new System.Uri(fi.FullName).AbsoluteUri );
                string urlFileName = System.Web.HttpUtility.UrlEncode(fi.FullName);
                root.a_attr.href = (System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath + "/ajax/getFileContent.ashx?file=").Replace("//","/") + urlFileName;

                ls.Add(root);
            } // Next fi

            /*
            System.IO.FileSystemInfo[] infos = di.GetFileSystemInfos();
            
            foreach (var fsi in infos)
            {

                if ((fsi.Attributes & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory)
                {
                    root.type = "Directory";
                }

            }
            */

            return ls;
        }

        public static string getHomePath()
        {
            // Not in .NET 2.0
            // System.Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
                return System.Environment.GetEnvironmentVariable("HOME");

            return System.Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }


        public static string getDownloadFolderPath()
        {
            if (true || System.Environment.OSVersion.Platform == System.PlatformID.Unix)
            {
                string pathDownload = System.IO.Path.Combine(getHomePath(), "Downloads");
                return pathDownload;
            }

            // http://stackoverflow.com/questions/10667012/getting-downloads-folder-in-c
            //string[] _knownFolderGuids = new string[]
            //{
            //    "{56784854-C6CB-462B-8169-88E350ACB882}", // Contacts
            //    "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", // Desktop
            //    "{FDD39AD0-238F-46AF-ADB4-6C85480369C7}", // Documents
            //    "{374DE290-123F-4565-9164-39C4925E467B}", // Downloads
            //    "{1777F761-68AD-4D8A-87BD-30B759FA33DD}", // Favorites
            //    "{BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968}", // Links
            //    "{4BD8D571-6D19-48D3-BE97-422220080E43}", // Music
            //    "{33E28130-4E1E-4676-835A-98395C3BC3BB}", // Pictures
            //    "{4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4}", // SavedGames
            //    "{7D1D3A04-DEBB-4115-95CF-2F29DA2920DA}", // SavedSearches
            //    "{18989B1D-99B5-455B-841C-AB7C74E4DDFC}", // Videos
            //};

            return System.Convert.ToString(
                Microsoft.Win32.Registry.GetValue(
                     @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
                    ,"{374DE290-123F-4565-9164-39C4925E467B}"
                    ,String.Empty
                )
            );
        }


        public static List<TreeItem> GetRootData()
        {
            List<TreeItem> ls = new List<TreeItem>();

            {
                TreeItem root = new TreeItem();
                root.id = System.Guid.NewGuid().ToString();
                root.text = "Home";
                //root.children = System.Convert.ToBoolean(dr["HasChildren"]);
                root.children = true;
                root.state = TreeItem.NodeState.closed;
                root.data = getHomePath();

                // root.type = 123;
                // root.type = type_t.SO;
                // root.type = (type_t)Enum.Parse(typeof(type_t), System.Convert.ToString(dr["objtype"]));
                root.type = "SpecialFolder";
                ls.Add(root);
            }

            {
                TreeItem root = new TreeItem();
                root.id = System.Guid.NewGuid().ToString();
                root.text = "Desktop";
                //root.children = System.Convert.ToBoolean(dr["HasChildren"]);
                root.children = true;
                root.state = TreeItem.NodeState.closed;
                root.data = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                // root.type = 123;
                // root.type = type_t.SO;
                // root.type = (type_t)Enum.Parse(typeof(type_t), System.Convert.ToString(dr["objtype"]));
                root.type = "SpecialFolder";
                ls.Add(root);
            }

            {
                TreeItem root = new TreeItem();
                root.id = System.Guid.NewGuid().ToString();
                root.text = "MyDocuments";
                //root.children = System.Convert.ToBoolean(dr["HasChildren"]);
                root.children = true;
                root.state = TreeItem.NodeState.closed;
                root.data = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // root.type = 123;
                // root.type = type_t.SO;
                // root.type = (type_t)Enum.Parse(typeof(type_t), System.Convert.ToString(dr["objtype"]));
                root.type = "SpecialFolder";
                ls.Add(root);
            }

            {
                TreeItem root = new TreeItem();
                root.id = System.Guid.NewGuid().ToString();
                root.text = "Downloads";
                //root.children = System.Convert.ToBoolean(dr["HasChildren"]);
                root.children = true;
                root.state = TreeItem.NodeState.closed;
                root.data = getDownloadFolderPath();

                // root.type = 123;
                // root.type = type_t.SO;
                // root.type = (type_t)Enum.Parse(typeof(type_t), System.Convert.ToString(dr["objtype"]));
                root.type = "SpecialFolder";
                ls.Add(root);
            }


            // System.IO.DriveInfo[] infos = System.IO.DriveInfo.GetDrives();

            //string[] drives = System.IO.Directory.GetLogicalDrives();
            //foreach (string str in drives)
            //{
            //    System.Console.WriteLine(str);
            //}

            foreach (System.IO.DriveInfo di in System.IO.DriveInfo.GetDrives())
            {
                System.Console.WriteLine(di.Name);
                System.Console.WriteLine(di.DriveType);

                TreeItem root = new TreeItem();
                root.id = System.Guid.NewGuid().ToString();
                root.text = di.Name;
                //root.children = System.Convert.ToBoolean(dr["HasChildren"]);
                root.children = true;
                root.state = TreeItem.NodeState.closed;
                root.data = di.Name;

                // root.type = 123;
                // root.type = type_t.SO;
                // root.type = (type_t)Enum.Parse(typeof(type_t), System.Convert.ToString(dr["objtype"]));
                root.type = di.DriveType;

                // root.a_attr.target = "_blank";
                // root.a_attr.href = "http://127.0.0.1";
                if (di.IsReady == true)
                    ls.Add(root);


                if (di.IsReady == true)
                {
                    double freeSpace = di.TotalFreeSpace;
                    double totalSpace = di.TotalSize;
                    double percentFree = (freeSpace / totalSpace) * 100;


                    Console.WriteLine("Drive:{0} With {1} % free", di.Name, percentFree);
                    Console.WriteLine("Space Remaining:{0}", di.AvailableFreeSpace);
                    Console.WriteLine("Percent Free Space:{0}", percentFree);
                    Console.WriteLine("Space used:{0}", di.TotalSize);

                    // TotalFreeSpace
                    Console.WriteLine("  Volume label: {0}", di.VolumeLabel);
                    Console.WriteLine("  File system: {0}", di.DriveFormat);
                    Console.WriteLine("  Available space to current user:{0, 15} bytes", di.AvailableFreeSpace);
                    Console.WriteLine("  Total available space:          {0, 15} bytes", di.TotalFreeSpace);
                    Console.WriteLine("  Total size of drive:            {0, 15} bytes ", di.TotalSize);
                }

            }

            return ls;
        }


        public void ProcessRequest(HttpContext context)
        {
            string strJSON = null;

            try
            {
                string strId = context.Request.Params["id"];
                string strData = context.Request.Params["data"];
                //type_t EntityType = (type_t)Enum.Parse(typeof(type_t), strData, true);

                System.Diagnostics.Debug.WriteLine(strId);
                System.Diagnostics.Debug.WriteLine(strData);
                // System.Diagnostics.Debug.WriteLine(EntityType);


                List<TreeItem> ls = null;

                if(StringComparer.InvariantCultureIgnoreCase.Equals(strData, "NULL"))
                    ls = GetRootData();
                else
                    ls = GetNonRootData(strData);

                strJSON = Tools.Serialization.JSON.Serialize(ls);
                // List<TreeItem > obj = Tools.Serialization.JSON.Deserialize<List<TreeItem >>(strJSON);
                // Console.WriteLine(obj);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                Tools.AJAX.cAjaxResult AjaxResult = new Tools.AJAX.cAjaxResult();
                AjaxResult.error = new Tools.AJAX.AJAXException(ex);
                strJSON = Tools.Serialization.JSON.Serialize(AjaxResult);
            }

            System.Diagnostics.Debug.WriteLine(strJSON);
            context.Response.ContentType = "application/json";
            context.Response.Write(strJSON);
        } // End Sub ProcessRequest


        public bool IsReusable
        {
            get
            {
                return false;
            }
        } // End Property IsReusable


    } // End Class MyAjax : IHttpHandler


} // End Namespace WebReportDesigner 
