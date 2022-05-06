namespace Zeni.Services.Category.Domain.Features.CategoryFeatures.Commands
{
    public class NewCategoryCommand : Command<bool>
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public StateEnum State { get; set; }
        public NewCategoryCommand(string categoryName, string categoryDescription, StateEnum state)
        {
            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
            State = state;
        }

        
    }
}
