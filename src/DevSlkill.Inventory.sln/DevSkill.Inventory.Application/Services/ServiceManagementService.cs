using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class ServiceManagementService : IServiceManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public ServiceManagementService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task CreateServiceAsync(Service service)
        {
            var isTitleDupliacate = _inventoryUnitOfWork.ServiceRepository.IsTitleDuplicate(service.ServiceName);

            if (!isTitleDupliacate)
            {
                await _inventoryUnitOfWork.ServiceRepository.AddAsync(service);
                await _inventoryUnitOfWork.SaveAsync();
            }
        }

        public async Task<(IList<Service> data, int total, int totalDisplay)> GetServicesAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return await _inventoryUnitOfWork.ServiceRepository.GetServiceList(pageIndex, pageSize, search, order);
        }

        public async Task<Service> GetServiceAsync(Guid serviceId)
        {
            return await _inventoryUnitOfWork.ServiceRepository.GetServiceAsync(serviceId);
        }

        public async Task DeleteServiceAsync(Guid id)
        {
            await _inventoryUnitOfWork.ServiceRepository.RemoveAsync(id);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task UpdateServiceAsync(Service service)
        {
            var isTitleDuplicate = _inventoryUnitOfWork.ServiceRepository.IsTitleDuplicate(service.ServiceName, service.Id);
            if (!isTitleDuplicate)
            {
                await _inventoryUnitOfWork.ServiceRepository.EditAsync(service);
                await _inventoryUnitOfWork.SaveAsync();
            }
        }

        public async Task<double> GetServicesCountAsync()
        {
            return await _inventoryUnitOfWork.ServiceRepository.GetCountAsync();
        }
    }
}
