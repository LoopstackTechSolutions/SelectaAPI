using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.Models
{
    [Table("tbNotificacao_Cliente")]
    public class tbNotificacao_ClienteModel
    {
        [Key]
        public int IdNotificacaoCliente { get; set; }
        public int IdNotificacao { get; set; }
        public int IdCliente { get; set; }
        public int IdContexto { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.Now;
        public bool IsLida { get; set; } = false;
        [ForeignKey("IdNotificacao")]
        public tbNotificacaoModel Notificacao { get; set; }
        [ForeignKey("IdCliente")]
        public tbClienteModel Cliente { get; set; }
    }
}
