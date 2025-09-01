namespace SelectaAPI.DTOs
{
    public class AddEmployeeDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public string? NivelAcesso { get; set; } // "comum" ou "gerente"
    }
}
