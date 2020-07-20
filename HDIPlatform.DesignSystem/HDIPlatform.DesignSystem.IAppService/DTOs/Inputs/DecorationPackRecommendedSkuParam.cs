using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Inputs
{
    /// <summary>
    /// 推荐商品参数模型
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Inputs")]
    public struct DecorationPackRecommendedSkuParam
    {
        #region 商品SKU 名称 —— string SkuName
        /// <summary>
        /// 商品SKU 名称
        /// </summary>
        [DataMember]
        public string SkuName { get; set; }
        #endregion

        #region 商品SKU Id —— Guid? SkuId
        /// <summary>
        /// 商品SKU Id
        /// </summary>
        [DataMember]
        public Guid? SkuId { get; set; }
        #endregion

        #region 是否已上架 —— bool? Shelved
        /// <summary>
        /// 是否已上架
        /// </summary>
        [DataMember]
        public bool? Shelved { get; set; }
        #endregion

        #region 三级品类Id —— Guid? CategoryId
        /// <summary>
        /// 三级品类Id
        /// </summary>
        [DataMember]
        public Guid? CategoryId { get; set; }

        #endregion

        #region 三级品类名称 —— string CategoryName
        /// <summary>
        /// 三级品类名称
        /// </summary>
        [DataMember]
        public string CategoryName { get; set; }

        #endregion

        #region 品牌Id —— Guid? BrandId
        /// <summary>
        /// 品牌Id
        /// </summary>
        [DataMember]
        public Guid? BrandId { get; set; }
        #endregion
        #region 品牌名称 —— string BrandName
        /// <summary>
        /// 品牌名称
        /// </summary>
        [DataMember]
        public string BrandName { get; set; }
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

        #region 商品工程量单位 ——  string UnitName
        /// <summary>
        ///   商品工程量单位
        /// </summary>
        [DataMember]
        public string UnitName { get; set; }
        #endregion

        #region 商品工程量成本价 ——  decimal CostPrice
        /// <summary>
        ///   商品工程量成本价
        /// </summary>
        [DataMember]
        public decimal CostPrice { get; set; }
        #endregion

        #region 商品工程量销售价 ——  decimal SalesPrice
        /// <summary>
        ///   商品工程量销售价
        /// </summary>
        [DataMember]
        public decimal SalesPrice { get;  set; }
        #endregion

        #region 备注 —— string Remark
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }
        #endregion

        #region 超链接 —— string HyperLink
        /// <summary>
        /// 超链接
        /// </summary>
        [DataMember]
        public string HyperLink { get; set; }
        #endregion

    }
}
