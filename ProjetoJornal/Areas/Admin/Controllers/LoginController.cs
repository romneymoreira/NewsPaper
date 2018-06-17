using ProjetoJornal.Areas.Admin.ViewModel;
using ProjetoJornal.Repository;
using ProjetoJornal.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace ProjetoJornal.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public readonly ILoginService _repository;
        public LoginController(ILoginService repository)
        {
            _repository = repository;
        }
        // GET: Admin/Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Autenticar(Models.Login log)
        {
            JsonResultModel jsonModel;
            try
            {
                var login = new Models.Login { User = log.User, Password = log.Password };

                var model = _repository.Autenticar(login);
                if (model != null)
                {
                    Session["usuarioLogadoID"] = model.Id;
                    Session["nomeUsuarioLogado"] = model.Nome;
                    System.Web.Security.FormsAuthentication.SetAuthCookie(model.Nome, false);
                    jsonModel = new JsonResultModel("S");
                }
                else //não encontrou o usuário
                    jsonModel = new JsonResultModel("N");
            }
            catch (Exception ex)
            {
                jsonModel = JsonResultModel.CreateError(ex);
            }
            return Json(jsonModel, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Sair()
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session["usuarioLogadoID"] = null;
            Session["nomeUsuarioLogado"] = null;
            return RedirectToAction("Login", "Login");
        }

        public ActionResult Autorizar(Models.Login log)
        {
            var login = new Models.Login() { User = log.User, Password = log.Password };
            var model = _repository.Autenticar(login);
            if (model != null)
            {
                Session["usuarioLogadoID"] = model.Id;
                Session["nomeUsuarioLogado"] = model.Nome;
                return RedirectToAction("Index", "Home");
            }
            return View("Login");
        }
    }
}