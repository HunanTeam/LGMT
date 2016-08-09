//Contributor:  Nicholas Mayne

using System;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Localization;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Media;

namespace Nop.Services.Authentication.External
{
    /// <summary>
    /// External authorizer
    /// </summary>
    public partial class ExternalAuthorizer : IExternalAuthorizer
    {
        #region Fields

        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ILocalizationService _localizationService;
        private readonly IWorkContext _workContext;
        private readonly CustomerSettings _customerSettings;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly LocalizationSettings _localizationSettings;

        private readonly IPictureService _pictureService;
        private readonly ILogger _logger;
        #endregion

        #region Ctor

        public ExternalAuthorizer(IAuthenticationService authenticationService,
            IOpenAuthenticationService openAuthenticationService,
            IGenericAttributeService genericAttributeService,
            ICustomerRegistrationService customerRegistrationService,
            ICustomerActivityService customerActivityService, ILocalizationService localizationService,
            IWorkContext workContext, CustomerSettings customerSettings,
            ExternalAuthenticationSettings externalAuthenticationSettings,
            IShoppingCartService shoppingCartService,
            IWorkflowMessageService workflowMessageService, LocalizationSettings localizationSettings,
            ILogger logger,
             IPictureService pictureService
            )
        {
            this._authenticationService = authenticationService;
            this._openAuthenticationService = openAuthenticationService;
            this._genericAttributeService = genericAttributeService;
            this._customerRegistrationService = customerRegistrationService;
            this._customerActivityService = customerActivityService;
            this._localizationService = localizationService;
            this._workContext = workContext;
            this._customerSettings = customerSettings;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._shoppingCartService = shoppingCartService;
            this._workflowMessageService = workflowMessageService;
            this._localizationSettings = localizationSettings;
            this._logger = logger;
            this._pictureService = pictureService;
        }

        #endregion

        #region Utilities

        private bool RegistrationIsEnabled()
        {
            return _customerSettings.UserRegistrationType != UserRegistrationType.Disabled && !_externalAuthenticationSettings.AutoRegisterEnabled;
        }

        private bool AutoRegistrationIsEnabled()
        {
            return _customerSettings.UserRegistrationType != UserRegistrationType.Disabled && _externalAuthenticationSettings.AutoRegisterEnabled;
        }

        private bool AccountDoesNotExistAndUserIsNotLoggedOn(Customer userFound, Customer userLoggedIn)
        {
            return userFound == null && userLoggedIn == null;
        }

        private bool AccountIsAssignedToLoggedOnAccount(Customer userFound, Customer userLoggedIn)
        {
            return userFound.Id.Equals(userLoggedIn.Id);
        }

        private bool AccountAlreadyExists(Customer userFound, Customer userLoggedIn)
        {
            return userFound != null && userLoggedIn != null;
        }

        #endregion

        #region Methods

