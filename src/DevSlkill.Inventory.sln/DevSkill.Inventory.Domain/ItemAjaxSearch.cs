using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain
{
    public struct ItemAjaxSearch
    {
        public string SearchItem { get; set; }
        public Guid WarehouseId { get; set; }
    }
}
