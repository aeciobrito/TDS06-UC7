using Microsoft.EntityFrameworkCore;
using Pousada.API.Models;

namespace Pousada.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Item> Itens { get; set; }
        public DbSet<Chale> Chales { get; set; }
    }
}
