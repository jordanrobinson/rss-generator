using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Xml.Linq;
using HtmlAgilityPack;
using RSSGenerator.Models;

namespace RSSGenerator.Controllers
{
    public class ArticleController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var sources = "";

            sources = sources;


            var feed = GetPage("Hardcore Gaming 101", "Random articles from the hardcore gaming 101 website");
            WriteFeed("hardcoregaming101.xml", feed);
            return feed;
        }

        private static void WriteFeed(string fileName, HttpResponseMessage content)
        {
            var contentString = content.Content.ReadAsStringAsync();

            // Write the string to a file.
            var writer = new StreamWriter("C:\\" + fileName);
            writer.Write(contentString.Result);
            writer.Close();
        }

        private static List<ItemModel> GetArticles()
        {
            var uri = new Uri("http://www.hardcoregaming101.net/alpha.htm");
            var document = new HtmlWeb().Load(uri.ToString());
            var model = new List<ItemModel>();

            var catalogs = document.DocumentNode.Descendants().Where(
                node => node.Attributes.Contains("class")
                && node.Attributes["class"].Value.Contains("catalog"));

            foreach (var catalog in catalogs)
            {
                foreach (var link in catalog.Descendants().Where(node => node.Name.Equals("a")))
                {
                    if (!string.IsNullOrEmpty(link.InnerText) && link.Attributes["href"] != null)
                    {
                        model.Add(new ItemModel()
                        {
                            Title = link.InnerText,
                            Description = link.Attributes["title"] != null ? link.Attributes["title"].Value : link.InnerText,
                            Link = new Uri(uri + "/" + link.Attributes["href"].Value)
                        });
                    }
                }
            }
            return model;
        }

        private static HttpResponseMessage GetPage(string title, string description)
        {
            var model = new ArticleModel
            {
                Title = title,
                Description = description,
                Items = GetArticles()
            };

            var xml = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement("rss",
                    new XAttribute("version", "2.0"),
                    new XElement("channel",
                        new XElement("title", model.Title),
                        new XElement("description", model.Description),
                        from element in model.Items.OrderBy(x => Guid.NewGuid()).Take(3) //grab 3 randomly selected
                        select new XElement("item",
                            new XElement("title", element.Title),
                            new XElement("description", element.Description),
                            new XElement("link", element.Link)))));

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
