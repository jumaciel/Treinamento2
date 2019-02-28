using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;
using Treinamento2._0.Models;

namespace Treinamento2._0.Consultas.ConsultaSpecies
{
    class ParserSpecies
    {
        public HtmlNode Node { get; set; }

        public ParserSpecies(HtmlNode node)
        {
            this.Node = node;
        }

        public bool HasNextPage(out string link)
        {
            link = "";

            var nodeNext = this.Node.SelectSingleNode("//a[@rel='next']");

            if (nodeNext != null)
            {
                var att = nodeNext.GetAttributeValue("href", string.Empty);
                link = "https://www.worldwildlife.org" + att;
            }

            return nodeNext != null;
        }

        public void ParseLinhas(List<Specie> listaSpecies)
        {
            var linhas = this.GetLinhas();

            if (linhas == null)
                throw new Exception("A linhas de Species não foram encontradas!");

            foreach (var linha in linhas)
            {
               
                    Specie novaSpecie = ParseSpecie(linha);
                listaSpecies.Add(novaSpecie);
                
            }
        }

        private Specie ParseSpecie(HtmlNode linha)
        {
            HtmlNode nomeComum = linha.SelectSingleNode("./td[1]");
            HtmlNode nomeCientifico = linha.SelectSingleNode("./td[2]");
            HtmlNode statusConcervacao = linha.SelectSingleNode("./td[3]");

            if (nomeComum == null || nomeCientifico == null || statusConcervacao == null)
            {
                throw new Exception("Não foi possivel capturar os atributos da especie!");
            }

            Specie specie = new Specie
            {
                NomeCientifico = nomeCientifico.InnerText,
                NomeComum = nomeComum.InnerText,
                StatusConcervacao = statusConcervacao.InnerText
            };

            return specie;
        }

        public HtmlNodeCollection GetLinhas()
        {
            return this.Node.SelectNodes("//tbody/tr");
        }

        internal void ParseData(List<Specie> listaSpecie)
        {
            this.ParseLinhas(listaSpecie);
        }       
    }
}
