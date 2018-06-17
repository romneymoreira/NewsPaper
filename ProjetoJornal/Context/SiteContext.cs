using ProjetoJornal.Areas.Admin.Models;
using ProjetoJornal.Mapping;
using ProjetoJornal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ProjetoJornal.Context
{
    public partial class SiteContext : DbContext
    {
        static SiteContext()
        {
            Database.SetInitializer<SiteContext>(null);
        }
        public DbSet<Noticia> Noticia { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Visualizacoes> Visualizacoes { get; set; }
        public DbSet<Autor> Autor { get; set; }
        public DbSet<Categoria> Categoria { get; set; }

        public SiteContext()
            : base("SiteConnection")
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = true;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new NoticiaMap());
            modelBuilder.Configurations.Add(new VisualizacoesMap());
            modelBuilder.Configurations.Add(new AutorMap());
            modelBuilder.Configurations.Add(new CategoriaMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
        }
    }
}