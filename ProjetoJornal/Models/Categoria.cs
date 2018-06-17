using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Models
{
    public class Categoria
    {
        public Categoria() { }
        public int Id { get; set; }
        public string Classe { get; set; }
        public string Descricao { get; set; }
        public string Status { get; set; }
    }
}