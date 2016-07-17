namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents an order status enumeration
    /// </summary>
    public enum OrderBalancePaymentStatus
    {
        /// <summary>
        /// ��֧��
        /// </summary>
        Pending = 0,

        /// <summary>
        /// ��֧��
        /// </summary>
        Paid = 10,

        /// <summary>
        /// �̼�ȷ����֧��
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
                case OrderBalancePaymentStatus.Pending: name = "��֧��"; break;
                case OrderBalancePaymentStatus.Paid: name = "��֧��"; break;
                case OrderBalancePaymentStatus.ConfirmPaid: name = "�̼�ȷ����֧��"; break;         
            }

            return name;
        }
    }
}
