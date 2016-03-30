using LangdiDomain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LangdiDb
{
    public class NewsMapping : EntityTypeConfiguration<News>
    {
        public NewsMapping(){
            ToTable("Langdi_News");
            HasRequired(n=>n.Category)
                .WithMany(s => s.News)
                .HasForeignKey(s => s.CategoryId)
                .WillCascadeOnDelete(false);
        }
    }
    public class LangdiDbContext : DbContext
    {
        public LangdiDbContext()
            : base("LangdiDb")
        {

        }
        public LangdiDbContext(String connString)
            : base(connString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Langdi_Users");
            modelBuilder.Entity<Category>().ToTable("Langdi_Categories");
            modelBuilder.Configurations.Add(new NewsMapping());
            modelBuilder.Entity<Video>().ToTable("Langdi_Videos");
            modelBuilder.Entity<Contacter>().ToTable("Langdi_Contacter");
        }

    }
}
