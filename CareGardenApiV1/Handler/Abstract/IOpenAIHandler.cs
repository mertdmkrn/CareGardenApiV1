using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Handler.Abstract
{
    public interface IOpenAIHandler
    {
        Task<string> SendOpenAIRequestAsync(string url);
    }
}
