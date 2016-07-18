using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.ExternalAuth.OAuth2.Models
{
    public class PublicInfoModel : BaseNopModel
    {
        public bool QQEnabled { get; set; }
        public bool WeiBoEnabled { get; set; }
    }
}
