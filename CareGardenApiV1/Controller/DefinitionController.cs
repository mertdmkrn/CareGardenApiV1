using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Repository;
using Microsoft.AspNetCore.Mvc;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Model.ResponseModel;
using HtmlAgilityPack;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    public class DefinitionController : ControllerBase
    {

        private readonly IBusinessService _businessService;
        private readonly IBusinessPropertiesService _businessPropertiesService;
        private readonly CareGardenApiDbContext _context;

        public DefinitionController(
            IBusinessService businessService,
            IBusinessPropertiesService businessPropertiesService,
            CareGardenApiDbContext context)
        {
            _businessService = businessService;
            _businessPropertiesService = businessPropertiesService;
            _context = context;
        }

        /// <summary>
        /// Get Cities
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("definition/getcities")]
        public async Task<IActionResult> GetCities()
        {
            ResponseModel<List<string>> response = new ResponseModel<List<string>>();
            response.Data = Constants.Cities;

            return Ok(response);
        }

        /// <summary>
        /// Get Cities
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("definition/starchips")]
        public async Task<IActionResult> GetStarChips()
        {
            ResponseModel<StarChipResponseModel> response = new ResponseModel<StarChipResponseModel>();
            response.Data = new StarChipResponseModel();

            return Ok(response);
        }

        /// <summary>
        /// Get Privacy Policy
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("definition/setbusinesses")]
        public async Task<IActionResult> SetBusinesses()
        {
            List<string> cityList = new List<string>() { "istanbul", "ankara", "izmir", "bursa", "adana", "antalya", "kocaeli", "sakarya", "konya", "kastamonu" };
            List<string> categoryList = new List<string>() { "sac-kesimi", "sac-boyama", "manikur", "spalar", "saglikli-yasam", "dovme-tattoo-merkezleri" };
            string baseUrl = "https://www.kolayrandevu.com/{0}/{1}/{2}";
            List<string> additionalLinks = new List<string>();

            Dictionary<string, List<string>> urlDict = new Dictionary<string, List<string>>();

            var businesses = await _businessService.GetBusinessListForCache(false);

            foreach (string city in cityList)
            {
                foreach (string category in categoryList)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        var url = string.Format(baseUrl, category, city, (i + 1).ToString());
                        var htmlDoc = new HtmlWeb().Load(url);

                        var links = htmlDoc.DocumentNode.SelectNodes("//h3[@class='listing-salon-title']/a");

                        if (links != null)
                        {
                            foreach (var link in links)
                            {
                                string href = link.Attributes["href"].Value;

                                if (!additionalLinks.Contains(href))
                                {
                                    if (urlDict.Count() == 0 || !urlDict.ContainsKey(url))
                                    {
                                        urlDict.Add(url, new List<string>() { href });
                                    }
                                    else
                                    {
                                        urlDict[url].Add(href);
                                    }

                                    additionalLinks.Add(href);
                                }
                            }
                        }
                    }

                }
            }

            List<Business> businessList = new List<Business>();
            var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

            foreach (var kvp in urlDict)
            {
                foreach (var link in kvp.Value)
                {
                    Business business = new Business();

                    var htmlDoc = new HtmlWeb().Load(link);

                    business.name = htmlDoc.DocumentNode.SelectSingleNode("//h1[@itemprop='name']")?.InnerText.Replace("&amp;", " ").Replace("  ", " ").Replace("&#039;", "'");

                    if (business.name.IsNullOrEmpty() || businesses.Any(x => x.name.Equals(business.name))) continue;

                    business.nameForUrl = business.name.GenerateUrlFriendlyName();
                    var telephone = htmlDoc.DocumentNode.SelectSingleNode("//a[@id='mobil-callcenter-number']")?.Attributes["href"]?.Value.Replace("tel://", "") ?? "05000000000";
                    business.telephone = telephone.Contains("0212") || telephone.Contains("0850") ? telephone : $"+9{telephone}";
                    business.description = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"single_tour_desc\"]/div/div[@class=\"col-md-9\"]/p")?.InnerText.Replace("&amp;", " ").Replace("  ", " ").Replace("&#039;", "'");
                    business.descriptionEn = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"single_tour_desc\"]/div/div[@class=\"col-md-9\"]/p")?.InnerText.Replace("&amp;", " ").Replace("  ", " ").Replace("&#039;", "'");
                    business.city = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop='addressRegion']")?.InnerText;
                    business.province = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop='addressLocality']")?.InnerText;
                    business.latitude = Convert.ToDouble(htmlDoc.DocumentNode.SelectSingleNode("//meta[@itemprop='latitude']")?.Attributes["content"]?.Value.Replace(".", ","));
                    business.longitude = Convert.ToDouble(htmlDoc.DocumentNode.SelectSingleNode("//meta[@itemprop='longitude']")?.Attributes["content"]?.Value.Replace(".", ","));

                    if (business.latitude > 0 && business.longitude > 0)
                    {
                        business.location = gf.CreatePoint(new NetTopologySuite.Geometries.Coordinate(business.latitude, business.longitude));
                    }

                    business.logoUrl = htmlDoc.DocumentNode.SelectSingleNode("//img[@itemprop='logo']")?.Attributes["data-src-defer"]?.Value;
                    business.address = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop='streetAddress']")?.InnerText + " " + business.city + " " + business.province;
                    business.isActive = true;
                    business.verified = true;
                    business.createDate = DateTime.Now;
                    business.updateDate = business.createDate;
                    business.workingGenderType = WorkingGenderType.Everyone;
                    business.email = business.name.Split(" ").FirstOrDefault().ToLower().Replace("'", "") + "123@gmail.com";
                    business.officialHolidayAvailable = true;
                    business.appointmentTimeInterval = 30;


                    await _context.Businesses.AddAsync(business);
                    await _context.SaveChangesAsync();

                    var hours = htmlDoc.DocumentNode.SelectNodes("//meta[@itemprop='openingHours']");

                    BusinessWorkingInfo businessWorkingInfo = new BusinessWorkingInfo();

                    if (hours != null && hours.Count() > 0)
                    {
                        businessWorkingInfo.mondayWorkHours = hours[0].Attributes["content"]?.Value?.Substring(3);
                        businessWorkingInfo.tuesdayWorkHours = hours.Count > 1 ? hours[1].Attributes["content"]?.Value?.Substring(3) : null;
                        businessWorkingInfo.wednesdayWorkHours = hours.Count > 2 ? hours[2].Attributes["content"]?.Value?.Substring(3) : null;
                        businessWorkingInfo.thursdayWorkHours = hours.Count > 3 ? hours[3].Attributes["content"]?.Value?.Substring(3) : null;
                        businessWorkingInfo.fridayWorkHours = hours.Count > 4 ? hours[4].Attributes["content"]?.Value?.Substring(3) : null;
                        businessWorkingInfo.saturdayWorkHours = hours.Count > 5 ? hours[5].Attributes["content"]?.Value?.Substring(3) : null;
                        businessWorkingInfo.sundayWorkHours = hours.Count > 6 ? hours[6].Attributes["content"]?.Value?.Substring(3) : null;
                        businessWorkingInfo.businessId = business.id;

                        await _context.BusinessWorkingInfos.AddAsync(businessWorkingInfo);
                        await _context.SaveChangesAsync();
                    }

                    var serviceNameNodes = htmlDoc.DocumentNode.SelectNodes("//tr[@class='hizmet-liste-select click_service']");
                    List<BusinessServiceModel> businessServiceModels = new List<BusinessServiceModel>();

                    if (serviceNameNodes != null && serviceNameNodes.Count() > 0)
                    {

                        for (int i = 0; i < serviceNameNodes.Count; i++)
                        {
                            var node = serviceNameNodes[i];
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(node.OuterHtml);

                            BusinessServiceModel businessServiceModel = new BusinessServiceModel();
                            businessServiceModel.name = doc.DocumentNode.SelectSingleNode("//tr[@class='hizmet-liste-select click_service']//label[@class='hizmet-listesi-label']")?.InnerText;

                            if (businessServiceModel.name != null && !businessServiceModels.Exists(x => x.name == businessServiceModel.name))
                            {
                                businessServiceModel.serviceId = getServiceId(link, businessServiceModel.name);
                                businessServiceModel.nameEn = doc.DocumentNode.SelectSingleNode("//tr[@class='hizmet-liste-select click_service']//label[@class='hizmet-listesi-label']")?.InnerText;
                                businessServiceModel.price = Convert.ToDouble(doc.DocumentNode.SelectSingleNode("//tr[@class='hizmet-liste-select click_service']//span")?.InnerText.Split("~").LastOrDefault().Replace("TL", "").Trim());

                                if (businessServiceModel.price == 0)
                                    businessServiceModel.price = 150;

                                businessServiceModel.isPopular = i % 3 == 0 ? true : false;
                                businessServiceModel.minDuration = i % 2 == 0 ? 30 : 45;
                                businessServiceModel.maxDuration = i % 5 == 0 ? businessServiceModel.minDuration : i % 2 == 0 ? 45 : 60;
                                businessServiceModel.businessId = business.id;
                                businessServiceModel.spot = "Spot";
                                businessServiceModel.spotEn = "Spot EN";

                                businessServiceModels.Add(businessServiceModel);
                            }
                        }

                        await _context.BusinessServices.AddRangeAsync(businessServiceModels);
                    }

                    List<BusinessGallery> businessGalleries = new List<BusinessGallery>();
                    var profilePhotoNode = htmlDoc.DocumentNode.SelectSingleNode("//img[@id='main-image']");

                    if (profilePhotoNode != null)
                    {
                        BusinessGallery photo = new BusinessGallery();

                        photo.imageUrl = profilePhotoNode.Attributes["src"]?.Value;
                        photo.isProfilePhoto = true;
                        photo.sortOrder = 1;
                        photo.businessId = business.id;

                        businessGalleries.Add(photo);
                    }

                    var galleryNodes = htmlDoc.DocumentNode.SelectNodes("//a[@data-lightbox='galeri']");

                    if (galleryNodes != null && galleryNodes.Count() > 0)
                    {
                        for (int i = 1; i < galleryNodes.Count; i++)
                        {
                            var node = galleryNodes[i];
                            BusinessGallery photo = new BusinessGallery();

                            photo.imageUrl = node.Attributes["href"]?.Value;
                            photo.isSliderPhoto = i < 4 ? true : false;
                            photo.sortOrder = i + 1;
                            photo.businessId = business.id;

                            businessGalleries.Add(photo);
                        }
                    }

                    await _context.BusinessGalleries.AddRangeAsync(businessGalleries);

                    var workerNodes = htmlDoc.DocumentNode.SelectNodes("//div[@itemprop='employee']");
                    var serviceIdList = businessServiceModels.Select(x => x.id).ToList();

                    if (workerNodes != null && workerNodes.Count() > 0)
                    {
                        List<Worker> workers = new List<Worker>();

                        for (int i = 0; i < workerNodes.Count; i++)
                        {
                            var node = workerNodes[i];
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(node.OuterHtml);

                            Worker worker = new Worker();
                            worker.name = doc.DocumentNode.SelectSingleNode("//img[@class='personel-image']")?.Attributes["alt"]?.Value;

                            if (worker.name != null && !workers.Exists(x => x.name == worker.name))
                            {
                                worker.title = "Takım Üyesi";
                                worker.path = doc.DocumentNode.SelectSingleNode("//img[@class='personel-image']")?.Attributes["data-src-defer"]?.Value;
                                worker.isActive = true;
                                worker.isAvailable = true;
                                worker.businessId = business.id;
                                worker.serviceIds = string.Join(";", serviceIdList.Skip(i % 2 == 0 ? serviceIdList.Count() / 2 : 0).Take(serviceIdList.Count() / 2));
                                worker.mondayWorkHours = businessWorkingInfo.mondayWorkHours;
                                worker.tuesdayWorkHours = businessWorkingInfo.tuesdayWorkHours;
                                worker.wednesdayWorkHours = businessWorkingInfo.wednesdayWorkHours;
                                worker.thursdayWorkHours = businessWorkingInfo.thursdayWorkHours;
                                worker.fridayWorkHours = businessWorkingInfo.fridayWorkHours;
                                worker.saturdayWorkHours = businessWorkingInfo.saturdayWorkHours;
                                worker.sundayWorkHours = businessWorkingInfo.sundayWorkHours;

                                workers.Add(worker);
                            }
                        }

                        await _context.Workers.AddRangeAsync(workers);
                        await _context.SaveChangesAsync();
                    }

                    var commentNodes = htmlDoc.DocumentNode.SelectNodes("//div[@itemprop='review']");

                    if (commentNodes != null && commentNodes.Count > 0)
                    {
                        List<Comment> comments = new List<Comment>();

                        List<Guid> userIds = new List<Guid>()
                            {
                                new Guid("5fe296fa-117c-428d-a34c-ac92081323e6"),
                                new Guid("4ffb3bb4-8390-4a5a-ace1-048b79e4d0e0"),
                                new Guid("44c09c38-9c15-4692-a4f1-3d16f5b536de"),
                            };

                        for (int i = 0; i < commentNodes.Count; i++)
                        {
                            var node = commentNodes[i];
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(node.OuterHtml);

                            Comment comment = new Comment();
                            comment.comment = doc.DocumentNode.SelectSingleNode("//p[@itemprop='reviewBody']")?.InnerHtml;

                            if (comment.comment != null && !comments.Exists(x => x.comment == comment.comment))
                            {
                                if (comment.comment.Length > 300)
                                    comment.comment = comment.comment.Substring(0, 299);

                                comment.point = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//span[@itemprop='ratingValue']")?.InnerHtml);
                                var dateStr = doc.DocumentNode.SelectSingleNode("//meta[@itemprop='datePublished']")?.Attributes["content"]?.Value;

                                comment.createDate = Convert.ToDateTime(dateStr);
                                comment.updateDate = comment.createDate;
                                comment.userId = userIds[i % 3];
                                comment.businessId = business.id;
                                comment.commentType = CommentType.User;

                                comments.Add(comment);
                            }
                        }

                        await _context.Comments.AddRangeAsync(comments);
                    }

                    await _context.SaveChangesAsync();
                    await _businessPropertiesService.SaveStaticBusinessPropertiesAsync(business.id);

                    Console.WriteLine(business.name + " eklendi.");
                }
            }

            await _businessService.GetBusinessListForCache(false);

            return Ok();
        }


        public static Guid getServiceId(string link, string name)
        {
            if (name.ToLowerInvariant().Contains("boyama"))
            {
                return new Guid("c2343025-15a8-4676-af1f-fb970a309c1b");
            }

            if(name.ToLowerInvariant().Contains("yıkama"))
            {
                return new Guid("430210f7-0fa6-453b-a752-c2fb28d65814");
            }

            if (name.ToLowerInvariant().Contains("kesim") || name.ToLowerInvariant().Contains("tıraş"))
            {
                return new Guid("b4306e08-47e9-4992-950c-f2f8249d878a");
            }

            if (name.ToLowerInvariant().Contains("sakal"))
            {
                return new Guid("9356f8a9-094e-4c7d-afad-90d1345ab752");
            }

            if (name.ToLowerInvariant().Contains("kaş") || name.ToLowerInvariant().Contains("kirpik"))
            {
                return new Guid("a69b3dfa-ba9c-42c3-a09d-42cd28f0bb6a");
            }

            if (name.ToLowerInvariant().Contains("mani"))
            {
                return new Guid("cdbb94ab-3ccb-4e59-aac3-7272a27e880f");
            }

            if (name.ToLowerInvariant().Contains("pedi"))
            {
                return new Guid("4094adc7-93f7-40ea-bdcb-6a7b75539edf");
            }

            if (name.ToLowerInvariant().Contains("oje") || name.ToLowerInvariant().Contains("el"))
            {
                return new Guid("4753fd7f-5e76-4d10-982b-5d2bca1b6f1a");
            }

            if (name.ToLowerInvariant().Contains("cilt") || name.ToLowerInvariant().Contains("dudak") || name.ToLowerInvariant().Contains("bakım"))
            {
                return new Guid("fc959581-f0de-4bda-926f-0555d9bc877f");
            }

            if (name.ToLowerInvariant().Contains("makyaj"))
            {
                return new Guid("015d8548-ae3c-409b-9690-d7db2a70b4e2");
            }

            if (name.ToLowerInvariant().Contains("diş"))
            {
                return new Guid("835f2ea7-ce97-410a-a4b2-240d99a93d38");
            }

            if (name.ToLowerInvariant().Contains("spa") || link.Contains("spalar"))
            {
                return new Guid("29bd5ef1-c9e7-4354-bc54-048ac31e4fca");
            }

            if (name.ToLowerInvariant().Contains("epilas") || name.ToLowerInvariant().Contains("ağda"))
            {
                return new Guid("29bd5ef1-c9e7-4354-bc54-048ac31e4fca");
            }

            return new Guid("42903897-54c4-4f89-a9fd-28d6696fc95b");
        }
    }
}
