using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Admin.Models.PageContent
{
    public partial class HomepageModuleModel : BaseNopEntityModel
    {
        public string Name { get; set; }
        public int ModuleType { get; set; }
        public string ModuleTypeName { get; set; }
        public int DisplayOrder { get; set; }
        public bool Published { get; set; }
        [UIHint("Picture")]
        public int PictureId1 { get; set; }
        public string Url1 { get; set; }
        [UIHint("Picture")]
        public int PictureId2 { get; set; }
        public string Url2 { get; set; }
        [UIHint("Picture")]
        public int PictureId3 { get; set; }
        public string Url3 { get; set; }
        [UIHint("Picture")]
        public int PictureId4 { get; set; }
        public string Url4 { get; set; }
        [UIHint("Picture")]
        public int PictureId5 { get; set; }
        public string Url5 { get; set; }
        [UIHint("Picture")]
        public int PictureId6 { get; set; }
        public string Url6 { get; set; }
        [UIHint("Picture")]
        public int PictureId7 { get; set; }
        public string Url7 { get; set; }
        [UIHint("Picture")]
        public int PictureId8 { get; set; }
        public string Url8 { get; set; }
        [UIHint("Picture")]
        public int PictureId9 { get; set; }
        public string Url9 { get; set; }
        [UIHint("Picture")]
        public int PictureId10 { get; set; }
        public string Url10 { get; set; }
    }
}
