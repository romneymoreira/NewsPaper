using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Areas.Admin.ViewModel
{
    public class NoticiaModel
    {
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public string Categoria { get; set; }
        public int IdVisualizacao { get; set; }
        public int IdAutor { get; set; }
        public string Autor { get; set; }
        public string Titulo { get; set; }
        public string Corpo { get; set; }
        public string CorpoSubString { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public string VaiParaHome { get; set; }
        public string FotoHome { get; set; }
    }
}