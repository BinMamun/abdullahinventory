using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class MeasurementUnitManagementService : IMeasurementUnitManagementService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public MeasurementUnitManagementService(IInventoryUnitOfWork inventoryUnitOfWork) 
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task<(IList<MeasurementUnit> data, int total, int totalDisplay)> GetMeasurementUnitsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order)
        {
            return 
            await _inventoryUnitOfWork.MeasurementUnitRepository.GetPagedMeasurementUnitsAsync(pageIndex, pageSize, search, order);
        }

        public async Task CreateMeasurementUnitAsync(MeasurementUnit measurementUnit)
        {
            var isDuplicateTitle = _inventoryUnitOfWork.MeasurementUnitRepository.IsTitleDuplicate(measurementUnit.Name);
            if (!isDuplicateTitle)
            {
                await _inventoryUnitOfWork.MeasurementUnitRepository.AddAsync(measurementUnit);
                await _inventoryUnitOfWork.SaveAsync();
            }
        }

        public async Task<MeasurementUnit> GetMeasurementUnitAsync(Guid id)
        {
            return await _inventoryUnitOfWork.MeasurementUnitRepository.GetByIdAsync(id);
        }

        public async Task UpdateMeasurementUnitAsync(MeasurementUnit measurementUnit)
        {
            var isDuplicate = _inventoryUnitOfWork.MeasurementUnitRepository.IsTitleDuplicate(measurementUnit.Name, measurementUnit.Id);
            if (!isDuplicate)
            { 
                await _inventoryUnitOfWork.MeasurementUnitRepository.EditAsync(measurementUnit);
                await _inventoryUnitOfWork.SaveAsync();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public async Task DeleteMeasurementUnitAsync(Guid id)
        {
            await _inventoryUnitOfWork.MeasurementUnitRepository.RemoveAsync(id);
            await _inventoryUnitOfWork.SaveAsync();
        }

        public async Task<IList<MeasurementUnit>> GetMeasurementUnitsAsync()
        {
            return await _inventoryUnitOfWork.MeasurementUnitRepository.GetAllAsync();
        }
    }
}
