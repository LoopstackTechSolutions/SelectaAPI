namespace SelectaAPI.DTOs
{
    public class EditProductDTO
    {
        // public int IdProduto { get; set; }
        public string Nome { get; set; }
        public int? Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public bool Condicao { get; set; }
        public int? Peso { get; set; }
        public string? Status { get; set; }
        public int? Nota { get; set; }
        public string? Descricao { get; set; }
    }
}
