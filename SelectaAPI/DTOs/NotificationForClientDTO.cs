namespace SelectaAPI.DTOs
{
    public class NotificationForClientDTO
    {
        public string Mensagem { get; set; }
        public bool? IsLida { get; set; } = false;
        public DateTime? DataCriacao { get; set; }
        public int Quantidade { get; set; }
    }
}
