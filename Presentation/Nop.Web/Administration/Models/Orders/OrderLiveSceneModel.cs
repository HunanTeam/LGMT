
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Admin.Models.Common;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Tax;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Nop.Admin.Models.Orders
{
    public class OrderLiveSceneModel : BaseNopEntityModel
    {
        public string Room { get; set; }
        public string Window { get; set; }
        [UIHint("Picture")]
        public int OriginalPictureId { get; set; }
        public string OriginalPictureUrl { get; set; }
        [UIHint("Picture")]
        public int EffectPictureId { get; set; }
        public string EffectPictureUrl { get; set; }
        public int DisplayOrder { get; set; }
    }
}