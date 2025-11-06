using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbCentro_Distribuicao")]
    public class tbCentro_DistribuicaoModel
    {
        [Key]
        public int IdCd { get; set; }
        [ForeignKey("Endereco")]
        public int IdEndereco {get; set;}
        public string Nome {get; set;}

        public tbEnderecoModel Endereco { get; set; }
    }
}
