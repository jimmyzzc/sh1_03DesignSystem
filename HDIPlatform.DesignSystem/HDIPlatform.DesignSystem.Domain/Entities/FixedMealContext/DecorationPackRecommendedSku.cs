using HDIPlatform.DesignSystem.Domain.EventSources.FixedMealContext;
using ShSoft.Infrastructure.EntityBase;
using ShSoft.Infrastructure.EventBase.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using ShSoft.Common.PoweredByLee;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.Domain.Entities.FixedMealContext
{
    /// <summary>
    /// 套餐模板推荐商品SKU项
    /// </summary>
    [Serializable]
    public class DecorationPackRecommendedSku : PlainEntity
    {
        #region # 构造器

        #region 01.无参构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        protected DecorationPackRecommendedSku()
        {
            //默认值
            //this.Default = false;
            //this.Shelved = true;
        }
        #endregion

        #region 02.创建套餐模板商品SKU项构造器

        /// <summary>
        /// 创建套餐模板商品SKU项构造器  
        /// </summary>
        /// <param name="skuId">商品SKU Id</param>
        /// <param name="skuName">商品SKU 名称</param>
        /// <param name="shelved">是否已上架</param>
        /// <param name="categoryId">三级品类Id</param>
        /// <param name="categoryName">品类名称</param>
        /// <param name="brandId">品牌Id</param>
        /// <param name="brandName">品牌名称</param>
        /// <param name="isDefault">是否是默认商品</param>
        /// <param name="skuQuantity">默认商品工程量(手动填写)</param>
        /// <param name="unitName">商品工程量单位</param>
        /// <param name="costPrice">商品工程量成本单价</param>
        /// <param name="salesPrice">商品工程量销售单价</param>
        /// <param name="remark">备注</param>
        /// <param name="hyperlink">超链接</param>
        public DecorationPackRecommendedSku(Guid? skuId,string skuName, bool? shelved, Guid? categoryId,string categoryName, Guid? brandId, string brandName, bool isDefault, decimal skuQuantity, string unitName,decimal costPrice, decimal salesPrice,string remark,string hyperlink)
            : this()
        {
            this.SkuName = skuName;     
            this.CategoryName = categoryName;
            this.BrandName = brandName;
            this.SkuId = skuId;
            this.Shelved = shelved;
            this.CategoryId = categoryId;
            this.BrandId = brandId;
            this.Default = isDefault;
            this.CostPrice = costPrice;
            this.SalesPrice = salesPrice;
            this.SkuQuantity = skuQuantity;
            this.Remark = remark;
            this.HyperLink = hyperlink;
            this.UnitName = unitName;
        }
        #endregion

        #endregion

        #region # 属性  

        #region 商品SKU 名称 —— string SkuName
        /// <summary>
        /// 商品SKU 名称
        /// </summary>
        public string SkuName { get; private set; }
        #endregion

        #region 商品SKU Id —— Guid? SkuId
        /// <summary>
        /// 商品SKU Id
        /// </summary>
        public Guid? SkuId { get; private set; }
        #endregion

        #region 是否默认 —— bool Default
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool Default { get; private set; }
        #endregion

        #region 是否已上架 —— bool? Shelved
        /// <summary>
        /// 是否已上架
        /// </summary>
        public bool? Shelved { get; private set; }
        #endregion

        #region 三级品类Id —— Guid? CategoryId
        /// <summary>
        /// 三级品类Id
        /// </summary>
        public Guid? CategoryId { get; private set; }

        #endregion

        #region 三级品类名称 —— string CategoryName
        /// <summary>
        /// 三级品类名称
        /// </summary>
        public string CategoryName { get; private set; }

        #endregion

        #region 品牌Id —— Guid? BrandId
        /// <summary>
        /// 品牌Id
        /// </summary>
        public Guid? BrandId { get; private set; }
        #endregion

        #region 品牌名称 —— string BrandName
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string BrandName { get; private set; }

        #endregion

        #region 商品工程量单位 ——  string UnitName
        /// <summary>
        ///   商品工程量单位
        /// </summary>
        public string UnitName { get; private set; }
        #endregion

        #region  默认商品SKU数量 ——  decimal SkuQuantity
        /// <summary>
        ///   默认商品SKU数量(手动填写)
        /// </summary>
        public decimal SkuQuantity { get; private set; }
        #endregion

        #region 商品工程量成本价 ——  decimal CostPrice
        /// <summary>
        ///   商品工程量成本价
        /// </summary>
        public decimal CostPrice { get; private set; }
        #endregion

        #region 商品工程量销售价 ——  decimal SalesPrice
        /// <summary>
        ///   商品工程量销售价
        /// </summary>
        public decimal SalesPrice { get; private set; }
        #endregion

        #region 备注 —— string Remark
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; private set; }
        #endregion

        #region 超链接 —— string HyperLink
        /// <summary>
        /// 超链接
        /// </summary>
        public string HyperLink { get; private set; }
        #endregion

        #region 导航属性 - 套餐模板推荐项 —— DecorationPackRecommendedItem PackRecommendedItem 
        /// <summary>
        /// 导航属性 - 套餐模板推荐项
        /// </summary>
        public virtual DecorationPackRecommendedItem PackRecommendedItem { get; internal set; }
        #endregion

        #endregion

    }
}
