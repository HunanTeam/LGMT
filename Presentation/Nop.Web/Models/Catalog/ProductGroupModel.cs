using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Catalog
{
    public partial class ProductGroupModel
    {
        public ProductGroupOverviewModel ProductGroup { get; set; }

        public List<ProductOverviewModel> Products { get; set; }
    }
}