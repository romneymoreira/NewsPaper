using PagedList;
using ProjetoJornal.Areas.Admin.ViewModel;
using ProjetoJornal.Base;
using ProjetoJornal.Models;
using ProjetoJornal.Repository.Interface;
using ProjetoJornal.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ProjetoJornal.Controllers
{
    public class HomeController : Controller
    {
        public readonly ISiteRepository _repository;
        public readonly Funcoes _funcoes;
        public HomeController(ISiteRepository repository, Funcoes funcoes)
        {
            _repository = repository;
            _funcoes = funcoes;
        }


        public ActionResult Index()
        {
            var home = new IndexModel();
            try
            {
                var noticias = _repository.ListarNoticiasHome();
                home.Ultimas = UltimasNoticias();
                home.MaisVisualizadas = MaisVisualizadas();
                home.Slides = ListarSlides();
                home.UltimasHoje = ListarUltimasHoje();
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            return View(home);
        }

        private string RetornaImagem(string imagem, string tamanho)
        {
            string result = "";
            if (!string.IsNullOrEmpty(imagem))
            {
                string[] foto = imagem.Split('_');
                string[] extensao = imagem.Split('.');

                result = foto[0] + "_" + tamanho + "." + extensao[1];
            }
            return result;
        }

        private List<UltimasModel> UltimasNoticias()
        {
            var model = new List<UltimasModel>();
            var noticias = _repository.ListarUltimasNoticias();

            foreach (var item in noticias)
            {
                string corpo = _funcoes.RemoveTagsHTML(item.Corpo);
                model.Add(new UltimasModel
                {
                    Categoria = item.Categoria.Descricao,
                    ClasseCategoria = item.Categoria.Classe,
                    FotoHome = item.FotoHome,
                    IdNoticia = item.Id,
                    Titulo = item.Titulo,
                    Autor = item.Autor.Nome,
                    CorpoSubstring = _funcoes.RetornarSubString(150, corpo),
                    Corpo = corpo,
                    Visualizacoes = item.Visualizacoes.Quantidade
                });
            }

            return model;
        }

        private List<MaisVisualizadasModel> MaisVisualizadas()
        {
            var model = new List<MaisVisualizadasModel>();
            var noticias = _repository.ListarNoticiasMaisVisualizadas();
            
            foreach (var item in noticias)
            {
                string corpo = _funcoes.RemoveTagsHTML(item.Corpo);

                model.Add(new MaisVisualizadasModel
                {
                    Categoria = item.Categoria.Descricao,
                    ClasseCategoria = item.Categoria.Classe,
                    FotoHome = item.FotoHome,//RetornaImagem(item.FotoHome, "80x80"),
                    IdNoticia = item.Id,
                    Titulo = item.Titulo,
                    Autor = item.Autor.Nome,
                    Corpo = _funcoes.RetornarSubString(200, corpo),
                    CorpoSubstring = corpo,
                    Visualizacoes = item.Visualizacoes.Quantidade
                });
            }

            return model;
        }

        private List<UltimasHojeModel> ListarUltimasHoje()
        {
            return _repository.ListarNoticiasHoje();
        }

        [HttpGet]
        public ActionResult Busca(int? page, string busca)
        {
            int pageSize = Constantes.PageSize;
            int pageIndex = Constantes.PageIndex;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var model = new List<BuscaAvancadaSiteModel>();
            IPagedList<BuscaAvancadaSiteModel> result = null;

            var noticias = _repository.ListarNoticiasBuscaAvancadaSite(busca);

            foreach (var item in noticias)
            {
                string corpo = _funcoes.RemoveTagsHTML(item.Corpo);
                model.Add(new BuscaAvancadaSiteModel
                {
                    Corpo = corpo,
                    CorpoSubString = _funcoes.RetornarSubString(300, corpo),
                    Data = item.Data,
                    FotoHome = item.FotoHome,
                    Id = item.Id,
                    Visualizacoes = item.Visualizacoes != null ? item.Visualizacoes.Quantidade : 0,
                    IdAutor = item.IdAutor,
                    IdCategoria = item.IdCategoria,
                    Status = item.Status,
                    Titulo = item.Titulo,
                    VaiParaHome = item.VaiParaHome,
                    Categoria = item.Categoria?.Descricao,
                    Autor = item.Autor?.Nome
                });
            }

            ViewBag.Busca = busca;
            ViewBag.ItensEncontrados = model.Count;

            result = model.ToPagedList(pageIndex, pageSize);

            return View(result);
        }

        [HttpPost]
        public ActionResult Busca2(string busca)
        {
            try
            {
                var model = new List<BuscaAvancadaSiteModel>();
                var noticias = _repository.ListarNoticiasBuscaAvancadaSite(busca);
              
                foreach (var item in noticias)
                {
                    string corpo = _funcoes.RemoveTagsHTML(item.Corpo);
                    model.Add(new BuscaAvancadaSiteModel
                    {
                        Corpo = corpo,
                        CorpoSubString = _funcoes.RetornarSubString(300, corpo),
                        Data = item.Data,
                        FotoHome = item.FotoHome,
                        Id = item.Id,
                        Visualizacoes = item.Visualizacoes != null ? item.Visualizacoes.Quantidade : 0,
                        IdAutor = item.IdAutor,
                        IdCategoria = item.IdCategoria,
                        Status = item.Status,
                        Titulo = item.Titulo,
                        VaiParaHome = item.VaiParaHome,
                        Categoria = item.Categoria?.Descricao,
                        Autor = item.Autor?.Nome
                    });
                }
                return View(noticias);
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
        }

        //[Route("View/{id}")]
        public ActionResult Post(int id)
        {
            try
            {
                var model = new NoticiaSiteModel();
                var noticia = _repository.ObterNoticiaPorId(id);
                if (noticia == null)
                    throw new HttpException(404, "Not Found");

                //adiciona clique na visualizacão
                Int64 clicks = AdicionaClick(noticia.IdVisualizacao);

                model.Id = noticia.Id;
                model.IdAutor = noticia.IdAutor;
                model.IdCategoria = noticia.IdCategoria;
                model.Status = noticia.Status;
                model.Titulo = noticia.Titulo;
                model.FotoHome = noticia.FotoHome;
                model.Corpo = noticia.Corpo;
                model.Autor = noticia.Autor?.Nome;
                model.Categoria = noticia.Categoria?.Descricao;
                model.Visualizacoes = clicks;

                return View(model);
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
        }

        public ActionResult Publicar(int id)
        {
            try
            {
                var model = new NoticiaSiteModel();
                var noticia = _repository.ObterNoticiaPorId(id);
                if (noticia == null)
                    throw new HttpException(404, "Not Found");

                model.Id = noticia.Id;
                model.IdAutor = noticia.IdAutor;
                model.IdCategoria = noticia.IdCategoria;
                model.Status = noticia.Status;
                model.Titulo = noticia.Titulo;
                model.FotoHome = noticia.FotoHome;
                model.Corpo = noticia.Corpo;
                model.Autor = noticia.Autor?.Nome;
                model.Categoria = noticia.Categoria?.Descricao;

                return View(model);
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
        }

        public JsonResult AutorizarPublicacao(int id)
        {
            try
            {
                var model = new NoticiaSiteModel();
                var noticia = _repository.ObterNoticiaPorId(id);
                if (noticia == null)
                    throw new HttpException(404, "Not Found");

                noticia.Status = "P";
                _repository.SalvarNoticia(noticia);

                return Json("S", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
        }

        public Int64 AdicionaClick(int idVisualizacao)
        {
            Int64 result = 0;
            if (idVisualizacao > 0)
            {
                var visualizacao = _repository.ObterVisualizacaoPorId(idVisualizacao);
                if (visualizacao == null)
                    return result;

                visualizacao.Quantidade = visualizacao.Quantidade + 1;

                _repository.SalvarVisualizacao(visualizacao);

                return visualizacao.Quantidade;
            }

            return result;
        }

        private List<SlidesModel> ListarSlides()
        {
            var model = new List<SlidesModel>();
            string caminho = AppDomain.CurrentDomain.BaseDirectory + "\\Uploads\\shuttler.JPG";
            try
            {
                var noticias = _repository.ListarUltimasSlides();
                foreach (var item in noticias)
                {
                    model.Add(new SlidesModel
                    {
                        Categoria = item.Categoria.Descricao,
                        ClasseCategoria = item.Categoria.Classe,
                        Foto = item.FotoHome,
                        IdNoticia = item.Id,
                        Titulo = item.Titulo,
                        Corpo = _funcoes.RemoveTagsHTML(item.Corpo),
                        //FotoByte = GetResizedImage(caminho, Constantes.ImagemW800, Constantes.ImagemH400)
                    });
                }
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
            return model;
        }

        public byte[] GetResizedImage(String path, int width, int height)
        {
            try
            {


                if (String.IsNullOrEmpty(path))
                    return null;

                Bitmap imgIn = new Bitmap(path);
                double y = imgIn.Height;
                double x = imgIn.Width;

                double factor = 1;
                if (width > 0)
                {
                    factor = width / x;
                }
                else if (height > 0)
                {
                    factor = height / y;
                }
                System.IO.MemoryStream outStream = new System.IO.MemoryStream();
                Bitmap imgOut = new Bitmap((int)(x * factor), (int)(y * factor));

                // Set DPI of image (xDpi, yDpi)
                imgOut.SetResolution(72, 72);

                Graphics g = Graphics.FromImage(imgOut);
                g.Clear(Color.White);
                g.DrawImage(imgIn, new Rectangle(0, 0, (int)(factor * x), (int)(factor * y)),
                  new Rectangle(0, 0, (int)x, (int)y), GraphicsUnit.Pixel);

                imgOut.Save(outStream, GetImageFormat(path));
                return outStream.ToArray();
            }
            catch (Exception ex)
            {
                throw new HttpException(500, ex.Message);
            }
        }

        public string GetContentType(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return "Image/bmp";
                case ".gif": return "Image/gif";
                case ".jpg": return "Image/jpeg";
                case ".png": return "Image/png";
                default: break;
            }
            return "";
        }

        public ImageFormat GetImageFormat(String path)
        {
            switch (Path.GetExtension(path))
            {
                case ".bmp": return ImageFormat.Bmp;
                case ".gif": return ImageFormat.Gif;
                case ".jpg": return ImageFormat.Jpeg;
                case ".png": return ImageFormat.Png;
                default: break;
            }
            return ImageFormat.Jpeg;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}