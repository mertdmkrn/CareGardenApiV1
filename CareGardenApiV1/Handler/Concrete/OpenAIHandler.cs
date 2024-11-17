using CareGardenApiV1.Handler.Abstract;
using System.Text.Json;
using System.Text;

namespace CareGardenApiV1.Handler.Concrete
{
    public class OpenAIHandler : IOpenAIHandler
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> SendOpenAIRequestAsync(string imageUrl)
        {
            var apiKey = "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";
            var apiUrl = "https://api.openai.com/v1/chat/completions";
            var isTurkish = Resource.Resource.Culture.ToString().Contains("tr");

            var prompt = isTurkish
                ? "Bir fotoğrafı analiz eden bir yapay zeka modelisin. Cilt tipini analiz edip, kullanıcının ihtiyaçlarına uygun ürünler öneriyorsun. Ürünlerin adı, kısa açıklaması ve fiyat bilgilerini TRY veriyorsun."
                : "Bir fotoğrafı analiz eden bir yapay zeka modelisin. Cilt tipini analiz edip, kullanıcının ihtiyaçlarına uygun ürünler öneriyorsun. Ürünlerin adı, kısa açıklaması ve fiyat bilgilerini TRY veriyorsun.";

            var jsonContent = $@"
            {{
              ""model"": ""gpt-4-turbo-2024-04-09"",
              ""messages"": [
                {{
                  ""role"": ""system"",
                  ""content"": ""{prompt}""
                }},
                {{
                  ""role"": ""user"",
                  ""content"": [
                    {{
                      ""type"": ""text"",
                      ""text"": ""Analyze the image""
                    }},
                    {{
                      ""type"": ""image_url"",
                      ""image_url"": {{
                        ""url"": ""{imageUrl}""
                      }}
                    }}
                  ]
                }}
              ],
              ""max_tokens"": 300
            }}";

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                var jsonDoc = JsonDocument.Parse(responseBody);
                var message = jsonDoc.RootElement
                                    .GetProperty("choices")[0]
                                    .GetProperty("message")
                                    .GetProperty("content")
                                    .GetString();

                return message;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }
    }
}
