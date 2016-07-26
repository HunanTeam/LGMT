using System.Web.Routing;
using Nop.Core.Plugins;
using Nop.Services.Authentication.External;
using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace Nop.Plugin.ExternalAuth.OAuth2
{
    /// <summary>
    /// Facebook externalAuth processor
    /// </summary>
    public class OAuth2ExternalAuthMethod : BasePlugin, IExternalAuthenticationMethod
    {
        #region Fields

        private readonly ISettingService _settingService;

        #endregion

        #region Ctor

        public OAuth2ExternalAuthMethod(ISettingService settingService)
        {
            this._settingService = settingService;
        }

        #endregion

        #region Methods
        
        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "ExternalAuthOAuth2";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.ExternalAuth.OAuth2.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying plugin in public store
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetPublicInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "ExternalAuthOAuth2";
            routeValues = new RouteValueDictionary { { "Namespaces", "Nop.Plugin.ExternalAuth.OAuth2.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //settings
            //settings
            var settings = new OAuth2ExternalAuthSettings
            {
                QQEnabled = false,
                QQClientKeyIdentifier = "",
                QQClientSecret = "",

                WeiBoEnabled = false,
                WeiBoClientKeyIdentifier = "",
                WeiBoClientSecret = "",
            };
          
            _settingService.SaveSetting(settings);

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Login", "Login using OAuth2 account");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Enable", "Enable");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Disable", "Disable");

            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQConnect", "Login By QQ");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQEnabled", "Enable");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQClientKeyIdentifier", "App ID/API Key");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQClientKeyIdentifier.Hint", "Enter your app ID/API key here. You can find it on your OAuth2 application page.");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQClientSecret", "App Secret");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQClientSecret.Hint", "Enter your app secret here. You can find it on your OAuth2 application page.");

            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoConnect", "Login By WeiBo");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoEnabled", "Enable");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoClientKeyIdentifier", "App ID/API Key");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoClientKeyIdentifier.Hint", "Enter your app ID/API key here. You can find it on your OAuth2 application page.");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoClientSecret", "App Secret");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoClientSecret.Hint", "Enter your app secret here. You can find it on your OAuth2 application page.");

            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatConnect", "Login By WeChat");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatEnabled", "Enable");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatClientKeyIdentifier", "App ID/API Key");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatClientKeyIdentifier.Hint", "Enter your app ID/API key here. You can find it on your OAuth2 application page.");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatClientSecret", "App Secret");
            this.AddOrUpdatePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatClientSecret.Hint", "Enter your app secret here. You can find it on your OAuth2 application page.");

            base.Install();
        }

        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<OAuth2ExternalAuthSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Login");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Enable");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.Disable");

            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQConnect");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQEnabled");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQClientKeyIdentifier");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQClientKeyIdentifier.Hint");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQClientSecret");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.QQClientSecret.Hint");

            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoConnect");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoEnabled");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoClientKeyIdentifier");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoClientKeyIdentifier.Hint");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoClientSecret");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeiBoClientSecret.Hint");

            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatConnect");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatEnabled");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatClientKeyIdentifier");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatClientKeyIdentifier.Hint");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatClientSecret");
            this.DeletePluginLocaleResource("Plugins.ExternalAuth.OAuth2.WeChatClientSecret.Hint");

            base.Uninstall();
        }

        #endregion
    }
}
