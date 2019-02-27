using HtmlAgilityPack;
using SpeciesPaginacao.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Treinamento2._0.Utils;

namespace Treinamento2._0.Controllers
{
    class TarefaEspecies
    {
        public Robo robo = new Robo();

        public List<Specie> listaSpecies = new List<Specie>();

        public string link = "https://www.worldwildlife.org/species/directory?sort=extinction_status";

        public List<Specie> GetListaSpecies() {
            return listaSpecies;
        }

        private Specie CriaEspecie(HtmlNode linha)
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

        private void AddEspecieNaLista()
        {
            var linhas = robo.GetHtmlNodes("//tbody/tr");

            foreach (var linha in linhas)
            {
                Specie especie = CriaEspecie(linha);

                listaSpecies.Add(especie);
            }
        }

        public void Paginacao()
        {
            robo.SetHtmlNode(link);
            var nodeNext = robo.GetHtmlNode("//a[@rel='next']");

            AddEspecieNaLista();

            if (nodeNext != null)
            {
                var att = nodeNext.GetAttributeValue("href", string.Empty);
                link = "https://www.worldwildlife.org" + att;

                Paginacao();
            }
        }
    }
}
