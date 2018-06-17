using ProjetoJornal.Areas.Admin.ViewModel;
using ProjetoJornal.Base;
using ProjetoJornal.Models;
using ProjetoJornal.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjetoJornal.Areas.Admin.Controllers
{
    public class AutorController : Controller
    {
        public readonly ISiteRepository _repository;
        public readonly Funcoes _funcoes;
        public AutorController(ISiteRepository repository, Funcoes funcoes)
        {
            _repository = repository;
            _funcoes = funcoes;
        }
        // GET: Admin/Autor
        public ActionResult Index()
        {
            var model = new List<ListarAutoresModel>();
            var autores = _repository.ListarAutores();
            foreach (var item in autores)
            {
                model.Add(new ListarAutoresModel
                {
                   Celular = item.Celular,
                   CelularFormatado = _funcoes.TelefoneFormatado(item.Celular),
                   Email = item.Email,
                   IdAutor = item.Id,
                   Nome = item.Nome
                });
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult Salvar(CadastarAutorModel model)
        {
            try
            {
                if (model.IdAutor > 0)
                {
                    var autor = _repository.ObterAutorPorId(model.IdAutor);
                    if (autor == null)
                        throw new Exception("Erro ao recuperar o autor");

                    autor.Celular = model.Celular;
                    autor.Nome = model.Nome;
                    autor.Email = model.Email;
                    _repository.SalvarAutor(autor);
                }
                else
                {
                    var autor = new Autor();
                    autor.Celular = model.Celular;
                    autor.Nome = model.Nome;
                    autor.Email = model.Email;
                    _repository.SalvarAutor(autor);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObterAutor(int id)
        {
            var model = new CadastarAutorModel();
            if (id > 0)
            {
                var autor = _repository.ObterAutorPorId(id);
                if (autor == null)
                    throw new Exception("Erro ao recuperar o autor");

                model.IdAutor = autor.Id;
                model.Email = autor.Email;
                model.Celular = autor.Celular;
                model.CelularFormatado = _funcoes.TelefoneFormatado(autor.Celular);
                model.Nome = autor.Nome;
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}