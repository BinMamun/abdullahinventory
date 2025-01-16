using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Utilities
{
    public interface IImageServiceUtility
    {
        Task<string?> UploadImage(IFormFile? Picture);
        Task DeleteImage(string? imagePath);
    }
}
