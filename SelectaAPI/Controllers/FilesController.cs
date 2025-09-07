using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SelectaAPI.Services.Interfaces;

namespace SelectaAPI.Controllers
{
    [Route("selectaAPI/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {

        private readonly IFilesUploadAWSService _filesUpload;

        public FilesController(IFilesUploadAWSService filesUpload)
        {
            _filesUpload = filesUpload;
        }

        [HttpPost("photo-select")]
        public async Task<IActionResult> UploadFiles(IFormFile file, string? prefix)
        {
            try
            {
                var uploadFiles = await _filesUpload.UploadFiles(file, prefix);
                return Ok(uploadFiles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro no upload: {ex.Message}");
            }
        }
    }
}
