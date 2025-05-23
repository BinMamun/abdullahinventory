﻿using DevSkill.Inventory.Domain.Entities;
using DevSkill.Inventory.Domain.RepositoryContracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Repositories
{
    public class ItemWarehouseRepository : CompositRepository<ItemWarehouse, Guid>, IItemWarehouseRepository
    {
        public ItemWarehouseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
