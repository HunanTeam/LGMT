using Nop.Core.Domain.Catalog;

namespace Nop.Data.Mapping.Catalog
{
    public partial class ProductGroupProductMap : NopEntityTypeConfiguration<ProductGroupProduct>
    {
        public ProductGroupProductMap()
        {
            this.ToTable("dbo.Product_ProductGroup_Mapping");
            this.HasKey(pc => pc.Id);

            this.HasRequired(pc => pc.ProductGroup)
                .WithMany()
                .HasForeignKey(pc => pc.ProductGroupId);

            this.HasRequired(pc => pc.Product)
                .WithMany(p => p.ProductGroupProducts)
                .HasForeignKey(pc => pc.ProductId);
        }
    }
}