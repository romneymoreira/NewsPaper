using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Areas.Admin.Models
{
    public class Usuario
    {
        public Usuario() { }
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string Senha { get; set; }
    }
}