using System;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;

using HtmlAgilityPack;
using System.IO;
using System.Web.Http;

namespace RSS_Generator.Controllers
{

    public class ArticleController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return GetPage();
        }

        private static HttpResponseMessage GetPage()
        {
            var htmlWeb = new HtmlWeb();
            var doc = htmlWeb.Load("http://www.hardcoregaming101.net/ninjagaiden/ninjagaiden.htm");
            doc.OptionOutputAsXml = true;
            doc.OptionFixNestedTags = true;
            doc.OptionAutoCloseOnEnd = true;

            var xml = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                    new XElement("rss",
                    new XAttribute("version", "2.0"),
                    new XElement("channel",
                    new XElement("title", "Randomly generated rss feed"),
                    new XElement("description", "Randomly generated rss feed"),
                    new XElement("item",
                        new XElement("title", "first entry"),
                        new XElement("description", "Randomly generated rss feed"),
                        new XElement("link", "http://google.com"),
                        new XElement("guid", "Randomly generated rss feed", new XAttribute("isPermaLink", "false"),
                        new XElement("pubdate", "Sun, 06 Sep 2009 16:20:00 +0000"))))));

            var writer = new StringWriter();
            xml.Save(writer);

            return new HttpResponseMessage
            {
                Content = new StringContent(writer.GetStringBuilder().ToString(),
                    Encoding.UTF8,
                    "application/xml")
            };
        }
    }
}
