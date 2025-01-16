using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;


namespace DevSkill.Inventory.Application.Services
{
    public interface IMeasurementUnitManagementService
    {
        Task CreateMeasurementUnitAsync(MeasurementUnit measurementUnit);
        Task<(IList<MeasurementUnit> data, int total, int totalDisplay)> GetMeasurementUnitsAsync(int pageIndex, int pageSize, DataTablesSearch search, string? order);
        Task<MeasurementUnit> GetMeasurementUnitAsync(Guid id);
        Task UpdateMeasurementUnitAsync(MeasurementUnit measurementUnit);
        Task DeleteMeasurementUnitAsync(Guid id);
        Task<IList<MeasurementUnit>> GetMeasurementUnitsAsync();
    }
}
