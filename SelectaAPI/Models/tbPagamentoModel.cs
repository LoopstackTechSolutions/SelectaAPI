using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbPagamento")]
    public class tbPagamentoModel
    {
        [Key]
        public int IdPagamento { get; set; }
        [ForeignKey("Pedido")]
        public int IdPedido { get; set; }
        public DateTime DataPagamento { get; set; }
        public string FormaPagamento { get; set; }

        public tbPedidoModel Pedido { get; set; }
    }
}
