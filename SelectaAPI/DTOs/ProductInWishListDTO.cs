namespace SelectaAPI.DTOs
{
    public class ProductInWishListDTO
    {
        public int IdProduto { get; set; }
        public string Nome { get; set; } = string.Empty;
        public decimal PrecoUnitario { get; set; }
        public int Quantidade { get; set; }
        public bool Condicao { get; set; }
        public string Status { get; set; }
        public int? Peso { get; set; }
    }
}
