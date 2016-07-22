using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetOpenAuth.OAuth;
using Nop.Core;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using DotNetOpenAuth.AspNet;
using Nop.Core.Infrastructure;
using Nop.Services.Logging;

namespace Nop.Plugin.ExternalAuth.OAuth2.Core
{
    public sealed class QQClient : Client
    {
        #region Constants and Fields

        /// <summary>
        /// The authorization endpoint.
        /// </summary>
        private const string AuthorizationEndpoint = "https://graph.qq.com/oauth2.0/authorize";

        /// <summary>
        /// The token endpoint.
        /// </summary>
        private const string TokenEndpoint = "https://graph.qq.com/oauth2.0/token";

        /// <summary>
        /// The user data endpoint.
        /// </summary>
        private const string UserDataEndPoint = "https://graph.qq.com/oauth2.0/me";

        /// <summary>
        /// The user info endpoint.
        /// </summary>
        private const string UserInfoEndpoint = "https://graph.qq.com/user/get_user_info";

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
        /// Initializes a new instance of the <see cref="QQClient"/> class
        /// with "get_user_info" as the scope.
        /// </summary>
        /// <param name="appId">
        /// The app id.
        /// </param>
        /// <param name="appSecret">
        /// The app secret.
        /// </param>
        public QQClient(string appId, string appSecret)
            : this(appId, appSecret, "get_user_info")
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
        public QQClient(string appId, string appSecret, params string[] scope)
            : base("qq")
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
            string url = string.Format("{0}plugins/externalauthoauth2/qqlogincallback", webHelper.GetStoreLocation());
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
                    { "client_id", _appId },
                    { "redirect_uri", returnUrl.AbsoluteUri },
                    { "scope", string.Join(" ", _scope) },
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
            var builder = new UriBuilder(UserDataEndPoint);
            builder.AppendQueryArgs(
                new Dictionary<string, string>
                    {
                        {"access_token", MessagingUtilities.EscapeUriDataStringRfc3986(accessToken)},
                    });

            using (var client = new WebClient())
            {
                client.Encoding = System.Text.Encoding.UTF8;

                string data = client.DownloadString(builder.Uri);
                if (string.IsNullOrEmpty(data))
                {
                    return null;
                }

                if (data.StartsWith("callback( "))
                {
                    data = data.Substring("callback( ".Length);
                    data = data.Substring(0, data.Length - 3);
                }

                var obj = JObject.Parse(data);
                var openid = (obj["openid"] == null) ? null : obj["openid"].ToString();
                if (string.IsNullOrEmpty(openid))
                    return null;

                #region 获取QQ昵称、头像、性别

                var nickname = "";
                var figureurl = "";
                var gender = "";
                var client_id = (obj["client_id"] == null) ? null : obj["client_id"].ToString();

                var getInfoBuilder = new UriBuilder(UserInfoEndpoint);
                getInfoBuilder.AppendQueryArgs(
                    new Dictionary<string, string>
                    {
                        {"access_token", MessagingUtilities.EscapeUriDataStringRfc3986(accessToken)},
                        {"oauth_consumer_key",MessagingUtilities.EscapeUriDataStringRfc3986(client_id)},
                        {"openid",MessagingUtilities.EscapeUriDataStringRfc3986(openid)},
                    });

                string info = client.DownloadString(getInfoBuilder.Uri);
                if (!string.IsNullOrEmpty(info))
                {
                    var infoObj = JObject.Parse(info);
                    nickname = (infoObj["nickname"] == null) ? "" : infoObj["nickname"].ToString();
                    figureurl = (infoObj["figureurl_2"] == null) ? "" : infoObj["figureurl_2"].ToString();
                    gender = (infoObj["gender"] == null) ? "" : infoObj["gender"].ToString();
                }

                #endregion

                openid = "QQ_" + openid;
                return new Dictionary<string, string>
                    {
                        {"openid", openid},
                        {"id", openid},
                        {"name", openid},
                        {"nickname",nickname},
                        {"figureurl",figureurl},
                        {"gender",gender},
                    };
            }

            //FacebookGraphData graphData;
            //var request = WebRequest.Create(builder.Uri);
            //using (var response = request.GetResponse()) {
            //    using (var responseStream = response.GetResponseStream()) {
            //        graphData = JsonHelper.Deserialize<FacebookGraphData>(responseStream);
            //    }
            //}

            //// this dictionary must contains 
            //var userData = new Dictionary<string, string>();
            //userData.AddItemIfNotEmpty("id", graphData.Id);
            //userData.AddItemIfNotEmpty("username", graphData.Email);
            //userData.AddItemIfNotEmpty("name", graphData.Name);
            //userData.AddItemIfNotEmpty("link", graphData.Link == null ? null : graphData.Link.AbsoluteUri);
            //userData.AddItemIfNotEmpty("gender", graphData.Gender);
            //userData.AddItemIfNotEmpty("birthday", graphData.Birthday);
            //return userData;
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
            // Note: Facebook doesn't like us to url-encode the redirect_uri value
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
            var result = base.VerifyAuthentication(context, returnPageUrl);
            if (_logger.IsEnabled(Nop.Core.Domain.Logging.LogLevel.Debug))
            {
                var resultStr = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                _logger.Information(resultStr);
            }
          

            return result;
        }

        #endregion

    }
}
