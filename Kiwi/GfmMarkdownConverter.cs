
namespace Kiwi
{


    public class GfmMarkdownConverter
    {

        private static Kiwi.Markdown.MarkdownService CreateServiceProvider()
        {
            Kiwi.Markdown.ContentProviders.FileContentProvider fcp = new Kiwi.Markdown.ContentProviders.FileContentProvider(System.IO.Path.GetTempPath());
            Kiwi.Markdown.MarkdownService ms = new Markdown.MarkdownService(fcp);
            return ms;
        }

        private static Kiwi.Markdown.MarkdownService s_ServiceProvider = CreateServiceProvider();


        public static string ToHtml(string path)
        {
            string result = null;

            if (!System.IO.File.Exists(path))
                return result;

            result = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);

            // return View(KiwiMarkdownService.Instance.GetDocument(docId));
            return s_ServiceProvider.ToHtml(result);
        }


    }


}
