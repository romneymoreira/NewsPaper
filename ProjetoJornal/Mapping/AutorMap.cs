using ProjetoJornal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Mapping
{
    public class AutorMap : EntityTypeConfiguration<Autor>
    {
        public AutorMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Email)
                .HasMaxLength(255).IsRequired();
            this.Property(t => t.Celular)
                .HasMaxLength(15).IsRequired();
            this.Property(t => t.Nome)
                .HasMaxLength(255).IsRequired();

        }
    }
}