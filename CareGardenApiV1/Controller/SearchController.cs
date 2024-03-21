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
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    [Route("user")]
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
        [HttpPost("searchbusinessbykeyword")]
        public async Task<IActionResult> SearchBusinessByKeyword([FromBody] KeywordSearchModel keywordSearchModel)
        {
            var culture = Request.Headers["Language"].ToString().IsNull("en");
            ResponseModel<KeywordSearchResponseModel> response = new ResponseModel<KeywordSearchResponseModel>();

            if (keywordSearchModel.keyWord.IsNullOrEmpty() || keywordSearchModel.keyWord.Length < 3)
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

            keywordSearchResponse.services = services
                .Where(x => culture.Equals("en")
                ? x.nameEn.ToLower(Resource.Resource.Culture).Contains(keywordSearchModel.keyWord.ToLower(Resource.Resource.Culture))
                : x.name.ToLower(Resource.Resource.Culture).Contains(keywordSearchModel.keyWord.ToLower(Resource.Resource.Culture)))
                .OrderBy(x => x.sortOrder)
                .ToList();

            //var businessDetails = await GetSearchBusinessWithElastic(keywordSearchModel.keyWord.ToLower(Resource.Resource.Culture));

            Point? userLocation = null;
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

            if (keywordSearchModel.latitude.HasValue && keywordSearchModel.longitude.HasValue
            && keywordSearchModel.latitude > 0 && keywordSearchModel.longitude > 0)
            {
                userLocation = gf.CreatePoint(new Coordinate(keywordSearchModel.latitude.Value, keywordSearchModel.longitude.Value));
            }

            var businessList = await _businessService.GetBusinessListForCache();

            keywordSearchResponse.businesses = businessList
                .Where(x => x.name.ToLower(Resource.Resource.Culture).Contains(keywordSearchModel.keyWord.ToLower(Resource.Resource.Culture)))
                .Select(x =>
                {
                    x.isOpen = HelperMethods.GetBusinessOpen(x.workingInfo, x.officialDayAvailable);
                    x.distance = userLocation != null && x.location != null ? x.location.Distance(gf.CreateGeometry(userLocation)) * Constants.DistanceValue : 0;
                    x.averageRating = Math.Round(x.averageRating, 1);
                    return x;
                })
                .OrderBy(x => x.distance)
                .ThenByDescending(x => x.averageRating)
                .ToList();


            keywordSearchResponse.keyWord = keywordSearchModel.keyWord;

            response.Data = keywordSearchResponse;

            return Ok(response);
        }

        /// <summary>
        /// Get Search
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "latitude": "41.08611",
        ///        "longitude": "29.00717",
        ///        "page": 0,
        ///        "take": 5,
        ///        "isStartPage": true
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("searchbusiness")]
        public async Task<IActionResult> SearchBusiness([FromBody] BusinessExploreModel businessExploreModel)
        {
            ResponseModel<IList<BusinessListModel>> response = new ResponseModel<IList<BusinessListModel>>();


            bool isSendNoLocationInfo = (!businessExploreModel.latitude.HasValue || businessExploreModel.latitude == 0)
                        && (!businessExploreModel.longitude.HasValue || businessExploreModel.longitude == 0)
                        && businessExploreModel.city.IsNullOrEmpty();

            if (businessExploreModel.isStartPage)
            {
                businessExploreModel.workingGenderType = WorkingGenderType.All;

                if (businessExploreModel.latitude.HasValue && businessExploreModel.longitude.HasValue
                && businessExploreModel.latitude > 0 && businessExploreModel.longitude > 0)
                {
                    businessExploreModel.sortByType = SortByType.Nearest;
                }
                else
                {
                    businessExploreModel.city = HelperMethods.GetClaimInfo(Request, ClaimTypes.Locality).IsNull("İstanbul");
                    businessExploreModel.sortByType = SortByType.TopRated;
                }

                response.Data = await _businessService.ExploreBusinesses(businessExploreModel);

                if (response.Data.IsNullOrEmpty() && businessExploreModel.city != "İstanbul")
                {
                    businessExploreModel.city = "İstanbul";
                    response.Data = await _businessService.ExploreBusinesses(businessExploreModel);
                }
            }
            else
            {
                if (isSendNoLocationInfo)
                {
                    businessExploreModel.city = HelperMethods.GetClaimInfo(Request, ClaimTypes.Locality).IsNull("İstanbul");
                }

                response.Data = await _businessService.ExploreBusinesses(businessExploreModel);

                if (isSendNoLocationInfo && response.Data.IsNullOrEmpty() && businessExploreModel.city != "İstanbul")
                {
                    businessExploreModel.city = "İstanbul";
                    response.Data = await _businessService.ExploreBusinesses(businessExploreModel);
                }
            }
            
            return Ok(response);
        }

        /// <summary>
        /// Get Search Location
        /// </summary>
        /// <returns></returns>
        [HttpPost("searchlocation")]
        public async Task<IActionResult> SearchLocation([FromBody] StringSearchModel searchModel)
        {
            ResponseModel<IEnumerable<LocationInfo>> response = new ResponseModel<IEnumerable<LocationInfo>>();

            if (searchModel.keyWord.IsNullOrEmpty() || searchModel.keyWord.Length < 3)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("keyword", Resource.Resource.BuAlaniBosBirakmayiniz));
                response.Message = Resource.Resource.BuAlaniBosBirakmayiniz;
            }

            if (response.HasError)
                return Ok(response);

            var locationList = Constants.LocationInfos.Where(x => x.baseName.ToLower(Resource.Resource.Culture).StartsWith(searchModel.keyWord.ToLower(Resource.Resource.Culture))).ToList();
            locationList.AddRange(Constants.LocationInfos.Where(x => x.name.ToLower(Resource.Resource.Culture).StartsWith(searchModel.keyWord.ToLower(Resource.Resource.Culture))));

            response.Data = locationList;

            return Ok(response);
        }

        /// <summary>
        /// Get Locations
        /// </summary>
        /// <returns></returns>
        [HttpPost("getlocations")]
        public async Task<IActionResult> GetLocations()
        {
            ResponseModel<List<LocationInfo>> response = new ResponseModel<List<LocationInfo>>();
            response.Data = Constants.LocationInfos;

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
