using Newtonsoft.Json.Linq;
using Nop.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.ExternalAuth.OAuth2.Core
{
    public sealed class WeiBoClient : Client
    {
        #region Constants and Fields

        /// <summary>
        /// The authorization endpoint.
        /// </summary>
        private const string AuthorizationEndpoint = "https://api.weibo.com/oauth2/authorize";

        /// <summary>
        /// The token endpoint.
        /// </summary>
        private const string TokenEndpoint = "https://api.weibo.com/oauth2/access_token";

        /// <summary>
        /// The user data endpoint.
        /// </summary>
        private const string UserDataEndPoint = "https://api.weibo.com/2/account/get_uid.json";

        /// <summary>
        /// The user info endpoint.
        /// </summary>
        private const string UserInfoEndpoint = "https://api.weibo.com/2/users/show.json";

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
        public WeiBoClient(string appId, string appSecret)
            : this(appId, appSecret, "email")
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
        public WeiBoClient(string appId, string appSecret, params string[] scope)
            : base("weibo")
        {
            _appId = appId;
            _appSecret = appSecret;
            _scope = scope;
        }

        #endregion

        #region Methods

        public override Uri GenerateLocalCallbackUri(IWebHelper webHelper)
        {
            string url = string.Format("{0}plugins/externalauthOAuth2/weibologincallback/", webHelper.GetStoreLocation());
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
                string data = client.DownloadString(builder.Uri);
                if (string.IsNullOrEmpty(data))
                {
                    return null;
                }

                var obj = JObject.Parse(data);
                var uid = (obj["uid"] == null) ? null : obj["uid"].ToString();
                if (string.IsNullOrEmpty(uid))
                    return null;

                #region 获取微博昵称、头像、性别（TODO：由于微博测试环境问题，获取用户信息的这些代码未经过测试）

                var nickname = "";
                var figureurl = "";
                var gender = "";

                try
                {
                    var getInfoBuilder = new UriBuilder(UserInfoEndpoint);
                    getInfoBuilder.AppendQueryArgs(
                        new Dictionary<string, string>
                    {
                        {"access_token", MessagingUtilities.EscapeUriDataStringRfc3986(accessToken)},
                        {"uid",MessagingUtilities.EscapeUriDataStringRfc3986(uid)},
                    });

                    string info = client.DownloadString(getInfoBuilder.Uri);
                    if (!string.IsNullOrEmpty(info))
                    {
                        var infoObj = JObject.Parse(info);
                        nickname = (infoObj["screen_name"] == null) ? "" : infoObj["screen_name"].ToString();
                        figureurl = (infoObj["avatar_large"] == null) ? "" : infoObj["avatar_large"].ToString();
                        gender = (infoObj["gender"] == null) ? "" : infoObj["gender"].ToString();
                    }
                }
                catch { }

                #endregion

                uid = "WeiBo_" + uid;
                return new Dictionary<string, string>
                    {
                        {"openid", uid},
                        {"id", uid},
                        {"name", uid},
                        {"nickname",nickname},
                        {"figureurl",figureurl},
                        {"gender",gender},
                    };
            }
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
                    { "client_id", _appId },
                    { "client_secret", _appSecret },
                    { "grant_type", "authorization_code"},
                    { "code", authorizationCode },
                    { "redirect_uri", NormalizeHexEncoding(returnUrl.AbsoluteUri) },
                });

            //using (var request = WebRequest.Create(builder.Uri))
            var request = WebRequest.Create(builder.Uri);
            {
                request.Method = "POST";
                using (var response = request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        var reader = new StreamReader(stream, true);
                        var data = reader.ReadToEnd();

                        var obj = JObject.Parse(data);
                        var accessToken = (obj["access_token"] == null) ? null : obj["access_token"].ToString();
                        return accessToken;
                    }
                }
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

        #endregion

    }
}
