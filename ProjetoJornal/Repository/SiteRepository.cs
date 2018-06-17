using ProjetoJornal.Areas.Admin.ViewModel;
using ProjetoJornal.Base;
using ProjetoJornal.Context;
using ProjetoJornal.Models;
using ProjetoJornal.Repository.Interface;
using ProjetoJornal.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace ProjetoJornal.Repository
{
    public class SiteRepository : RepositoryBase<SiteContext>, ISiteRepository
    {
        public SiteRepository(IUnitOfWork<SiteContext> unit)
            : base(unit)
        {
        }

        public List<Autor> ListarAutores()
        {
            return Context.Autor.ToList();
        }

        public List<Categoria> ListarCategorias()
        {
            return Context.Categoria.ToList();
        }

        public List<Noticia> ListarNoticiasHome()
        {
            return Context.Noticia.ToList();
        }
        public List<Noticia> ListarNoticiasMaisVisualizadas()
        {
            return Context.Noticia.Include(x => x.Visualizacoes).OrderByDescending(x => x.Visualizacoes.Quantidade).Take(6).ToList();
        }
        public List<UltimasHojeModel> ListarNoticiasHoje()
        {
            var data = DateTime.Now;
            string sql = "select Titulo as \"Titulo\", Id as \"IdNoticia\" from noticia where Data >= DATE_FORMAT(STR_TO_DATE(@p0, '%d/%m/%Y'), '%Y-%m-%d')";
            return Context.Database.SqlQuery<UltimasHojeModel>(sql, data.ToShortDateString()).ToList();
        }
        public List<Noticia> ListarNoticiasTake(int take)
        {
            return Context.Noticia.OrderByDescending(x => x.Data).Take(take).ToList();
        }
       
        public List<Noticia> ListarNoticiasBuscaAvancadaSite(string search)
        {
            return Context.Noticia
                .Include(x => x.Visualizacoes)
                .Include(x => x.Autor)
                .Include(x => x.Categoria)
                .Where(x => x.Titulo.ToLower().Contains(search.ToLower()) || x.Corpo.ToLower().Contains(search.ToLower()))
                .OrderByDescending(x => x.Data).ToList();
        }
        public List<Noticia> ListarNoticiasBuscaAvancada(BuscaModel search)
        {
            var result = Context.Noticia.OrderByDescending(x => x.Data).Take(50);

            if (search.IdAutor > 0)
                result.Where(x => x.IdAutor == search.IdAutor);

            if (search.IdCategoria > 0)
                result.Where(x => x.IdCategoria == search.IdCategoria);

            if (!string.IsNullOrEmpty(search.Titulo))
                result.Where(x => x.Titulo.ToLower().Contains(search.Titulo.ToLower()));

            if (search.DataFinal != null && search.DataInicial != null)
                result.Where(x => x.Data.Date >= search.DataInicial.Date && x.Data.Date <= search.DataFinal.Date);

            return result.ToList();
        }
        public Noticia ObterNoticiaPorId(int idNoticia)
        {
            return Context.Noticia.Include(x => x.Visualizacoes).Include(x => x.Autor).Include(x => x.Categoria).FirstOrDefault(x => x.Id == idNoticia);
        }
        public Categoria ObterCategoriaPorId(int idCategoria)
        {
            return Context.Categoria.FirstOrDefault(x => x.Id == idCategoria);
        }
        public Autor ObterAutorPorId(int idAutor)
        {
            return Context.Autor.FirstOrDefault(x => x.Id == idAutor);
        }
        public Noticia SalvarNoticia(Noticia noticia)
        {
            if (noticia.Id > 0)
                Context.Entry(noticia).State = EntityState.Modified;
            else
                Context.Noticia.Add(noticia);

            Context.SaveChanges();
            return noticia;
        }

        public Visualizacoes ObterVisualizacaoPorId(int id)
        {
            return Context.Visualizacoes.FirstOrDefault(x => x.Id == id);
        }
        public Visualizacoes SalvarVisualizacao(Visualizacoes visualizacao)
        {
            if (visualizacao.Id > 0)
                Context.Entry(visualizacao).State = EntityState.Modified;
            else
                Context.Visualizacoes.Add(visualizacao);

            Context.SaveChanges();
            return visualizacao;
        }

        public List<Noticia> ListarUltimasNoticias()
        {
            return Context.Noticia.Include(a => a.Visualizacoes).Include(a => a.Categoria).OrderByDescending(x => x.Data).Take(6).ToList();
        }

        public List<Noticia> ListarUltimasSlides()
        {
            return Context.Noticia.OrderByDescending(x => x.Data).Take(5).ToList();
        }

        public Categoria SalvarCategoria(Categoria categoria)
        {
            if (categoria.Id > 0)
                Context.Entry(categoria).State = EntityState.Modified;
            else
                Context.Categoria.Add(categoria);

            Context.SaveChanges();
            return categoria;
        }

        public Autor SalvarAutor(Autor autor)
        {
            if (autor.Id > 0)
                Context.Entry(autor).State = EntityState.Modified;
            else
                Context.Autor.Add(autor);

            Context.SaveChanges();
            return autor;
        }

       
    }
}