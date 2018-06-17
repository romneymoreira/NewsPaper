using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.ViewModel
{
    public class MaisVisualizadasModel
    {
        public int IdNoticia { get; set; }
        public string ClasseCategoria { get; set; }
        public Int64 Visualizacoes { get; set; }
        public string FotoHome { get; set; }
        public string Categoria { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Corpo { get; set; }
        public string CorpoSubstring { get; set; }
        public Byte[] FotoByte { get; set; }
    }
}