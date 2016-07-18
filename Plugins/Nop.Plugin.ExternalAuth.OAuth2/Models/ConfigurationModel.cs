using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.ExternalAuth.OAuth2.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.ExternalAuth.OAuth2.QQEnabled")]
        public bool QQEnabled { get; set; }
        [NopResourceDisplayName("Plugins.ExternalAuth.OAuth2.QQClientKeyIdentifier")]
        public string QQClientKeyIdentifier { get; set; }
        [NopResourceDisplayName("Plugins.ExternalAuth.OAuth2.QQClientSecret")]
        public string QQClientSecret { get; set; }

        [NopResourceDisplayName("Plugins.ExternalAuth.OAuth2.WeiBoEnabled")]
        public bool WeiBoEnabled { get; set; }
        [NopResourceDisplayName("Plugins.ExternalAuth.OAuth2.WeiBoClientKeyIdentifier")]
        public string WeiBoClientKeyIdentifier { get; set; }
        [NopResourceDisplayName("Plugins.ExternalAuth.OAuth2.WeiBoClientSecret")]
        public string WeiBoClientSecret { get; set; }
    }
}