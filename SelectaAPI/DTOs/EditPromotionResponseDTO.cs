namespace SelectaAPI.DTOs
{
    public class EditPromotionResponseDTO
    {
        public int ValorDesconto { get; set; }
        public DateTime? Validade { get; set; }
        public string Status { get; set; }

        public decimal NovoValor { get; set; }
        public decimal ValorAnterior { get; set; }
    }
}
