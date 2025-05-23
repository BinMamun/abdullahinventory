﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Entities
{
    public class Tax : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Parcentage { get; set; }
    }
}
