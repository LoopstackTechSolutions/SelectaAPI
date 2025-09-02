using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{

    [Table("tbCategoria_Cliente")]
    [PrimaryKey(nameof(IdCategoria), nameof(IdCliente))]
    public class tbCategoria_Cliente
    {
        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }
        public tbClienteModel Cliente { get; set; }

        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }
        public tbProdutoModel Categoria { get; set; }
    }
}
