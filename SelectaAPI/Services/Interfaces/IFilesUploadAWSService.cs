using SelectaAPI.DTOs;

namespace SelectaAPI.Services.Interfaces
{
    public interface IFilesUploadAWSService
    {
        Task<S3ObjectDTO> UploadFiles(IFormFile file, string? prefix);
        Task<string?> GetImage(string s3Key);
    }
}
