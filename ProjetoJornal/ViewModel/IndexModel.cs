using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjetoJornal.ViewModel
{
    public class IndexModel
    {
        public IndexModel()
        {
            Slides = new List<SlidesModel>();
            Ultimas = new List<UltimasModel>();
            MaisVisualizadas = new List<MaisVisualizadasModel>();
            UltimasHoje = new List<UltimasHojeModel>();
        }
        public List<SlidesModel> Slides { get; set; }
        public List<UltimasModel> Ultimas { get; set; }
        public List<MaisVisualizadasModel> MaisVisualizadas { get; set; }
        public List<UltimasHojeModel> UltimasHoje { get; set; }

    }
}