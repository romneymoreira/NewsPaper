using ProjetoJornal.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoJornal.Areas.Admin.Models;
using ProjetoJornal.Base;
using ProjetoJornal.Context;

namespace ProjetoJornal.Repository
{
    public class LoginService : RepositoryBase<SiteContext>, ILoginService
    {
        public LoginService(IUnitOfWork<SiteContext> unit)
            : base(unit)
        {
        }
        public Usuario Autenticar(Login login)
        {
            return Context.Usuario.FirstOrDefault(x => x.Email.ToLower().Equals(login.User.ToLower()) && x.Senha.Equals(login.Password));
        }
    }
}