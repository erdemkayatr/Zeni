using Zeni.Infra.Logging;

namespace Zeni.Services.Category.Api.Controllers
{
    public class CategoryController : CategoryControllerBase
    {

        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ILogger<CategoryController> logger)
        {
            _logger = logger;
        }

        [HttpPost("getLog")]
        [ServiceLog(true,true)]
        public async Task<Deneme> GetLog(Deneme log)
        {
            log.Deneme1 = "Response";
            return log;
        }
    }

    public class Deneme
    {
        public string Deneme1 { get; set; } = "Erdem";
        public string Deneme2 { get; set; } = "Kaya;";
    }
}
