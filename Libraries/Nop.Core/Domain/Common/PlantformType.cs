using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Common
{
    /// <summary>
    /// 客户端平台类型
    /// </summary>
    public enum PlantformType : int
    {
        /// <summary>
        /// 默认（所有平台）
        /// </summary>
        Default = 0,

        /// <summary>
        /// PC端
        /// </summary>
        PC = 1,

        /// <summary>
        /// 手机APP
        /// </summary>
        APP = 2,

        /// <summary>
        /// 微信
        /// </summary>
        WeChat = 3,
    }
}
