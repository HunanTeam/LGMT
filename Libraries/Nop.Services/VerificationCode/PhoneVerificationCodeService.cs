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
using Nop.Core.Domain.Common;

namespace Nop.Services.VerificationCode
{
    public partial class PhoneVerificationCodeService : IPhoneVerificationCodeService
    {
        private readonly IRepository<PhoneVerificationCode> _phoneVerificationCodeRepository;
        private readonly PhoneVerificationSettings _authenticodeSettings;

        public PhoneVerificationCodeService(IRepository<PhoneVerificationCode> phoneVerificationCodeRepository,
            PhoneVerificationSettings authenticodeSettings)
        {
            this._phoneVerificationCodeRepository = phoneVerificationCodeRepository;
            this._authenticodeSettings = authenticodeSettings;
        }

        public virtual PhoneVerificationCode GetCodeByPhone(Guid customerGuid, string phone, VerificationCodeType verificationType)
        {
            if (string.IsNullOrEmpty(phone))
                return null;

            var code = _phoneVerificationCodeRepository.Table.OrderByDescending(m => m.CreatedDate)
                .FirstOrDefault(m => m.CustomerGuid == customerGuid && m.Phone == phone && m.VerificationType == (int)verificationType);
            return code;
        }

        public virtual PhoneVerificationCode InsertVerificationCode(Guid customerGuid, string phone, VerificationCodeType verificationType)
        {
            var code = new PhoneVerificationCode();
            code.CustomerGuid = customerGuid;
            code.Phone = phone;
            code.Code = GenerateRndCode();
            code.ExpirationTime = DateTime.Now.AddSeconds(_authenticodeSettings.EffectiveTime); //默认180秒
            code.CreatedDate = DateTime.Now;
            code.VerificationTypeEnum = verificationType;

            _phoneVerificationCodeRepository.Insert(code);
            return code;
        }

        public virtual bool ValidatePhoneVerificationCode(Guid customerGuid, string phone, VerificationCodeType verificationType, string code)
        {
            var pvc = GetCodeByPhone(customerGuid, phone, verificationType);
            var result = pvc != null && pvc.Code == code.Trim() && pvc.ExpirationTime > DateTime.Now;

            return result;
        }



        private string GenerateRndCode()
        {
            var code = new RandomHelper().GenerateCheckCodeNum(6);
            return code;
        }





    }
}
