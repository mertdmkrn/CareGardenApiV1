namespace CareGardenApiV1.Handler.Abstract
{
    public interface IFileHandler
    {
        Task<string> UploadFile(IFormFile file, string pathName = "", string fileName = "");
        Task<string> UploadFreeImageServer(IFormFile file);
        bool DeleteFile(string pathName = "", string fileName = "");
    }
}
