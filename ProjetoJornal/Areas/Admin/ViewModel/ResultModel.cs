using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ProjetoJornal.Areas.Admin.ViewModel
{
    public class ResultModel
    {
        
        public int CodigoErro { get; set; }
        public HttpStatusCode Resposta { get; set; }
        public string Mensagem { get; set; }
        public string ClasseDiv { get; set; }
        public bool Exibir { get; set; }
    }
}