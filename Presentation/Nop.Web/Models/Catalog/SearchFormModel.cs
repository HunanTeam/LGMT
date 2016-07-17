using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.Catalog
{
    public partial class SearchFormModel : BaseNopModel
    {
        public SearchFormModel()
        {
            this.AvailableCategories = new List<SelectListItem>();
            this.AvailableTags = new List<SelectListItem>();
        }

        public string Warning { get; set; }

        public bool NoResults { get; set; }

        public IList<SelectListItem> AvailableCategories { get; set; }
        public IList<SelectListItem> AvailableTags { get; set; }
    }
}