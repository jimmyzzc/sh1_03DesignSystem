using ShSoft.Infrastructure.DTOBase;
using ShSoft.ValueObjects.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext
{
    /// <summary>
    /// 套餐模板推荐项数据传输对象
    /// </summary>
    /// <remarks>勿忘[DataMember]</remarks>
    [DataContract(Namespace = "http://HDIPlatform.DesignSystem.IAppService.DTOs.Outputs.FixedMealContext")]
    public class DecorationPackRecommendedItemInfo : BaseDTO
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

        #region 备注 —— string Remark
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Remark { get; set; }
        #endregion

        #region 导航属性 - 套餐模板 —— DecorationPackInfo DecorationPackInfo
        /// <summary>
        /// 导航属性 - 套餐模板
        /// </summary>
        [DataMember]
        public IEnumerable<DecorationPackRecommendedSkuInfo> PackRecommendedSkuInfos { get; set; }
        #endregion

    }
}
