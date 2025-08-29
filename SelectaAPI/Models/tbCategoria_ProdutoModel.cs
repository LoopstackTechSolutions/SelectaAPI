using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbCategoria_Produto")]
    [PrimaryKey(nameof(IdCategoria), nameof(IdProduto))]
    public class tbCategoria_ProdutoModel
    {
        [ForeignKey("Categoria")]
        public int IdCategoria { get; set; }
        public tbCategoriaModel Categoria { get; set; }

        [ForeignKey("Produto")]
        public int IdProduto { get; set; }
        public tbProdutoModel Produto { get; set; }
    }
}
