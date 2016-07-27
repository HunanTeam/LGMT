using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.VerificationCode;
using Nop.Services.Events;
using Nop.Services.Stores;
using Nop.Core.Domain.Common;

namespace Nop.Services.VerificationCode
{
    public partial interface IPhoneVerificationCodeService
    {

        PhoneVerificationCode GetCodeByPhone(Guid customerGuid, string phone, VerificationCodeType verificationType);

        PhoneVerificationCode InsertVerificationCode(Guid customerGuid, string phone, VerificationCodeType verificationType);
        /// <summary>
        /// 验证用户输入的手机验证码
        /// </summary>
        /// <param name="customerGuid"></param>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <param name="verificationType"></param>
        /// <returns></returns>
        bool ValidatePhoneVerificationCode(Guid customerGuid, string phone, VerificationCodeType verificationType, string code);
    }
}
