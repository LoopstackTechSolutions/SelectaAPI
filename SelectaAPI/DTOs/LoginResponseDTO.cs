namespace SelectaAPI.DTOs
{
    public class LoginResponseDTO
    {
       public string? Nome { get; set; }
       public int IdCliente { get; set; }
       public int IdFuncionario { get; set; }
       public string? NivelAcesso { get; set; }
       public string Email { get; set; }
       public string Token {get; set; }
    }
}
