using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbProduto_Pedido")]
    [PrimaryKey(nameof(IdPedido), nameof(IdProduto))]
    public class tbProduto_PedidoModel
    {
       
        [ForeignKey("Produto")]
        public int IdProduto { get; set; }
        public tbProdutoModel Produto { get; set; }
        
        [ForeignKey("Pedido")]
        public int IdPedido { get; set; }
        public tbPedidoModel Pedido { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public decimal Frete { get; set; }
        public string Status { get; set; } = "pendente";
        public bool TipoEntrega { get; set; }
    }
}
