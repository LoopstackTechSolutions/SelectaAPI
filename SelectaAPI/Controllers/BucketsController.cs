
/*using Amazon.S3;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("selectaAPI/[controller]")]
public class BucketsController : ControllerBase
{
    private readonly IAmazonS3 _s3Client;

    public BucketsController(IAmazonS3 s3Client)
    {
        _s3Client = s3Client ?? throw new ArgumentNullException(nameof(s3Client));
    }

    [HttpPost("bucket-test")]
    public async Task<IActionResult> CreateBucket([FromQuery] string bucketName)
    {
        var exists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client, bucketName);
        if (exists)
            return BadRequest("Bucket já existe (na sua conta ou em outra conta AWS).");

        await _s3Client.PutBucketAsync(bucketName);
        return Created("", new { BucketName = bucketName });
    }
}
*/
