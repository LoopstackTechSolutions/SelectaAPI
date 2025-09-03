using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SelectaAPI.Controllers
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IAmazonS3 _s3Client;
        private const string DefaultBucketName = "selecta-images"; 

        public FilesController(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
        }

        [HttpPost("photo-select")]
        public async Task<IActionResult> UploadFiles(IFormFile file, string? prefix)
        {
            try
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
                    Expires = DateTime.UtcNow.AddMinutes(1)
                };
                string temporaryUrl = _s3Client.GetPreSignedURL(urlRequest);

                return Ok(new
                {
                    Message = "Upload realizado com sucesso",
                    FileName = file.FileName,
                    TemporaryUrl = temporaryUrl
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro no upload: {ex.Message}");
            }
        }
    }
}
