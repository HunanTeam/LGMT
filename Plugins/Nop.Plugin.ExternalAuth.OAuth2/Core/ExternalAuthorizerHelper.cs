using Nop.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Nop.Services.Authentication.External
{
    public static partial class ExternalAuthorizerHelperEx
    {
        #region 微信认证的用户信息

        public static bool IsWxLoginOrRegister
        {
            get
            {
                var openId = ExternalAuthorizerHelperEx.GetWxCustomerOpenId();
                var plantform = ExternalAuthorizerHelperEx.GetWxCustomerPlantform();
                return (!string.IsNullOrEmpty(openId) && plantform.HasValue);
            }
        }

        public static void StoreWxCustomerOpenId(string openId)
        {
            var session = GetSession();
            session["wx.customer.openid"] = openId;
        }

        public static void StoreWxCustomerPlantform(int plantform)
        {
            var session = GetSession();
            session["wx.customer.plantform"] = plantform;
        }

        public static void StoreWxCustomerReturnUrl(string returnUrl)
        {
            var session = GetSession();
            session["wx.customer.returnurl"] = returnUrl;
        }

        public static string GetWxCustomerOpenId()
        {
            var session = GetSession();
            return session["wx.customer.openid"] as string;
        }

        public static int? GetWxCustomerPlantform()
        {
            var session = GetSession();
            return session["wx.customer.plantform"] as int?;
        }

        public static string GetWxCustomerReturnUrl()
        {
            var session = GetSession();
            return session["wx.customer.returnurl"] as string;
        }

        public static void RemoveWxCustomerParameters()
        {
            var session = GetSession();
            session.Remove("wx.customer.openid");
            session.Remove("wx.customer.plantform");
            session.Remove("wx.customer.returnurl");
        }
        private static HttpSessionStateBase GetSession()
        {
            var session = EngineContext.Current.Resolve<HttpSessionStateBase>();
            return session;
        }
        #endregion
    }
}
