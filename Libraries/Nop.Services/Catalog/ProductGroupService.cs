using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Common;
using Nop.Data;
using Nop.Services.Events;

namespace Nop.Services.Catalog
{
    /// <summary>
    /// Product tag service
    /// </summary>
    public partial class ProductGroupService : IProductGroupService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : store ID
        /// </remarks>
        private const string ProductGroup_COUNT_KEY = "Nop.ProductGroup.count-{0}";

        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ProductGroup_PATTERN_KEY = "Nop.ProductGroup.";

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// {1} : category ID
        /// {2} : page index
        /// {3} : page size
        /// {4} : current customer ID
        /// {5} : store ID
        /// </remarks>
        private const string PRODUGROUPPRODUCTS_ALLBYGROUPID_KEY = "Nop.productgroupproduct.allbyproductgroupid-{0}-{1}-{2}-{3}-{4}-{5}";

        #endregion

        #region Fields

        private readonly IRepository<ProductGroup> _productGroupRepository;
        private readonly IRepository<ProductGroupProduct> _productGroupProductRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductTag> _productTagRepository;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly CommonSettings _commonSettings;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="productGroupRepository">Product tag repository</param>
        /// <param name="dataProvider">Data provider</param>
        /// <param name="dbContext">Database Context</param>
        /// <param name="commonSettings">Common settings</param>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="eventPublisher">Event published</param>
        public ProductGroupService(IRepository<ProductGroup> productGroupRepository,
            IRepository<ProductGroupProduct> productGroupProductRepository,
            IRepository<Product> productRepository,
            IRepository<ProductTag> productTagRepository,
            IDataProvider dataProvider,
            IDbContext dbContext,
            IWorkContext workContext,
            IStoreContext storeContext,
            CommonSettings commonSettings,
            ICacheManager cacheManager,
            IEventPublisher eventPublisher)
        {
            this._productGroupRepository = productGroupRepository;
            this._productGroupProductRepository = productGroupProductRepository;
            this._productRepository = productRepository;
            this._productTagRepository = productTagRepository;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._commonSettings = commonSettings;
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Nested classes

        private class ProductGroupWithCount
        {
            public int ProductGroupId { get; set; }
            public int ProductCount { get; set; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Delete a product tag
        /// </summary>
        /// <param name="ProductGroup">Product tag</param>
        public virtual void DeleteProductGroup(ProductGroup productGroup)
        {
            if (productGroup == null)
                throw new ArgumentNullException("productGroup");

            _productGroupRepository.Delete(productGroup);

            //cache
            _cacheManager.RemoveByPattern(ProductGroup_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(productGroup);
        }

        /// <summary>
        /// Gets all product tags
        /// </summary>
        /// <returns>Product tags</returns>
        public virtual IList<ProductGroup> GetAllProductGroups()
        {
            var query = _productGroupRepository.Table;
            var productGroups = query.ToList();
            return productGroups;
        }

        /// <summary>
        /// Gets product tag
        /// </summary>
        /// <param name="productGroupId">Product tag identifier</param>
        /// <returns>Product tag</returns>
        public virtual ProductGroup GetProductGroupById(int productGroupId)
        {
            if (productGroupId == 0)
                return null;

            return _productGroupRepository.GetById(productGroupId);
        }

        /// <summary>
        /// Gets product tag by name
        /// </summary>
        /// <param name="name">Product tag name</param>
        /// <returns>Product tag</returns>
        public virtual ProductGroup GetProductGroupByName(string name)
        {
            var query = from pt in _productGroupRepository.Table
                        where pt.Name == name
                        select pt;

            var ProductGroup = query.FirstOrDefault();
            return ProductGroup;
        }

        /// <summary>
        /// Inserts a product tag
        /// </summary>
        /// <param name="ProductGroup">Product tag</param>
        public virtual void InsertProductGroup(ProductGroup productGroup)
        {
            if (productGroup == null)
                throw new ArgumentNullException("productGroup");

            _productGroupRepository.Insert(productGroup);

            //cache
            _cacheManager.RemoveByPattern(ProductGroup_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(productGroup);
        }

        /// <summary>
        /// Updates the product tag
        /// </summary>
        /// <param name="ProductGroup">Product tag</param>
        public virtual void UpdateProductGroup(ProductGroup productGroup)
        {
            if (productGroup == null)
                throw new ArgumentNullException("productGroup");

            _productGroupRepository.Update(productGroup);

            //cache
            _cacheManager.RemoveByPattern(ProductGroup_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(productGroup);
        }

        public virtual IPagedList<ProductGroupProduct> GetProductsByProductGroupId(int productGroupId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (productGroupId == 0)
                return new PagedList<ProductGroupProduct>(new List<ProductGroupProduct>(), pageIndex, pageSize);

            var query = from pgp in _productGroupProductRepository.Table
                        join p in _productRepository.Table on pgp.ProductId equals p.Id
                        where pgp.ProductGroupId == productGroupId &&
                              !p.Deleted && (showHidden || p.Published)
                        orderby pgp.DisplayOrder
                        select pgp;

            var productGroupProducts = new PagedList<ProductGroupProduct>(query, pageIndex, pageSize);
            return productGroupProducts;
        }

        public virtual IPagedList<ProductGroup> GetProductGroups(int pageIndex = 0, int pageSize = int.MaxValue,
         int? productTagId = null, bool showHidden = false)
        {
            var query = _productGroupRepository.Table.Where(p => showHidden || p.Published);

            if (productTagId.HasValue && productTagId.Value > 0)
                query = query.Where(p => p.ProductTags.Select(pt => pt.Id).Contains(productTagId.Value));

            query = query.OrderBy(p => p.DisplayOrder);

            var productGroups = new PagedList<ProductGroup>(query, pageIndex, pageSize);
            return productGroups;
        }

        public virtual void InsertProductGroupProduct(ProductGroupProduct productGroupProduct)
        {
            if (productGroupProduct == null)
                throw new ArgumentNullException("productGroupProduct");

            _productGroupProductRepository.Insert(productGroupProduct);
        }

        #endregion
    }
}
