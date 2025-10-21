namespace SelectaAPI.DTOs
{
    public class S3ObjectDTO
    {
        public string? Name { get; set; }
        public string? PresignedUrl { get; set; }
        public string? S3Key { get; set; }
    }
}
