namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents an order status enumeration
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Pending(Ԥѡ)
        /// </summary>
        Pending = 10,

        /// <summary>
        /// Processing
        /// </summary>
        Processing = 20,   

        /// <summary>
        /// ȷ��Ԥ��
        /// </summary>
        ConfirmBooking=22,

        /// <summary>
        /// �����ͱ���
        /// </summary>
        MeasureForPrice=24,

        /// <summary>
        /// ֧������
        /// </summary>
        PayDeposit = 26,

        /// <summary>
        /// �����Ͱ�װ
        /// </summary>
        ManufactureAndInstallment=28,

        /// <summary>
        /// ֧��β��
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
