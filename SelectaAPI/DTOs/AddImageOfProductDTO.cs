namespace SelectaAPI.DTOs
{
    public class AddImageOfProductDTO
    {
        public bool Principal { get; set; }
        public int IdProduto { get; set; }
        public string? Prefix { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
