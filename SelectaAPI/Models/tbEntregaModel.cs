using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbEntrega")]
    public class tbEntregaModel
    {
        [ForeignKey("Entrega")]
        public int IdEntrega { get; set; }

        [ForeignKey("Produto")]
        public int IdProduto { get; set; }

        public tbEntregaModel Entrega { get; set; }
        public tbProdutoModel Produto { get; set; }
    }
}
