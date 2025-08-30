using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;
using SelectaAPI.Models;

namespace SelectaAPI.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tbCategoria_ProdutoModel>()
                  .HasKey(m => new { m.IdCategoria, m.IdProduto });
        }

        public DbSet<tbProdutoModel> produtos { get; set; }
        public DbSet<tbPromocaoModel> promocoes { get; set; }
        public DbSet<tbClienteModel> clientes { get; set; }
        public DbSet<tbLista_DesejoModel> listasDesejo { get; set; }
        public DbSet<tbPedidoModel> pedidos { get; set; }
        public DbSet<tbProduto_PedidoModel> produtosPedidos { get; set; }
        public DbSet<tbCategoria_ProdutoModel> categoriasProdutos { get; set; }
        public DbSet<tbNotificacaoModel> notificacoes {  get; set; }
        public DbSet<tbNotificacao_ClienteModel> notificacoesClientes { get; set; }
    }
}
