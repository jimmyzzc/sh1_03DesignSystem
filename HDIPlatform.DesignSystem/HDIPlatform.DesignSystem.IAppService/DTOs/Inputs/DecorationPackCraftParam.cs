using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Inputs
{
    /// <summary>
    /// 工艺参数模型
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Inputs")]
    public struct DecorationPackCraftParam
    {
        #region 工艺实体Id —— Guid CraftEntityId
        /// <summary>
        /// 工艺实体Id
        /// </summary>
        [DataMember]
        public Guid CraftEntityId { get; set; }
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

        #region 是否默认 —— bool Default
        /// <summary>
        /// 是否默认
        /// </summary>
        [DataMember]
        public bool Default { get; set; }
        #endregion


        #region  默认工艺工程量 ——  decimal CraftQuantity
        /// <summary>
        ///  默认工艺工程量
        /// </summary>
        [DataMember]
        public decimal CraftQuantity { get; set; }
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
