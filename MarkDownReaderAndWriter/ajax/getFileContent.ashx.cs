
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
            string file=context.Request.Params["file"];
            if (string.IsNullOrEmpty(file))
            {
                context.Response.ContentType="text/plain";
                context.Response.Output.WriteLine("File parameter not provided");
                return;
            }

            
            if (!System.IO.File.Exists(file))
            {
                context.Response.ContentType="text/plain";
                context.Response.Output.WriteLine("File doesn't exist...");
                return;
            }


            string ext=System.IO.Path.GetExtension(file);
            if (ext != null)
                ext=ext.ToLower();

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".htm") || System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".html"))
            {
                context.Response.ContentType="text/html";
                context.Response.WriteFile(file);
                return;
            }

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".md") || System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".gmd"))
            {
                MarkDownWriter.Write(context, file);
                return;
            }

            context.Response.ContentType="text/plain";
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