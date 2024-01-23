using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Model;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using CareGardenApiV1.Helpers;
using RestSharp;
using System.Text.Json.Nodes;
using System.Text.Json;

namespace CareGardenApiV1.Handler.Concrete
{
    public class FileHandler : IFileHandler
    {
        public async Task<bool> UploadFile(IFormFile file, string pathName = "", string fileName = "")
        {
            string path = "";

            if (file.Length > 0)
            {
                path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, pathName.IsNullOrEmpty() ? "StaticFiles/UploadedFiles" : "StaticFiles/UploadedFiles\\" + pathName));
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

        public async Task<string> UploadFreeImageServer(IFormFile file)
        {
            var client = new RestClient("https://freeimage.host");
            var request = new RestRequest("api/1/upload");

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                request.AddFileBytes("source", fileBytes, file.FileName);
            }

            request.AddQueryParameter("key", "6d207e02198a847aa98d0a2a901485a5");
            request.AddQueryParameter("format", "json");

            var response = await client.ExecutePostAsync(request);

            var data = JsonSerializer.Deserialize<JsonNode>(response.Content!);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var image = data["image"];

                if (image != null)
                {
                    return (string)image["url"];
                }
            }

            return null;
        }
    }
}
