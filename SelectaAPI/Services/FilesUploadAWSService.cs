using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using SelectaAPI.DTOs;
using SelectaAPI.Models;
using SelectaAPI.Services.Interfaces;

namespace SelectaAPI.Services
{
    public class FilesUploadAWSService : IFilesUploadAWSService
    {
        private readonly IAmazonS3 _s3Client;
        private const string DefaultBucketName = "selecta-images";

        public FilesUploadAWSService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        public async Task<S3ObjectDTO> UploadFiles(IFormFile file, string? prefix)
        {
            var bucketName = DefaultBucketName;

            var exists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
            if (!exists)
            {
                await _s3Client.PutBucketAsync(new PutBucketRequest
                {
                    BucketName = bucketName,
                    UseClientRegion = true
                });
            }

            var key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}";

            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = key,
                InputStream = file.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", file.ContentType);

            await _s3Client.PutObjectAsync(request);

            var urlRequest = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = key,
                Expires = DateTime.UtcNow.AddHours(1)
            };
            string temporaryUrl = _s3Client.GetPreSignedURL(urlRequest);

            return new S3ObjectDTO
            {
                Name = bucketName,
                PresignedUrl = temporaryUrl
            };
        }
    }
}
