using System;
using System.Linq;
using System.Web.Mvc;
using Nop.Admin.Extensions;
using Nop.Admin.Models.PageContent;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Kendoui;
using Nop.Core;
using Nop.Core.Domain.PageContent;
using Nop.Services.PageContent;

namespace Nop.Admin.Controllers
{
    public class PageContentController : BaseAdminController
    {
        private readonly IPageContentService _pageContentService;
        private readonly IPermissionService _permissionService;

        public PageContentController(IPageContentService pageContentService,
            IPermissionService permissionService)
        {
            this._pageContentService = pageContentService;
            this._permissionService = permissionService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();
            return View();
        }

        [HttpPost]
        public ActionResult List(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            var homepageModels = _pageContentService.GetAllHomepageModules(pageIndex: command.Page - 1, pageSize: command.PageSize, showHidden: true)
                .Select(x => x.ToModel())
                .ToList();

            var gridModel = new DataSourceResult
            {
                Data = homepageModels,
                Total = homepageModels.Count
            };

            return Json(gridModel);
        }

        #region Create / Edit / Delete

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            var model = new HomepageModuleModel();
            //default values
            model.DisplayOrder = 1;

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(HomepageModuleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var homepageModule = model.ToEntity();
                _pageContentService.InsertHomepageModule(homepageModule);
                return continueEditing ? RedirectToAction("Edit", new { id = homepageModule.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form     
            return View(model);
        }

        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            var homepageModule = _pageContentService.GetHomepageModuleById(id);
            if (homepageModule == null)
                //No topic found with the specified id
                return RedirectToAction("List");

            var model = homepageModule.ToModel();
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(HomepageModuleModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            var homepageModule = _pageContentService.GetHomepageModuleById(model.Id);
            if (homepageModule == null)
                //No topic found with the specified id
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                homepageModule = model.ToEntity(homepageModule);
                _pageContentService.UpdateHomepageModule(homepageModule);

                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabIndex();

                    return RedirectToAction("Edit", new { id = homepageModule.Id });
                }
                return RedirectToAction("List");
            }


            //If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageTopics))
                return AccessDeniedView();

            var topic = _pageContentService.GetHomepageModuleById(id);
            if (topic == null)
                //No topic found with the specified id
                return RedirectToAction("List");

            _pageContentService.DeleteHomepageModule(topic);

            return RedirectToAction("List");
        }

        #endregion
    }
}