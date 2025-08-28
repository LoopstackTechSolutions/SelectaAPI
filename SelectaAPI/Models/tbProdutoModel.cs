using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbProduto")]
    public class tbProdutoModel
    {
        [Key]
        public int IdProduto { get; set; }
        public string Nome { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public int Peso { get; set; }
        public string Status { get; set; }
        public int IdVendedor { get; set; }
       // public tbVendedorModel Vendedor { get; set; }
    }
}
