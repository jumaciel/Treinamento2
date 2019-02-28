using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace Treinamento2._0.Utils
{
    public class Robo
    {
        protected HtmlDocument GetHtmlDocument(string url)
        {
            var web = new HtmlWeb();
            return web.Load(url);
        }
    }
}
