using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Application.Services
{
    public class DisplayLogService : IDisplayLogService
    {
        private IInventoryUnitOfWork _inventoryUnitOfWork;
        public DisplayLogService(IInventoryUnitOfWork inventoryUnitOfWork)
        {
            _inventoryUnitOfWork = inventoryUnitOfWork;
        }

    }
}
