using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext
{
    /// <summary>
    /// 套餐模板项数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext")]
    public class DecorationPackItemInfo : BaseDTO
    {
        #region 套餐模板Id —— Guid PackId
        /// <summary>
        /// 套餐模板Id
        /// </summary>
        [DataMember]
        public Guid PackId { get; set; }
        #endregion

        #region 套餐模板空间Id —— Guid PackSpaceId
        /// <summary>
        /// 套餐模板空间Id
        /// </summary>
        [DataMember]
        public Guid PackSpaceId { get; set; }
        #endregion

        #region 套餐模板空间名称 —— string PackSpaceName
        /// <summary>
        /// 套餐模板空间名称
        /// </summary>
        [DataMember]
        public string PackSpaceName { get; set; }
        #endregion

        #region 品类Id —— Guid? CategoryId
        /// <summary>
        /// 品类Id
        /// </summary>
        [DataMember]
        public Guid? CategoryId { get; set; }
        #endregion

        #region 默认商品SKU数量 —— float? DefaultSkuQuantity
        /// <summary>
        /// 默认商品SKU数量
        /// </summary>
        [DataMember]
        public float? DefaultSkuQuantity { get; set; }
        #endregion

        #region 默认工艺工程量 —— float? DefaultCraftQuantity
        /// <summary>
        /// 默认工艺工程量
        /// </summary>
        [DataMember]
        public float? DefaultCraftQuantity { get; set; }
        #endregion

        #region 品类区域 —— CategoryArea CategoryArea
        /// <summary>
        /// 品类区域
        /// </summary>
        [DataMember]
        public CategoryArea CategoryArea { get; set; }
        #endregion

        #region  品类集 —— IList<Guid> CategoryIds
        /// <summary>
        ///  品类集
        /// </summary>
        [DataMember]
        public IList<Guid> CategoryIds { get; set; }
        #endregion

        #region   品牌Id集 —— IList<Guid> BrandIds
        /// <summary>
        /// 品牌Id集
        /// </summary>
        [DataMember]
        public IList<Guid> BrandIds { get; set; }
        #endregion

        #region 设计要求 —— string Requirement
        /// <summary>
        /// 设计要求
        /// </summary>
        [DataMember]
        public string Requirement { get; set; }
        #endregion

        #region 导航属性 - 套餐模板 —— DecorationPackInfo DecorationPackInfo
        /// <summary>
        /// 导航属性 - 套餐模板
        /// </summary>
        [DataMember]
        public DecorationPackInfo DecorationPackInfo { get; set; }
        #endregion
    }
}
