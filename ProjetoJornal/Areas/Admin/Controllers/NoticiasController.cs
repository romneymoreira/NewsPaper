using PagedList;
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
        public ActionResult Index(int? page, int? idCategoria, int? idAutor, string titulo, DateTime? dataInicial, DateTime? dataFinal, bool? avancada)
        {
            int pageSize = Constantes.PageSize;
            int pageIndex = Constantes.PageIndex;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var model = new List<ListarNoticiasModel>();
            IPagedList<ListarNoticiasModel> result = null;
            try
            {
                var noticias = new List<Noticia>();
                if (avancada.HasValue)
                {
                    var search = new BuscaModel();

                    if (dataFinal.HasValue)
                        search.DataFinal = Convert.ToDateTime(dataFinal);

                    if (dataInicial.HasValue)
                        search.DataInicial = Convert.ToDateTime(dataInicial);

                    if (idAutor.HasValue)
                        search.IdAutor = Convert.ToInt32(idAutor);

                    if (!string.IsNullOrEmpty(titulo))
                        search.Titulo = titulo;

                    if (idCategoria.HasValue)
                        search.IdCategoria = Convert.ToInt32(idCategoria);

                    model = BuscaAvancada(search);
                }
                else
                {
                    noticias = _repository.ListarNoticiasTake(20);
                    foreach (var item in noticias)
                    {
                        string corpo = _funcoes.RemoveTagsHTML(item.Corpo);
                        model.Add(new ListarNoticiasModel
                        {
                            Corpo = corpo,
                            CorpoSubString = _funcoes.RetornarSubString(200, corpo),
                            Data = item.Data.ToLongDateString(),
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
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            result = model.ToPagedList(pageIndex, pageSize);
            return View(result);
        }

        private List<ListarNoticiasModel> BuscaAvancada(BuscaModel search)
        {
            return _repository.ListarNoticiasBuscaAvancada(search);
        }

        [HttpPost]
        public async Task<ActionResult> UploadHomeReport(string id)
        {
            DateTime data = DateTime.Now;
            string subdiretorio = data.Year.ToString() + "-" + data.Month.ToString() + "-" + data.Day.ToString();
            string caminhocompleto = Server.MapPath("~/Areas/Admin/Upload/") + subdiretorio;
            int posicao = 1;
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
                        string filename = posicao.ToString() + "_" + GerarID().ToString();
                        posicao++;
                        //sempre crio um subdiretorio dento da pasta raiz 
                        if (!Directory.Exists(caminhocompleto))
                            Directory.CreateDirectory(caminhocompleto);

                        //Caminho a onde será salvo
                        file.SaveAs(caminhocompleto + "/" + filename + extensao);

                        //Prefixo P/ img pequena
                        var prefixoP = "_80x80";
                        //Prefixo M/ img media
                        var prefixoM = "_400x250";
                        //Prefixo G/ img grande
                        var prefixoG = "_800x400";

                        //pega o arquivo já carregado
                        string pth = caminhocompleto + "/" + filename + extensao;

                        string arquivo = Constantes.UrlImagens + filename + prefixoG + extensao;
                        string imagem1 = Redefinir.resizeImageAndSave(pth, Constantes.ImagemW80, Constantes.ImagemH80, prefixoP);
                        string imagem2 = Redefinir.resizeImageAndSave(pth, Constantes.ImagemW400, Constantes.ImagemH250, prefixoM);
                        string imagem3 = Redefinir.resizeImageAndSave(pth, Constantes.ImagemW800, Constantes.ImagemH400, prefixoG);
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message);
            }
            return Json("sucesso");
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

                    DateTime data = DateTime.Now;
                    string subdiretorio = data.Year.ToString() + "-" + data.Month.ToString() + "-" + data.Day.ToString();
                    string caminhocompleto = Server.MapPath("~/uploads/fotos/") + subdiretorio;
                    //sempre crio um subdiretorio dento da pasta raiz 
                    if (!Directory.Exists(caminhocompleto))
                        Directory.CreateDirectory(caminhocompleto);


                    //Caminho a onde será salvo
                    file.SaveAs(caminhocompleto + "/" + filename + extensao);

                    //Prefixo p/ img pequena
                    var prefixoP = "-p";
                    //Prefixo p/ img grande
                    var prefixoG = "-g";

                    //pega o arquivo já carregado
                    string pth = caminhocompleto + "/" + filename + extensao;

                    //Redefine altura e largura da imagem e Salva o arquivo + prefixo
                    Redefinir.resizeImageAndSave(pth, 70, 53, prefixoP);
                    Redefinir.resizeImageAndSave(pth, 500, 331, prefixoG);
                }
            }

            return View();
        }

        public JsonResult ListarDiretorios()
        {
            var lista = new List<string>();
            string[] diretorios = Directory.GetDirectories(Server.MapPath("~/Areas/Admin/Upload/"));
            foreach (string dir in diretorios)
            {
                var info = new DirectoryInfo(dir);
                var dirName = info.Name;
                lista.Add(dirName);
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListarArquivos(string diretorio)
        {
            var lista = new List<ListarImagens>();
            string raiz = Server.MapPath("~/Areas/Admin/Upload/") + diretorio;
            string[] arquivos = Directory.GetFiles(raiz);
            int id = 1;
            foreach (string arq in arquivos)
            {
                string result = Path.GetFileName(arq);
                if (result.Contains("80x80"))
                {
                    lista.Add(new ListarImagens()
                    {
                        Url = Constantes.UrlProjeto + "/Areas/Admin/Upload/" + diretorio + "/" + result,
                        Nome = result,
                        Id = id
                    });
                    id++;
                }
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public Guid GerarID()
        {
            try
            {
                // Create and display the value of two GUIDs.
                return Guid.NewGuid();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public JsonResult Autores()
        {
            var model = ListarAutores();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Categorias()
        {
            var model = ListarCategorias();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SalvarNoticia(CadastrarNoticiaModel model)
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

                if (string.IsNullOrEmpty(model.Titulo))
                    throw new Exception("O campo notícia deve ser preenchido.");

                if (string.IsNullOrEmpty(model.Corpo))
                    throw new Exception("O campo corpo deve ser preenchido.");

                noticia.Corpo = model.Corpo;
                noticia.Categoria = categoria;
                noticia.VaiParaHome = model.VaiParaHome;
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

                var noticia = new Noticia(model.Titulo, model.Corpo, model.VaiParaHome, categoria, autor);
                noticia.FotoHome = model.FotoHome;
                var result = _repository.SalvarNoticia(noticia);
                model.Id = result.Id;
            }

            var retorno = new CadastrarNoticiaModel
            {
                AutoresListar = ListarAutores(),
                CategoriasListar = ListarCategorias()
            };
            return Json(model, JsonRequestBehavior.AllowGet);
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
                model.Status = noticia.Status;
                model.IdCategoria = noticia.IdCategoria;
                model.Titulo = noticia.Titulo;
                model.VaiParaHome = noticia.VaiParaHome;
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