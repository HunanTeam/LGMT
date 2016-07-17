using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Web.Models.PageContent
{
    public partial class HomepageModuleModel : BaseNopEntityModel
    {
        public string Name { get; set; }
        public int ModuleType { get; set; }
        public int DisplayOrder { get; set; }
        public bool Published { get; set; }
        public string PictureUrl1 { get; set; }
        public string Url1 { get; set; }
        public string PictureUrl2 { get; set; }
        public string Url2 { get; set; }
        public string PictureUrl3 { get; set; }
        public string Url3 { get; set; }
        public string PictureUrl4 { get; set; }
        public string Url4 { get; set; }
        public string PictureUrl5 { get; set; }
        public string Url5 { get; set; }
        public string PictureUrl6 { get; set; }
        public string Url6 { get; set; }
        public string PictureUrl7 { get; set; }
        public string Url7 { get; set; }
        public string PictureUrl8 { get; set; }
        public string Url8 { get; set; }
        public string PictureUrl9 { get; set; }
        public string Url9 { get; set; }
        public string PictureUrl10 { get; set; }
        public string Url10 { get; set; }
    }
}
