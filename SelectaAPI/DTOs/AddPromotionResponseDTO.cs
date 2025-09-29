namespace SelectaAPI.DTOs
{
    public class AddPromotionResponseDTO
    {
       public int IdProduto { get; set; }
       public int ValorDesconto {get; set; }
       public DateTime? Validade { get; set; }
       public string Status { get; set;}

       public decimal NovoValor { get; set; }
       public decimal ValorAnterior { get; set; }
    }
}
