using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.VerificationCode;
using Nop.Services.Events;
using Nop.Services.Stores;

namespace Nop.Services.VerificationCode
{
    public partial class PhoneVerificationCodeService : IPhoneVerificationCodeService
    {
        private readonly IRepository<PhoneVerificationCode> _phoneVerificationCodeRepository;

        public PhoneVerificationCodeService(IRepository<PhoneVerificationCode> phoneVerificationCodeRepository)
        {
            _phoneVerificationCodeRepository = phoneVerificationCodeRepository;
        }

        public virtual PhoneVerificationCode GetCodeByPhone(Guid customerGuid, string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return null;

            var code = _phoneVerificationCodeRepository.Table.OrderByDescending(m => m.CreatedDate)
                .FirstOrDefault(m => m.CustomerGuid == customerGuid && m.Phone == phone);
            return code;
        }

        public virtual PhoneVerificationCode InsertVerificationCode(Guid customerGuid, string phone)
        {
            var code = new PhoneVerificationCode();
            code.CustomerGuid = customerGuid;
            code.Phone = phone;
            code.Code = GenerateRndCode();
            code.ExpirationTime = DateTime.Now.AddMinutes(30); //默认30分钟内有效
            code.CreatedDate = DateTime.Now;
            _phoneVerificationCodeRepository.Insert(code);
            return code;
        }

        private string GenerateRndCode()
        {
            var code = new RandomHelper().GenerateCheckCodeNum(6);
            return code;
        }
    }
}
