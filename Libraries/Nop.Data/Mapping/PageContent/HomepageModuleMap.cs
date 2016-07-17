using Nop.Core.Domain.PageContent;

namespace Nop.Data.Mapping.PageContent
{
    public partial class HomepageModuleMap : NopEntityTypeConfiguration<HomepageModule>
    {
        public HomepageModuleMap()
        {
            this.ToTable("HomepageModule");
            this.HasKey(ni => ni.Id);  
        }
    }
}
