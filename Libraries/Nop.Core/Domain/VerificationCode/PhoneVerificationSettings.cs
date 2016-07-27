
using Nop.Core.Configuration;

namespace Nop.Core.Domain.VerificationCode
{
    public class PhoneVerificationSettings : ISettings
    {
        /// <summary>
        /// 验证码有效时间（秒钟）
        /// </summary>
        public int EffectiveTime { get; set; }

        /// <summary>
        /// 重复获取验证码间隔时间（秒钟）
        /// </summary>
        public int AgainGetSpacingTime { get; set; }
    }
}
