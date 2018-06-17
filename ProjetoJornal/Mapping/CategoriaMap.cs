using ProjetoJornal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Mapping
{
    public class CategoriaMap : EntityTypeConfiguration<Categoria>
    {
        public CategoriaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Descricao)
                .HasMaxLength(255).IsRequired();
            this.Property(t => t.Status)
                .HasMaxLength(1).IsRequired();
            this.Property(t => t.Classe)
                .HasMaxLength(255).IsRequired();

        }
    }
}