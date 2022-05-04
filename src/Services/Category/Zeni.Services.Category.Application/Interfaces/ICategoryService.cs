namespace Zeni.Services.Category.Application.Services
{
    public interface ICategoryService
    {
        Task<string> GetCategory(string categoryName);
    }
}
