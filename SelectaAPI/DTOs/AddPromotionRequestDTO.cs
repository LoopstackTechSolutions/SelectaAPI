using Org.BouncyCastle.Asn1.Crmf;

namespace SelectaAPI.DTOs
{
    public class AddPromotionRequestDTO
    {
        public int IdProduto { get; set; }
        public int Desconto { get; set; }
        public string Status { get; set; }
     public DateTime? Validade { get; set; }
    }
}
