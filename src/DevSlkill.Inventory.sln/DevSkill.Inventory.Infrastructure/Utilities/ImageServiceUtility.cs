using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevSkill.Inventory.Infrastructure.Utilities
{
    public class ImageServiceUtility : IImageServiceUtility
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageServiceUtility(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteImage(string? imagePath)
        {
            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath);

            await Task.Run(() =>
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            });
        }

        public async Task<string?> UploadImage(IFormFile? Picture)
        {
            string? imagePath = null;
            if (Picture != null && Picture.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}-{Path.GetFileName(Picture.FileName)}";
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await Picture.CopyToAsync(stream);
                }
                imagePath = Path.Combine("images", fileName);
            }
            return imagePath;
        }
    }
}
