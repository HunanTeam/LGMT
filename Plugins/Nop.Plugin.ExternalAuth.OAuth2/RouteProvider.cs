using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.ExternalAuth.OAuth2
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.ExternalAuth.OAuth2.QQ.Login",
                    "Plugins/ExternalAuthOAuth2/QQLogin",
                    new { controller = "ExternalAuthOAuth2", action = "QQLogin" },
                    new[] { "Nop.Plugin.ExternalAuth.OAuth2.Controllers" }
               );

            routes.MapRoute("Plugin.ExternalAuth.OAuth2.QQ.LoginCallback",
                 "Plugins/ExternalAuthOAuth2/QQLoginCallback",
                 new { controller = "ExternalAuthOAuth2", action = "QQLoginCallback" },
                 new[] { "Nop.Plugin.ExternalAuth.OAuth2.Controllers" }
            );

            routes.MapRoute("Plugin.ExternalAuth.OAuth2.WeiBo.Login",
                 "Plugins/ExternalAuthOAuth2/WeiBoLogin",
                 new { controller = "ExternalAuthOAuth2", action = "WeiBoLogin" },
                 new[] { "Nop.Plugin.ExternalAuth.OAuth2.Controllers" }
            );

            routes.MapRoute("Plugin.ExternalAuth.OAuth2.WeiBo.LoginCallback",
                 "Plugins/ExternalAuthOAuth2/WeiBoLoginCallback",
                 new { controller = "ExternalAuthOAuth2", action = "WeiBoLoginCallback" },
                 new[] { "Nop.Plugin.ExternalAuth.OAuth2.Controllers" }
            );

            routes.MapRoute("Plugin.ExternalAuth.OAuth2.WeChat.Login",
                "Plugins/ExternalAuthOAuth2/WeChatLogin",
                new { controller = "ExternalAuthOAuth2", action = "WeChatLogin" },
                new[] { "Nop.Plugin.ExternalAuth.OAuth2.Controllers" }
            );
        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
