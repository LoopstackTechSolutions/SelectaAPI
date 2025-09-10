namespace SelectaAPI.DTOs
{
    public class SearchProductsByCategoryDTO
    {
        public string Nome { get; set; }
        public int? Peso { get; set; }
        public decimal? PrecoUnitario { get; set; } = 0;
        public bool? Condicao { get; set; }
        public string Status { get; set; }
        public int Nota { get; set; }
        public int? Quantidade { get; set; }
        public string NomeCategoria { get; set; }
    }
}
