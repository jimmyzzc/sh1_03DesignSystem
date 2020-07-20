using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Inputs
{
    /// <summary>
    /// 商品参数模型
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Inputs")]
    public struct DecorationPackSkuParam
    {
        #region 商品SKU Id —— Guid SkuId
        /// <summary>
        /// 商品SKU Id
        /// </summary>
        [DataMember]
        public Guid SkuId { get; set; }
        #endregion

        #region 是否已上架 —— bool Shelved
        /// <summary>
        /// 是否已上架
        /// </summary>
        [DataMember]
        public bool Shelved { get; set; }
        #endregion

        #region 排序 —— int Sort
        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int Sort { get; set; }
        #endregion

        #region 三级品类Id —— Guid CategoryId
        /// <summary>
        /// 三级品类Id
        /// </summary>
        [DataMember]
        public Guid CategoryId { get; set; }

        #endregion

        #region 品牌Id —— Guid BrandId
        /// <summary>
        /// 品牌Id
        /// </summary>
        [DataMember]
        public Guid BrandId { get; set; }
        #endregion


        #region 是否默认 —— bool Default
        /// <summary>
        /// 是否默认
        /// </summary>
        [DataMember]
        public bool Default { get; set; }
        #endregion


        #region  默认商品SKU数量 ——  decimal SkuQuantity
        /// <summary>
        ///   默认商品SKU数量(手动填写)
        /// </summary>
        [DataMember]
        public decimal SkuQuantity { get; set; }
        #endregion


        #region  放置位置集 —— Dictionary<SkuCraftPosition, decimal> SkuCraftPositions

        /// <summary>
        /// 只读属性 - 放置位置集
        /// </summary>
        [DataMember]
        public Dictionary<SkuCraftPosition, decimal> SkuCraftPositions { get; set; }

        #endregion


        #region 成本价 ——  decimal CostPrice
        /// <summary>
        ///   成本价
        /// </summary>
        [DataMember]
        public decimal CostPrice { get; set; }
        #endregion

        #region 是否已变价 —— bool Changed
        /// <summary>
        /// 是否已变价
        /// </summary>
        [DataMember]
        public bool Changed { get; set; }
        #endregion
    }
}
