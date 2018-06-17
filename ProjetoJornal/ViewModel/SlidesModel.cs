using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.ViewModel
{
    public class SlidesModel
    {
        public int IdNoticia { get; set; }
        public string ClasseCategoria { get; set; }
        public string Foto { get; set; }
        public string Categoria { get; set; }
        public string Titulo { get; set; }
        public string Corpo { get; set; }
        public Byte[] FotoByte { get; set; }
    }
}