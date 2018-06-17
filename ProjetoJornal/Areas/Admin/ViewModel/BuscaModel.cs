using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Areas.Admin.ViewModel
{
    public class BuscaModel
    {
        public int IdCategoria { get; set; }
        public int IdAutor { get; set; }
        public string Titulo { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }

    }
}