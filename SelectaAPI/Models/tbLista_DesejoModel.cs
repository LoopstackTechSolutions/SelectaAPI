using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbLista_Desejo")]
    public class tbLista_DesejoModel
    {
        [Key, ForeignKey("Produto")]
        public int IdProduto { get; set; }
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public tbClienteModel Cliente {get; set;}
        public tbProdutoModel Produto { get; set; }
    }
}
