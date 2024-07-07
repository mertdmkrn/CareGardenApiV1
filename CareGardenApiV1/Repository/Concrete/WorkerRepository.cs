using CareGardenApiV1.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using CareGardenApiV1.Model.ResponseModel;
using CareGardenApiV1.Helpers;
using CareGardenApiV1.Model.RequestModel;
using CareGardenApiV1.Model.TableModel;

namespace CareGardenApiV1.Repository.Concrete
{
    public class WorkerRepository : IWorkerRepository
    {
        private readonly CareGardenApiDbContext _context;

        public WorkerRepository(CareGardenApiDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteWorkerAsync(Guid id)
        {
            await _context.Workers
                .Where(x => x.id == id)
                .ExecuteDeleteAsync();

            return true;
        }

        public async Task<bool> DeleteWorkersByBusinessIdAsync(Guid businessId)
        {
            await _context.Workers
                .Where(x => x.businessId == businessId)
                .ExecuteDeleteAsync();

            return true;
        }

        public async Task<Worker> GetWorkerByIdAsync(Guid id)
        {
            return await _context.Workers
                .FindAsync(id);
        }

        public async Task<List<Worker>> GetWorkersByBusinessIdAsync(Guid businessId)
        {
            return await _context.Workers
                .AsNoTracking()
                .Where(x => x.businessId == businessId)
                .ToListAsync();
        }

        public async Task<List<AppointmentWorkerResponseModel>> GetWorkersByAppointmentSearchModelAsync(AppointmentSearchRequestModel searchModel)
        {
            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            return await _context.Workers
                .AsNoTracking()
                .WhereIf(searchModel.isActive.HasValue, x => x.isActive == searchModel.isActive.Value)
                .WhereIf(searchModel.businessServiceId.IsNotNullOrEmpty(), x => x.serviceIds.ToLower().Contains(searchModel.businessServiceId.Value.ToString().ToLower()))
                .WhereIf(searchModel.businessId.IsNotNullOrEmpty(), x => x.businessId == searchModel.businessId)
                .Select(x => new AppointmentWorkerResponseModel
                {
                    id = x.id,
                    name = x.name,
                    path = x.path,
                    title = isTurkish ? x.title : x.titleEn.IsNull(x.title),
                    isActive = x.isActive,
                    serviceIds = x.serviceIds,
                    mondayWorkHours = x.mondayWorkHours,
                    tuesdayWorkHours = x.tuesdayWorkHours,
                    wednesdayWorkHours = x.wednesdayWorkHours,
                    thursdayWorkHours = x.thursdayWorkHours,
                    fridayWorkHours = x.fridayWorkHours,
                    saturdayWorkHours = x.saturdayWorkHours,
                    sundayWorkHours = x.sundayWorkHours
                })
                .ToListAsync();
        }

        public async Task<WorkerDetailResponseModel> GetWorkerDetailByIdAsync(Guid id)
        {
            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            return await _context.Workers
                .AsNoTracking()
                .Where(x => x.id == id)
                .Select(x => new WorkerDetailResponseModel
                {
                    id = x.id,
                    name = x.name,
                    path = x.path,
                    title = isTurkish ? x.title : x.titleEn.IsNull(x.title),
                    about = isTurkish ? x.about : x.aboutEn.IsNull(x.about),
                    serviceIds = x.serviceIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<AppointmentWorkerResponseModel>> GetWorkersByWorkerIdsAsync(List<Guid?> workerIds)
        {
            bool isTurkish = Resource.Resource.Culture.ToString().Equals("tr");

            return await _context.Workers
                .AsNoTracking()
                .Where(x => workerIds.Contains(x.id))
                .Select(x => new AppointmentWorkerResponseModel
                {
                    id = x.id,
                    isActive = x.isActive,
                    mondayWorkHours = x.mondayWorkHours,
                    tuesdayWorkHours = x.tuesdayWorkHours,
                    wednesdayWorkHours = x.wednesdayWorkHours,
                    thursdayWorkHours = x.thursdayWorkHours,
                    fridayWorkHours = x.fridayWorkHours,
                    saturdayWorkHours = x.saturdayWorkHours,
                    sundayWorkHours = x.sundayWorkHours,
                    serviceIds = x.serviceIds
                })
                .ToListAsync();
        }

        public async Task<Worker> SaveWorkerAsync(Worker worker)
        {
            worker.isActive = true;

            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();
            return worker;
        }

        public async Task<Worker> UpdateWorkerAsync(Worker worker)
        {
            _context.Workers.Update(worker);
            await _context.SaveChangesAsync();
            return worker;
        }
    }
}