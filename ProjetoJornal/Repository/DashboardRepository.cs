using ProjetoJornal.Areas.Admin.ViewModel;
using ProjetoJornal.Base;
using ProjetoJornal.Context;
using ProjetoJornal.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Repository
{
    public class DashboardRepository : RepositoryBase<SiteContext>, IDashboardRepository
    {
        public DashboardRepository(IUnitOfWork<SiteContext> unit)
            : base(unit)
        {
        }
        public DashboardModel Dashboard()
        {
            var model = new DashboardModel();

            DateTime data = DateTime.Now.AddDays(-7);

            var noticias = Context.Noticia.Where(x => x.Status == "P").ToList();

            var totalVisualizacoes = noticias;
            model.TotalVizualizacoes = Context.Visualizacoes.Sum(X => X.Quantidade);
            model.TotalNoticiasSemana = totalVisualizacoes.Where(x => x.Data >= data).ToList().Count();
            model.TotalNoticias = noticias.Count();

            var maisNoticias = noticias;
            var mais = maisNoticias.OrderByDescending(x => x.Visualizacoes.Quantidade).Take(5);

            foreach (var item in mais)
            {
                model.MaisVisualizadas.Add(new MaisVisualizadasDashboard()
                {
                    IdNoticia = item.Id,
                    Titulo = item.Titulo,
                    Visualizacoes = item.Visualizacoes.Quantidade
                });
            }

            var categoria = Context.Categoria.Where(x => x.Status == "A").ToList();
            foreach (var item in categoria)
            {
                var result = noticias.Where(x => x.IdCategoria == item.Id).ToList();
                model.NoticiasPorCategoria.Add(new NoticiasPorCategoriaDashboard()
                {
                    Descricao = item.Descricao,
                    Total = result.Count()
                });
            }

            return model;
        }
    }
}