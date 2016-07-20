using Nop.Core.Configuration;

namespace Nop.Plugin.ExternalAuth.OAuth2
{
    public class OAuth2ExternalAuthSettings : ISettings
    {
        public bool QQEnabled { get; set; }
        public string QQClientKeyIdentifier { get; set; }
        public string QQClientSecret { get; set; }

        public bool WeiBoEnabled { get; set; }
        public string WeiBoClientKeyIdentifier { get; set; }
        public string WeiBoClientSecret { get; set; }
    }
}
