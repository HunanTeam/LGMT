using Nop.Core.Domain.VerificationCode;

namespace Nop.Data.Mapping.VerificationCode
{
    public class PhoneVerificationCodeMap : NopEntityTypeConfiguration<PhoneVerificationCode>
    {
        public PhoneVerificationCodeMap()
        {
            this.ToTable("PhoneVerificationCode");
            this.HasKey(t => t.Id);
        }
    }
}
