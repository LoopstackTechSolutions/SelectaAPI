using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbFuncionario")]
    public class tbFuncionarioModel
    {
        [Key]
        public int IdFuncionario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public string? NivelAcesso { get; set; } // "comum" ou "gerent
    }
}
