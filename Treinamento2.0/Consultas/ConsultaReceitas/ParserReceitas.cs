using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Treinamento2._0.Models;

namespace Treinamento2._0.Consultas.ConsultaReceitas
{
    public class ParserReceitas
    {
        public HtmlNode Node { get; set; }

        public ParserReceitas(HtmlNode node)
        {
            this.Node = node;
        }

        public bool HasNextPage(out string link)
        {
            link = "";

            var nodeNext = this.Node.SelectSingleNode("//div[@class= 'pages']/span/following-sibling::a");

            if (nodeNext != null)
            {
                var att = nodeNext.GetAttributeValue("href", string.Empty);
                link = "https://pt.petitchef.com" + att;
            }

            return nodeNext != null;
        }

        public void ParseLinhas(List<Receita> listaReceitas)
        {
            var linhas = this.GetLinhas();

            if (linhas == null)
                throw new Exception("A linhas de receitas não foram encontradas!");

            foreach (var linha in linhas)
            {
                if (this.IsReceita(linha))
                {
                    Receita novaReceita = ParseReceita(linha);
                    listaReceitas.Add(novaReceita);
                }
            }
        }

        private Receita ParseReceita(HtmlNode linha)
        {
            Receita receita = new Receita();

            this.GetTitulo(receita, linha);
            this.GetContemGluten(receita, linha);
            this.GetAvaliacaoVotos(receita, linha);
            this.GetIngredientes(receita, linha);
            this.GetUrl(receita, linha);
            this.GetPropriedades(receita, linha);
            this.GetAmeis(receita, linha);
            this.GetComentarios(receita, linha);

            return receita;
        }

        public HtmlNodeCollection GetLinhas()
        {
            return this.Node.SelectNodes("//div[@class ='i-right']");
        }

        public bool IsReceita(HtmlNode linha)
        {
            HtmlNode propagandaOrReceita = linha.SelectSingleNode("./p | ./fieldset ");
            HtmlNode ingredientes = linha.SelectSingleNode("./div[@class='ingredients']");

            return propagandaOrReceita == null && ingredientes != null;
        }

        public void GetTitulo(Receita receita, HtmlNode linha)
        {
            HtmlNode titulo = linha.SelectSingleNode("./h2/a");
            receita.Titulo = titulo.InnerText;
        }

        internal void ParseData(List<Receita> listaReceitas)
        {
            this.ParseLinhas(listaReceitas);
        }

        public void GetContemGluten(Receita receita, HtmlNode linha)
        {
            var gluten = linha.SelectSingleNode("./div/img[@title='sem glúten']");

            if (gluten == null) receita.Gluten = true;

            else receita.Gluten = false;
        }

        public void GetAvaliacaoVotos(Receita receita, HtmlNode linha)
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
                var notamod = texto.Groups[1].Value;
                notamod = notamod.Replace(".", ",");

                receita.Nota = this.StringForDouble(notamod);

                receita.Votos = this.StringForInt(texto.Groups[2].Value);
            }
        }

        public void GetAmeis(Receita receita, HtmlNode linha)
        {
            var likes = linha.SelectSingleNode("./div[contains(@class,'ir-vote')]/i[contains(@class, 'fa-heart')]/following-sibling::text()");
            if (likes != null)
            {
                var rgxAmeis = Regex.Match(likes.InnerText, @"(\()(\d+)(\))");
                receita.Ameis = this.StringForInt(rgxAmeis.Groups[2].Value);
            }
        }

        public void GetComentarios(Receita receita, HtmlNode linha)
        {
            var comentarios = linha.SelectSingleNode("./div[contains(@class,'ir-vote')]/i[contains(@class, 'fa-comments')]/following-sibling::text()");
            if (comentarios != null)
            {
                var rgComentarios = Regex.Match(comentarios.InnerText, @"(\()(\d+)(\))");

                if (rgComentarios == null) throw new Exception("Não foi possivel capturar os comentarios da receita!");

                receita.Comentarios = this.StringForInt(rgComentarios.Groups[2].Value);
            }
        }

        public void GetIngredientes(Receita receita, HtmlNode linha)
        {
            HtmlNode ingredientes = linha.SelectSingleNode("./div[@class='ingredients']");

            receita.Ingredientes = ingredientes.InnerText;
        }

        public void GetUrl(Receita receita, HtmlNode linha)
        {
            HtmlNode url = linha.SelectSingleNode("//h2[@class='ir-title']/a");
            var href = url.GetAttributeValue("href", string.Empty);

            if (href.Equals(string.Empty))
            {
                throw new Exception("Não foi possivel capturar o link da receita!");
            }

            receita.Link = href;
        }

        public void GetPropriedades(Receita receita, HtmlNode linha)
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
                            var rgxCalorias = Regex.Match(text, @"(\d+)(.+)");

                            receita.Calorias = this.StringForInt(rgxCalorias.Groups[1].Value);
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

        private int StringForInt(string stg)
        {
            int teste;
            if ((int.TryParse(stg.ToString(), out teste)))
            {
                return teste;
            }

            return 0;
        }

        private double StringForDouble(string strg)
        {
            double tst;
            if ((double.TryParse(strg.ToString(), out tst)))
            {
                return tst;
            }

            return 0;
        }
    }
}
