namespace SelectaAPI.DTOs
{
    public class PedidoDTO
    {
        public int IdProduto { get; set; }
        public int Quantidade { get; set; }
        public string FormaPagamento { get; set; } // "credito", "debito", "pix"
        public int IdEnderecoEntrega { get; set; }
    }
}
