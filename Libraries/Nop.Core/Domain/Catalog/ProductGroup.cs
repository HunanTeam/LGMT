using System;
using System.Collections.Generic;
using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// ÉÌÆ·Ì×²Í
    /// </summary>
    public partial class ProductGroup : BaseEntity, ILocalizedEntity
    {
        private ICollection<ProductTag> _productTags;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        public bool Recommended { get; set; }

        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureId { get; set; }

        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the product tags
        /// </summary>
        public virtual ICollection<ProductTag> ProductTags
        {
            get { return _productTags ?? (_productTags = new List<ProductTag>()); }
            protected set { _productTags = value; }
        }
    }
}
