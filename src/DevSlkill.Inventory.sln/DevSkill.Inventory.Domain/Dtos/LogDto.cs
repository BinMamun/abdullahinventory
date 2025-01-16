using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Domain.Dtos
{
    public class LogListDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public DateTime EventTime { get; set; }
        public string? Exception { get; set; }
    }
}
