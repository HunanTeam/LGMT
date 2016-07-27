using System;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Seo;
using Nop.Core.Domain.Stores;

namespace Nop.Core.Domain.VerificationCode
{
    public partial class PhoneVerificationCode : BaseEntity
    {
        public Guid CustomerGuid { get; set; }
        public string Phone { get; set; }
        public string Code { get; set; }
        public string ClientIp { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public int VerificationType { get; set; }
        public VerificationCodeType VerificationTypeEnum
        {
            get
            {
                return (VerificationCodeType)VerificationType;
            }
            set
            {
                this.VerificationType = (int)value;
            }
        }
    }
}
