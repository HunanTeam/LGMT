using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Validators.Customer;

namespace Nop.Web.Models.Customer
{
    public partial class PasswordRecoveryStep1Model : BaseNopModel
    {
        public string UserName { get; set; }
        public string Message { get; set; }
    }
}