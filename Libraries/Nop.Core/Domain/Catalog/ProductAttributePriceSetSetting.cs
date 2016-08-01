using Nop.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Core.Domain.Catalog
{
    public class ProductAttributePriceSetSetting : ISettings
    {

        /// 遮光布
        public decimal ZheGuangBuPriceS { get; set; }
        public decimal ZheGuangBuPriceM { get; set; }
        public decimal ZheGuangBuPriceX { get; set; }
       
        public decimal ZheGuangBuPriceXL { get; set; }

        /// 窗纱
        public decimal ChuangShaPriceS { get; set; }
        public decimal ChuangShaPriceM { get; set; }
        public decimal ChuangShaPriceX { get; set; }
       
        public decimal ChuangShaPriceXL { get; set; }

        /// 轨道
        public decimal GuiDaoPriceS { get; set; }
        public decimal GuiDaoPriceM { get; set; }
        public decimal GuiDaoPriceX { get; set; }
        
        public decimal GuiDaoPriceXL { get; set; }
    }
}
