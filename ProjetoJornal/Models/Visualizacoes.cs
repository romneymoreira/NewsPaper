using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Models
{
    public class Visualizacoes
    {
        public Visualizacoes() { Quantidade = 0; }

        public int Id { get; set; }
        public Int64 Quantidade { get; set; }
    }
}