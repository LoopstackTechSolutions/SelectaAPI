using SelectaAPI.Models;

namespace SelectaAPI.DTOs
{
    public class DetalhesDoPedidoDTO
    {
        public int IdPedido { get; set; }
        public string Nome { get; set; }
        public decimal Frete { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public tbProdutoModel Produtos { get; set; }
    }
}
