namespace SelectaAPI.DTOs
{
    public class ProductsWithPromotionDTO
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
        public bool Condicao { get; set; }
        public decimal Peso { get; set; }
        public int Quantidade { get; set; }
        public string Status { get; set; }

        public DateTime? ValidoAte { get; set; }
        public decimal Desconto { get; set; }
    }
}
