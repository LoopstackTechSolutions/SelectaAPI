namespace SelectaAPI.DTOs
{
    public class GetClientCarDTO
    {
        public int IdProduto {get; set;}    
        public int Quantidade { get; set;}
        public int? Peso { get; set; }
        public bool Condicao { get; set; }
        public string Nome { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
