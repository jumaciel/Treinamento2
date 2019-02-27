using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Treinamento2._0.Utils
{
    public class Robo
    {
        public static HtmlDocument htmldoc;

        public void SetHtmlNode(string url)
        {
            var web = new HtmlWeb();
            htmldoc = web.Load(url);
        }

        public HtmlNodeCollection GetHtmlNodes(string xpath)
        {
            return htmldoc.DocumentNode.SelectNodes(xpath);
        }

        public HtmlNode GetHtmlNode(string xpath)
        {
            return htmldoc.DocumentNode.SelectSingleNode(xpath);
        }
    }
}
