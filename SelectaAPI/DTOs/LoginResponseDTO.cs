namespace SelectaAPI.DTOs
{
    public class LoginResponseDTO
    {
       public string? NomeCliente { get; set; }
       public string? IdCliente { get; set; }
       public int? IdFuncionario { get; set; }
       public string? NomeFuncionario { get; set; }
       public string? AccessToken { get; set; }
       public int ExpiressIn { get; set; }
    }
}
