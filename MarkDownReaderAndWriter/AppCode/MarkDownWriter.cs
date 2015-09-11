
using System.Web;


namespace MarkDownReaderAndWriter
{


    public class MarkDownWriter
    {

        public static void WriteHtml(HttpContext context, string file)
        {
            string cont = System.IO.File.ReadAllText(file, System.Text.Encoding.UTF8);

            cont = cont.Replace("</head>", "<base href =\"/ajax/Resource.ashx/\"" 
                + System.Web.HttpUtility.UrlPathEncode( 
                    Base64Encode(System.IO.Path.GetDirectoryName(file))
                )
                +"\" /></head>");
            context.Response.Output.Write(cont);
        }



        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static void Write(HttpContext context, string file)
        {
            Write(context, file, false);
        }


        public static void Write(HttpContext context, string file, bool gitHubFlavour)
        {

            context.Response.ContentType = "text/html";

            // < html xmlns = ""http://www.w3.org/1999/xhtml"">
            context.Response.Output.Write(@"<!DOCTYPE html>
<html>
<head>
    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge,chrome=1"" />
    <meta charset=""utf-8"">
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <title>");

            context.Response.Output.Write(System.IO.Path.GetFileNameWithoutExtension(file));

            context.Response.Output.Write(@"</title>
    <meta http-equiv=""cache-control"" content=""max-age=0"" />
    <meta http-equiv=""cache-control"" content=""no-cache"" />
    <meta http-equiv=""expires"" content=""0"" />
    <meta http-equiv=""expires"" content=""Tue, 01 Jan 1980 1:00:00 GMT"" />
    <meta http-equiv=""pragma"" content=""no-cache"" />
    
    <base href =""/ajax/Resource.ashx/");
            context.Response.Output.Write(System.Web.HttpUtility.UrlPathEncode(
            Base64Encode(System.IO.Path.GetDirectoryName(file))
             ));
            context.Response.Output.Write(@"/"" />
    <link rel=""canonical"" href=""");

            string str = new System.Uri(file).AbsoluteUri;
            context.Response.Output.Write(str);

            context.Response.Output.Write(@""" />");

            context.Response.Output.Write("<style type=\"text/css\">");

            // http://stackoverflow.com/questions/6784799/what-is-this-char-65279
            // It's a zero-width no-break space. It's more commonly used as a byte-order mark (BOM).
            // context.Response.WriteFile(System.Web.Hosting.HostingEnvironment.MapPath("~/style.css"));
            // http://www.fileformat.info/info/unicode/char/feff/index.htm
            // \32\32\32\32\32\32\32\32\32\32\32\32\32\32\32\32\64\102\111\110\116\45\102\97\99\101\32\123
            // \32\32\32\32\32\32\32\32\32\32\32\32\32\32\32\32\65279\64\102\111\110\116\45\102\97\99\101\32\123

            context.Response.Output.Write(
                System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/style.css"), System.Text.Encoding.UTF8)
            );
            context.Response.Output.Write("</style>");

            context.Response.Output.Write(@"
    <link type=""text/css"" rel=""stylesheet"" href=""Content/style.css"" />                                         
    <script type=""text/javascript"" src=""./Scripts/libs/jquery.js""></script>
</head>
<body>
");




            if (gitHubFlavour)
            {
                context.Response.Output.Write(Kiwi.GfmMarkdownConverter.ToHtml(file));
            }
            else
            {
                MarkdownSharp.Markdown md = new MarkdownSharp.Markdown();
                context.Response.Output.Write(md.Transform(System.IO.File.ReadAllText(file)));
            }
            
            context.Response.Output.Write(@"</body></html>");
        }


    }
}
