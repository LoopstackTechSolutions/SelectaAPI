using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbNotificacao")]
    public class tbNotificacaoModel
    {
        [Key]
        public int IdNotificacao { get; set; }
        public string Mensagem { get; set; }
        public string TabelaContexto { get; set; }
    }
}
