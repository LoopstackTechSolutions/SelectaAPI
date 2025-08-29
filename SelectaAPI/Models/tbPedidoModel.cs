using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbPedido")]
    public class tbPedidoModel
    {
        [Key]
        public int IdPedido { get; set; }
        [ForeignKey("Cliente")]
        public int IdComprador { get; set; }
        public tbClienteModel Cliente { get; set; }
        [ForeignKey("Endereco")]
        public int IdEnderecoEntrega { get; set; }
        public tbEnderecoModel Endereco { get; set; }
        public DateTime DataPedido { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public decimal Frete { get; set; }
        public string StatusPagamento { get; set; } = "pendente";
    }
}
