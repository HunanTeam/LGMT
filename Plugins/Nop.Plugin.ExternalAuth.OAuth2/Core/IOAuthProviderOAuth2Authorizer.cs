using Nop.Services.Authentication.External;

namespace Nop.Plugin.ExternalAuth.OAuth2.Core
{
    public interface IOAuthProviderOAuth2Authorizer : IExternalProviderAuthorizer
    {
          ClientType ClientType { get; set; }
    }
}