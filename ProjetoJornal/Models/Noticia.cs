using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Models
{
    public class Noticia
    {
        public Noticia() { }
        public int Id { get; set; }
        public int IdCategoria { get; set; }
        public int IdVisualizacao { get; set; }
        public int IdAutor { get; set; }
        public string Titulo { get; set; }
        public string Corpo { get; set; }
        public DateTime Data { get; set; }
        public string Status { get; set; }
        public string VaiParaHome { get; set; }
        public string FotoHome { get; set; }
        public Noticia(string titulo, string corpo, string vaiParaHome, Categoria categoria, Autor autor)
        {
            Titulo = titulo;
            Corpo = corpo;
            VaiParaHome = vaiParaHome;
            Categoria = categoria;
            Autor = autor;
            Data = DateTime.Now;
            Status = "A";
            Visualizacoes = new Visualizacoes();
        }
        public virtual Categoria Categoria { get; set; }
        public virtual Autor Autor { get; set; }
        public virtual Visualizacoes Visualizacoes { get; set; }
    }
}