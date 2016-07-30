﻿using DotNetOpenAuth.AspNet;
using Newtonsoft.Json.Linq;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Logging;
using Senparc.Weixin.MP.AdvancedAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nop.Plugin.ExternalAuth.OAuth2.Core
{
    public sealed class WeChatClient : Client
    {
        #region Constants and Fields

        /// <summary>
        /// The authorization endpoint.
        /// </summary>
        private const string AuthorizationEndpoint = "https://open.weixin.qq.com/connect/qrconnect";

        /// <summary>
        /// The token endpoint.
        /// </summary>
        private const string TokenEndpoint = "https://api.weixin.qq.com/sns/oauth2/access_token";

        /// <summary>
        /// The user data endpoint.
        /// </summary>
        private const string UserDataEndPoint = "https://graph.qq.com/oauth2.0/me";

        /// <summary>
        /// The user info endpoint.
        /// </summary>
        private const string UserInfoEndpoint = "https://api.weixin.qq.com/sns/userinfo";

        /// <summary>
        /// The _app id.
        /// </summary>
        private readonly string _appId;

        /// <summary>
        /// The _app secret.
        /// </summary>
        private readonly string _appSecret;

        /// <summary>
        /// The scope.
        /// </summary>
        private readonly string[] _scope;

        private readonly ILogger _logger;



        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the   
        /// with "get_user_info" as the scope.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        public WeChatClient(string appId, string appSecret)
            : this(appId, appSecret, "snsapi_login")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QQClient"/> class.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        /// <param name="scope">
        /// The scope of authorization to request when authenticating with Facebook. The default is "get_user_info".
        /// </param>
        public WeChatClient(string appId, string appSecret, params string[] scope)
            : base("wx")
        {
            _appId = appId;
            _appSecret = appSecret;
            _scope = scope;

            _logger = EngineContext.Current.Resolve<ILogger>();
        }

        #endregion

        #region Methods

        public override Uri GenerateLocalCallbackUri(IWebHelper webHelper)
        {
            string url = string.Format("{0}plugins/externalauthoauth2/wechatcallback", webHelper.GetStoreLocation());
            return new Uri(url);
        }

        public override Uri GenerateServiceLoginUrl(Uri returnUrl)
        {
            return GetServiceLoginUrl(returnUrl);
        }

        /// <summary>
        /// The get service login url.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>An absolute URI.</returns>
        protected override Uri GetServiceLoginUrl(Uri returnUrl)
        {
            // Note: Facebook doesn't like us to url-encode the redirect_uri value
            var builder = new UriBuilder(AuthorizationEndpoint);
            builder.AppendQueryArgs(
                new Dictionary<string, string> {
                    { "response_type", "code"},
                    { "appid", _appId },
                    { "redirect_uri",HttpUtility.UrlEncode( returnUrl.AbsoluteUri )},
                    { "scope", string.Join(" ", _scope) },
                    { "state", "lgmt" }
                });



            return builder.Uri;
        }

        /// <summary>
        /// The get user data.
        /// </summary>
        /// <param name="accessToken">
        /// The access token.
        /// </param>
        /// <returns>A dictionary of profile data.</returns>
        protected override IDictionary<string, string> GetUserData(string accessToken)
        {

            //  OAuthApi.GetUserInfo(accessToken)
            if (HttpContext.Current.Session["OAuth2.Weixin.openid"] != null)
            {
                var openId = HttpContext.Current.Session["OAuth2.Weixin.openid"].ToString();
                var result = OAuthApi.GetUserInfo(accessToken, openId);


                HttpContext.Current.Session.Remove("OAuth2.Weixin.openid");

                return new Dictionary<string, string>
                        {
                            {"openid", openId},
                            {"id", openId},
                            {"name", openId},
                            {"nickname",result.nickname},
                            {"figureurl",result.headimgurl},
                            {"gender",result.sex.ToString()},
                        };


            }
            else
            {
                _logger.Debug(string.Format("wx: GetUserData 没有包含openid;accessToken:[{0}]", accessToken));
            }

            return null;

        }

        /// <summary>
        /// Obtains an access token given an authorization code and callback URL.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <param name="authorizationCode">
        /// The authorization code.
        /// </param>
        /// <returns>
        /// The access token.
        /// </returns>
        protected override string QueryAccessToken(Uri returnUrl, string authorizationCode)
        {
            var builder = new UriBuilder(TokenEndpoint);
            builder.AppendQueryArgs(
                new Dictionary<string, string> {
                    {"grant_type", "authorization_code"},
                    { "client_id", _appId },
                    { "client_secret", _appSecret },
                    { "code", authorizationCode },
                    { "redirect_uri", NormalizeHexEncoding(returnUrl.AbsoluteUri) },
                });

            using (var client = new WebClient())
            {
                string data = client.DownloadString(builder.Uri);
                if (string.IsNullOrEmpty(data))
                {
                    return null;
                }

                var parsedQueryString = HttpUtility.ParseQueryString(data);

                HttpContext.Current.Session["OAuth2.Weixin.openid"] = parsedQueryString["openid"];

                return parsedQueryString["access_token"];
            }
        }

        /// <summary>
        /// Converts any % encoded values in the URL to uppercase.
        /// </summary>
        /// <param name="url">The URL string to normalize</param>
        /// <returns>The normalized url</returns>
        /// <example>NormalizeHexEncoding("Login.aspx?ReturnUrl=%2fAccount%2fManage.aspx") returns "Login.aspx?ReturnUrl=%2FAccount%2FManage.aspx"</example>
        /// <remarks>
        /// There is an issue in Facebook whereby it will rejects the redirect_uri value if
        /// the url contains lowercase % encoded values.
        /// </remarks>
        private static string NormalizeHexEncoding(string url)
        {
            var chars = url.ToCharArray();
            for (int i = 0; i < chars.Length - 2; i++)
            {
                if (chars[i] == '%')
                {
                    chars[i + 1] = char.ToUpperInvariant(chars[i + 1]);
                    chars[i + 2] = char.ToUpperInvariant(chars[i + 2]);
                    i += 2;
                }
            }
            return new string(chars);
        }


        public override AuthenticationResult VerifyAuthentication(HttpContextBase context, Uri returnPageUrl)
        {
            var code = context.Request.QueryString["code"];
            var state = context.Request.QueryString["state"];



            var result2 = OAuthApi.GetAccessToken(_appId, _appSecret, code);




            _logger.Debug(string.Format("登录  @WX  VerifyAuthentication：{0}", returnPageUrl));
            var result = base.VerifyAuthentication(context, returnPageUrl);
            if (_logger.IsEnabled(Nop.Core.Domain.Logging.LogLevel.Debug))
            {
                var resultStr = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                _logger.Debug(string.Format("登录  @WX返回数据格式", resultStr));
            }


            return result;
        }

        #endregion



    }
}
