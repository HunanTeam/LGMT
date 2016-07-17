using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.PageContent;
using Nop.Core.Domain.Media;
using Nop.Services.PageContent;
using Nop.Services.Media;
using Nop.Web.Models.PageContent;

namespace Nop.Web.Controllers
{
    public class PageContentController : BasePublicController
    {
        private readonly IPageContentService _pageContentService;
        private readonly IPictureService _pictureService;

        public PageContentController(IPageContentService pageContentService,
            IPictureService pictureService)
        {
            this._pageContentService = pageContentService;
            this._pictureService = pictureService;
        }

        public ActionResult HomepageModules()
        {
            var moduleModels = _pageContentService.GetAllHomepageModules()
                .Select(x =>
                {
                    var model = new HomepageModuleModel
                    {
                        Id = x.Id,
                        DisplayOrder = x.DisplayOrder,
                        Name = x.Name,
                        ModuleType = x.ModuleType,
                        Url1 = x.Url1,
                        Url2 = x.Url2,
                        Url3 = x.Url3,
                        Url4 = x.Url4,
                        Url5 = x.Url5,
                        Url6 = x.Url6,
                        Url7 = x.Url7,
                        Url8 = x.Url8,
                        Url9 = x.Url9,
                        Url10 = x.Url10
                    };

                    model.PictureUrl1 = x.PictureId1 > 0 ?
                        _pictureService.GetPictureUrl(x.PictureId1) : null;
                    model.PictureUrl2 = x.PictureId2 > 0 ?
                        _pictureService.GetPictureUrl(x.PictureId2) : null;
                    model.PictureUrl3 = x.PictureId3 > 0 ?
                        _pictureService.GetPictureUrl(x.PictureId3) : null;
                    model.PictureUrl4 = x.PictureId4 > 0 ?
                        _pictureService.GetPictureUrl(x.PictureId4) : null;
                    model.PictureUrl5 = x.PictureId5 > 0 ?
                        _pictureService.GetPictureUrl(x.PictureId5) : null;
                    model.PictureUrl6 = x.PictureId6 > 0 ?
                        _pictureService.GetPictureUrl(x.PictureId6) : null;
                    model.PictureUrl7 = x.PictureId7 > 0 ?
                        _pictureService.GetPictureUrl(x.PictureId7) : null;
                    model.PictureUrl8 = x.PictureId8 > 0 ?
                        _pictureService.GetPictureUrl(x.PictureId8) : null;
                    model.PictureUrl9 = x.PictureId9 > 0 ?
                        _pictureService.GetPictureUrl(x.PictureId9) : null;
                    model.PictureUrl10 = x.PictureId10 > 0 ?
                        _pictureService.GetPictureUrl(x.PictureId10) : null;

                    return model;
                }).ToList();


            return View(moduleModels);
        }
    }
}