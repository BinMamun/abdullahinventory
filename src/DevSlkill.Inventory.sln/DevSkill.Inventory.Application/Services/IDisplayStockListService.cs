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
    public interface IDisplayStockListService
    {
        Task<(IList<StockListDto> data, int total, int totalDisplay)> GetStockListAsync(
            int pageIndex, int pageSize, StockListSearchDto search, string? order);
        Task<IList<ItemWarehouse>> GetTotalStockValueAsync();
    }
}
