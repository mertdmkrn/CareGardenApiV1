using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Nest;
using NetTopologySuite.Geometries;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly IBusinessService _businessService;
        private readonly IServicesService _servicesService;
        private readonly IMemoryCache _memoryCache;
        private readonly IElasticClient _elasticClient;

        public SearchController(
            IBusinessService businessService,
            IServicesService servicesService,
            IMemoryCache memoryCache,
            IElasticClient elasticClient)
        {
            _businessService = businessService;
            _servicesService = servicesService;
            _memoryCache = memoryCache;
            _elasticClient = elasticClient;
        }

        /// <summary>
        /// Get Search By Keyword
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("user/searchbusinessbykeyword")]
        public async Task<IActionResult> SearchBusinessByKeyword([FromBody] KeywordSearchModel keywordSearchModel)
        {
            var culture = Request.Headers["Language"].ToString().IsNull("en");
            ResponseModel<KeywordSearchResponseModel> response = new ResponseModel<KeywordSearchResponseModel>();

            if (keywordSearchModel.keyword.IsNullOrEmpty() || keywordSearchModel.keyword.Length < 3)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("keyword", Resource.Resource.BuAlaniBosBirakmayiniz));
                response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
            }

            if (response.HasError)
                return Ok(response);

            KeywordSearchResponseModel keywordSearchResponse = new KeywordSearchResponseModel();

            var services = new List<Services>();

            if (_memoryCache.TryGetValue("services", out object list))
            {
                services = (List<Services>)list;
            }
            else
            {
                services = await _servicesService.GetServicesAsync();
                _memoryCache.Set("services", services, new MemoryCacheEntryOptions
                {
                    Priority = CacheItemPriority.Normal
                });
            }

            keywordSearchResponse.services = services.Where(x => culture.Equals("en")
                ? x.nameEn.ToLower(Resource.Resource.Culture).Contains(keywordSearchModel.keyword.ToLower(Resource.Resource.Culture))
                : x.name.ToLower(Resource.Resource.Culture).Contains(keywordSearchModel.keyword.ToLower(Resource.Resource.Culture)))
                .OrderBy(x => x.sortOrder).ToList();

            var businessDetails = await GetSearchBusinessWithElastic(keywordSearchModel.keyword.ToLower(Resource.Resource.Culture));

            Point? userLocation = null;
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

            if (keywordSearchModel.latitude.HasValue && keywordSearchModel.longitude.HasValue)
            {
                userLocation = gf.CreatePoint(new Coordinate(keywordSearchModel.latitude.Value, keywordSearchModel.longitude.Value));
            }

            keywordSearchResponse.businesses = businessDetails
                                                .Select(x => new BusinessListModel(x, gf, userLocation))
                                                .OrderBy(x => x.distance)
                                                .ThenByDescending(x => x.averageRating)
                                                .ToList();

            keywordSearchResponse.keyword = keywordSearchModel.keyword;

            response.Data = keywordSearchResponse;

            return Ok(response);
        }

        /// <summary>
        /// Get Search
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("user/searchbusiness")]
        public async Task<IActionResult> SearchBusiness([FromBody] BusinessExploreModel BusinessExploreModel)
        {
            ResponseModel<IList<BusinessListModel>> response = new ResponseModel<IList<BusinessListModel>>();

            response.Data = await _businessService.ExploreBusinesses(BusinessExploreModel);
            return Ok(response);
        }

        private async Task<List<BusinessDetailModel>> GetSearchBusinessWithElastic(string keyword)
        {
            var data = new List<BusinessDetailModel>();
            ClusterHealthResponse? elasticHealthResponse = await _elasticClient.Cluster.HealthAsync();

            if (elasticHealthResponse.IsValid)
            {
                var searchResponse = await _elasticClient.SearchAsync<BusinessDetailModel>(s => s
                    .Query(q => q
                        .QueryString(d => d.Query($"*{keyword}*"))
                    )
                    .Size(15)
                );

                data = searchResponse.Documents.ToList();
                return data;
            }

            return data;
        }
    }
}
