using ProjetoJornal.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Mapping
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(150).IsRequired();
            this.Property(t => t.Status)
                .HasMaxLength(1).IsRequired();
            this.Property(t => t.Email)
                .HasMaxLength(200).IsRequired();
            this.Property(t => t.Senha)
               .HasMaxLength(20).IsRequired();

        }
    }
}