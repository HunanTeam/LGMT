using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.UI.Paging;

namespace Nop.Web.Models.Catalog
{
    public partial class ProductGroupsModel : BaseNopModel
    {
        public ProductGroupsModel()
        {
            this.ProductGroups = new List<ProductGroupOverviewModel>();
            this.AvailableTags = new List<SelectListItem>();
        }

        public bool NoResults { get; set; }
        public ProductGroupsFilteringModel PagingFilteringContext { get; set; }
        public IList<ProductGroupOverviewModel> ProductGroups { get; set; }
        public IList<SelectListItem> AvailableTags { get; set; }

        /// <summary>
        /// 标签Id
        /// </summary>
        public int tid { get; set; }

        public partial class ProductGroupsFilteringModel : BasePageableModel
        {
            public ProductGroupsFilteringModel() { }
        }
    }
}