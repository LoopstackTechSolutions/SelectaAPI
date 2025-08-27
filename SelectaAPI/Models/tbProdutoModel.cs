namespace SelectaAPI.Models
{
    public class tbProdutoModel
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Peso { get; set; }
        public string Status { get; set; }
        public tbVendedorModel IdVendedor { get; set; }
    }
}
