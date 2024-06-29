using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Service.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CareGardenApiV1.Model.RequestModel;

namespace CareGardenApiV1.Controller
{
    [ApiController]
    [Route("worker")]
    public class WorkerController : ControllerBase
    {
        private readonly IWorkerService _workerService;
        private readonly IBusinessServicesService _businessServicesService;
        private readonly ICommentService _commentService;

        public WorkerController(IWorkerService workerService, IBusinessServicesService businessServicesService, ICommentService commentService)
        {
            _workerService = workerService;
            _businessServicesService = businessServicesService;
            _commentService = commentService;
        }

        /// <summary>
        /// Get Worker By Id
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "00000000-0000-0000-0000-000000000000"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getbyid")]
        [Authorize]
        public async Task<IActionResult> GetById([FromBody] string? id)
        {
            ResponseModel<Worker> response = new ResponseModel<Worker>();

            if (id.IsNullOrEmpty() || !id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = await _workerService.GetWorkerByIdAsync(id.ToGuid());
            return Ok(response);
        }

        /// <summary>
        /// Get Worker By Business Id
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "00000000-0000-0000-0000-000000000000"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getbybusinessid")]
        [Authorize]
        public async Task<IActionResult> GetByBusinessId([FromBody] string? businessId)
        {
            ResponseModel<List<Worker>> response = new ResponseModel<List<Worker>>();

            if (businessId.IsNullOrEmpty() || !businessId.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = await _workerService.GetWorkersByBusinessIdAsync(businessId.ToGuid());
            return Ok(response);
        }

        /// <summary>
        /// Get Worker By Business Service Id
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "00000000-0000-0000-0000-000000000000"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getbybusinessserviceid")]
        [Authorize]
        public async Task<IActionResult> GetByBusinessServiceId([FromBody] string? businessServiceId)
        {
            ResponseModel<List<AppointmentWorkerModel>> response = new ResponseModel<List<AppointmentWorkerModel>>();

            if (businessServiceId.IsNullOrEmpty() || !businessServiceId.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessServiceId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = await _workerService.GetWorkersByAppointmentSearchModelAsync(new AppointmentSearchModel()
            {
                businessServiceId = businessServiceId.ToGuid()
            });
            return Ok(response);
        }

        /// <summary>
        /// Get Worker By Business Id
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "00000000-0000-0000-0000-000000000000"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("getdetailbyid")]
        public async Task<IActionResult> GetDetailById([FromBody] string? id)
        {
            ResponseModel<WorkerDetailResponseModel> response = new ResponseModel<WorkerDetailResponseModel>();

            if (id.IsNullOrEmpty() || !id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            var data = await _workerService.GetWorkerDetailByIdAsync(id.ToGuid());
            data.serviceInfos = await _businessServicesService.GetWorkerDetailServiceInfoByIdsAsync(data.serviceIds.Select(x => x.ToGuid()).ToList());
            
            var pointList = await _commentService.GetCommentPointListForCache(workerId: id.ToGuid());

            if(!pointList.IsNullOrEmpty())
            {
                data.countRating = pointList.Count;
                data.point = Math.Round(pointList.Average(x => x.point), 1);
            }

            response.Data = data;

            return Ok(response);
        }


        /// <summary>
        /// Save Worker
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "name" : "Mert DEMİRKIRAN",
        ///        "title" : "Hair Stylist",
        ///        "titleEn" : "Hair Stylist",
        ///        "about" : "Lorem ipsum",
        ///        "aboutEn" : "Lorem ipsum",
        ///        "path" : "mert.jpg",
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///        "serviceIds" : "00000000-0000-0000-0000-000000000000;00000000-0000-0000-0000-000000000001",
        ///        "mondayWorkHours" : "09:00-21:00",
        ///        "tuesdayWorkHours" : "09:00-21:00",
        ///        "wednesdayWorkHours" : "09:00-21:00",
        ///        "thursdayWorkHours" : "09:00-21:00",
        ///        "fridayWorkHours" : "09:00-21:00",
        ///        "saturdayWorkHours" : "09:00-13:00",
        ///        "sundayWorkHours" : null,
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("save")]
        [Authorize(Roles = "Business,Admin")]
        public async Task<IActionResult> Save([FromBody] Worker worker)
        {
            ResponseModel<Worker> response = new ResponseModel<Worker>();

            if (worker.name.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (worker.title.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("replyId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!worker.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (worker.serviceIds.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("serviceIds", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            worker.createdUserId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid).ToGuid();

            response.Data = await _workerService.SaveWorkerAsync(worker);
            response.Message = Resource.Resource.KayitBasarili;
            return Ok(response);
        }

        /// <summary>
        /// Update Worker
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "id" : "00000000-0000-0000-0000-000000000000",
        ///        "name" : "Mert DEMİRKIRAN",
        ///        "title" : "Hair Stylist",
        ///        "titleEn" : "Hair Stylist",
        ///        "about" : "Lorem ipsum",
        ///        "aboutEn" : "Lorem ipsum",
        ///        "path" : "mert.jpg",
        ///        "businessId" : "00000000-0000-0000-0000-000000000000",
        ///        "serviceIds" : "00000000-0000-0000-0000-000000000000;00000000-0000-0000-0000-000000000001",
        ///        "mondayWorkHours" : "09:00-21:00",
        ///        "tuesdayWorkHours" : "09:00-21:00",
        ///        "wednesdayWorkHours" : "09:00-21:00",
        ///        "thursdayWorkHours" : "09:00-21:00",
        ///        "fridayWorkHours" : "09:00-21:00",
        ///        "saturdayWorkHours" : "09:00-13:00",
        ///        "sundayWorkHours" : null,
        ///        "isActive" : true
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("update")]
        [Authorize(Roles = "Business,Admin")]
        public async Task<IActionResult> Update([FromBody] Worker updateWorker)
        {
            ResponseModel<Worker> response = new ResponseModel<Worker>();

            if (updateWorker.name.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("name", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateWorker.title.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("replyId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (!updateWorker.businessId.HasValue)
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (updateWorker.serviceIds.IsNullOrEmpty())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("serviceIds", Resource.Resource.BuAlaniBosBirakmayiniz));
            }

            if (response.HasError)
            {
                response.Message = Resource.Resource.KayitYapilamadi;
                return Ok(response);
            }

            Worker worker = await _workerService.GetWorkerByIdAsync(updateWorker.id);

            if (worker == null)
            {
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            worker.name = updateWorker.name.IsNull(worker.name);
            worker.title = updateWorker.title.IsNull(worker.title);
            worker.titleEn = updateWorker.titleEn.IsNull(worker.titleEn);
            worker.about = updateWorker.about.IsNull(worker.about);
            worker.aboutEn = updateWorker.aboutEn.IsNull(worker.aboutEn);
            worker.path = updateWorker.path.IsNull(worker.path);
            worker.businessId = updateWorker.businessId ?? worker.businessId;
            worker.serviceIds = updateWorker.serviceIds.IsNull(worker.serviceIds);
            worker.createdUserId = HelperMethods.GetClaimInfo(Request, ClaimTypes.PrimarySid).ToGuid();
            worker.isActive = updateWorker.isActive;
            worker.mondayWorkHours = updateWorker.mondayWorkHours;
            worker.tuesdayWorkHours = updateWorker.tuesdayWorkHours;
            worker.wednesdayWorkHours = updateWorker.wednesdayWorkHours;
            worker.thursdayWorkHours = updateWorker.thursdayWorkHours;
            worker.fridayWorkHours = updateWorker.fridayWorkHours;
            worker.saturdayWorkHours = updateWorker.saturdayWorkHours;
            worker.sundayWorkHours = updateWorker.sundayWorkHours;

            response.Data = await _workerService.UpdateWorkerAsync(worker);
            response.Message = Resource.Resource.KayitBasarili;
            return Ok(response);
        }

        /// <summary>
        /// Get Worker By Id
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "00000000-0000-0000-0000-000000000000"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("delete")]
        [Authorize(Roles = "Business,Admin")]
        public async Task<IActionResult> Delete([FromBody] string? id)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (id.IsNullOrEmpty() || !id.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("id", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = await _workerService.DeleteWorkerAsync(id.ToGuid());
            response.Message = Resource.Resource.KayitSilindi;

            return Ok(response);
        }

        /// <summary>
        /// Get Worker By Busimess Id
        /// </summary>
        /// <remarks>
        /// **Sample request body:**
        ///
        ///     { 
        ///        "00000000-0000-0000-0000-000000000000"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpPost("deletebybusinessid")]
        [Authorize(Roles = "Business,Admin")]
        public async Task<IActionResult> DeleteByBusinessId([FromBody] string? businessId)
        {
            ResponseModel<bool> response = new ResponseModel<bool>();

            if (businessId.IsNullOrEmpty() || !businessId.IsGuid())
            {
                response.HasError = true;
                response.ValidationErrors.Add(new ValidationError("businessId", Resource.Resource.IdParametreHatasi));
                response.Message = Resource.Resource.KayitBulunamadi;
                return Ok(response);
            }

            response.Data = await _workerService.DeleteWorkersByBusinessIdAsync(businessId.ToGuid());
            return Ok(response);
        }
    }
}