        public virtual AuthorizationResult Authorize(OpenAuthenticationParameters parameters)
        {
            var userFound = _openAuthenticationService.GetUser(parameters);
            if (userFound != null)
            {
                _logger.Debug(string.Format(" ExternalAuthorizer@Authorize：找到第三方用户:[{0}]", "XX"));
            }

            var userLoggedIn = _workContext.CurrentCustomer.IsRegistered() ? _workContext.CurrentCustomer : null;
            _logger.Debug(string.Format(" ExternalAuthorizer@Authorize is null? userLoggedIn [{0}], userFound is null?[{1}]", userLoggedIn == null, userFound == null));
            if (userLoggedIn != null)
            {
                _logger.Debug(string.Format("  ExternalAuthorizer@Authorize  (userLoggedIn) is [{0}]", "XX"));
            }

            if (AccountAlreadyExists(userFound, userLoggedIn))
            {
                _logger.Debug(string.Format("  ExternalAuthorizer@Authorize  (AccountAlreadyExists){0}", "-XX"));
                if (AccountIsAssignedToLoggedOnAccount(userFound, userLoggedIn))
                {
                    _logger.Debug(string.Format("  ExternalAuthorizer@Authorize  (AccountIsAssignedToLoggedOnAccount){0}", "-XX"));
                    // The person is trying to log in as himself.. bit weird
                    return new AuthorizationResult(OpenAuthenticationStatus.Authenticated);
                }

                var result = new AuthorizationResult(OpenAuthenticationStatus.Error);
                result.AddError("Account is already assigned");
                return result;
            }
            if (AccountDoesNotExistAndUserIsNotLoggedOn(userFound, userLoggedIn))
            {
                _logger.Debug(string.Format(" ExternalAuthorizer@Authorize (AccountDoesNotExistAndUserIsNotLoggedOn) is {0}", " xx "));
                ExternalAuthorizerHelper.StoreParametersForRoundTrip(parameters);

                if (AutoRegistrationIsEnabled())
                {
                    #region Register user

                    var currentCustomer = _workContext.CurrentCustomer;
                    var details = new RegistrationDetails(parameters);
                    var randomPassword = CommonHelper.GenerateRandomDigitCode(20);


                    bool isApproved =
                        //standard registration
                        (_customerSettings.UserRegistrationType == UserRegistrationType.Standard) ||
                        //skip email validation?
                        (_customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation &&
                         !_externalAuthenticationSettings.RequireEmailValidation);

                    var registrationRequest = new CustomerRegistrationRequest(currentCustomer, string.Empty, details.EmailAddress,
                        _customerSettings.UsernamesEnabled ? details.UserName : details.EmailAddress, randomPassword, PasswordFormat.Clear, isApproved, RegisterSource.ThirdParty);
                    var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
                    if (registrationResult.Success)
                    {


                        #region 保存从QQ返回的用户信息

                        //昵称
                        if (!String.IsNullOrEmpty(details.Nickname))
                            _genericAttributeService.SaveAttribute(currentCustomer, SystemCustomerAttributeNames.FirstName, details.Nickname);
                        else
                            _genericAttributeService.SaveAttribute(currentCustomer, SystemCustomerAttributeNames.FirstName, currentCustomer.Username);
                        //性别
                        if (!String.IsNullOrEmpty(details.Gender))
                        {
                            var genderStr = string.Empty;
                            if (details.Gender == "男" || string.Compare(details.Gender, "male",true) == 0
                                || string.Compare(details.Gender, "male", true) == 0
                                || string.Compare(details.Gender, "m", true) == 0
                                || string.Compare(details.Gender, "man", true) == 0
                                )
                            {
                                genderStr = "M";
                            }
                            else
                            {
                                genderStr = "F";
                            }
                            
                            _genericAttributeService.SaveAttribute(currentCustomer, SystemCustomerAttributeNames.Gender, genderStr);
                        }
                        //头像
                        if (!String.IsNullOrEmpty(details.FigureUrl))
                        {
                            var fileBinary = CommonHelper.GetRemotePictureBytes(details.FigureUrl);
                            if (fileBinary != null)
                            {
                                try
                                {
                                    var mimeType = CommonHelper.GetPictureMimeType(fileBinary);
                                    var picture = _pictureService.InsertPicture(fileBinary, mimeType, null);
                                    _genericAttributeService.SaveAttribute(currentCustomer, SystemCustomerAttributeNames.AvatarPictureId, picture.Id);
                                }
                                catch { }
                            }
                        }

                        #endregion

                        var username = _customerRegistrationService.GenerateUsername(currentCustomer.Id);
                        _customerRegistrationService.SetUsername(currentCustomer, username);

                        userFound = currentCustomer;
                        _openAuthenticationService.AssociateExternalAccountWithUser(currentCustomer, parameters);
                        ExternalAuthorizerHelper.RemoveParameters();

                        //code below is copied from CustomerController.Register method

                        //authenticate
                        if (isApproved)
                            _authenticationService.SignIn(userFound ?? userLoggedIn, false);

                        //notifications
                        if (_customerSettings.NotifyNewCustomerRegistration)
                            _workflowMessageService.SendCustomerRegisteredNotificationMessage(currentCustomer, _localizationSettings.DefaultAdminLanguageId);

                        if (isApproved)
                        {
                            //standard registration
                            //or
                            //skip email validation

                            //send customer welcome message
                            //do:删除欢迎用户的邮件
                            //_workflowMessageService.SendCustomerWelcomeMessage(currentCustomer, _workContext.WorkingLanguage.Id);

                            //result
                            return new AuthorizationResult(OpenAuthenticationStatus.AutoRegisteredStandard);
                        }
                        else if (_customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation)
                        {
                            //email validation message
                            _genericAttributeService.SaveAttribute(currentCustomer, SystemCustomerAttributeNames.AccountActivationToken, Guid.NewGuid().ToString());
                            _workflowMessageService.SendCustomerEmailValidationMessage(currentCustomer, _workContext.WorkingLanguage.Id);

                            //result
                            return new AuthorizationResult(OpenAuthenticationStatus.AutoRegisteredEmailValidation);
                        }
                        else if (_customerSettings.UserRegistrationType == UserRegistrationType.AdminApproval)
                        {
                            //result
                            return new AuthorizationResult(OpenAuthenticationStatus.AutoRegisteredAdminApproval);
                        }
                    }
                    else
                    {
                        ExternalAuthorizerHelper.RemoveParameters();

                        var result = new AuthorizationResult(OpenAuthenticationStatus.Error);
                        foreach (var error in registrationResult.Errors)
                            result.AddError(string.Format(error));
                        return result;
                    }

                    #endregion
                }
                else if (RegistrationIsEnabled())
                {
                    return new AuthorizationResult(OpenAuthenticationStatus.AssociateOnLogon);
                }
                else
                {
                    ExternalAuthorizerHelper.RemoveParameters();

                    var result = new AuthorizationResult(OpenAuthenticationStatus.Error);
                    result.AddError("Registration is disabled");
                    return result;
                }
            }
            if (userFound == null)
            {
                _openAuthenticationService.AssociateExternalAccountWithUser(userLoggedIn, parameters);
            }
            _logger.Debug(string.Format(" ExternalAuthorizer@Authorize  MigrateShoppingCart begin {0}", " xx "));
            //migrate shopping cart
            _shoppingCartService.MigrateShoppingCart(_workContext.CurrentCustomer, userFound ?? userLoggedIn, true);
            _logger.Debug(string.Format(" ExternalAuthorizer@Authorize  SignIn begin {0}", " xx "));
            //authenticate
            _authenticationService.SignIn(userFound ?? userLoggedIn, false);
            if (_logger.IsEnabled(Core.Domain.Logging.LogLevel.Debug))
            {
                _logger.Debug(string.Format("ExternalAuthorizer@Authorize SignIn end ：{0}", "XX"));
            }
            _logger.Debug(string.Format("ExternalAuthorizer@Authorize PublicStore.Login end ID：{0}", (userFound ?? userLoggedIn).Id));
            //activity log
            _customerActivityService.InsertActivity("PublicStore.Login", _localizationService.GetResource("ActivityLog.PublicStore.Login"),
                userFound ?? userLoggedIn);


            return new AuthorizationResult(OpenAuthenticationStatus.Authenticated);
        }

        #endregion
    }
}