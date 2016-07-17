using System.Collections.Generic;
using Nop.Core.Domain.PageContent;
using Nop.Core;

namespace Nop.Services.PageContent
{
    public partial interface IPageContentService
    {
        #region Homepage content

        /// <summary>
        /// Inserts a homepageModule item
        /// </summary>
        /// <param name="homepageModule">HomepageModule item</param>
        void InsertHomepageModule(HomepageModule homepageModule);

        /// <summary>
        /// Updates the homepageModule item
        /// </summary>
        /// <param name="homepageModule">HomepageModule item</param>
        void UpdateHomepageModule(HomepageModule homepageModule);

        /// <summary>
        /// Deletes a homepageModule
        /// </summary>
        /// <param name="homepageModuleItem">HomepageModule item</param>
        void DeleteHomepageModule(HomepageModule homepageModuleItem);

        /// <summary>
        /// Gets a homepageModule
        /// </summary>
        /// <param name="homepageModuleId">The homepageModule identifier</param>
        /// <returns>HomepageModule</returns>
        HomepageModule GetHomepageModuleById(int homepageModuleId);

        IPagedList<HomepageModule> GetAllHomepageModules(int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        #endregion
    }
}
