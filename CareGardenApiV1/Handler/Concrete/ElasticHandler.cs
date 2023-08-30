using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Nest;
using NetTopologySuite.Index.HPRtree;

namespace CareGardenApiV1.Handler.Concrete
{
    public class ElasticHandler : IElasticHandler
    {
        private IBusinessService _businessService;
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<ElasticHandler> _logger;

        public ElasticHandler(IElasticClient elasticClient, ILogger<ElasticHandler> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
            _businessService = new BusinessService();
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
            var businessDetailClient = await _elasticClient.GetAsync<BusinessDetailModel>(id);

            if (businessDetailClient != null)
            {
                await _elasticClient.UpdateAsync<BusinessDetailModel>(businessDetail.id, u => u
                .Index("businesses")
                .Doc(businessDetail));
            }
            else
            {
                await _elasticClient.IndexDocumentAsync(businessDetail);
            }

            return true;
        }

        public async Task<bool> UpdateOrCreateIndexBusiness(BusinessDetailModel businessDetail)
        {
            var businessDetailClient = await _elasticClient.GetAsync<BusinessDetailModel>(businessDetail.id);

            if (businessDetailClient.IsValid)
            {
                await _elasticClient.UpdateAsync<BusinessDetailModel>(businessDetail.id, u => u
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
            await _elasticClient.DeleteAsync<BusinessDetailModel>(id);
            return true;
        }
    }
}
