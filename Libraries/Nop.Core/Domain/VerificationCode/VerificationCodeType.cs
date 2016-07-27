using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.VerificationCode
{
    /// <summary>
    /// 验证类型
    /// </summary>
    public enum VerificationCodeType
    {
        /// <summary>
        /// 注册(邮箱或手机号码验证)
        /// </summary>
        Register = 0,

        /// <summary>
        /// 修改密码(身份验证)
        /// </summary>
        ChangePassword = 1,

        /// <summary>
        /// 修改手机号码(身份验证)
        /// </summary>
        ChangeMobileAuthenticate = 2,

        /// <summary>
        /// 修改手机号码(新手机号码验证)
        /// </summary>
        ChangeMobileNewMoblieAuthenticate = 3,

        /// <summary>
        /// 修改邮箱(身份验证)
        /// </summary>
        ChangeEmialAuthenticate = 4,

        /// <summary>
        /// 修改邮箱(新手机号码验证)
        /// </summary>
        ChangeEmailNewEmailAuthenticate = 5,

        /// <summary>
        /// 忘记密码(身份验证)
        /// </summary>
        PasswordRecoveryAuthenticode = 6,

        /// <summary>
        /// 第三方登录绑定用户
        /// </summary>
        ExternalAuthBind=7,
    }
}
