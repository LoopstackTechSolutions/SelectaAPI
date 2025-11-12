namespace SelectaAPI.DTOs
{
    public class AddEntregadorDTO
    {
        public int IdEntregador { get; set; }  
        public int IdEndereco { get; set; }
        public string Cnh { get; set; } = string.Empty;
    }
}
