using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext
{
    /// <summary>
    /// 套餐模板商品SKU项数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext")]
    public class App_DecorationPackSkuInfo : BaseDTO
    {
        #region 商品SKU Id —— Guid SkuId
        /// <summary>
        /// 商品SKU Id
        /// </summary>
        [DataMember]
        public Guid SkuId { get; set; }
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

        #region 选区名称 —— string PackItemName
        /// <summary>
        /// 选区名称
        /// </summary>
        [DataMember]
        public string PackItemName { get; set; }
        #endregion

        #region 空间名称 —— string SpaceName
        /// <summary>
        /// 空间名称
        /// </summary>
        [DataMember]
        public string SpaceName { get; set; }
        #endregion

        #region 空间Id —— Guid SpaceId
        /// <summary>
        /// 空间Id
        /// </summary>
        [DataMember]
        public Guid SpaceId { get; set; }
        #endregion

        #region 五大类 ——  ProductType ProductType
        /// <summary>
        /// 五大类
        /// </summary>
        [DataMember]
        public ProductType ProductType { get; set; }

        #endregion

        #region 品类 —— Guid string
        /// <summary>
        /// 品类
        /// </summary>
        [DataMember]
        public string Category { get; set; }

        #endregion

        #region 品牌 ——  string Brand 
        /// <summary>
        /// 品牌
        /// </summary>
        [DataMember]
        public string Brand { get; set; }
        #endregion
    }
}
