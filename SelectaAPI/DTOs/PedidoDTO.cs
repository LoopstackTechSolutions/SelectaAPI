namespace SelectaAPI.DTOs
{
    public class PedidoDTO
    {
        public int IdPedido { get; set; }
        public decimal Total { get; set; }
        public decimal Frete { get; set; }
        public string StatusPagamento { get; set; }
    }
}
