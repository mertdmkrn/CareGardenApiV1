using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Authorize]
    [Route("notification")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IBusinessService _businessService;

        public NotificationController(INotificationService notificationService, IUserService userService, IBusinessService businessService)
        {
            _notificationService = notificationService;
            _userService = userService;
            _businessService = businessService;
        }


        /// <summary>
        /// Get Notifications(When BusinessId and UserId are sent null, notifications of the user in the session are received.)
        /// </summary>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "userId": null,
        ///        "businessId": null,
        ///        "page": 0,
        ///        "take": 5
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("get")]
        public async Task<IActionResult> Get([FromBody]NotificationSearchModel notificationSearchModel)
        {
            ResponseModel<NotificationSearchResponseModel> response = new ResponseModel<NotificationSearchResponseModel>();

            var id = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);
            var userRole = HelperMethods.GetClaimInfo(Request, ClaimTypes.Role);

            if (id.IsNullOrEmpty() && !notificationSearchModel.userId.HasValue && !notificationSearchModel.businessId.HasValue)
            {
                response.HasError = true;
                response.Message = Resource.Resource.KullaniciBulunamadi;
                return Ok(response);
            }

            if (!notificationSearchModel.userId.HasValue && !notificationSearchModel.businessId.HasValue || (notificationSearchModel.userId == Guid.Empty && notificationSearchModel.businessId == Guid.Empty))
            {
                if (userRole.Equals("Business"))
                {
                    notificationSearchModel.businessId = id.ToGuid();
                }
                else
                {
                    notificationSearchModel.userId = id.ToGuid();
                }
            }

            var model = await _notificationService.SearchNotificationAsync(notificationSearchModel);

            if (model.notifications.Exists(x => x.type == NotificationType.Business && x.redirectId.HasValue)) 
            { 
                var businesses = await _businessService.GetBusinessListForCache();
                model.notifications = model.notifications
                    .Select(x =>
                    {
                        x.relatedBusiness = x.type == NotificationType.Business && x.redirectId.HasValue ? businesses.FirstOrDefault(b => b.id == x.redirectId.Value) : null;
                        return x;
                    })
                    .ToList();
            }

            response.Data = model;

            return Ok(response);
        }

        /// <summary>
        /// Save Notification (Publish Date is the date the notification will start to appear. If null, it will appear immediately.)
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "userId": "00000000-0000-0000-0000-000000000000",
        ///        "businessId": null,
        ///        "publishDate": "2024-03-29T08:30:00",
        ///        "title": "BİLGİLENDİRME",
        ///        "titleEn": "INFORMATION",
        ///        "description": "CareGarden' a hoşgeldin.",
        ///        "description": "CareGarden' a hoşgeldin.",
        ///        "type": 0,
        ///        "redirectId": null,
        ///        "redirectUrl": null
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] Notification notification)
        {
            ResponseModel<Notification> response = new ResponseModel<Notification>();

            if (notification.title.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("title", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (notification.titleEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("titleEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (notification.description.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("description", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (notification.descriptionEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("descriptionEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!notification.userId.HasValue && !notification.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("userId", Resource.Resource.BuAlaniBosBirakmayiniz));
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }      

            response.Data = await _notificationService.SaveNotificationAsync(notification);
            response.Message = Resource.Resource.KayitBasarili;

            if(notification.userId.HasValue)
            {
                await _userService.UpdateHasNotificationAsync(new List<Guid>() { notification.userId.Value }, true);
            }
            else if(notification.businessId.HasValue)
            {
                await _businessService.UpdateHasNotificationAsync(new List<Guid>() { notification.businessId.Value }, true);
            }

            return Ok(response);
        }


        /// <summary>
        /// Save Notification All Users (Publish Date is the date the notification will start to appear. If null, it will appear immediately.)
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "publishDate": "2024-03-29T08:30:00",
        ///        "title": "BİLGİLENDİRME",
        ///        "titleEn": "INFORMATION",
        ///        "description": "CareGarden' a hoşgeldin."
        ///        "descriptionEn": "Welcome to CareGarden."
        ///        "type": 0
        ///        "redirectId": null,
        ///        "redirectUrl": null
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("saveallusers")]
        public async Task<IActionResult> SaveAllUsers([FromBody] Notification notification)
        {
            ResponseModel<List<Notification>> response = new ResponseModel<List<Notification>>();

            if (notification.title.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("title", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (notification.titleEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("titleEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (notification.description.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("description", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (notification.descriptionEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("descriptionEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            var userIds = await _userService.GetUserIds();

            List<Notification> notifications = new List<Notification>();

            foreach (var userId in userIds)
            {
                notifications.Add(new Notification()
                {
                    userId = userId,
                    description = notification.description,
                    descriptionEn = notification.descriptionEn,
                    type = notification.type,
                    publishDate = notification.publishDate.HasValue ? notification.publishDate : DateTime.Now,
                    redirectId = notification.redirectId,
                    redirectUrl = notification.redirectUrl
                });
            }

            response.Data = await _notificationService.SaveNotificationsAsync(notifications);
            await _userService.UpdateHasNotificationAsync(userIds, true);
            response.Message = Resource.Resource.KayitBasarili;

            return Ok(response);
        }

        /// <summary>
        /// Save Notification All Businesses (Publish Date is the date the notification will start to appear. If null, it will appear immediately.)
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "publishDate": "2024-03-29T08:30:00",
        ///        "title": "BİLGİLENDİRME",
        ///        "titleEn": "INFORMATION",
        ///        "description": "CareGarden' a hoşgeldin."
        ///        "descriptionEn": "Welcome to CareGarden."
        ///        "type": 0
        ///        "redirectId": null,
        ///        "redirectUrl": null
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("saveallbusinesses")]
        public async Task<IActionResult> SaveAllBusinesses([FromBody] Notification notification)
        {
            ResponseModel<List<Notification>> response = new ResponseModel<List<Notification>>();

            if (notification.title.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("title", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (notification.titleEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("titleEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (notification.description.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("description", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (notification.descriptionEn.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("descriptionEn", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            var businessIds = await _businessService.GetBusinessIds();

            List<Notification> notifications = new List<Notification>();

            foreach (var businessId in businessIds)
            {
                notifications.Add(new Notification()
                {
                    businessId = businessId,
                    description = notification.description,
                    descriptionEn = notification.descriptionEn,
                    type = notification.type,
                    publishDate = notification.publishDate.HasValue ? notification.publishDate : DateTime.Now,
                    redirectId = notification.redirectId,
                    redirectUrl = notification.redirectUrl
                });
            }

            response.Data = await _notificationService.SaveNotificationsAsync(notifications);
            await _businessService.UpdateHasNotificationAsync(businessIds, true);
            response.Message = Resource.Resource.KayitBasarili;

            return Ok(response);
        }

        /// <summary>
        /// Notification Set Read ({} boş gönderildiğinde session daki kullanıcının tüm bildirimleri okundu yapar.)
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "ids": [
        ///             "00000000-0000-0000-0000-000000000000",
        ///             "00000000-0000-0000-0000-000000000001"
        ///        ]
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("setread")]
        public async Task<IActionResult> SetRead([FromBody] IdListSearchModel idListSearchModel)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            var id = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid);
            var userRole = HelperMethods.GetClaimInfo(Request, ClaimTypes.Role);

            if (idListSearchModel.ids.IsNullOrEmpty())
            {
                if(id.IsNullOrEmpty())
                {
                    response.HasError = true;
                    response.ValidationErrors.Add(new ValidationError("ids", Resource.Resource.BuAlaniBosBirakmayiniz));
                    response.Message = Resource.Resource.KayitYapilamadi;
                    return Ok(response);
                }
                else
                {
                    if (userRole.Equals("Business"))
                    {
                        response.Data = await _notificationService.UpdateNotificationsReadAsync(null, id.ToGuid());
                        await _businessService.UpdateHasNotificationAsync(new List<Guid>() { id.ToGuid() }, false);
                    }
                    else
                    {
                        response.Data = await _notificationService.UpdateNotificationsReadAsync(id.ToGuid(), null);
                        await _userService.UpdateHasNotificationAsync(new List<Guid>() { id.ToGuid() }, false);
                    }
                }
            }
            else
            {
                response.Data = await _notificationService.UpdateNotificationsReadAsync(idListSearchModel.ids);
            }

            response.Message = Resource.Resource.KayitBasarili;

            return Ok(response);
        }

        /// <summary>
        /// Delete Notifications
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "ids": [
        ///             "00000000-0000-0000-0000-000000000000",
        ///             "00000000-0000-0000-0000-000000000001"
        ///        ]
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody] IdListSearchModel idListSearchModel)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (idListSearchModel.ids.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("ids", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitSilinemedi;
                return Ok(response);
            }

            response.Data = await _notificationService.DeleteNotificationsAsync(idListSearchModel.ids);
            response.Message = Resource.Resource.KayitSilindi;

            return Ok(response);
        }
    }
}
