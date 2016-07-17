using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class ProductGroupMap : NopEntityTypeConfiguration<ProductGroup>
    {
        public ProductGroupMap()
        {
            this.ToTable("ProductGroup");
            this.HasKey(pt => pt.Id);
            this.Property(pt => pt.Name).IsRequired().HasMaxLength(400);

            this.HasMany(p => p.ProductTags)
                .WithMany(pt => pt.ProductGroups)
                .Map(m => m.ToTable("ProductGroup_ProductTag_Mapping"));
        }
    }
}