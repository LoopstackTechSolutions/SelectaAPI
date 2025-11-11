using System.ComponentModel.DataAnnotations.Schema;

namespace SelectaAPI.DTOs
{
    public class EditPromotionRequestDTO
    {
        public int IdPromocao { get; set; }
        public string Status { get; set; }
        public int Desconto { get; set; }
        public DateTime? ValidaAte { get; set; }
    }
}
