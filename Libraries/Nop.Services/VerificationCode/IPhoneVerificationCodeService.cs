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

namespace Nop.Services.VerificationCode
{
    public partial interface IPhoneVerificationCodeService
    {

        PhoneVerificationCode GetCodeByPhone(Guid customerGuid, string phone);

        PhoneVerificationCode InsertVerificationCode(Guid customerGuid, string phone);
    }
}
