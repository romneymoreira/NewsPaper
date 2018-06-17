using ProjetoJornal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Mapping
{
    public class NoticiaMap : EntityTypeConfiguration<Noticia>
    {
        public NoticiaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Titulo)
                .HasMaxLength(400).IsRequired();
            this.Property(t => t.Status)
                .HasMaxLength(1).IsRequired();
            this.Property(t => t.VaiParaHome)
               .HasMaxLength(1).IsRequired();
            this.Property(t => t.Corpo)
                .IsRequired();
            this.Property(t => t.Data)
              .IsRequired();
            this.Property(t => t.FotoHome)
               .HasMaxLength(4000).IsOptional();

            this.HasRequired(x => x.Categoria).WithMany().HasForeignKey(x => x.IdCategoria);
            this.HasRequired(x => x.Autor).WithMany().HasForeignKey(x => x.IdAutor);
            this.HasRequired(x => x.Visualizacoes).WithMany().HasForeignKey(x => x.IdVisualizacao);

        }
    }
}