namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents an order status enumeration
    /// </summary>
    public enum OrderDepositPaymentStatus
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

    public static class OrderDepositPaymentStatusExtensions
    {
        public static string ToName(this OrderDepositPaymentStatus status)
        {
            var name = string.Empty;

            switch (status)
            {
                case OrderDepositPaymentStatus.Pending: name = "��֧��"; break;
                case OrderDepositPaymentStatus.Paid: name = "��֧��"; break;
                case OrderDepositPaymentStatus.ConfirmPaid: name = "�̼�ȷ����֧��"; break;         
            }

            return name;
        }
    }
}
