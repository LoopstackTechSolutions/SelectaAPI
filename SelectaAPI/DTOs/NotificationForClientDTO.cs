namespace SelectaAPI.DTOs
{
    public class NotificationForClientDTO
    {
        public string Mensagem { get; set; }
        public bool? IsLida { get; set; } = false;
        public DateTime? DataCriacao { get; set; }
        public string Titulo { get; set; }
        public ICollection<NotificationForClientDTO> Notifications { get; set; }
    }
}
