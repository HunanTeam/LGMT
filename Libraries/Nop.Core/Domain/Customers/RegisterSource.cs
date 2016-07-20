using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Customers
{
    public enum RegisterSource
    {
        System = 0,
        /// <summary>
        /// 第三方登录
        /// </summary>
        ThirdParty = 10
    }
}
