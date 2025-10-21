using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbImagemProduto")]
    public class tbImagem_ProdutoModel
    {
        [Key]
        public int IdImagem { get; set; }
        public string S3Key { get; set; }
        public bool IsPrincipal { get; set; }
        [ForeignKey("Produto")]
        public int IdProduto { get; set; }
        public tbProdutoModel Produto { get; set; }
    }
}
