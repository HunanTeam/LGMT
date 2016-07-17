using System;
using System.Collections.Generic;
using Nop.Core.Domain.Catalog;

namespace Nop.Core.Domain.Orders
{
    public partial class OrderLiveScene : BaseEntity
    {
        /// <summary>
        /// Gets or sets the order identifier
        /// </summary>
        public int OrderId { get; set; }

        public string Room { get; set; }
        public string Window { get; set; }
        public int OriginalPictureId { get; set; }
        public int EffectPictureId { get; set; }
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets the order
        /// </summary>
        public virtual Order Order { get; set; }
    }
}
