using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbCarrinho")]
    public class tbCarrinhoModel
    {
        [ForeignKey("Produto")]
        [Key]
        public int IdProduto { get; set; }
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public int Quantidade { get; set; }

        public tbProdutoModel Produto { get; set; }
        public tbClienteModel Cliente { get; set; }
    }
}
