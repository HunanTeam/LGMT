using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.AspNet;
using DotNetOpenAuth.AspNet.Clients;
using Newtonsoft.Json.Linq;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Services.Authentication.External;

namespace Nop.Plugin.ExternalAuth.OAuth2.Core
{
    public class OAuth2ProviderAuthorizer : IOAuthProviderOAuth2Authorizer
    {
        #region Fields

        private readonly IExternalAuthorizer _authorizer;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly OAuth2ExternalAuthSettings _oAuth2ExternalAuthSettings;
        private readonly HttpContextBase _httpContext;
        private readonly IWebHelper _webHelper;
        private Client _clientApplication;

        #endregion

        #region Ctor

        public OAuth2ProviderAuthorizer(IExternalAuthorizer authorizer,
            ExternalAuthenticationSettings externalAuthenticationSettings,
            OAuth2ExternalAuthSettings oAuth2ExternalAuthSettings,
            HttpContextBase httpContext,
            IWebHelper webHelper)
        {
            _authorizer = authorizer;
            _externalAuthenticationSettings = externalAuthenticationSettings;
            _oAuth2ExternalAuthSettings = oAuth2ExternalAuthSettings;
            _httpContext = httpContext;
            _webHelper = webHelper;
        }

        #endregion

        #region Property
        public ClientType ClientType { get; set; }
        #endregion

        #region Utilities

        private Client ClientApplication
        {
            get
            {
                switch (ClientType)
                {
                    case ClientType.QQ:
                        return _clientApplication ?? (_clientApplication = new QQClient(_oAuth2ExternalAuthSettings.QQClientKeyIdentifier, _oAuth2ExternalAuthSettings.QQClientSecret));
                    case ClientType.WeiBo:
                        return _clientApplication ?? (_clientApplication = new WeiBoClient(_oAuth2ExternalAuthSettings.WeiBoClientKeyIdentifier, _oAuth2ExternalAuthSettings.WeiBoClientSecret));
                    default:
                        return _clientApplication ?? (_clientApplication = new QQClient(_oAuth2ExternalAuthSettings.QQClientKeyIdentifier, _oAuth2ExternalAuthSettings.QQClientSecret));
                }
            }
        }

        private AuthorizeState VerifyAuthentication(string returnUrl)
        {
            var authResult = ClientApplication.VerifyAuthentication(_httpContext, ClientApplication.GenerateLocalCallbackUri(_webHelper));

            if (authResult.IsSuccessful)
            {
                if (!authResult.ExtraData.ContainsKey("id"))
                    throw new Exception("Authentication result does not contain id data");

                if (!authResult.ExtraData.ContainsKey("accesstoken"))
                    throw new Exception("Authentication result does not contain accesstoken data");

                var parameters = new OAuthAuthenticationParameters(GetProviderName())
                {
                    ExternalIdentifier = authResult.ProviderUserId,
                    OAuthAccessToken = authResult.ProviderUserId,
                    OAuthToken = authResult.ExtraData["accesstoken"]
                };

                if (_externalAuthenticationSettings.AutoRegisterEnabled)
                    ParseClaims(authResult, parameters);

                var result = _authorizer.Authorize(parameters);

                return new AuthorizeState(returnUrl, result);
            }

            var state = new AuthorizeState(returnUrl, OpenAuthenticationStatus.Error);
            var error = authResult.Error != null ? authResult.Error.Message : "Unknown error";
            state.AddError(error);
            return state;
        }

        private void ParseClaims(AuthenticationResult authenticationResult, OAuthAuthenticationParameters parameters)
        {
            var claims = new UserClaims
            {
                Contact = new ContactClaims()
            };
            if (authenticationResult.ExtraData.ContainsKey("username"))
                claims.Contact.Email = authenticationResult.ExtraData["username"];

            claims.Name = new NameClaims();
            if (authenticationResult.ExtraData.ContainsKey("nickname"))
                claims.Name.Nickname = authenticationResult.ExtraData["nickname"];
            if (authenticationResult.ExtraData.ContainsKey("name"))
            {
                var name = authenticationResult.ExtraData["name"];
                if (!String.IsNullOrEmpty(name))
                {
                    var nameSplit = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (nameSplit.Length >= 2)
                    {
                        claims.Name.First = nameSplit[0];
                        claims.Name.Last = nameSplit[1];
                    }
                    else
                    {
                        claims.Name.Last = nameSplit[0];
                    }
                }
            }

            claims.Person = new PersonClaims();
            //if (authenticationResult.ExtraData.ContainsKey("figureurl"))
            //    claims.Person.FigureUrl = authenticationResult.ExtraData["figureurl"];
            if (authenticationResult.ExtraData.ContainsKey("gender"))
                claims.Person.Gender = authenticationResult.ExtraData["gender"];

            parameters.AddClaim(claims);
        }

        private AuthorizeState RequestAuthentication()
        {
            //var authUrl = GenerateServiceLoginUrl().AbsoluteUri;
            var authUrl = ClientApplication.GenerateServiceLoginUrl(ClientApplication.GenerateLocalCallbackUri(_webHelper)).AbsoluteUri;
            // authUrl = "https://graph.qq.com/oauth2.0/authorize?response_type=code&client_id=101336160&redirect_uri=http://www.dmall.la/plugins/externalauthoauth2/qqlogincallback&scope=get_user_info";
            return new AuthorizeState("", OpenAuthenticationStatus.RequiresRedirect) { Result = new RedirectResult(authUrl) };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Authorize response
        /// </summary>
        /// <param name="returnUrl">Return URL</param>
        /// <param name="verifyResponse">true - Verify response;false - request authentication;null - determine automatically</param>
        /// <returns>Authorize state</returns>
        public AuthorizeState Authorize(string returnUrl, bool? verifyResponse = null)
        {
            if (!verifyResponse.HasValue)
                throw new ArgumentException("OAuth2 plugin cannot automatically determine verifyResponse property");

            if (verifyResponse.Value)
            {
                return VerifyAuthentication(returnUrl);
            }
            else
            {
                return RequestAuthentication();
            }
        }

        private string GetProviderName()
        {
            switch (this.ClientType)
            {
                case ClientType.QQ:
                    return Provider.SystemNameQQ;

                case ClientType.WeiBo:
                    return Provider.SystemNameQQ;

                case ClientType.WeChat:
                    return Provider.SystemNameQQ;

                default:
                    return "unkown";

            }
        }
        #endregion
    }
}