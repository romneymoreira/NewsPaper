using ProjetoJornal.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Repository.Interface
{
    public interface ILoginService
    {
        Usuario Autenticar(Login login);
    }
}