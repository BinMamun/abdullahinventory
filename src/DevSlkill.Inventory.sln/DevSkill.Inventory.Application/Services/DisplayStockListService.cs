using DevSkill.Inventory.Domain.Dtos;
using DevSkill.Inventory.Domain.Entities;
using Inventory.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class DisplayStockListService : IDisplayStockListService
    {
        private readonly IInventoryUnitOfWork _inventoryUnitOfWork;
        public DisplayStockListService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

        public async Task<(IList<StockListDto> data, int total, int totalDisplay)> GetStockListAsync(int pageIndex, int pageSize, StockListSearchDto search, string? order)
        {
            return await _inventoryUnitOfWork.GetPagedStockListAsync(pageIndex, pageSize, search, order);
        }

        public async Task<IList<ItemWarehouse>> GetTotalStockValueAsync()
        {
            return (IList<ItemWarehouse>)await _inventoryUnitOfWork
                                            .ItemWarehouseRepository.GetAllAsync();
        }
    }
}
