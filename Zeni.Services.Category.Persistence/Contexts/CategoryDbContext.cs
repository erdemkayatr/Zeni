
namespace Zeni.Services.Category.Persistence.Contexts
{
    public class CategoryDbContext : DbContext
    {
        public CategoryDbContext(DbContextOptions<CategoryDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Categories>(ConfiguresCategories.ConfigureCategories);
        }
    }
}
