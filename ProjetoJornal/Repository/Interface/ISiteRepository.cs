﻿using ProjetoJornal.Areas.Admin.ViewModel;
using ProjetoJornal.Models;
using ProjetoJornal.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Repository.Interface
{
    public interface ISiteRepository
    {
        List<Noticia> ListarNoticiasHome();
        List<Noticia> ListarUltimasDaSemana();
        List<Noticia> ListarNoticiasMaisVisualizadas();
        List<UltimasHojeModel> ListarNoticiasHoje();
        List<Noticia> ListarUltimasSlides();
        List<Noticia> ListarUltimasNoticias();
        List<Noticia> ListarNoticiasTake(int take);
        List<ListarNoticiasModel> ListarNoticiasBuscaAvancada(BuscaModel search);
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