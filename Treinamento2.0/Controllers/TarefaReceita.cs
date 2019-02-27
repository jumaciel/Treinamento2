using HtmlAgilityPack;
using PetitChef.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Treinamento2._0.Utils;

namespace Treinamento2._0.Controllers
{
    public class TarefaReceita
    {

        public Robo robo = new Robo();

        public List<Receita> listaReceitas = new List<Receita>();

        public string link= "https://pt.petitchef.com/receitas/rapida";

        public List<Receita> GetListaReceitas() {
            return listaReceitas;
        }

        public bool IsReceita(HtmlNode linha)
        {
            HtmlNode propagandaOrReceita = linha.SelectSingleNode("./p | ./fieldset ");
            HtmlNode ingredientes = linha.SelectSingleNode("./div[@class='ingredients']");

            if (propagandaOrReceita != null | ingredientes == null)
            {
                return false;
            }

            return true;
        }

        public Receita CriarNovaReceita(HtmlNode linha)
        {
            Receita receita = new Receita();

            Receita.GetTitulo(receita, linha);
            Receita.GetContemGluten(receita, linha);
            Receita.GetAvaliacaoVotos(receita, linha);
            Receita.GetIngredientes(receita, linha);
            Receita.GetUrl(receita, linha);
            Receita.GetPropriedades(receita, linha);
            Receita.GetAmeis(receita, linha);
            Receita.GetComentarios(receita, linha);

            return receita;
        }

        public void AddReceitaNaLista()
        {
            var linhas = robo.GetHtmlNodes("//div[@class ='i-right']");
            foreach (var linha in linhas)
            {
                if (IsReceita(linha))
                {
                    Receita novaReceita = CriarNovaReceita(linha);
                    listaReceitas.Add(novaReceita);
                }
            }
        }

        public void Paginacao()
        {
            robo.SetHtmlNode(link);
            var nodeNext = robo.GetHtmlNode("//div[@class= 'pages']/span/following-sibling::a[1]");

            AddReceitaNaLista();

            if (nodeNext != null)
            {
                var att = nodeNext.GetAttributeValue("href", string.Empty);
                link = "https://pt.petitchef.com" + att;

                Paginacao();
            }
        }

        public static int StringForInt(string stg) {
            int teste;
            if ((int.TryParse(stg.ToString(), out teste)))
            {
                return teste;
            }
            return 0;
        }

    }
}
