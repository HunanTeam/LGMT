namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents an order status enumeration
    /// </summary>
    public enum OrderBalancePaymentStatus
    {
        /// <summary>
        /// 待支付
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 已支付
        /// </summary>
        Paid = 10,

        /// <summary>
        /// 商家确认已支付
        /// </summary>
        ConfirmPaid = 20,        
    }

    public static class OrderBalancePaymentStatusExtensions
    {
        public static string ToName(this OrderBalancePaymentStatus status)
        {
            var name = string.Empty;

            switch (status)
            {
                case OrderBalancePaymentStatus.Pending: name = "待支付"; break;
                case OrderBalancePaymentStatus.Paid: name = "已支付"; break;
                case OrderBalancePaymentStatus.ConfirmPaid: name = "商家确认已支付"; break;         
            }

            return name;
        }
    }
}
