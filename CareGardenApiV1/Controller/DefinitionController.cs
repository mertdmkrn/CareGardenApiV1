using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Repository;
using Microsoft.AspNetCore.Mvc;
using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Service.Abstract;
using CareGardenApiV1.Model.ResponseModel;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    public class DefinitionController : ControllerBase
    {
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

        ///// <summary>
        ///// Get Privacy Policy
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("definition/setbusinesses")]
        //public async Task<IActionResult> SetBusinesses()
        //{
        //    List<string> cityList = new List<string>() { "istanbul", "ankara", "izmir", "bursa", "adana", "antalya", "kocaeli", "sakarya", "konya", "kastamonu" };
        //    List<string> categoryList = new List<string>() { "sac-kesimi", "sac-boyama", "manikur", "spalar", "saglikli-yasam", "dovme-tattoo-merkezleri" };
        //    string baseUrl = "https://www.kolayrandevu.com/{0}/{1}/{2}";
        //    List<string> additionalLinks = new List<string>();

        //    Dictionary<string, List<string>> urlDict = new Dictionary<string, List<string>>();

        //    var businesses = await _businessService.GetBusinessListForCache();

        //    foreach (string city in cityList)
        //    {
        //        foreach (string category in categoryList)
        //        {
        //            for (int i = 0; i < 5; i++)
        //            {
        //                var url = string.Format(baseUrl, category, city, (i + 1).ToString());
        //                var htmlDoc = new HtmlWeb().Load(url);

        //                var links = htmlDoc.DocumentNode.SelectNodes("//h3[@class='listing-salon-title']/a");

        //                if (links != null)
        //                {
        //                    foreach (var link in links)
        //                    {
        //                        string href = link.Attributes["href"].Value;

        //                        if (!additionalLinks.Contains(href))
        //                        {
        //                            if (urlDict.Count() == 0 || !urlDict.ContainsKey(url))
        //                            {
        //                                urlDict.Add(url, new List<string>() { href });
        //                            }
        //                            else
        //                            {
        //                                urlDict[url].Add(href);
        //                            }

        //                            additionalLinks.Add(href);
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //    }

        //    List<Business> businessList = new List<Business>();

        //    foreach (var kvp in urlDict)
        //    {
        //        foreach (var link in kvp.Value)
        //        {
        //            Business business = new Business();

        //            var htmlDoc = new HtmlWeb().Load(link);

        //            business.name = htmlDoc.DocumentNode.SelectSingleNode("//h1[@itemprop='name']")?.InnerText.Replace(" &amp; ", " ");

        //            if (business.name.IsNullOrEmpty() || businesses.Any(x => x.name.Equals(business.name))) continue;

        //            business.name = htmlDoc.DocumentNode.SelectSingleNode("//h1[@itemprop='name']")?.InnerText.Replace("&amp;", " ").Replace("  ", " ");
        //            business.telephone = htmlDoc.DocumentNode.SelectSingleNode("//a[@id='mobil-callcenter-number']")?.Attributes["href"]?.Value.Replace("tel://", "") ?? "+905467335939";
        //            business.description = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"single_tour_desc\"]/div/div[@class=\"col-md-9\"]/p")?.InnerText.Replace("&amp;", " ").Replace("  ", " ");
        //            business.descriptionEn = htmlDoc.DocumentNode.SelectSingleNode("//*[@id=\"single_tour_desc\"]/div/div[@class=\"col-md-9\"]/p")?.InnerText;
        //            business.city = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop='addressRegion']")?.InnerText;
        //            business.province = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop='addressLocality']")?.InnerText;
        //            business.latitude = Convert.ToDouble(htmlDoc.DocumentNode.SelectSingleNode("//meta[@itemprop='latitude']")?.Attributes["content"]?.Value.Replace(".", ","));
        //            business.longitude = Convert.ToDouble(htmlDoc.DocumentNode.SelectSingleNode("//meta[@itemprop='longitude']")?.Attributes["content"]?.Value.Replace(".", ","));
        //            business.logoUrl = htmlDoc.DocumentNode.SelectSingleNode("//img[@itemprop='logo']")?.Attributes["data-src-defer"]?.Value;
        //            business.address = htmlDoc.DocumentNode.SelectSingleNode("//span[@itemprop='streetAddress']")?.InnerText + " " + business.city + " " + business.province; 
        //            business.isActive = true;
        //            business.verified = true;
        //            business.createDate = DateTime.Now;
        //            business.updateDate = business.createDate;
        //            business.workingGenderType = WorkingGenderType.Everyone;
        //            business.email = business.name.Split(" ").FirstOrDefault().ToLower() + "123@gmail.com";
        //            business.password = "12345678";
        //            business.officialHolidayAvailable = true;

        //            await _context.Businesses.AddAsync(business);
        //            await _context.SaveChangesAsync();

        //            var hours = htmlDoc.DocumentNode.SelectNodes("//meta[@itemprop='openingHours']");

        //            if (hours.Count() > 0)
        //            {
        //                BusinessWorkingInfo businessWorkingInfo = new BusinessWorkingInfo();

        //                businessWorkingInfo.mondayWorkHours = hours[0].Attributes["content"]?.Value?.Substring(3);
        //                businessWorkingInfo.tuesdayWorkHours = hours.Count > 1 ? hours[1].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.wednesdayWorkHours = hours.Count > 2 ? hours[2].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.thursdayWorkHours = hours.Count > 3 ? hours[3].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.fridayWorkHours = hours.Count > 4 ? hours[4].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.saturdayWorkHours = hours.Count > 5 ? hours[5].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.sundayWorkHours = hours.Count > 6 ? hours[6].Attributes["content"]?.Value?.Substring(3) : null;
        //                businessWorkingInfo.businessId = business.id;

        //                await _context.BusinessWorkingInfos.AddAsync(businessWorkingInfo);
        //                await _context.SaveChangesAsync();
        //            }

        //            var serviceNameNodes = htmlDoc.DocumentNode.SelectNodes("//tr[@class='favhizmet-liste-select click_service']");
        //            List<BusinessServiceModel> businessServiceModels = new List<BusinessServiceModel>();

        //            if (serviceNameNodes != null && serviceNameNodes.Count() > 0)
        //            {

        //                for (int i = 0; i < serviceNameNodes.Count; i++)
        //                {
        //                    var node = serviceNameNodes[i];
        //                    HtmlDocument doc = new HtmlDocument();
        //                    doc.LoadHtml(node.OuterHtml);

        //                    BusinessServiceModel businessServiceModel = new BusinessServiceModel();
        //                    businessServiceModel.name = doc.DocumentNode.SelectSingleNode("//tr[@class='favhizmet-liste-select click_service']//label[@class='hizmet-listesi-label']")?.InnerText;

        //                    if (businessServiceModel.name != null && !businessServiceModels.Exists(x => x.name == businessServiceModel.name))
        //                    {
        //                        businessServiceModel.serviceId = getServiceId(link, businessServiceModel.name);
        //                        businessServiceModel.nameEn = doc.DocumentNode.SelectSingleNode("//tr[@class='favhizmet-liste-select click_service']//label[@class='hizmet-listesi-label']")?.InnerText;
        //                        businessServiceModel.price = Convert.ToDouble(doc.DocumentNode.SelectSingleNode("//tr[@class='favhizmet-liste-select click_service']//span")?.InnerText.Split("~").LastOrDefault().Replace("TL", "").Trim());

        //                        if (businessServiceModel.price == 0)
        //                            businessServiceModel.price = 150;

        //                        businessServiceModel.isPopular = i % 3 == 0 ? true : false;
        //                        businessServiceModel.minDuration = i % 2 == 0 ? 30 : 45;
        //                        businessServiceModel.maxDuration = i % 5 == 0 ? 0 : i % 2 == 0 ? 45 : 60;
        //                        businessServiceModel.businessId = business.id;
        //                        businessServiceModel.spot = "Spot";
        //                        businessServiceModel.spotEn = "Spot EN";

        //                        businessServiceModels.Add(businessServiceModel);
        //                    }
        //                }

        //                await _context.BusinessServices.AddRangeAsync(businessServiceModels);
        //                await _context.SaveChangesAsync();
        //            }

        //            List<BusinessGallery> businessGalleries = new List<BusinessGallery>();
        //            var profilePhotoNode = htmlDoc.DocumentNode.SelectSingleNode("//img[@id='main-image']");

        //            if (profilePhotoNode != null)
        //            {
        //                BusinessGallery photo = new BusinessGallery();

        //                photo.imageUrl = profilePhotoNode.Attributes["src"]?.Value;
        //                photo.isProfilePhoto = true;
        //                photo.sortOrder = 1;
        //                photo.businessId = business.id;

        //                businessGalleries.Add(photo);
        //            }

        //            var galleryNodes = htmlDoc.DocumentNode.SelectNodes("//a[@data-lightbox='galeri']");

        //            if (galleryNodes != null && galleryNodes.Count() > 0)
        //            {
        //                for (int i = 1; i < galleryNodes.Count; i++)
        //                {
        //                    var node = galleryNodes[i];
        //                    BusinessGallery photo = new BusinessGallery();

        //                    photo.imageUrl = node.Attributes["href"]?.Value;
        //                    photo.isSliderPhoto = i < 4 ? true : false;
        //                    photo.sortOrder = i + 1;
        //                    photo.businessId = business.id;

        //                    businessGalleries.Add(photo);
        //                }
        //            }

        //            await _context.BusinessGalleries.AddRangeAsync(businessGalleries);
        //            await _context.SaveChangesAsync();

        //            var workerNodes = htmlDoc.DocumentNode.SelectNodes("//div[@itemprop='employee']");
        //            var serviceIdList = businessServiceModels.Select(x => x.id).ToList();

        //            if (workerNodes != null && workerNodes.Count() > 0)
        //            {
        //                List<Worker> workers = new List<Worker>();

        //                for (int i = 0; i < workerNodes.Count; i++)
        //                {
        //                    var node = workerNodes[i];
        //                    HtmlDocument doc = new HtmlDocument();
        //                    doc.LoadHtml(node.OuterHtml);

        //                    Worker worker = new Worker();
        //                    worker.name = doc.DocumentNode.SelectSingleNode("//img[@class='personel-image']")?.Attributes["alt"]?.Value;

        //                    if (worker.name != null && !workers.Exists(x => x.name == worker.name))
        //                    {
        //                        worker.title = "Takım Üyesi";
        //                        worker.path = doc.DocumentNode.SelectSingleNode("//img[@class='personel-image']")?.Attributes["data-src-defer"]?.Value;
        //                        worker.isActive = true;
        //                        worker.isAvailable = true;
        //                        worker.businessId = business.id;
        //                        worker.serviceIds = string.Join(";", serviceIdList.Skip(i % 2 == 0 ? serviceIdList.Count() / 2 : 0).Take(serviceIdList.Count() / 2));

        //                        workers.Add(worker);
        //                    }
        //                }

        //                await _context.Workers.AddRangeAsync(workers);
        //                await _context.SaveChangesAsync();
        //            }

        //            var commentNodes = htmlDoc.DocumentNode.SelectNodes("//div[@itemprop='review']");

        //            if (commentNodes != null && commentNodes.Count > 0)
        //            {
        //                List<Comment> comments = new List<Comment>();

        //                List<Guid> userIds = new List<Guid>()
        //                {
        //                    new Guid("304d4e72-e439-43b0-9025-aab46041aabd"),
        //                    new Guid("815c9c66-6117-4e11-92e0-40d1f640261e"),
        //                    new Guid("0b3e3b42-971d-4caa-8bad-309d540212fc"),
        //                    new Guid("987c12fd-b14b-47e2-815b-3cf335f02d78"),
        //                    new Guid("9d807e31-483b-48ce-aea5-a2106d9df526"),
        //                    new Guid("f3530132-c501-4f51-8acf-e0e716f5f95b"),
        //                    new Guid("72e5ce1e-7ad5-423b-af1b-7d9ef4a25a54"),
        //                };

        //                for (int i = 0; i < commentNodes.Count; i++)
        //                {
        //                    var node = commentNodes[i];
        //                    HtmlDocument doc = new HtmlDocument();
        //                    doc.LoadHtml(node.OuterHtml);

        //                    Comment comment = new Comment();
        //                    comment.comment = doc.DocumentNode.SelectSingleNode("//p[@itemprop='reviewBody']")?.InnerHtml;

        //                    if (comment.comment != null && !comments.Exists(x => x.comment == comment.comment))
        //                    {
        //                        if (comment.comment.Length > 300)
        //                            comment.comment = comment.comment.Substring(0, 299);

        //                        comment.point = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//span[@itemprop='ratingValue']")?.InnerHtml);
        //                        var dateStr = doc.DocumentNode.SelectSingleNode("//meta[@itemprop='datePublished']")?.Attributes["content"]?.Value;

        //                        comment.createDate = Convert.ToDateTime(dateStr);
        //                        comment.updateDate = comment.createDate;
        //                        comment.userId = userIds[i % 6];
        //                        comment.businessId = business.id;
        //                        comment.commentType = CommentType.User;

        //                        comments.Add(comment);
        //                    }
        //                }

        //                await _context.Comments.AddRangeAsync(comments);
        //                await _context.SaveChangesAsync();
        //            }

        //            await _businessPropertiesService.SaveStaticBusinessPropertiesAsync(business.id);

        //            Console.WriteLine(business.name + " eklendi.");
        //        }
        //    }

        //    await _businessService.GetBusinessListForCache(false);

        //    return Ok();
        //}


        //public static Guid getServiceId(string link, string name)
        //{
        //    if (name.ToLower().Contains("boyama"))
        //    {
        //        return new Guid("d10a8836-7011-4017-b456-7aab046ae915");
        //    }

        //    if (name.ToLower().Contains("kesim"))
        //    {
        //        return new Guid("f5b4a17b-2fb8-4b66-8aff-16ed4e77053d");
        //    }

        //    if (name.ToLower().Contains("sakal"))
        //    {
        //        return new Guid("fa88ef64-0fbd-45a5-97e2-26c4f2da8339");
        //    }

        //    if (name.ToLower().Contains("kaş") || name.ToLower().Contains("kirpik"))
        //    {
        //        return new Guid("059b3bce-fdec-48e7-8200-3a101988c014");
        //    }

        //    if (name.ToLower().Contains("mani"))
        //    {
        //        return new Guid("27bcb7ae-9038-440e-8bf8-edeafdadc20f");
        //    }

        //    if (name.ToLower().Contains("pedi"))
        //    {
        //        return new Guid("520e8c0e-5911-43e1-bfbb-f9b2f0023ede");
        //    }

        //    if (name.ToLower().Contains("oje"))
        //    {
        //        return new Guid("3016aa94-a5cf-46a4-804a-cb71c37473eb");
        //    }

        //    if (name.ToLower().Contains("cilt") || name.ToLower().Contains("dudak"))
        //    {
        //        return new Guid("dc54d9c6-cfba-4c45-8fef-872325c00581");
        //    }

        //    if (name.ToLower().Contains("makyaj"))
        //    {
        //        return new Guid("e911185f-d392-4d2c-83f2-d8de1efdbf35");
        //    }

        //    if (name.ToLower().Contains("diş"))
        //    {
        //        return new Guid("a1fb5b25-f34b-43ee-be9c-b1a808a7491c");
        //    }

        //    if (name.ToLower().Contains("spa") || link.Contains("spalar"))
        //    {
        //        return new Guid("7422f809-e063-41a0-a411-a2ae956c3df1");
        //    }

        //    if (name.ToLower().Contains("epilas") || name.ToLower().Contains("ada"))
        //    {
        //        return new Guid("24f962c1-64ef-43f9-bbf5-323d2eda99a1");
        //    }

        //    return new Guid("f5b4a17b-2fb8-4b66-8aff-16ed4e77053d");
        //}
    }
}
