using ProjetoJornal.Areas.Admin.ViewModel;
using ProjetoJornal.Base;
using ProjetoJornal.Models;
using ProjetoJornal.Repository.Interface;
using ProjetoJornal.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProjetoJornal.Areas.Admin.Controllers
{
    public class NoticiasController : Controller
    {
        public readonly ISiteRepository _repository;
        public readonly Funcoes _funcoes;
        public NoticiasController(ISiteRepository repository, Funcoes funcoes)
        {
            _repository = repository;
            _funcoes = funcoes;
        }
        // GET: Admin/Noticias
        public ActionResult Index()
        {
            var result = new ResultModel()
            {
                ClasseDiv = "",
                CodigoErro = 0,
                Exibir = false,
                Mensagem = "",
                Resposta = HttpStatusCode.Continue
            };
            var model = new ListarNoticiasModel();
            try
            {
                var noticias = _repository.ListarNoticiasTake(20);

                foreach (var item in noticias)
                {
                    string corpo = _funcoes.RemoveTagsHTML(item.Corpo);
                    model.NoticiasListar.Add(new NoticiasListar
                    {
                        Corpo = corpo,
                        CorpoSubString = _funcoes.RetornarSubString(200, corpo),
                        Data = item.Data,
                        FotoHome = item.FotoHome,
                        Id = item.Id,
                        IdAutor = item.IdAutor,
                        IdCategoria = item.IdCategoria,
                        Status = item.Status,
                        Titulo = item.Titulo,
                        VaiParaHome = item.VaiParaHome,
                        Categoria = item.Categoria?.Descricao,
                        Autor = item.Autor?.Nome
                    });
                }

                model.CategoriasListar = ListarCategorias();
                model.AutoresListar = ListarAutores();
            }
            catch (Exception ex)
            {
                result.Mensagem = ex.Message;
                result.CodigoErro = 1;
                result.Exibir = true;
                result.Resposta = HttpStatusCode.BadRequest;
            }
            ViewBag.Result = result;
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(BuscaModel search)
        {
            var result = new ResultModel()
            {
                ClasseDiv = "",
                CodigoErro = 0,
                Exibir = false,
                Mensagem = "",
                Resposta = HttpStatusCode.Continue
            };

            var model = new ListarNoticiasModel();
            var noticias = _repository.ListarNoticiasBuscaAvancada(search);

            if(noticias.Count == 0)
            {
                result.Mensagem = "Não foi encontrado nenhuma notícia para a busca realizada.";
                result.Exibir = true;
                result.ClasseDiv = "sucsses";
            }

            foreach (var item in noticias)
            {
                string corpo = _funcoes.RemoveTagsHTML(item.Corpo);
                model.NoticiasListar.Add(new NoticiasListar
                {
                    Corpo = corpo,
                    CorpoSubString = _funcoes.RetornarSubString(200, corpo),
                    Data = item.Data,
                    FotoHome = item.FotoHome,
                    Id = item.Id,
                    IdAutor = item.IdAutor,
                    IdCategoria = item.IdCategoria,
                    Status = item.Status,
                    Titulo = item.Titulo,
                    VaiParaHome = item.VaiParaHome,
                    Categoria = item.Categoria?.Descricao,
                    Autor = item.Autor?.Nome
                });
            }

            model.CategoriasListar = ListarCategorias();
            model.AutoresListar = ListarAutores();
            ViewBag.Result = result;
            return RedirectToAction("Index", model);
        }

        [HttpPost]
        public async Task<ActionResult> UploadHomeReport(string id)
        {
            string table = "";
            int idResult = 0;
            try
            {
                foreach (string item in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
                    if (file.ContentLength == 0)
                        continue;
                    if (file.ContentLength > 0)
                    {
                        //Pega o nome do arquivo
                        string nome = System.IO.Path.GetFileName(file.FileName);
                        //Pega a extensão do arquivo
                        string extensao = Path.GetExtension(file.FileName);
                        //Gera nome novo do Arquivo numericamente
                        string filename = string.Format("{0:00000000000000}", GerarID());
                        //Caminho a onde será salvo
                        file.SaveAs(Server.MapPath("~/Areas/Admin/Upload/") + filename + extensao);

                        //Prefixo P/ img pequena
                        var prefixoP = "_80x80";
                        //Prefixo M/ img media
                        var prefixoM = "_400x250";
                        //Prefixo G/ img grande
                        var prefixoG = "_800x400";

                        //pega o arquivo já carregado
                        string pth = Server.MapPath("~/Areas/Admin/Upload/") + filename + extensao;

                        string arquivo = Constantes.UrlImagens + filename + prefixoG + extensao;
                        string imagem1 = Redefinir.resizeImageAndSave(pth, Constantes.ImagemW80, Constantes.ImagemH80, prefixoP);
                        string imagem2 = Redefinir.resizeImageAndSave(pth, Constantes.ImagemW400, Constantes.ImagemH250, prefixoM);
                        string imagem3 = Redefinir.resizeImageAndSave(pth, Constantes.ImagemW800, Constantes.ImagemH400, prefixoG);

                        idResult++;// = RetornaId(idResult);
                        table += "<tr><td>" + idResult.ToString() + "</td><td><span id='p_" + idResult + "'>" + arquivo + "</span></td><td><a href='javascript:;' onclick='copyToClipboard(\"#p_" + idResult + "\")' class='btn btn-icon-only grey-cascade'><i class='fa fa-link'></i></a><a href='" + arquivo + "' target='_blank' class='btn btn-icon-only green'><i class='fa fa-file-image-o'></i></a></td></tr>";
                        //idResult++;
                        //table += "<tr><td>" + idResult.ToString() + "</td><td><span id='p_" + idResult + "'>" + imagem2 + "</span></td><td>400x250</td><td><a href='javascript:;' onclick='copyToClipboard(\"#p_" + idResult + "\")' class='btn btn-icon-only grey-cascade'><i class='fa fa-link'></i></a></td></tr>";
                        //idResult++;
                        //table += "<tr><td>" + idResult.ToString() + "</td><td><span id='p_" + idResult + "'>" + imagem3 + "</span></td><td>800x400</td><td><a href='javascript:;' onclick='copyToClipboard(\"#p_" + idResult + "\")' class='btn btn-icon-only grey-cascade'><i class='fa fa-link'></i></a></td></tr>";
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
            return Content(table);
        }

        [HttpPost]
        public ActionResult Upload(FormCollection formCollection)
        {
            foreach (string item in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[item] as HttpPostedFileBase;
                if (file.ContentLength == 0)
                    continue;
                if (file.ContentLength > 0)
                {
                    //Pega o nome do arquivo
                    string nome = System.IO.Path.GetFileName(file.FileName);
                    //Pega a extensão do arquivo
                    string extensao = Path.GetExtension(file.FileName);
                    //Gera nome novo do Arquivo numericamente
                    string filename = string.Format("{0:00000000000000}", GerarID());
                    //Caminho a onde será salvo
                    file.SaveAs(Server.MapPath("~/uploads/fotos/") + filename + extensao);

                    //Prefixo p/ img pequena
                    var prefixoP = "-p";
                    //Prefixo p/ img grande
                    var prefixoG = "-g";

                    //pega o arquivo já carregado
                    string pth = Server.MapPath("~/uploads/fotos/") + filename + extensao;

                    //Redefine altura e largura da imagem e Salva o arquivo + prefixo
                    Redefinir.resizeImageAndSave(pth, 70, 53, prefixoP);
                    Redefinir.resizeImageAndSave(pth, 500, 331, prefixoG);
                }
            }

            return View();
        }

        public Int64 GerarID()
        {
            try
            {
                DateTime data = new DateTime();
                data = DateTime.Now;
                string s = data.ToString().Replace("/", "").Replace(":", "").Replace(" ", "");
                return Convert.ToInt64(s);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SalvarNoticia(CadastrarNoticiaModel model)
        {
            if (model.Id > 0)
            {
                var noticia = _repository.ObterNoticiaPorId(model.Id);
                if (noticia == null)
                    throw new Exception("Notícia não encontrada.");

                var categoria = _repository.ObterCategoriaPorId(model.IdCategoria);
                if (categoria == null)
                    throw new Exception("Categoria não encontrada.");

                var autor = _repository.ObterAutorPorId(model.IdAutor);
                if (autor == null)
                    throw new Exception("Autor não encontrado.");


                string home = model.VaiParaHome ? "S" : "N";

                noticia.Corpo = model.Corpo;
                noticia.Categoria = categoria;
                noticia.VaiParaHome = home;
                noticia.Status = model.Status;
                noticia.Autor = autor;
                noticia.Titulo = model.Titulo;
                noticia.FotoHome = model.FotoHome;
                _repository.SalvarNoticia(noticia);
            }
            else
            {
                var categoria = _repository.ObterCategoriaPorId(model.IdCategoria);
                if (categoria == null)
                    throw new Exception("Categoria não encontrada.");

                var autor = _repository.ObterAutorPorId(model.IdAutor);
                if (autor == null)
                    throw new Exception("Autor não encontrado.");

                string home = model.VaiParaHome ? "S" : "N";

                var noticia = new Noticia(model.Titulo, model.Corpo, home, categoria, autor);
                noticia.FotoHome = model.FotoHome;
                var result = _repository.SalvarNoticia(noticia);
                model.Id = result.Id;
            }

            var retorno = new CadastrarNoticiaModel
            {
                AutoresListar = ListarAutores(),
                CategoriasListar = ListarCategorias()
            };
            return View("~/Areas/Admin/Views/Noticias/Cadastrar.cshtml", retorno);
        }

        public ActionResult Cadastrar(int idNoticia)
        {
            var model = new CadastrarNoticiaModel
            {
                AutoresListar = ListarAutores(),
                CategoriasListar = ListarCategorias()
            };
            if (idNoticia > 0)
            {
                var noticia = _repository.ObterNoticiaPorId(idNoticia);
                if (noticia == null)
                    throw new Exception("Notícia não encontrada.");

                model.Corpo = noticia.Corpo;
                model.Data = noticia.Data;
                model.FotoHome = noticia.FotoHome;
                model.Id = noticia.Id;
                model.IdAutor = noticia.IdAutor;
                model.IdCategoria = noticia.IdCategoria;
                model.Titulo = noticia.Titulo;
            }
            return View(model);
        }
        private List<AutoresListar> ListarAutores()
        {
            var model = new List<AutoresListar>();
            var autores = _repository.ListarAutores();
            foreach (var item in autores)
            {
                model.Add(new AutoresListar
                {
                    IdAutor = item.Id,
                    Nome = item.Nome
                });
            }
            return model;
        }
        private List<CategoriasListar> ListarCategorias()
        {
            var model = new List<CategoriasListar>();
            var categorias = _repository.ListarCategorias();
            foreach (var item in categorias)
            {
                model.Add(new CategoriasListar
                {
                    IdCategoria = item.Id,
                    Nome = item.Descricao
                });
            }
            return model;
        }
    }
}