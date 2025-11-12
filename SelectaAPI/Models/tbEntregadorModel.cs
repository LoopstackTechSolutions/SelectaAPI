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

        public string Cnh { get; set; }
        public bool Eligibilidade { get; set; } = false;
    }
}
