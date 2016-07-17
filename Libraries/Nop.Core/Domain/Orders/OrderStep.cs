namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents an order status enumeration
    /// </summary>
    public enum OrderStep
    {
        /// <summary>
        /// 心愿单预选
        /// </summary>
        Pending = 0,

        /// <summary>
        /// 确认预定
        /// </summary>
        ConfirmBooking = 10,

        /// <summary>
        /// 测量和报价
        /// </summary>
        MeasureForPrice = 20,

        /// <summary>
        /// 支付定金
        /// </summary>
        PayDeposit = 30,

        /// <summary>
        /// 制作安装
        /// </summary>
        ManufactureAndInstallment = 40,

        /// <summary>
        /// 支付尾款
        /// </summary>
        PayBalance = 50,

        /// <summary>
        /// 订单完成
        /// </summary>
        Complete = 60
    }

    public static class OrderStepExtensions
    {
        public static string ToName(this OrderStep step)
        {
            var name = string.Empty;

            switch (step)
            {
                case OrderStep.Pending: name = "心愿单预选"; break;
                case OrderStep.ConfirmBooking: name = "确认预定"; break;
                case OrderStep.MeasureForPrice: name = "测量报价"; break;
                case OrderStep.PayDeposit: name = "支付定金"; break;
                case OrderStep.ManufactureAndInstallment: name = "制作安装"; break;
                case OrderStep.PayBalance: name = "支付尾款"; break;
                case OrderStep.Complete: name = "订单完成"; break;
            }

            return name;
        }
    }
}
