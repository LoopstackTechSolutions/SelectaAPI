using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbEntregador")]
    public class tbEntregadorModel
    {
        [ForeignKey("Cliente")]
        [Key]
        public int IdEntregador { get; set; }
        public tbClienteModel Cliente { get; set; }

        [ForeignKey("Endereco")]
        public int IdEndereco { get; set; }
        public tbEnderecoModel Endereco { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessage = "A CNH deve ter exatamente 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "A CNH deve conter apenas números.")]
        public string Cnh { get; set; }
        public bool Eligibilidade { get; set; } = false;
    }
}
