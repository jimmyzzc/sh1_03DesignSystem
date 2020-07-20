using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Inputs
{
    /// <summary>
    /// 套餐模板方案空间定价参数模型
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Inputs")]
    public struct PackSchemeSpacePricingParam
    {
        /// <summary>
        /// 套餐模板空间Id
        /// </summary>
        [DataMember]
        public Guid PackSpaceId { get; set; }

        #region 套餐空间定价类型 ——  DecorationPackSpacePricingType DecorationPackSpacePricingType
        /// <summary>
        /// 套餐空间定价类型
        /// </summary>
        [DataMember]
        public DecorationPackSpacePricingType DecorationPackSpacePricingType { get; set; }
        #endregion

        #region 标准面积 —— float CriterionSquare
        /// <summary>
        /// 标准面积（公式）
        /// </summary>
        [DataMember]
        public float CriterionSquare { get; set; }
        #endregion

        #region 空间占比 —— decimal SpaceRatio
        /// <summary>
        /// 空间占比（标准空间面积/标准整体面积|5/60）
        /// </summary>
        [DataMember]
        public decimal SpaceRatio { get; set; }
        #endregion

        #region 单价 —— decimal UnitPrice
        /// <summary>
        /// 超出面积单价
        /// </summary>
        [DataMember]
        public decimal UnitPrice { get; set; }
        #endregion
    }
}
