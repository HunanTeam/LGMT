using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Product tag service interface
    /// </summary>
    public partial interface IProductGroupService
    {
        /// <summary>
        /// Delete a product tag
        /// </summary>
        /// <param name="productGroup">Product tag</param>
        void DeleteProductGroup(ProductGroup productGroup);

        /// <summary>
        /// Gets all product groups
        /// </summary>
        /// <returns>Product groups</returns>
        IList<ProductGroup> GetAllProductGroups();

        /// <summary>
        /// Gets product tag
        /// </summary>
        /// <param name="productGroupId">Product tag identifier</param>
        /// <returns>Product tag</returns>
        ProductGroup GetProductGroupById(int productGroupId);

        /// <summary>
        /// Gets product tag by name
        /// </summary>
        /// <param name="name">Product tag name</param>
        /// <returns>Product tag</returns>
        ProductGroup GetProductGroupByName(string name);

        /// <summary>
        /// Inserts a product tag
        /// </summary>
        /// <param name="productGroup">Product tag</param>
        void InsertProductGroup(ProductGroup productGroup);

        /// <summary>
        /// Updates the product tag
        /// </summary>
        /// <param name="productGroup">Product tag</param>
        void UpdateProductGroup(ProductGroup productGroup);

        IPagedList<ProductGroupProduct> GetProductsByProductGroupId(int productGroupId,
        int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        IPagedList<ProductGroup> GetProductGroups(int pageIndex = 0, int pageSize = int.MaxValue,
         int? productTagId = null, bool showHidden = false);

        void InsertProductGroupProduct(ProductGroupProduct productGroupProduct);
    }
}
