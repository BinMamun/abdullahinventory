using DevSkill.Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventry.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        public Worker(
            ILogger<Worker> logger,
            ApplicationDbContext context,
            IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _serviceProvider = serviceProvider;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string folderPath = Path.GetFullPath(_configuration["ImageFolderPath"], AppContext.BaseDirectory);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var files = Directory.GetFiles(folderPath);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        foreach (var file in files)
                        {
                            var fileName = Path.GetFileName(file);

                            var imagePath = Path.Combine("images", fileName);

                            bool existsInDb = await _context.Items
                                .AnyAsync(x => x.Picture.Equals(imagePath), stoppingToken);

                            if (!existsInDb)
                            {
                                File.Delete(file);
                            }
                            await Task.Delay(1000, stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing images.");
                }

                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
    }
}
