using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Stores;
using Nop.Core.Domain.PageContent;
using Nop.Services.Events;
using Nop.Services.Stores;

namespace Nop.Services.PageContent
{
    public partial class PageContentService : IPageContentService
    {
        private readonly IRepository<HomepageModule> _homepageModuleRepository;

        public PageContentService(IRepository<HomepageModule> homepageModuleRepository)
        {
            this._homepageModuleRepository = homepageModuleRepository;
        }

        /// <summary>
        /// Deletes a homepageModule
        /// </summary>
        /// <param name="homepageModule">HomepageModule</param>
        public virtual void DeleteHomepageModule(HomepageModule homepageModule)
        {
            if (homepageModule == null)
                throw new ArgumentNullException("homepageModule");

            _homepageModuleRepository.Delete(homepageModule);
        }

        /// <summary>
        /// Gets a homepageModule
        /// </summary>
        /// <param name="homepageModuleId">The homepageModule identifier</param>
        /// <returns>HomepageModule</returns>
        public virtual HomepageModule GetHomepageModuleById(int homepageModuleId)
        {
            if (homepageModuleId == 0)
                return null;

            return _homepageModuleRepository.GetById(homepageModuleId);
        }

        public virtual void InsertHomepageModule(HomepageModule homepageModule)
        {
            if (homepageModule == null)
                throw new ArgumentNullException("homepageModule");

            _homepageModuleRepository.Insert(homepageModule);
        }

        public virtual void UpdateHomepageModule(HomepageModule homepageModule)
        {
            if (homepageModule == null)
                throw new ArgumentNullException("homepageModule");

            _homepageModuleRepository.Update(homepageModule);
        }

        public virtual IPagedList<HomepageModule> GetAllHomepageModules(int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var query = _homepageModuleRepository.Table;

            if (!showHidden)
                query = query.Where(p => p.Published);

            query = query.OrderBy(t => t.DisplayOrder);

            return new PagedList<HomepageModule>(query, pageIndex, pageSize);
        }
    }
}
