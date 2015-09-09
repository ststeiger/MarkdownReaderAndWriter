
using System.Web;


namespace MarkDownReaderAndWriter.ajax
{


    /// <summary>
    /// Summary description for Resource
    /// </summary>
    public class Resource : IHttpHandler
    {


        public static string Base64Decode(string encodedString)
        {
            byte[] data = System.Convert.FromBase64String(encodedString);
            return System.Text.Encoding.UTF8.GetString(data);
        }


        public void ProcessRequest(HttpContext context)
        {
            string fi = this.GetType().Name + ".ashx/";

            string url = context.Request.Url.OriginalString;
            int pos = url.IndexOf(fi);
            url = url.Substring(pos + fi.Length);
            System.Console.WriteLine(url);

            pos = url.IndexOf('/');
            string fol = url.Substring(0, pos);
            url = url.Substring(fol.Length + 1);
            fol = Base64Decode(fol);

            if (url != null)
            {
                pos = url.IndexOf("?");
                if (pos != -1)
                    url = url.Substring(0, pos);

                if (url.StartsWith("/"))
                    url = url.Substring(1);

                url = url.Replace('/', System.IO.Path.DirectorySeparatorChar);
            }

            string path = System.IO.Path.Combine(fol, url);
            string ext = System.IO.Path.GetExtension(path);


            if (ext != null)
                ext = ext.ToLower();


            if (System.IO.File.Exists(path))
            {
                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".css"))
                {
                    context.Response.ContentType = "text/css";
                    context.Response.WriteFile(path);
                    return;
                }

                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".js"))
                {
                    context.Response.ContentType = "application/javascriptr";
                    context.Response.WriteFile(path);
                    return;
                }

                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".svg"))
                {
                    context.Response.ContentType = "image/svg+xml";
                    context.Response.WriteFile(path);
                    return;
                }


                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".jpg") || System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".jpeg"))
                {
                    context.Response.ContentType = "image/jpeg";
                    context.Response.WriteFile(path);
                    return;
                }


                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".png"))
                {
                    context.Response.ContentType = "image/png";
                    context.Response.WriteFile(path);
                    return;
                }


                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".gif"))
                {
                    context.Response.ContentType = "image/gif";
                    context.Response.WriteFile(path);
                    return;
                }

                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".json"))
                {
                    context.Response.ContentType = "application/json";
                    context.Response.WriteFile(path);
                    return;
                }

                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".htm") || System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".html"))
                {
                    context.Response.ContentType = "text/html";
                    context.Response.WriteFile(path);
                    return;
                }

                if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".md") || System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".gmd"))
                {
                    MarkDownWriter.Write(context, path);
                    return;
                }

                context.Response.ContentType = "application/octet-stream";
                context.Response.WriteFile(path);
            }


            if (System.StringComparer.InvariantCultureIgnoreCase.Equals(ext, ".css"))
                context.Response.ContentType = "text/css";
            else
                context.Response.ContentType = "text/plain";

            context.Response.Write("/* Not found / not exists / Error */");
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