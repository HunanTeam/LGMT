using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Web.Models.Catalog
{
    public class HomepageProductTagsModel
    {
        public HomepageProductTagsModel()
        {
            this.Products = new List<ProductOverviewModel>();
            this.ProductTags = new List<ProductTagModel>();
        }

        public List<ProductOverviewModel> Products { get; set; }
        public List<ProductTagModel> ProductTags { get; set; }
    }
}