using RestWithASPNET.Api.Dto.Response;

namespace RestWithASPNET.Api.Business
{
    public interface IFileBusiness
    {
        byte[] GetFile(string filename);

        Task<FileDetailsResponse> SaveFileToDisk(IFormFile file);

        Task<List<FileDetailsResponse>> SaveFilesToDisk(IList<IFormFile> files);
    }
}