using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Areas.Admin.ViewModel
{
    public class ListarCategoriasModel
    {
        public int IdCategoria { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
        public string StatusTexto { get; set; }
        public string StatusLabel{ get; set; }
        public string Classe { get; set; }
        public int IdClasse { get; set; }
    }
}