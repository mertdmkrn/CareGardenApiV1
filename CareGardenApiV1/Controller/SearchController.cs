using CareGardenApiV1.Handler.Abstract;
using CareGardenApiV1.Handler.Concrete;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Service.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Nest;
using NetTopologySuite.Geometries;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private IBusinessService _businessService;
        private IBusinessWorkingInfoService _businessWorkingInfoService;
        private IBusinessGalleryService _businessGalleryService;
        private IServicesService _servicesService;
        private IMemoryCache _memoryCache;
        private IFileHandler _fileHandler;
        private readonly ILoggerHandler _loggerHandler;
        private readonly IElasticClient _elasticClient;

        public SearchController(ILoggerHandler loggerHandler, IMemoryCache memoryCache, IElasticClient elasticClient)
        {
            _businessService = new BusinessService();
            _businessGalleryService = new BusinessGalleryService();
            _businessWorkingInfoService = new BusinessWorkingInfoService();
            _servicesService = new ServicesService();
            _fileHandler = new FileHandler();
            _loggerHandler = loggerHandler;
            _memoryCache = memoryCache;
            _elasticClient = elasticClient;
        }

        /// <summary>
        /// Get Search By Keyword
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("user/getsearchbykeyword")]
        public async Task<IActionResult> GetUserById([FromBody] KeywordSearchModel keywordSearchModel)
        {
            var culture = Request.Headers["Language"].ToString().IsNull("en");
            ResponseModel<KeywordSearchResponseModel> response = new ResponseModel<KeywordSearchResponseModel>();
            Resource.Resource.Culture = new System.Globalization.CultureInfo(culture);

            try
            {
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

                keywordSearchResponse.services =  services.Where(x => culture.Equals("en") 
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
                                                    .OrderBy(x=> x.distance)
                                                    .ThenByDescending(x=>x.averageRating)
                                                    .ToList();

                keywordSearchResponse.keyword = keywordSearchModel.keyword;

                response.Data = keywordSearchResponse;

                return Ok(response);
            }
            catch (Exception ex)
            {
                _loggerHandler.LogMessage(ex);
                response.HasError = true;
                response.Message = "Exception => " + ex.Message;
                return Ok(response);
            }

        }

        private async Task<List<BusinessDetailModel>> GetSearchBusinessWithElastic(string keyword)
        {
            //Elastic available control
            var result =  await _elasticClient.SearchAsync<BusinessDetailModel>(s => s.Query(q => q.QueryString(d => d.Query('*' + keyword + '*'))).Size(15));
            var finalContent = result.Documents.ToList();
            return finalContent;
        }

    }
}
