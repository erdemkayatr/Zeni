namespace Zeni.Services.Category.Domain.Features.CategoryFeatures.Commands
{
    public class UpdateCategoryCommand : Command<UpdateCategoryCommand>
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public StateEnum State { get; set; }
        public UpdateCategoryCommand(Guid id, string categoryName, string categoryDescription, StateEnum state)
        {
            Id = id;
            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
            State = state;
        }


    }
}