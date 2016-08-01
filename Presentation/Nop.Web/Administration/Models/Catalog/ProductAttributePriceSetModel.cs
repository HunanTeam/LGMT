using Nop.Core.Domain.Catalog;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nop.Admin.Models.Catalog
{
    public class ProductAttributePriceSetModel : BaseNopEntityModel
    {
        public ProductAttributePriceSetModel()
        {
            ZheGuangBu = new Catalog.ProductAttributePriceItemSetModel("遮光布");
            ChuangSha = new Catalog.ProductAttributePriceItemSetModel("窗纱");
            GuiDao = new Catalog.ProductAttributePriceItemSetModel("轨道");
        }



        public static void LoadData(ProductAttributePriceSetModel model, ProductAttributePriceSetSetting setting)
        {
            if (setting != null)
            {
                model.ZheGuangBu.PriceS = setting.ZheGuangBuPriceS;
                model.ZheGuangBu.PriceM = setting.ZheGuangBuPriceM;
                model.ZheGuangBu.PriceX = setting.ZheGuangBuPriceX;
                model.ZheGuangBu.PriceXL = setting.ZheGuangBuPriceXL;

                model.ChuangSha.PriceS = setting.ChuangShaPriceS;
                model.ChuangSha.PriceM = setting.ChuangShaPriceM;
                model.ChuangSha.PriceX = setting.ChuangShaPriceX;
                model.ChuangSha.PriceXL = setting.ChuangShaPriceXL;


                model.GuiDao.PriceS = setting.GuiDaoPriceS;
                model.GuiDao.PriceM = setting.GuiDaoPriceM;
                model.GuiDao.PriceX = setting.GuiDaoPriceX;
                model.GuiDao.PriceXL = setting.GuiDaoPriceXL;
            }
        }
        public static void SetData(ProductAttributePriceSetSetting setting, ProductAttributePriceSetModel model)
        {
            setting.ZheGuangBuPriceS = model.ZheGuangBu.PriceS;
            setting.ZheGuangBuPriceM = model.ZheGuangBu.PriceM;
            setting.ZheGuangBuPriceX = model.ZheGuangBu.PriceX;
            setting.ZheGuangBuPriceXL = model.ZheGuangBu.PriceXL;

            setting.ChuangShaPriceS = model.ChuangSha.PriceS;
            setting.ChuangShaPriceM = model.ChuangSha.PriceM;
            setting.ChuangShaPriceX = model.ChuangSha.PriceX;
            setting.ChuangShaPriceXL = model.ChuangSha.PriceXL;


            setting.GuiDaoPriceS = model.GuiDao.PriceS;
            setting.GuiDaoPriceM = model.GuiDao.PriceM;
            setting.GuiDaoPriceX = model.GuiDao.PriceX;
            setting.GuiDaoPriceXL = model.GuiDao.PriceXL;
        }


        /// <summary>
        /// 遮光布
        /// </summary>
        public ProductAttributePriceItemSetModel ZheGuangBu { get; set; }
        /// <summary>
        /// 窗纱
        /// </summary>
        public ProductAttributePriceItemSetModel ChuangSha { get; set; }
        /// <summary>
        /// 轨道
        /// </summary>
        public ProductAttributePriceItemSetModel GuiDao { get; set; }
    }
    public class ProductAttributePriceItemSetModel
    {
        public ProductAttributePriceItemSetModel(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
        [NopResourceDisplayName("价格（S）")]
        public decimal PriceS { get; set; }
        [NopResourceDisplayName("价格（M）")]
        public decimal PriceM { get; set; }
        [NopResourceDisplayName("价格（X）")]
        public decimal PriceX { get; set; }
        [NopResourceDisplayName("价格（XL）")]
        public decimal PriceXL { get; set; }

    }
}