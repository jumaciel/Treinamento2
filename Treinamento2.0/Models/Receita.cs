using HtmlAgilityPack;
using System;
using System.Text.RegularExpressions;

namespace PetitChef.Models
{
    /// <summary>
    /// Classe 
    /// </summary>
    public class Receita
    {
        public string Titulo { get; set; }

        public string Nota { get; set; }

        public string Votos { get; set; }

        public string Comentarios { get; set; }

        public string Ameis { get; set; }

        public string Ingredientes { get; set; }

        public string Tipo { get; set; }

        public string Dificuldade { get; set; }

        public string Tempo { get; set; }

        public string Calorias { get; set; }

        public bool Gluten { get; set; }

        public string Link { get; set; }

        public string Cozedura { get; set; }

        public static void GetTitulo(Receita receita, HtmlNode linha)
        {
            HtmlNode titulo = linha.SelectSingleNode("./h2/a");
            receita.Titulo = titulo.InnerText;
        }
         
        public static void GetContemGluten(Receita receita, HtmlNode linha)
        {
            var gluten = linha.SelectSingleNode("./div/img[@title='sem glúten']");

            if (gluten == null) receita.Gluten = true;

            else receita.Gluten = false;
        }
         
        public static void GetAvaliacaoVotos(Receita receita, HtmlNode linha)
        {
            HtmlNode nota = linha.SelectSingleNode("./div/i[contains(@class, 'note-fa')]");

            if (nota != null)
            {
                var notaVotos = nota.GetAttributeValue("title", string.Empty);
                if (notaVotos.Equals(string.Empty))
                {
                    throw new Exception("Não foi possivel capturar os votos e/ou notas da receita!");
                }

                var texto = Regex.Match(notaVotos, @"(.+)/5 \((.+)( votos)\)");
                receita.Nota = texto.Groups[1].Value;
                receita.Votos = texto.Groups[2].Value;
            }

            receita.Nota = "";
            receita.Votos = "";
        }
         
        public static void GetAmeis(Receita receita, HtmlNode linha)
        {
            var likes = linha.SelectSingleNode("./div[contains(@class,'ir-vote')]/i[contains(@class, 'fa-heart')]/following-sibling::text()");
            if (likes != null)
            {
                receita.Ameis = likes.InnerText;
            }
            receita.Ameis = "";
        }
         
        public static void GetComentarios(Receita receita, HtmlNode linha)
        {
            var comentarios = linha.SelectSingleNode("./div[contains(@class,'ir-vote')]/i[contains(@class, 'fa-comments')]/following-sibling::text()");
            if (comentarios != null)
            {
                var rgComentarios = Regex.Match(comentarios.InnerText, @"(\(\d+\))");

                if (rgComentarios == null) throw new Exception("Não foi possivel capturar os comentarios da receita!");

                receita.Comentarios = rgComentarios.Value;
            }

            receita.Comentarios = "";
        }
         
        public static void GetIngredientes(Receita receita, HtmlNode linha)
        {
            HtmlNode ingredientes = linha.SelectSingleNode("./div[@class='ingredients']");

            receita.Ingredientes = ingredientes.InnerText;
        }
         
        public static void GetUrl(Receita receita, HtmlNode linha)
        {
            HtmlNode url = linha.SelectSingleNode("//h2[@class='ir-title']/a");
            var href = url.GetAttributeValue("href", string.Empty);

            if (href.Equals(string.Empty))
            {
                throw new Exception("Não foi possivel capturar o link da receita!");
            }

            receita.Link = href;
        }
         
        public static void GetPropriedades(Receita receita, HtmlNode linha)
        {
            var propriedades = linha.SelectNodes("./div[@class='prop']/span");

            foreach (var procuraProp in propriedades)
            {
                var text = procuraProp.GetAttributeValue("title", string.Empty);

                if (text.Equals(string.Empty))
                {
                    throw new Exception("Não foi possivel capturar as propriedades da receita!");
                }

                var texto = Regex.Match(text, @"(.+): (.+)");

                switch (texto.Groups[1].Value)
                {
                    case "Tipo de receita":
                        {
                            receita.Tipo = texto.Groups[2].Value;
                            break;
                        }

                    case "Dificuldade":
                        {
                            receita.Dificuldade = texto.Groups[2].Value;
                            break;
                        }

                    case "Pronto em":
                        {
                            receita.Tempo = texto.Groups[2].Value;
                            break;
                        }

                    case "Calorias":
                        {
                            receita.Calorias = texto.Groups[2].Value;
                            break;
                        }

                    case "Cozedura":
                        {
                            receita.Cozedura = texto.Groups[2].Value;
                            break;
                        }

                    case "Preparação":
                        {
                            receita.Tempo = texto.Groups[2].Value;
                            break;
                        }

                    default:
                        throw new Exception("Existe uma nova propriedade não mapeada.");
                }
            }
        }
    }
}
