using Nop.Core.Plugins;
using Nop.Services.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Nop.Plugin.ExternalAuth.OAuth2
{
    public class QQVerifyPlugin : BasePlugin, IWidgetPlugin
    {
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = string.Empty;
            controllerName = string.Empty;
            routeValues = null;
        }

        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "WidgetsQQVerify";
            routeValues = new RouteValueDictionary
            {
                {"Namespaces", "Nop.Plugin.ExternalAuth.QQ.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }

        public IList<string> GetWidgetZones()
        {
            return new List<string>() {
                "head_html_tag"
            };
        }
    }
}
