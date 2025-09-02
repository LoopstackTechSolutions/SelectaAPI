using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbCategoria_Cliente")]
    public class tbCategoria_Cliente
    {
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public tbClienteModel Cliente { get; set; }

        [ForeignKey("Produto")]
        public int IdProduto { get; set; }
        public tbProdutoModel Produto { get; set; }
    }
}
