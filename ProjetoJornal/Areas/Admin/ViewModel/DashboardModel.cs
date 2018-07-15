using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Areas.Admin.ViewModel
{
    public class DashboardModel
    {
        public DashboardModel()
        {
            MaisVisualizadas = new List<MaisVisualizadasDashboard>();
            NoticiasPorCategoria = new List<NoticiasPorCategoriaDashboard>();
        }
        public List<MaisVisualizadasDashboard> MaisVisualizadas { get; set; }
        public List<NoticiasPorCategoriaDashboard> NoticiasPorCategoria { get; set; }
        public long TotalVizualizacoes { get; set; }
        public long TotalNoticias { get; set; }
        public long TotalNoticiasSemana { get; set; }
    }

    public class MaisVisualizadasDashboard
    {
        public int IdNoticia { get; set; }
        public string Titulo { get; set; }
        public Int64 Visualizacoes { get; set; }
    }

    public class NoticiasPorCategoriaDashboard
    {
        public Int64 Total { get; set; }
        public string Descricao { get; set; }
    }
}