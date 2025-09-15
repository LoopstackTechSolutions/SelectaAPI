namespace SelectaAPI.DTOs
{
    public class GetClientByIdDTO
    {
        public string Nome { get; set; }
        public int IdCliente { get; set; }
        public decimal Saldo { get; set; }
        public string Email { get; set; }
    }
}
