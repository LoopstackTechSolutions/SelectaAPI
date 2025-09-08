using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbVendedor")]
    public class tbVendedorModel
    {
        [ForeignKey("Cliente")]
        [Key]
        public int IdVendedor {get; set;}
        public int Nota {get; set;}  
        public int TaxaFrete {get; set;}
        public tbClienteModel Cliente { get; set; }
    }
}
