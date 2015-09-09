using System;
using System.Collections.Generic;
using System.Web;

namespace MarkDownReaderAndWriter
{
    public class MimeHandling
    {


        public static System.Data.DataTable GetMimeData()
        {
            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/MimeTypes.json");
            string JSON = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
            System.Data.DataTable dt = Tools.Serialization.JSON.Deserialize<System.Data.DataTable>(JSON);

            return dt;
        }


        public static System.Data.DataTable dt = GetMimeData();


        public static System.Data.DataRow[] GetMimeInfos(string ext)
        {
             return dt.Select(@" MIME_FileExtension = '" + ext.Replace("'", "''") + @"' ");
        }


        public static string GetMimeType(string ext)
        {
            System.Data.DataRow[] filteredDataTable = dt.Select(@" MIME_FileExtension = '" + ext.Replace("'", "''") + @"' ");
            // AND MIME_IsTextFile = true ");


            foreach (System.Data.DataRow dr in filteredDataTable)
            {
                string mime = System.Convert.ToString(dr["MIME_Type"]);
                return mime;

            }

            return "text/plain";
        }


    }
}