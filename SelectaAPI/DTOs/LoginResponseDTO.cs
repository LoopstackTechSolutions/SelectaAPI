namespace SelectaAPI.DTOs
{
    public class LoginResponseDTO
    {
       public string? Nome { get; set; }
       public int IdCliente { get; set; }
       public int IdFuncionario { get; set; }
       public string? NivelAcesso { get; set; }
    }
}
