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
    public class CategoriaController : Controller
    {
        public readonly ISiteRepository _repository;
        public readonly Funcoes _funcoes;
        public CategoriaController(ISiteRepository repository, Funcoes funcoes)
        {
            _repository = repository;
            _funcoes = funcoes;
        }
        // GET: Admin/Categoria
        public ActionResult Index()
        {
            var model = new List<ListarCategoriasModel>();
            var categorias = _repository.ListarCategorias();
            foreach (var item in categorias)
            {
                model.Add(new ListarCategoriasModel
                {
                    Classe = item.Classe,
                    Descricao = item.Descricao,
                    IdCategoria = item.Id,
                    Status = item.Status,
                    StatusLabel = RetornaLabelStatus(item.Status),
                    StatusTexto = RetornaTextoStatus(item.Status),
                    IdClasse = RetornaIdClasseCategoria(item.Classe)
                });
            }
            return View(model);
        }
        [HttpPost]
        public JsonResult Salvar(ListarCategoriasModel model)
        {
            try
            {
                if (model.IdCategoria > 0)
                {
                    var categoria = _repository.ObterCategoriaPorId(model.IdCategoria);
                    if (categoria == null)
                        throw new Exception("Erro ao recuperar a categoria");

                    categoria.Classe = RetornaClasseCategoria(model.IdClasse);
                    categoria.Descricao = model.Descricao;
                    categoria.Status = model.Status;
                    _repository.SalvarCategoria(categoria);
                }
                else
                {
                    var categoria = new Categoria();
                    categoria.Classe = RetornaClasseCategoria(model.IdClasse);
                    categoria.Descricao = model.Descricao;
                    categoria.Status = model.Status;
                    _repository.SalvarCategoria(categoria);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ModalEditar(int id)
        {
            var model = new ListarCategoriasModel();
            model.IdClasse = 1;
            if (id > 0)
            {
                var categoria = _repository.ObterCategoriaPorId(id);
                if (categoria == null)
                    throw new Exception("Erro ao recuperar a categoria");

                model.Classe = categoria.Classe;
                model.Descricao = categoria.Descricao;
                model.IdCategoria = categoria.Id;
                model.Status = categoria.Status;
                model.StatusLabel = RetornaLabelStatus(categoria.Status);
                model.StatusTexto = RetornaTextoStatus(categoria.Status);
                model.IdClasse = RetornaIdClasseCategoria(categoria.Classe);
            }
            //return PartialView("~/Areas/Admin/Views/Categoria/ModalEditar.cshtml", model);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        private string RetornaTextoStatus(string status)
        {
            switch (status)
            {
                case "A":
                    return "Ativo";
                case "C":
                    return "Cancelado";
                case "I":
                    return "Inativo";
                default:
                    return "";
            }
        }

        private int RetornaIdClasseCategoria(string classe)
        {
            switch (classe)
            {
                case "category category-travel"://travel
                    return 1;
                case "category category-food"://food
                    return 2;
                case "category category-world"://world
                    return 3;
                case "category category-tech"://tech
                    return 4;
                case "category category-fashion"://fashion
                    return 5;
                case "category category-video"://video
                    return 6;
                case "category category-sport"://sport
                    return 7;
                case "category category-politics"://politics
                    return 8;
                default:
                    return 1;
            }
        }

        private string RetornaClasseCategoria(int id)
        {
            switch (id)
            {
                case 1://travel
                    return "category category-travel";
                case 2://food
                    return "category category-food";
                case 3://world
                    return "category category-world";
                case 4://tech
                    return "category category-tech";
                case 5://fashion
                    return "category category-fashion";
                case 6://video
                    return "category category-video";
                case 7://sport
                    return "category category-sport";
                case 8://politics
                    return "category category-politics";
                default:
                    return "category category-travel";
            }
        }

        private string RetornaLabelStatus(string status)
        {
            switch (status)
            {
                case "A":
                    return "<span class='label label-primary'>Ativo</span>";
                case "C":
                    return "<span class='label label-danger'>Cancelado</span>"; ;
                case "I":
                    return "<span class='label label-danger'>Inativo</span>"; ;
                default:
                    return "";
            }
        }
    }
}