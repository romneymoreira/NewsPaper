using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Areas.Admin.ViewModel
{
    public class ListarNoticiasModel
    {
        public ListarNoticiasModel()
        {
            NoticiasListar = new List<ViewModel.NoticiasListar>();
            AutoresListar = new List<ViewModel.AutoresListar>();
            CategoriasListar = new List<ViewModel.CategoriasListar>();
        }
        public List<NoticiasListar> NoticiasListar { get; set; }
        public List<CategoriasListar> CategoriasListar { get; set; }
        public List<AutoresListar> AutoresListar { get; set; }

    }
    public class NoticiasListar
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