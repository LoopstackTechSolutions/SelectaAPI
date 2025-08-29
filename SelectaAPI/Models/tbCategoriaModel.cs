using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbCategoria")]
    public class tbCategoriaModel
    {
        [Key]
        public int IdCategoria {  get; set; }
        public string Nome { get; set; }
    }
}
