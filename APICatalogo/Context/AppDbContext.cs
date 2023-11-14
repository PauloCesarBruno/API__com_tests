using APICatalogo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
// CONTEXTO//

namespace APICatalogo.Context
{
    // A herança agora é de "IdentityDbContext" e é preciso instalar
    // o pacote "Microsoft.AspNetCore.Identity.EntityFrameworkCore".
    public class AppDbContext : IdentityDbContext
    {           
        public AppDbContext(DbContextOptions<AppDbContext> options)  : base (options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Produto>()
                .Property(p => p.Preco)
                .HasColumnType("decimal(18,2)");
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
    }
}
