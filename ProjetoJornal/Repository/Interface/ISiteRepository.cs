using ProjetoJornal.Areas.Admin.ViewModel;
using ProjetoJornal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Repository.Interface
{
    public interface ISiteRepository
    {
        List<Noticia> ListarNoticiasHome();
        List<Noticia> ListarUltimasSlides();
        List<Noticia> ListarNoticiasTake(int take);
        List<Noticia> ListarNoticiasBuscaAvancada(BuscaModel search);
        List<Noticia> ListarNoticiasBuscaAvancadaSite(string search);
        List<Autor> ListarAutores();
        List<Categoria> ListarCategorias();
        Categoria ObterCategoriaPorId(int idCategoria);
        Categoria SalvarCategoria(Categoria categoria);
        Autor ObterAutorPorId(int idAutor);
        Autor SalvarAutor(Autor autor);
        Noticia ObterNoticiaPorId(int idNoticia);
        Noticia SalvarNoticia(Noticia noticia);
        Visualizacoes ObterVisualizacaoPorId(int id);
        Visualizacoes SalvarVisualizacao(Visualizacoes visualizacao);

    }
}