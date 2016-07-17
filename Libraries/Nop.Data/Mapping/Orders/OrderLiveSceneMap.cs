using Nop.Core.Domain.Orders;

namespace Nop.Data.Mapping.Orders
{
    public partial class OrderLiveSceneMap : NopEntityTypeConfiguration<OrderLiveScene>
    {
        public OrderLiveSceneMap()
        {
            this.ToTable("OrderLiveScene");
            this.HasKey(ols => ols.Id);

            this.HasRequired(orderItem => orderItem.Order)
            .WithMany(o => o.OrderLiveScenes)
            .HasForeignKey(orderItem => orderItem.OrderId);
        }
    }
}
