using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbCliente")]
    public class tbClienteModel
    {
        [Key]
        public int IdCliente { get; set; }
        public string Nome { get; set;}
        public string Email { get; set;}
        public string Senha { get; set;}
        public decimal Saldo { get; set; } = 0;
    }
}
