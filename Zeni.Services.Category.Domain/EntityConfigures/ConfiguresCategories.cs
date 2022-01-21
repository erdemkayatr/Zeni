

namespace Zeni.Services.Category.Domain.EntityConfigures
{
    public  class ConfiguresCategories
    {
        public static void ConfigureCategories(EntityTypeBuilder<Categories> builder)
        {
            builder.ToTable("zeni_categories");
            builder.HasKey(p => p.Id);
            builder.Property(p=>p.CategoryName)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(p => p.CategoryDescription)
                .HasMaxLength(1000);

            builder.Property(p => p.CreatedBy)
                .IsRequired();

            builder.Property(p => p.CreatedDate)
                .HasDefaultValue(new DateTime())
                .IsRequired();

        }
    }
}
