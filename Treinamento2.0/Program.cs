using PetitChef.Models;
using System;
using System.Collections.Generic;
using Treinamento2._0.Controllers;
using Treinamento2._0.Utils;

namespace Treinamento2._0
{
    class Program
    {
        static void Main(string[] args)
        {
            TarefaReceita tarefa1 = new TarefaReceita();
            tarefa1.Paginacao();
            var listareceitas = tarefa1.GetListaReceitas();

            //TarefaEspecies tarefa2 = new TarefaEspecies();
            //tarefa2.Paginacao();
            //var listaSpecies = tarefa2.GetListaSpecies();
            
        }
    }
}
