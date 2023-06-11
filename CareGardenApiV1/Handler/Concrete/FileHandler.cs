using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Model;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using CareGardenApiV1.Helpers;

namespace CareGardenApiV1.Handler.Concrete
{
    public class FileHandler : IFileHandler
    {
        public async Task<bool> UploadFile(IFormFile file, string pathName = "", string fileName = "")
        {
            string path = "";
            try
            {
                if (file.Length > 0)
                {
                    path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, pathName.IsNullOrEmpty() ? "UploadedFiles" : "UploadedFiles\\" + pathName));
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (var fileStream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
