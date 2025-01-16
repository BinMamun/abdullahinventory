using DevSkill.Inventory.Domain;
using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Inventory.Domain;

namespace DevSkill.Inventory.Application
{
    public interface IInventoryUnitOfWork : IUnitOfWork
    {
        public ICategoryRepository CategoryRepository { get; }
        public IMeasurementUnitRepository MeasurementUnitRepository { get; }
        public ITaxRepository TaxRepository { get; }
        public IServiceRepository ServiceRepository { get; }
        public IItemRepository ItemRepository { get; }
        public IWarehouseRepository WarehouseRepository { get; }
        public IItemWarehouseRepository ItemWarehouseRepository { get; }
        public IStockTransferRepository StockTransferRepository { get; }
        public IStockTransferItemRepository StockTransferItemRepository { get; }
        public IStockAdjustmentReasonRepository StockAdjustmentReasonRepository { get; }
        public IStockAdjustmentItemRepository StockAdjustmentItemRepository { get; }
        public IStockAdjustmentRepository StockAdjustmentRepository { get; }

        Task<(IList<StockListDto> data, int total, int totalDisplay)> GetPagedStockListAsync(
            int pageIndex, int pageSize, StockListSearchDto search, string? order);
    }
}
