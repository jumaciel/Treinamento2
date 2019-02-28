using HtmlAgilityPack;
using Treinamento2._0.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Treinamento2._0.Utils;
using Treinamento2._0.Consultas.ConsultaSpecies;

namespace Treinamento2._0.Consultas.ConsultaSpecie
{
    class FonteSpecie : Robo
    {
        public List<Specie> listaSpecies = new List<Specie>();

        public string Link = "https://www.worldwildlife.org/species/directory?sort=extinction_status";

        public void GetData()
        {
            string link = Link;
            ParserSpecies parser;

            do
            {
                parser = this.NavegaPaginaSpecies(link);

                parser.ParseData(listaSpecies);
            } while (parser.HasNextPage(out link));
        }

        private ParserSpecies NavegaPaginaSpecies(string link)
        {
            HtmlDocument document = this.GetHtmlDocument(link);

            return new ParserSpecies(document.DocumentNode);
        }
    }
}
