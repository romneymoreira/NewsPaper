using ProjetoJornal.Base;
using ProjetoJornal.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetoJornal.Areas.Admin.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public readonly IDashboardRepository _repository;
        public readonly Funcoes _funcoes;
        public HomeController(IDashboardRepository repository, Funcoes funcoes)
        {
            _repository = repository;
            _funcoes = funcoes;
        }
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Dashboard()
        {
            var model = _repository.Dashboard();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}