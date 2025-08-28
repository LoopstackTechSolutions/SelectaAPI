using Microsoft.EntityFrameworkCore;
using SelectaAPI.Models;

namespace SelectaAPI.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<tbProdutoModel> produtos { get; set; }
        public DbSet<tbPromocaoModel> promocoes { get; set; }
    }
}
