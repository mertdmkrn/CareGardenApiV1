using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using Nest;

namespace CareGardenApiV1.Handler.Concrete
{
    public class ElasticHandler : IElasticHandler
    {
        private readonly IBusinessService _businessService;
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ElasticHandler> _logger;

        public ElasticHandler(
            IBusinessService businessService,
            IElasticClient elasticClient,
            ILogger<ElasticHandler> logger)
        {
            _businessService = businessService;
            _elasticClient = elasticClient;
            _logger = logger;
        }


        public async Task<bool> MakeIndexBusiness()
        {
            var businessDetails = await _businessService.GetBusinessesAsync();
            businessDetails.ToList().ForEach(async x => { await UpdateOrCreateIndexBusiness(x); });
            return true;
        }

        public async Task<bool> UpdateOrCreateIndexBusiness(Guid id)
        {
            var businessDetail = await _businessService.GetBusinessDetailByIdAsync(id);
            var businessDetailClient = await _elasticClient.GetAsync<BusinessDetailResponseModel>(id);

            if (businessDetailClient != null)
            {
                await _elasticClient.UpdateAsync<BusinessDetailResponseModel>(businessDetail.id, u => u
                .Index("businesses")
                .Doc(businessDetail));
            }
            else
            {
                await _elasticClient.IndexDocumentAsync(businessDetail);
            }

            return true;
        }

        public async Task<bool> UpdateOrCreateIndexBusiness(BusinessDetailResponseModel businessDetail)
        {
            var businessDetailClient = await _elasticClient.GetAsync<BusinessDetailResponseModel>(businessDetail.id);

            if (businessDetailClient.IsValid)
            {
                await _elasticClient.UpdateAsync<BusinessDetailResponseModel>(businessDetail.id, u => u
                .Index("businesses")
                .Doc(businessDetail));
            }
            else
            {
                await _elasticClient.IndexDocumentAsync(businessDetail);
            }

            return true;
        }

        public async Task<bool> DeleteIndexBusiness(Guid id)
        {
            await _elasticClient.DeleteAsync<BusinessDetailResponseModel>(id);
            return true;
        }
    }
}
