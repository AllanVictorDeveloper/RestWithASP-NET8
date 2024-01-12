using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.Api.Business;
using RestWithASPNET.Api.Dto.Response;

namespace RestWithASPNET.Api.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    //[Authorize("Bearer")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileBusiness _fileBusiness;

        public FileController(IFileBusiness fileBusiness)
        {
            _fileBusiness = fileBusiness;
        }

        [HttpGet]
        [Route("downloadFile/{fileName}")]
        [ProducesResponseType((200), Type = typeof(byte[]))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> downloadFileAsync(string fileName)
        {
            byte[] buffer = _fileBusiness.GetFile(fileName);

            if (buffer is not null)
            {
                HttpContext.Response.ContentType = $"application/{Path.GetExtension(fileName).Replace(".", "")}";
                HttpContext.Response.Headers.Append("content-length", buffer.Length.ToString());

                await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            }

            return new ContentResult();
        }

        [HttpPost]
        [Route("uploadFile")]
        [ProducesResponseType((200), Type = typeof(FileDetailsResponse))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadOneFile([FromForm] IFormFile file)
        {
            FileDetailsResponse detail = await _fileBusiness.SaveFileToDisk(file);

            if (detail.DocumentName is null)
                return BadRequest("Não  foi possivel salvar o arquivo.");

            return new OkObjectResult(detail);
        }

        [HttpPost]
        [Route("uploadMultipleFile")]
        [ProducesResponseType((200), Type = typeof(List<FileDetailsResponse>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> uploadMultipleFile([FromForm] List<IFormFile> files)
        {
            List<FileDetailsResponse> details = await _fileBusiness.SaveFilesToDisk(files);

            if (details.Count.Equals(0))
                return BadRequest("Não foi possivel salvar os arquivos.");

            return new OkObjectResult(details);
        }
    }
}