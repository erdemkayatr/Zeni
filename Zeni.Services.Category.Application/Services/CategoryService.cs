using Microsoft.Extensions.Logging;

namespace Zeni.Services.Category.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ILogger<CategoryService> logger)
        {
            this._logger = logger;
        }

        public string GetCategory(string categoryName)
        {
            _logger.LogInformation(categoryName);
            return categoryName;
        }
    }
}
