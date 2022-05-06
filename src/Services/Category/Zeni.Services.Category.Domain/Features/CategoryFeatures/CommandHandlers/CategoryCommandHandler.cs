namespace Zeni.Services.Category.Domain.Features.CategoryFeatures.CommandHandlers
{
    public class CategoryCommandHandler : 
        IRequestHandler<NewCategoryCommand, bool>, 
        IRequestHandler<UpdateCategoryCommand, UpdateCategoryCommand>,
        IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// new category model
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(NewCategoryCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Categories>();
            var entity = new Categories
            {
                CategoryDescription = request.CategoryDescription,
                CategoryName = request.CategoryName,
                CreatedBy = Guid.NewGuid(),
                State = StateEnum.NotPublished,
                CreatedDate = DateTime.Now
            };

            await repo.InsertAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        /// <summary>
        /// update category model
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<UpdateCategoryCommand> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Categories>();
            var entity = await repo.FindAsync(request.Id);
            entity.CategoryDescription = request.CategoryDescription;
            entity.CategoryName = request.CategoryName;
            entity.UpdatedBy = Guid.NewGuid();
            entity.UpdatedDate = DateTime.Now;

            repo.Update(entity);
            await _unitOfWork.SaveChangesAsync(true);
            return request;
        }
        /// <summary>
        /// Soft Delete Category
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.GetRepository<Categories>();
            var entity = await repo.FindAsync(request.Id);
            entity.State = StateEnum.Deleted;
            repo.Update(entity);
            await _unitOfWork.SaveChangesAsync(true);
            return true;
        }
    }
}
