using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbEndereco")]
    public class tbEnderecoModel
    {
        [Key]
        public int IdEndereco { get; set; }
        public int Cep { get; set; }
        public string Logradouro { get; set; }
        public bool isPrincipal { get; set; } = true;
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public tbClienteModel Cliente { get; set; }
        public int Numero { get; set; }
    }
}
