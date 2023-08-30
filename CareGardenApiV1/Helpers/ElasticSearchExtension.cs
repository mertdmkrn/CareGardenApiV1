using CareGardenApiV1.Model.ResponseModel;
using Nest;

namespace CareGardenApiV1.Helpers
{
    public static class ElasticSearchExtension
    {
        public static void AddElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var baseUrl = configuration["ElasticSettings:baseUrl"];
            var index = configuration["ElasticSettings:defaultIndex"];
            var fingerPrint = configuration["ElasticSettings:fingerPrint"];
            var userName = configuration["ElasticSettings:userName"];
            var password = configuration["ElasticSettings:password"];
            var settings = new ConnectionSettings(new Uri(baseUrl ?? "")).PrettyJson().CertificateFingerprint(fingerPrint).BasicAuthentication(userName, password).DefaultIndex(index);
            settings.EnableApiVersioningHeader();
            AddDefaultMappings(settings);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(client);
            CreateIndex(client, index);
        }
        private static void AddDefaultMappings(ConnectionSettings settings)
        {
            settings.DefaultMappingFor<BusinessDetailModel>(m => m);
        }

        private static void CreateIndex(IElasticClient client, string indexName)
        {
            var createIndexResponse = client.Indices.Create(indexName, index => index.Map<BusinessDetailModel>(x => x.AutoMap()));
        }
    }
}
