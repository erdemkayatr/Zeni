using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Zeni.Infra.Logging;
using Zeni.Services.Category.Application.Services;
using Zeni.Services.Category.Domain.Entities;

namespace Zeni.Services.Category.Api.Controllers
{
    public class CategoryController : CategoryControllerBase
    {

        private readonly ILogger<CategoryController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(ILogger<CategoryController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("getLog")]
        [ServiceLog(true, true)]
        [Authorize]
        public async Task<string> GetLog(string log)
        {
            return "";
        }
    }

}
