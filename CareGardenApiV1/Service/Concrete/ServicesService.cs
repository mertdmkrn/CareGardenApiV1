using CareGardenApiV1.Model.TableModel;
using CareGardenApiV1.Repository.Abstract;
using CareGardenApiV1.Service.Abstract;

namespace CareGardenApiV1.Service.Concrete
{
    public class ServicesService : IServicesService
    {
        private readonly IServicesRepository _servicesRepository;

        public ServicesService(IServicesRepository servicesRepository)
        {
            _servicesRepository = servicesRepository;
        }

        public async Task<List<Services>> GetServicesAsync()
        {
            return await _servicesRepository.GetServicesAsync();
        }

        public async Task<Services> GetServiceByIdAsync(Guid id)
        {
            return await _servicesRepository.GetServiceByIdAsync(id);
        }

        public async Task<Services> GetServiceByNameAsync(string name)
        {
            return await _servicesRepository.GetServiceByNameAsync(name);
        }

        public async Task<Services> GetServiceByNameEnAsync(string nameEn)
        {
            return await _servicesRepository.GetServiceByNameEnAsync(nameEn);
        }

        public async Task<Services> SaveServiceAsync(Services services)
        {
            return await _servicesRepository.SaveServiceAsync(services);
        }

        public async Task<Services> UpdateServiceAsync(Services services)
        {
            return await _servicesRepository.UpdateServiceAsync(services);
        }

        public async Task<bool> DeleteServiceAsync(Services services)
        {
            return await _servicesRepository.DeleteServiceAsync(services);
        }
    }
}
