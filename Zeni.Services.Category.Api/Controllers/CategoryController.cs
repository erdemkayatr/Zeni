using Zeni.Infra.Logging;
using Zeni.Services.Category.Application.Services;

namespace Zeni.Services.Category.Api.Controllers
{
    public class CategoryController : CategoryControllerBase
    {

        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryService _categoryService;
        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpPost("getLog")]
        [ServiceLog(true,true)]
        public async Task<string> GetLog(string log)
        {
            return _categoryService.GetCategory(log);
        }
    }

    //public class Deneme
    //{
    //    public string Deneme1 { get; set; } = "Erdem";
    //    public string Deneme2 { get; set; } = "Kaya;";
    //}
}
