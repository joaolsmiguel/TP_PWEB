using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TP_PWEB2.Models;

namespace TP_PWEB2.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public virtual DbSet<CategoriaCheck_List> CategoryCheckList { get; set; }
        public virtual DbSet<Check_List> Check_List { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }
        public virtual DbSet<Avaliacao> Avaliacao { get; set; }
        public virtual DbSet<Alojamento> Alojamento { get; set; }
        public virtual DbSet<Reserva> Reserva { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<CategoriaCheck_List>()
                .HasKey(x => new { x.CategoriaId, x.Check_ListId });

            builder.Entity<CategoriaCheck_List>()
                .HasOne(x => x.Categoria)
                .WithMany(x => x.CategoriaCheck_List)
                .HasForeignKey(x => x.CategoriaId);

            builder.Entity<CategoriaCheck_List>()
                .HasOne(x => x.Check)
                .WithMany(x => x.CategoriaCheck_List)
                .HasForeignKey(x => x.Check_ListId);
            builder.Entity<Categoria>().HasData(new Categoria{ nome = "quarto" , CategoriaId=1});
            base.OnModelCreating(builder);
        }

    }
}
