using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Core.Plugins;
using Nop.Plugin.ExternalAuth.OAuth2.Core;
using Nop.Plugin.ExternalAuth.OAuth2.Models;
using Nop.Services.Authentication;
using Nop.Services.Authentication.External;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.ExternalAuth.OAuth2.Controllers
{
    public class ExternalAuthOAuth2Controller : Controller
    {
        private readonly ISettingService _settingService;
        private readonly OAuth2ExternalAuthSettings _oAuth2ExternalAuthSettings;
        private readonly IOAuthProviderOAuth2Authorizer _ioAuthProviderOAuthProviderIoAuthProviderOAuth2Authorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly IPermissionService _permissionService;
        private readonly IStoreContext _storeContext;
        private readonly IPluginFinder _pluginFinder;
        //  private readonly IWxCustomerService _wxCustomerService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerService _customerService;
        private readonly IPictureService _pictureService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly LocalizationSettings _localizationSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly IAuthenticationService _authenticationService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILogger _logger;
        public ExternalAuthOAuth2Controller(ISettingService settingService,
            OAuth2ExternalAuthSettings oAuth2ExternalAuthSettings,
            IOAuthProviderOAuth2Authorizer ioAuthProviderOAuthProviderIoAuthProviderOAuth2Authorizer,
            IOpenAuthenticationService openAuthenticationService,
            ExternalAuthenticationSettings externalAuthenticationSettings,
            IPermissionService permissionService,
            IStoreContext storeContext, IPluginFinder pluginFinder,
            //IWxCustomerService wxCustomerService,
            IWorkContext workContext, ICustomerRegistrationService customerRegistrationService,
            ICustomerService customerService,
            IPictureService pictureService, IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService, LocalizationSettings localizationSettings,
             CustomerSettings customerSettings, IAuthenticationService authenticationService,
             IWorkflowMessageService workflowMessageService, ICustomerActivityService customerActivityService,
             ILogger logger
            )
        {
            _settingService = settingService;
            _oAuth2ExternalAuthSettings = oAuth2ExternalAuthSettings;
            _ioAuthProviderOAuthProviderIoAuthProviderOAuth2Authorizer = ioAuthProviderOAuthProviderIoAuthProviderOAuth2Authorizer;
            _openAuthenticationService = openAuthenticationService;
            _externalAuthenticationSettings = externalAuthenticationSettings;
            _permissionService = permissionService;
            _storeContext = storeContext;
            _pluginFinder = pluginFinder;
            // _wxCustomerService = wxCustomerService;
            _workContext = workContext;
            _customerRegistrationService = customerRegistrationService;
            _customerService = customerService;
            _pictureService = pictureService;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _localizationSettings = localizationSettings;
            _customerSettings = customerSettings;
            _authenticationService = authenticationService;
            _workflowMessageService = workflowMessageService;
            _customerActivityService = customerActivityService;
            _logger = logger;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return Content("Access denied");

            var model = new ConfigurationModel
            {
                QQEnabled = _oAuth2ExternalAuthSettings.QQEnabled,
                QQClientKeyIdentifier = _oAuth2ExternalAuthSettings.QQClientKeyIdentifier,
                QQClientSecret = _oAuth2ExternalAuthSettings.QQClientSecret,

                WeiBoEnabled = _oAuth2ExternalAuthSettings.WeiBoEnabled,
                WeiBoClientKeyIdentifier = _oAuth2ExternalAuthSettings.WeiBoClientKeyIdentifier,
                WeiBoClientSecret = _oAuth2ExternalAuthSettings.WeiBoClientSecret
            };

            return View("~/Plugins/ExternalAuth.OAuth2/Views/ExternalAuthOAuth2/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return Content("Access denied");

            if (!ModelState.IsValid)
                return Configure();

            //save settings
            _oAuth2ExternalAuthSettings.QQEnabled = model.QQEnabled;
            _oAuth2ExternalAuthSettings.QQClientKeyIdentifier = model.QQClientKeyIdentifier;
            _oAuth2ExternalAuthSettings.QQClientSecret = model.QQClientSecret;

            _oAuth2ExternalAuthSettings.WeiBoEnabled = model.WeiBoEnabled;
            _oAuth2ExternalAuthSettings.WeiBoClientKeyIdentifier = model.WeiBoClientKeyIdentifier;
            _oAuth2ExternalAuthSettings.WeiBoClientSecret = model.WeiBoClientSecret;

            _settingService.SaveSetting(_oAuth2ExternalAuthSettings);

            return View("~/Plugins/ExternalAuth.OAuth2/Views/ExternalAuthOAuth2/Configure.cshtml", model);
        }

        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            var model = new PublicInfoModel
            {
                QQEnabled = _oAuth2ExternalAuthSettings.QQEnabled,
                WeiBoEnabled = _oAuth2ExternalAuthSettings.WeiBoEnabled
            };
            return View("~/Plugins/ExternalAuth.OAuth2/Views/ExternalAuthOAuth2/PublicInfo.cshtml", model);
        }

        [NonAction]
        private ActionResult LoginInternal(string returnUrl, bool verifyResponse, ClientType clientType)
        {
            _logger.Debug(string.Format("登录LoginInternal：{0}", clientType));
            var processor = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName("ExternalAuth.OAuth2");
            if (processor == null ||
                !processor.IsMethodActive(_externalAuthenticationSettings) ||
                !processor.PluginDescriptor.Installed ||
                !_pluginFinder.AuthenticateStore(processor.PluginDescriptor, _storeContext.CurrentStore.Id))
                throw new NopException("OAuth2 module cannot be loaded");

            var viewModel = new LoginModel();
            TryUpdateModel(viewModel);

            _ioAuthProviderOAuthProviderIoAuthProviderOAuth2Authorizer.ClientType = clientType;
            _logger.Debug(string.Format("登录Result  Authorize beign clientType：{0}", clientType));
            var result = _ioAuthProviderOAuthProviderIoAuthProviderOAuth2Authorizer.Authorize(returnUrl, verifyResponse);
            _logger.Debug(string.Format("登录Result Authorize after AuthenticationStatus：{0}", result.AuthenticationStatus));
            switch (result.AuthenticationStatus)
            {
                case OpenAuthenticationStatus.Error:
                    {
                        if (!result.Success)
                        {
                            foreach (var error in result.Errors)
                            {
                                ExternalAuthorizerHelper.AddErrorsToDisplay(error);
                            }
                            _logger.Debug(string.Format("登录Result,ERROR：{0}", string.Concat<string>(result.Errors)));
                        }

                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                case OpenAuthenticationStatus.AssociateOnLogon:
                    {
                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                case OpenAuthenticationStatus.AutoRegisteredEmailValidation:
                    {
                        //result
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.EmailValidation });
                    }
                case OpenAuthenticationStatus.AutoRegisteredAdminApproval:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.AdminApproval });
                    }
                case OpenAuthenticationStatus.AutoRegisteredStandard:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Standard });
                    }
                default:
                    break;
            }

            if (result.Result != null)
                return result.Result;

            return HttpContext.Request.IsAuthenticated ?
                new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/")
                : new RedirectResult(Url.LogOn(returnUrl));
        }

        public ActionResult QQLogin(string returnUrl)
        {
            //if (ExternalAuthorizerHelperEx.IsWxLoginOrRegister)
            //    //PrepareWxLoginOrRegister();

            return LoginInternal(returnUrl, false, ClientType.QQ);
        }

        public ActionResult QQLoginCallback(string returnUrl)
        {
            return LoginInternal(returnUrl, true, ClientType.QQ);
        }

        public ActionResult WeiBoLogin(string returnUrl)
        {
            //if (ExternalAuthorizerHelperEx.IsWxLoginOrRegister)
            //   // PrepareWxLoginOrRegister();

            return LoginInternal(returnUrl, false, ClientType.WeiBo);
        }

        public ActionResult WeiBoLoginCallback(string returnUrl)
        {
            return LoginInternal(returnUrl, true, ClientType.WeiBo);
        }

        #region 微信一键注册登录

        //public ActionResult WeChatLogin(string returnUrl)
        //{
        //    if (!ExternalAuthorizerHelperEx.IsWxLoginOrRegister)
        //        return new HttpUnauthorizedResult();

        //    var openId = ExternalAuthorizerHelperEx.GetWxCustomerOpenId();
        //    var plantform = ExternalAuthorizerHelperEx.GetWxCustomerPlantform();
        //    var wxCustomer = _wxCustomerService.GetWxCustomerByOpenId(openId);
        //    if (wxCustomer == null)
        //        return new HttpUnauthorizedResult();

        //    if (!wxCustomer.IsBind || !wxCustomer.CustomerId.HasValue)
        //    {
        //        Customer customer = null;
        //        if (wxCustomer.LastestWxCustomerId.HasValue)
        //            customer = _customerService.GetCustomerById(wxCustomer.LastestWxCustomerId.Value);

        //        if (customer == null)
        //        {
        //            //如果未绑定过nop用户，自动创建一个用户并绑定
        //            //以下自动注册用户的代码来源于方法
        //            //Nop.Services.Authentication.External.ExternalAuthorizer.Authorize(OpenAuthenticationParameters parameters)
        //            #region 创建一个新的Nop账号

        //            if (_workContext.CurrentCustomer.IsRegistered())
        //            {
        //                //Already registered customer. 
        //                _authenticationService.SignOut();
        //                //Save a new record
        //                _workContext.CurrentCustomer = _customerService.InsertGuestCustomer();
        //            }

        //            customer = _workContext.CurrentCustomer;
        //            var randomPassword = CommonHelper.GenerateRandomDigitCode(20);

        //            bool isApproved = true;
        //            var registrationRequest = new CustomerRegistrationRequest(customer, null,
        //                null, randomPassword, PasswordFormat.Clear, _storeContext.CurrentStore.Id, isApproved);
        //            var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
        //            if (registrationResult.Success)
        //            {
        //                string username = (10000000 + customer.Id).ToString();
        //                _customerRegistrationService.SetUsername(customer, username);

        //                //昵称
        //                if (!String.IsNullOrEmpty(wxCustomer.NickName))
        //                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.FirstName, wxCustomer.NickName);
        //                //性别    
        //                if (wxCustomer.Sex != 0)
        //                {
        //                    var gender = wxCustomer.Sex == 1 ? "M" : "F"; //TODO: 规范代码
        //                    _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.Gender, gender);
        //                }
        //                //头像
        //                if (!String.IsNullOrEmpty(wxCustomer.HeadImageUrl))
        //                {
        //                    var fileBinary = CommonHelperEx.GetRemotePictureBytes(wxCustomer.HeadImageUrl);
        //                    if (fileBinary != null)
        //                    {
        //                        try
        //                        {
        //                            var mimeType = CommonHelperEx.GetPictureMimeType(fileBinary);
        //                            var picture = _pictureService.InsertPicture(fileBinary, mimeType, string.Empty);
        //                            _genericAttributeService.SaveAttribute(customer, SystemCustomerAttributeNames.AvatarPictureId, picture.Id);
        //                        }
        //                        catch { }
        //                    }
        //                }

        //                //notifications
        //                //if (_customerSettings.NotifyNewCustomerRegistration)
        //                //_workflowMessageService.SendCustomerRegisteredNotificationMessage(customer, _localizationSettings.DefaultAdminLanguageId);

        //                //authenticate
        //                //_authenticationService.SignIn(customer, false);
        //                ////activity log
        //                //_customerActivityService.InsertActivity("PublicStore.Login", _localizationService.GetResource("ActivityLog.PublicStore.Login"),
        //                //    customer);
        //            }

        //            #endregion
        //        }

        //        if (wxCustomer.LastestWxCustomerId != customer.Id)
        //        {
        //            wxCustomer.LastestWxCustomerId = customer.Id;
        //            _wxCustomerService.UpdateWxCustomer(wxCustomer);
        //        }
        //        _wxCustomerService.BindWxCustomer(customer, wxCustomer);
        //    }

        //    ExternalAuthorizerHelperEx.RemoveWxCustomerParameters();
        //    return RedirectToRoute("WxCustomerExternalAuthResult", new
        //    {
        //        openId = openId,
        //        plantform = plantform,
        //        clientType = "wx",
        //        returnUrl = returnUrl
        //    });
        //}

        //private ActionResult PrepareWxLoginOrRegisterResult(ClientType clientType)
        //{
        //    var openId = ExternalAuthorizerHelperEx.GetWxCustomerOpenId();
        //    var plantform = ExternalAuthorizerHelperEx.GetWxCustomerPlantform();
        //    var returnUrl = ExternalAuthorizerHelperEx.GetWxCustomerReturnUrl();
        //    var wxCustomer = _wxCustomerService.GetWxCustomerByOpenId(openId);
        //    if (wxCustomer != null)
        //        _wxCustomerService.BindWxCustomer(_workContext.CurrentCustomer, wxCustomer);

        //    ExternalAuthorizerHelperEx.RemoveWxCustomerParameters();
        //    return RedirectToRoute("WxCustomerExternalAuthResult", new
        //    {
        //        openId = openId,
        //        plantform = plantform,
        //        clientType = clientType.ToString().ToLowerInvariant(),
        //        returnUrl = returnUrl
        //    });
        //}

        //private void PrepareWxLoginOrRegister()
        //{
        //    if (_workContext.CurrentCustomer.IsRegistered())
        //    {
        //        //Already registered customer. 
        //        _authenticationService.SignOut();
        //        //Save a new record
        //        _workContext.CurrentCustomer = _customerService.InsertGuestCustomer();
        //    }
        //}

        #endregion
    }
}
