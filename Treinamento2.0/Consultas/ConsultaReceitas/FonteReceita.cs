using HtmlAgilityPack;
using Treinamento2._0.Models;
using System.Collections.Generic;
using Treinamento2._0.Utils;
using Treinamento2._0.Consultas.ConsultaReceitas;

namespace Treinamento2._0.Consultas.ConsultaReceita
{
    public class FonteReceita : Robo
    {
        public List<Receita> listaReceitas = new List<Receita>();

        public string Link = "https://pt.petitchef.com/receitas/rapida";

        public void GetData()
        {
            string link = Link;
            ParserReceitas parser;

            do
            {
                parser = this.NavegaPaginaReceitas(link);

                parser.ParseData(listaReceitas);
            } while (parser.HasNextPage(out link));
        }

        private ParserReceitas NavegaPaginaReceitas(string link)
        {
            HtmlDocument document = this.GetHtmlDocument(link);

            return new ParserReceitas(document.DocumentNode);
        }
    }
}
