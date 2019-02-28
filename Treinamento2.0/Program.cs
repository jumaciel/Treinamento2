using Treinamento2._0.Consultas.ConsultaReceita;
using Treinamento2._0.Consultas.ConsultaSpecie;

namespace Treinamento2
{
    class Program
    {
        static void Main(string[] args)
        {
            //FonteReceita fonteReceita = new FonteReceita();
            //fonteReceita.GetData();
            //var listareceitas = fonteReceita.listaReceitas;

            FonteSpecie fonteSpecie = new FonteSpecie();
            fonteSpecie.GetData();
            var listaSpecies = fonteSpecie.listaSpecies;
        }
    }
}
