using RestWithASPNET.Api.Dto.Response;

namespace RestWithASPNET.Api.Business.Implementations
{
    public class FileBusiness : IFileBusiness
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;

        public FileBusiness(IHttpContextAccessor context)
        {
            _context = context;
            _basePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
        }

        public byte[] GetFile(string filename)
        {
            var filePath = _basePath + filename;

            return File.ReadAllBytes(filePath);
        }

        public async Task<FileDetailsResponse> SaveFileToDisk(IFormFile file)
        {
            FileDetailsResponse fileDetails = new FileDetailsResponse();

            var fileType = Path.GetExtension(file.FileName);

            var baseUrl = _context.HttpContext.Request.Host;

            if (fileType.ToLower() == ".pdf" ||
                fileType.ToLower() == ".jpg" ||
                fileType.ToLower() == ".png" ||
                fileType.ToLower() == ".jpeg" ||
                fileType.ToLower() == ".xlsx" ||
                fileType.ToLower() == ".xlsm" ||
                fileType.ToLower() == ".docx"

                )

            {
                var docName = Path.GetFileName(file.FileName);

                if (file is not null && file.Length > 0)
                {
                    var destination = Path.Combine(_basePath, "", docName);

                    fileDetails.DocumentName = docName;
                    fileDetails.DocType = fileType;
                    fileDetails.DocUrl = Path.Combine(baseUrl + "/api/file/v1/" + fileDetails.DocumentName);

                    using var stream = new FileStream(destination, FileMode.Create);

                    await file.CopyToAsync(stream);
                }
            }

            return fileDetails;
        }

        public async Task<List<FileDetailsResponse>> SaveFilesToDisk(IList<IFormFile> files)
        {
            List<FileDetailsResponse> list = new List<FileDetailsResponse>();

            foreach (var file in files)
            {
                list.Add(await SaveFileToDisk(file));
            }

            return list;
        }
    }
}