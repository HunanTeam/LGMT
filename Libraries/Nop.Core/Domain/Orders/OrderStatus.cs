namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents an order status enumeration
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Pending(预选)
        /// </summary>
        Pending = 10,

        /// <summary>
        /// Processing
        /// </summary>
        Processing = 20,   

        /// <summary>
        /// 确认预定
        /// </summary>
        ConfirmBooking=22,

        /// <summary>
        /// 测量和报价
        /// </summary>
        MeasureForPrice=24,

        /// <summary>
        /// 支付定金
        /// </summary>
        PayDeposit = 26,

        /// <summary>
        /// 制作和安装
        /// </summary>
        ManufactureAndInstallment=28,

        /// <summary>
        /// 支付尾款
        /// </summary>
        PayBalance=29,

        /// <summary>
        /// Complete
        /// </summary>
        Complete = 30,

        /// <summary>
        /// Cancelled
        /// </summary>
        Cancelled = 40
    }
}
