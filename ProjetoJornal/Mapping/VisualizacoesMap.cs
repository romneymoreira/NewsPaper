using ProjetoJornal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Mapping
{
    public class VisualizacoesMap : EntityTypeConfiguration<Visualizacoes>
    {
        public VisualizacoesMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);
            this.Property(t => t.Quantidade)
                .IsRequired();
        }
    }
}