using System.Collections.Generic;
using Nop.Core.Domain.Localization;

namespace Nop.Core.Domain.Catalog
{
    /// <summary>
    /// Represents a product tag
    /// </summary>
    public partial class ProductTag : BaseEntity, ILocalizedEntity
    {
        private ICollection<Product> _products;
        private ICollection<ProductGroup> _productGroups;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        public bool Recommended { get; set; }

        public int PictureId { get; set; }

        public int DisplayOrder { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the products
        /// </summary>
        public virtual ICollection<Product> Products
        {
            get { return _products ?? (_products = new List<Product>()); }
            protected set { _products = value; }
        }

        public virtual ICollection<ProductGroup> ProductGroups
        {
            get { return _productGroups ?? (_productGroups = new List<ProductGroup>()); }
            protected set { _productGroups = value; }
        }
    }
}
