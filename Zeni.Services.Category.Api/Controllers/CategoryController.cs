namespace Zeni.Services.Category.Api.Controllers
{
    public class CategoryController : CategoryControllerBase
    {

        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        [HttpGet("getLog")]
        public bool GetLog()
        {
            _logger.LogError("Trying log");
            return true;
        }
    }
}
