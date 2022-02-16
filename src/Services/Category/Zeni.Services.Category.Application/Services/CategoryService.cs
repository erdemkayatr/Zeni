using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.Extensions.Logging;
using Zeni.Services.Category.Domain.Entities;

namespace Zeni.Services.Category.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(ILogger<CategoryService> logger, IUnitOfWork unitOfWork)
        {
            this._logger = logger;
            _unitOfWork=unitOfWork;
        }

        public async Task<string> GetCategory(string categoryName)
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
            var catList = categoryRepo.GetPagedListAsync();
            return catList.ToString();
        }
    }
}
