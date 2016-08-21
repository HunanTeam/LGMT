
using System;
using System.Collections.Generic;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Common;
using Nop.Web.Models.Media;
using Nop.Core.Domain.Orders;

namespace Nop.Web.Models.Order
{
    public partial class StepInfoModel : BaseNopEntityModel
    {
        public StepInfoModel()
        { }

        public int OrderId { get; set; }

        public OrderStep Step { get; set; }

        public string Description { get; set; }

        public bool HasUploadLiveScenes { get; set; }

        public bool? CurrentPayInOnline { get; set; }
    }
}