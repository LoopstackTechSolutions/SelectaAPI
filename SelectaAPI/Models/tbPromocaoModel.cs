using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbPromocao")]
    public class tbPromocaoModel
    {
        [Key]
        public int IdPromocao { get; set; }
        [ForeignKey("Produto")]
        public int IdProduto { get; set; }
        public string Status {  get; set; }
        public int Desconto { get; set; }
        public DateTime? ValidaAte { get; set; }
        public tbProdutoModel Produto { get; set; }
    }
}
