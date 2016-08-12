using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Validators.Customer;

namespace Nop.Web.Models.Customer
{
    [Validator(typeof(BindPhoneValidator))]
    public partial class BindPhoneModel : BaseNopModel
    {
        /// <summary>
        /// 客户手机
        /// </summary>
        [NopResourceDisplayName("Account.ChangePassword.Fields.CustomerPhone")]
        public string CustomerPhone { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        [NopResourceDisplayName("Account.ChangePassword.Fields.Password")]
        public string Password { get; set; }

        [AllowHtml]
        [DataType(DataType.Password)]
        [NopResourceDisplayName("Account.ChangePassword.Fields.ConfirmPassword")]
        public string ConfirmPassword { get; set; }

        public string Result { get; set; }

        public string CustomerFrom { get; set; }

        public string NikeName { get; set; }

        public string AvatarUrl { get; set; }

        /// <summary>
        /// 手机验证码
        /// </summary>
        public string PhoneAuthCode { get; set; }
    }
}