namespace CareGardenApiV1.Handler.Abstract
{
    public interface IFileHandler
    {
        Task<bool> UploadFile(IFormFile file, string pathName = "", string fileName = "");
        Task<string> UploadFreeImageServer(IFormFile file);
    }
}
