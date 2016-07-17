namespace Nop.Core.Domain.Orders
{
    /// <summary>
    /// Represents an order status enumeration
    /// </summary>
    public enum OrderStep
    {
        /// <summary>
        /// ��Ը��Ԥѡ
        /// </summary>
        Pending = 0,

        /// <summary>
        /// ȷ��Ԥ��
        /// </summary>
        ConfirmBooking = 10,

        /// <summary>
        /// �����ͱ���
        /// </summary>
        MeasureForPrice = 20,

        /// <summary>
        /// ֧������
        /// </summary>
        PayDeposit = 30,

        /// <summary>
        /// ������װ
        /// </summary>
        ManufactureAndInstallment = 40,

        /// <summary>
        /// ֧��β��
        /// </summary>
        PayBalance = 50,

        /// <summary>
        /// �������
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
                case OrderStep.Pending: name = "��Ը��Ԥѡ"; break;
                case OrderStep.ConfirmBooking: name = "ȷ��Ԥ��"; break;
                case OrderStep.MeasureForPrice: name = "��������"; break;
                case OrderStep.PayDeposit: name = "֧������"; break;
                case OrderStep.ManufactureAndInstallment: name = "������װ"; break;
                case OrderStep.PayBalance: name = "֧��β��"; break;
                case OrderStep.Complete: name = "�������"; break;
            }

            return name;
        }
    }
}
