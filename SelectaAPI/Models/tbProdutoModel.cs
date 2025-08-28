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
        public int? Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public bool Condicao { get; set; }
        public int? Peso { get; set; }
        public string Status { get; set; }
<<<<<<< HEAD
        public int IdVendedor { get; set; }
       // public tbVendedorModel Vendedor { get; set; }
=======
        public int? IdVendedor { get; set; }
>>>>>>> d61e0a12100fd4a24634297fa317c52f1b64a9c3
    }
}
