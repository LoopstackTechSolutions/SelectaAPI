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

        public DbSet<tbProdutoModel> produtos { get; set; }
        public DbSet<tbPromocaoModel> promocoes { get; set; }
        public DbSet<tbClienteModel> clientes { get; set; }
        public DbSet<tbLista_DesejoModel> listasDesejo { get; set; }
        public DbSet<tbPedidoModel> pedidos { get; set; }
        public DbSet<tbProduto_PedidoModel> produtosPedidos { get; set; }
        public DbSet<tbCategoria_ProdutoModel> categoriasProdutos { get; set; }
        public DbSet<tbNotificacaoModel> notificacoes {  get; set; }
        public DbSet<tbNotificacao_ClienteModel> notificacoesClientes { get; set; }
        public DbSet<tbFuncionarioModel> funcionarios { get; set; }
        public DbSet<tbCategoriaModel> categorias { get; set; }
        public DbSet<tbCategoria_Cliente> categoriaClientes { get; set; }
        public DbSet<tbCategoria_ProdutoModel> categoriaProdutos { get; set; }
        public DbSet<tbCarrinhoModel> carrinho { get; set; }
        public DbSet<tbEntregadorModel> entregadores { get; set; }
        public DbSet<tbVendedorModel> vendedores { get; set; }
        public DbSet<tbImagem_ProdutoModel> imagensProdutos { get; set; }
        public DbSet<tbEnderecoModel> enderecos { get; set; }
    }
}
