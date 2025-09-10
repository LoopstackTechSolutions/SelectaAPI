using MySqlX.XDevAPI;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbReview")]
    public class tbReviewModel
    {
        [Key]
        public int IdReview { get; set; }

        [ForeignKey("Cliente")]
        public int IdCliente { get; set; }

        [ForeignKey("Produto")]
        public int IdProduto { get; set; }

        [Range(1, 5, ErrorMessage = "A nota deve estar entre 1 e 5 estrelas.")]
        public byte Nota { get; set; } // tinyint vira byte

        public string Comentario { get; set; } = string.Empty;

        
        public tbClienteModel Cliente { get; set; }
        public tbProdutoModel Produto { get; set; }
    }
}
