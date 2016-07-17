using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Catalog
{
    public partial class ProductTagModel : BaseNopEntityModel
    {
        public string Name { get; set; }

        public string SeName { get; set; }

        public string PictureUrl { get; set; }

        public int ProductCount { get; set; }

        public ProductOverviewModel Product { get; set; }
    }
}