using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Warehouse : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<ItemWarehouse>? ItemWarehouses { get; set; }
    }
}
