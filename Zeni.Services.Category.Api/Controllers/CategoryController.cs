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
            var categoryRepo = _unitOfWork.GetRepository<Categories>();
            var isCat = await categoryRepo.GetPagedListAsync();
            if (isCat.Items.Count < 1)
            {
                var cat = new Categories
                {
                    CategoryName = "ZeniCategory",
                    CategoryDescription = "FistCategory Added"
                };
                await categoryRepo.InsertAsync(cat);
                await _unitOfWork.SaveChangesAsync();
            }
            var catList = categoryRepo.GetAll();

            return "true";
        }
    }

}
