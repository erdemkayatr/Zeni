namespace Zeni.Services.Category.Domain.Features.CategoryFeatures.Queries
{
    public class GetAllCategoryQuery : Command<IEnumerable<GetAllCategoryQuery>>
    {
        public GetAllCategoryQuery(Guid id, string categoryName, string categoryDescription, StateEnum stateEnum)
        {
            Id = id;
            CategoryName = categoryName;
            CategoryDescription = categoryDescription;
            StateEnum = stateEnum;
        }

        public GetAllCategoryQuery()
        {
            
        }

        public Guid Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public StateEnum StateEnum { get; set; }
        
    }
}
