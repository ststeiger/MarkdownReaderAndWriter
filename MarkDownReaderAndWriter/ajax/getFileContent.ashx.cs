
using System.Web;


namespace MarkDownReaderAndWriter.ajax
{


    /// <summary>
    /// Summary description for getFileContent
    /// </summary>
    public class getFileContent : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            string file = context.Request.Params["file"];
            if (string.IsNullOrEmpty(file))
            {
                context.Response.ContentType = "text/plain";
                context.Response.Output.WriteLine("File parameter not provided");
                return;
            }


            if (!System.IO.File.Exists(file))
            {
                context.Response.ContentType = "text/plain";
                context.Response.Output.WriteLine("File doesn't exist or is inaccessible ...");
                return;
            }


            string ext = System.IO.Path.GetExtension(file);
            if (ext != null)
                ext = ext.ToLower();

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".md"))
            {
                MarkDownWriter.Write(context, file);
                return;
            }

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".gfm"))
            {
                MarkDownWriter.Write(context, file, true);
                return;
            }

            System.Data.DataRow[] mimeInfos = MimeHandling.GetMimeInfos(ext);

            if (mimeInfos == null || mimeInfos.Length == 0)
            {
                context.Response.ContentType = "application/octet-stream";
                context.Response.WriteFile(file);
                return;
            }

            string contType = System.Convert.ToString(mimeInfos[0]["MIME_Type"]);
            context.Response.ContentType = contType;
            context.Response.WriteFile(file);
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }


    }


}
