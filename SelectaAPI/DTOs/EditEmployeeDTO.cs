namespace SelectaAPI.DTOs
{
    public class EditEmployeeDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public string? NivelAcesso { get; set; } 
    }
}
