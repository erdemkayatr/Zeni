namespace Zeni.Services.Category.Domain.Features.CategoryFeatures.CommandHandlers
{
    public class CategoryQueryHandlers : IRequestHandler<GetAllCategoryQuery,IEnumerable<GetAllCategoryQuery>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryQueryHandlers(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetAllCategoryQuery>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            //TODO: Redis Will Added
            var repo = _unitOfWork.GetRepository<Categories>();
            var list = await repo.GetPagedListAsync(cancellationToken: cancellationToken,disableTracking:true);

            //TODO: AutoMapper Will Added
            return list.Items.Select(x=> new GetAllCategoryQuery
            {
                Id = x.Id,
                CategoryDescription = x.CategoryDescription,
                CategoryName = x.CategoryName,
                StateEnum = x.State
            });
        }
    }
}
