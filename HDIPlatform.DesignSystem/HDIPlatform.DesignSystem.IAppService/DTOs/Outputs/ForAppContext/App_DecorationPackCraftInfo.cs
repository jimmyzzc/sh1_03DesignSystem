using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext;
using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext
{
    /// <summary>
    /// 套餐模板工艺项数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.ForAppContext")]
    public class App_DecorationPackCraftInfo : BaseDTO
    {
        #region 工艺实体Id —— Guid CraftEntityId
        /// <summary>
        /// 工艺实体Id
        /// </summary>
        [DataMember]
        public Guid CraftEntityId { get; set; }
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
    }
}
